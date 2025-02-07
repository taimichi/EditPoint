using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClipOperation : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform targetImage;  //クリップ画像のRectTransform
    private Vector2 v2_initSizeDelta;
    private Vector2 v2_initMousePos;

    private Vector2 v2_resizeOffset;
    private Vector2 v2_moveOffset;

    private bool isResizeRight;  // 右側をリサイズ中かどうかのフラグ

    private Vector2 v2_size;
    private Vector2 v2_deltaPivot;
    private Vector3 v2_deltaPos;

    private Vector2 v2_startSize;

    [SerializeField, Header("クリップの最小サイズ")] private float f_minSize = 350;
    [SerializeField, Header("クリップの最大サイズ")] private float f_maxSize = 1400;
    private float f_newSize;

    [SerializeField, Header("サイズ変更を受け付ける範囲(左右共通)")] private float f_edgeRange = 10f;

    private float f_dotMove = 0;
    private float f_onetick;            //サイズ変更時、1回にサイズ変更する量

    private Vector2 v2_mousePos;        //マウスの座標
    private float f_dotWidth = 0f;
    private float f_newWidth;           //新しいX座標
    private float f_dotHeight = 0f;
    private float f_newHeight;          //新しいY座標
    private float f_oneWidth;           //移動時、一回に移動する量　横
    private float f_oneHeight;          //移動時、一回に移動する量　縦

    private float timeBarLimitPos = 0f; //タイムバーの限界X座標

    private int i_resizeCount = 0;

    private RectTransform rect_UpLeft;    //タイムラインの左上
    private RectTransform rect_DownRight;   //タイムラインの右下

    private GameObject[] Clips;
    private RectTransform[] ClipsRect;

    private Vector3 v3_beforePos;
    private Vector2 savePos;

    //クリップの移動、サイズ変更機能が使用可能かどうか
    [SerializeField] private bool b_Lock = false;

    private CheckOverlap checkOverlap = new CheckOverlap();

    private enum CLIP_MODE
    {
        normal,
        resize,
        move,
    }
    private CLIP_MODE mode = CLIP_MODE.normal;

    private PlaySound playSound;

    private void Awake()
    {
        //リサイズ用
        f_onetick = TimelineData.TimelineEntity.f_oneResize;

        //クリップ移動用
        f_oneWidth = TimelineData.TimelineEntity.f_oneTickWidht;
        f_oneHeight = TimelineData.TimelineEntity.f_oneTickHeight;

        //タイムラインの端のRectTransform取得
        rect_UpLeft = GameObject.Find("UpLeftOutLine").GetComponent<RectTransform>();
        rect_DownRight = GameObject.Find("DownRightOutLine").GetComponent<RectTransform>();

        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        //クリップの位置を調整
        CalculationWidth(targetImage.localPosition.x);
        CalculationHeight(targetImage.localPosition.y);
        CheckWidth();
        CheckHeight();
        targetImage.localPosition = new Vector3(f_newWidth, f_newHeight, 0);
        v2_startSize = targetImage.sizeDelta;
        targetImage.sizeDelta = new Vector2(v2_startSize.x, v2_startSize.y);

        int childNum = targetImage.parent.transform.childCount;
        transform.SetSiblingIndex(childNum - 2);
    }
    private void Start()
    {
        //タイムバーの限界座標を取得
        timeBarLimitPos = GameObject.Find("Timebar").GetComponent<TimeBar>().ReturnLimitPos();

        //作成したばっかのクリップの時
        if (this.gameObject.tag == "CreateClip")
        {
            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                //他のクリップと重なった場合
                if (checkOverlap.IsOverlap(targetImage, ClipsRect[i]))
                {
                    //重なったクリップの下に移動
                    f_newHeight = ClipsRect[i].localPosition.y - f_oneHeight;
                    CheckHeight();

                    //クリップが一番下で重なった場合
                    if (ClipsRect[i].localPosition.y <= rect_DownRight.localPosition.y)
                    {
                        Debug.Log("一番下");
                        f_newHeight = 0 * f_oneHeight - 15f;

                        //重なったクリップの右端の座標を取得
                        float rightEdge = ClipsRect[i].anchoredPosition.x + (ClipsRect[i].rect.width * (1 - ClipsRect[i].pivot.x));

                        f_newWidth = rightEdge + f_oneWidth;

                        CalculationWidth(f_newWidth);
                        CalculationHeight(f_newHeight);

                        targetImage.localPosition = new Vector3(f_newWidth, f_newHeight, 0);
                        for(int j = 0; j < 5 /*タイムラインのレイヤー数*/ ; j++)
                        {
                            if (checkOverlap.IsOverlap(targetImage, ClipsRect[j]))
                            {
                                f_newHeight -= f_oneHeight;
                                targetImage.localPosition = new Vector3(f_newWidth, f_newHeight, 0);
                            }
                        }
                    }
                    targetImage.localPosition = new Vector3(f_newWidth, f_newHeight, 0);
                }
            }
            //タグ変更
            this.gameObject.tag = "SetClip";
        }
        v3_beforePos = this.transform.localPosition;

    }

    private void Update()
    {
        GetClipRect();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        if (!b_Lock)
        {
            v2_initSizeDelta = targetImage.sizeDelta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetImage,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localMousePos
            );

            // マウス位置が画像の右端か左端かをチェック
            if (Mathf.Abs(localMousePos.x - (-targetImage.rect.width * targetImage.pivot.x)) <= f_edgeRange)
            {
                // 左端
                SetPivot(targetImage, new Vector2(1, 0.5f));
                isResizeRight = false;
                mode = CLIP_MODE.resize;
            }
            else if (Mathf.Abs(localMousePos.x - (targetImage.rect.width * (1 - targetImage.pivot.x))) <= f_edgeRange)
            {
                // 右端
                SetPivot(targetImage, new Vector2(0, 0.5f));
                isResizeRight = true;
                mode = CLIP_MODE.resize;
            }
            else
            {
                // 端以外の場合はリサイズを無効化、クリップ移動モードにする
                mode = CLIP_MODE.move;
                SetPivot(targetImage, new Vector2(0, 0.5f));
                v2_moveOffset.x = targetImage.position.x - localMousePos.x;
            }

            if (mode == CLIP_MODE.resize)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)targetImage.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out v2_initMousePos
                );
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        if (!b_Lock)
        {
            if (mode != CLIP_MODE.resize)
            {
                if (targetImage == null)
                {
                    mode = CLIP_MODE.normal;
                    return;
                }

                //クリップ移動処理
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)targetImage.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out v2_mousePos
                );

                //ドット移動用
                CalculationWidth(v2_mousePos.x + v2_moveOffset.x);
                CalculationHeight(v2_mousePos.y);

                //タイムラインの範囲外に出た時
                CheckWidth();
                CheckHeight();

                targetImage.localPosition = new Vector3(f_newWidth, f_newHeight, 0);

                ////タイムバーの限界座標を超えたら
                //if (CheckLimitPos())
                //{
                //    v2_newPos.x = timeBarLimitPos - targetImage.rect.width - 30;
                //    this.targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);
                //}

                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)targetImage.parent,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 currentMousePos
            );

            v2_resizeOffset = currentMousePos - v2_initMousePos;

            if (isResizeRight)
            {
                f_newSize = v2_initSizeDelta.x + v2_resizeOffset.x;
            }
            else
            {
                f_newSize = v2_initSizeDelta.x - v2_resizeOffset.x;
            }

            CalculationSize();

            //クリップの長さ変更の際に最大・最小サイズを超えないようにする
            f_newSize = Mathf.Clamp(f_newSize, f_minSize, f_maxSize);

            //タイムラインの左端、右端を超えるとき
            if (targetImage.position.x > rect_UpLeft.position.x 
                || targetImage.position.x < rect_DownRight.position.x)
            {
                if (i_resizeCount == 0)
                {
                    i_resizeCount = 1;
                }

                //リサイズ前が大きい場合
                if (targetImage.sizeDelta.x > f_newSize)
                {
                    i_resizeCount--;
                }
                //リサイズ前が小さい場合
                else if (targetImage.sizeDelta.x < f_newSize)
                {
                    i_resizeCount++;
                }
            }

            targetImage.sizeDelta = new Vector2(f_newSize, targetImage.sizeDelta.y);

            ////サイズ変更時にタイムバーの限界座標に行ったとき
            //if (CheckLimitPos())
            //{
            //    targetImage.sizeDelta = savePos;

            //}
            //savePos = targetImage.sizeDelta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        if (!b_Lock)
        {
            //ピボットを初期のものに
            SetPivot(targetImage, new Vector2(0, 0.5f));
            //重なった場合
            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                ClipsRect[i].localPosition = new Vector3(
                    ClipsRect[i].localPosition.x - 0.1f, ClipsRect[i].localPosition.y, ClipsRect[i].localPosition.z);

                if (checkOverlap.IsOverlap(targetImage, ClipsRect[i]))
                {
                    //同じオブジェクトじゃないとき
                    if (targetImage.name != Clips[i].name)
                    {
                        Debug.Log("重なった");
                        targetImage.localPosition = v3_beforePos;
                    }
                }
            }

            v3_beforePos = this.transform.localPosition;

            if (mode != CLIP_MODE.normal)
            {
                playSound.PlaySE(PlaySound.SE_TYPE.objMove);
                //タイムラインの左端とクリップが重なってる場合
                if (checkOverlap.IsOverlap(targetImage, rect_UpLeft))
                {
                    //サイズ変更による場合
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                //タイムラインの右端とクリップが重なってる場合
                else if (checkOverlap.IsOverlap(targetImage, rect_DownRight))
                {
                    //サイズ変更による場合
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                i_resizeCount = 0;
                mode = CLIP_MODE.normal;
            }
        }

    }

    private void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        v2_size = rectTransform.rect.size;
        v2_deltaPivot = rectTransform.pivot - pivot;
        v2_deltaPos = new Vector3(v2_deltaPivot.x * v2_size.x, v2_deltaPivot.y * v2_size.y);

        rectTransform.pivot = pivot;
        rectTransform.localPosition -= v2_deltaPos * rectTransform.localScale.x;
    }

    /// <summary>
    /// サイズ変更するための計算
    /// </summary>
    private void CalculationSize()
    {
        f_dotMove = (float)Math.Round(f_newSize / f_onetick);
        f_newSize = f_dotMove * f_onetick;
    }

    /// <summary>
    /// クリップが画面外に行った際の処理
    /// </summary>
    private void ReCalculationSize()
    {
        f_dotMove -= i_resizeCount;
        f_newSize = f_dotMove * f_onetick;
        targetImage.sizeDelta = new Vector2(f_newSize, targetImage.sizeDelta.y);
    }


    /// <summary>
    /// X座標の計算用
    /// </summary>
    private void CalculationWidth(float posX)
    {
        f_dotWidth = posX - ((float)Math.Round(posX / f_oneWidth) * f_oneWidth);
        if (f_dotWidth < f_oneWidth / 2)
        {
            f_newWidth = (float)Math.Round(posX / f_oneWidth) * f_oneWidth + 30f;
        }
        else
        {
            f_newWidth = ((float)Math.Round(posX / f_oneWidth) + 1) * f_oneWidth + 30f;
        }
        //計算にある 30 はタイムラインの枠の分
    }

    /// <summary>
    /// Y座標の計算用
    /// </summary>
    private void CalculationHeight(float posY)
    {
        f_dotHeight = posY - ((float)Math.Round(posY / f_oneHeight) * f_oneHeight);
        if (f_dotHeight < f_oneHeight / 2)
        {
            f_newHeight = (float)Math.Round(posY / f_oneHeight) * f_oneHeight - 15f;
        }
        else
        {
            f_newHeight = ((float)Math.Round(posY / f_oneHeight) + 1) * f_oneHeight - 15f;
        }
        //計算にある 15 はタイムラインの枠の分
    }

    /// <summary>
    /// クリップがタイムラインの左右の範囲外に出た時
    /// </summary>
    private void CheckWidth()
    {
        //左側
        if (targetImage.localPosition.x < rect_UpLeft.localPosition.x)
        {
            f_newWidth = rect_UpLeft.localPosition.x;
        }
        //右側
        else if (targetImage.localPosition.x > rect_DownRight.localPosition.x - targetImage.sizeDelta.x)
        {
            f_newWidth = rect_DownRight.localPosition.x - targetImage.sizeDelta.x;
        }
    }

    /// <summary>
    /// クリップがタイムラインの上下の範囲外に出た時
    /// </summary>
    private void CheckHeight()
    {
        //上
        if (targetImage.localPosition.y > rect_UpLeft.localPosition.y)
        {
            f_newHeight = rect_UpLeft.localPosition.y;
        }
        //下
        else if (targetImage.localPosition.y < rect_DownRight.localPosition.y)
        {
            f_newHeight = rect_DownRight.localPosition.y;
        }
    }

    private void GetClipRect()
    {
        Clips = GameObject.FindGameObjectsWithTag("SetClip");
        ClipsRect = new RectTransform[Clips.Length];
        for(int i = 0; i < Clips.Length; i++)
        {
            ClipsRect[i] = Clips[i].GetComponent<RectTransform>();
        }
    }

    /// <summary>
    /// タイムバーの限界座標を超えたかの判定
    /// </summary>
    /// <returns>false=超えてない　true=超えた</returns>
    private bool CheckLimitPos()
    {
        bool isCheck = false;

        //タイムバーの限界座標を超えたら
        float rightEdge = targetImage.anchoredPosition.x + (targetImage.rect.width * (1 - targetImage.pivot.x));
        if (rightEdge > timeBarLimitPos)
        {
            isCheck = true;
        }


        return isCheck;
    }
}
