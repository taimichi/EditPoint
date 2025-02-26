using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Pixeye.Unity;

public class ClipOperation : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Foldout("Start"), SerializeField, Header("クリップの長さ(秒)")]
    private float startLength = 5f;

    [SerializeField] private RectTransform targetImage;  //クリップ画像のRectTransform
    private Vector2 initSizeDelta;
    private Vector2 initMousePos;

    private Vector2 resizeOffset;
    private Vector2 moveOffset;

    private bool isResizeRight;  // 右側をリサイズ中かどうかのフラグ

    private Vector2 size;
    private Vector2 deltaPivot;
    private Vector3 deltaPos;

    private Vector2 startSize;

    [SerializeField, Header("クリップの最小サイズ")] private float minWidth = 350;
    [SerializeField, Header("クリップの最大サイズ")] private float maxWidth = 1400;
    private float newWidth;

    [SerializeField, Header("サイズ変更を受け付ける範囲(左右共通)")] private float edgeRange = 10f;

    private float dotMove = 0;
    private float onetick;            //サイズ変更時、1回にサイズ変更する量

    private Vector2 mousePos;        //マウスの座標
    private float dotPosX = 0f;
    private float dotPosY = 0f;
    private float newPosY;          //新しいY座標
    private float oneWidth;           //移動時、一回に移動する量　横
    private float oneHeight;          //移動時、一回に移動する量　縦

    private int resizeCount = 0;

    private RectTransform rect_UpLeft;    //タイムラインの左上
    private RectTransform rect_DownRight;   //タイムラインの右下

    private GameObject[] Clips;
    private RectTransform[] ClipsRect;

    private Vector3 startPos;   //初期位置

    private CheckOverlap checkOverlap = new CheckOverlap();

    private FunctionLookManager functionLook;

    /// <summary>
    /// クリップ操作の種類
    /// </summary>
    private enum CLIP_MODE
    {
        /// <summary>
        /// 何もしていない、ノーマル
        /// </summary>
        normal,
        /// <summary>
        /// サイズ変更操作
        /// </summary>
        resize,
        /// <summary>
        /// 移動操作
        /// </summary>
        move,
    }
    //現在のクリップ操作の状態
    private CLIP_MODE mode = CLIP_MODE.normal;

    private PlaySound playSound;

    //クリップがタイムラインの外に出たか
    private bool isOut = false;

    [SerializeField] private bool isLook = false;

    private void Awake()
    {
        //リサイズ用
        onetick = TimelineData.TimelineEntity.oneResize;

        //クリップ移動用
        oneWidth = TimelineData.TimelineEntity.oneTickWidth;
        oneHeight = TimelineData.TimelineEntity.oneTickHeight;

        //タイムラインの端のRectTransform取得
        rect_UpLeft = GameObject.Find("UpLeftOutLine").GetComponent<RectTransform>();
        rect_DownRight = GameObject.Find("DownRightOutLine").GetComponent<RectTransform>();

        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        functionLook = GameObject.FindWithTag("GameManager").GetComponent<FunctionLookManager>();

        //初期の長さ
        targetImage.sizeDelta = new Vector2(
            startLength * onetick * 2, targetImage.sizeDelta.y
            );

        //クリップの位置を調整
        CalculationWidth(targetImage.localPosition.x);
        CalculationHeight(targetImage.localPosition.y);
        CheckWidth();
        CheckHeight();
        targetImage.localPosition = new Vector3(newWidth, newPosY, 0);
        startSize = targetImage.sizeDelta;
        targetImage.sizeDelta = new Vector2(startSize.x, startSize.y);

        //子オブジェクトの順番を変更
        int childNum = targetImage.parent.transform.childCount;
        transform.SetSiblingIndex(childNum - 2);
    }
    private void Start()
    {
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
                    newPosY = ClipsRect[i].localPosition.y - oneHeight;
                    CheckHeight();

                    //クリップが一番下で重なった場合
                    if (ClipsRect[i].localPosition.y <= rect_DownRight.localPosition.y)
                    {
                        Debug.Log("一番下");
                        newPosY = 0 * oneHeight - 15f;

                        //重なったクリップの右端の座標を取得
                        float rightEdge = ClipsRect[i].anchoredPosition.x + (ClipsRect[i].rect.width * (1 - ClipsRect[i].pivot.x));

                        newWidth = rightEdge + oneWidth;

                        CalculationWidth(newWidth);
                        CalculationHeight(newPosY);

                        targetImage.localPosition = new Vector3(newWidth, newPosY, 0);
                        for(int j = 0; j < 5 /*タイムラインのレイヤー数*/ ; j++)
                        {
                            if (checkOverlap.IsOverlap(targetImage, ClipsRect[j]))
                            {
                                newPosY -= oneHeight;
                                targetImage.localPosition = new Vector3(newWidth, newPosY, 0);
                            }
                        }
                    }
                    targetImage.localPosition = new Vector3(newWidth, newPosY, 0);
                }
            }
            //タグ変更
            this.gameObject.tag = "SetClip";
        }
        startPos = this.transform.localPosition;
        isOut = false;
    }

    private void Update()
    {
        GetClipRect();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow || isLook)
        {
            return;
        }

        if ((functionLook.FunctionLook & LookFlags.ClipAccess) == 0)
        {
            initSizeDelta = targetImage.sizeDelta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetImage,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localMousePos
            );

            // マウス位置が画像の右端か左端かをチェック
            if (Mathf.Abs(localMousePos.x - (-targetImage.rect.width * targetImage.pivot.x)) <= edgeRange)
            {
                // 左端
                SetPivot(targetImage, new Vector2(1, 0.5f));
                isResizeRight = false;
                mode = CLIP_MODE.resize;
            }
            else if (Mathf.Abs(localMousePos.x - (targetImage.rect.width * (1 - targetImage.pivot.x))) <= edgeRange)
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
                moveOffset.x = targetImage.position.x - localMousePos.x;
            }

            if (mode == CLIP_MODE.resize)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)targetImage.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out initMousePos
                );
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow || isLook)
        {
            return;
        }

        if ((functionLook.FunctionLook & LookFlags.ClipAccess) == 0)
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
                    out mousePos
                );

                //ドット移動用
                CalculationWidth(mousePos.x + moveOffset.x);
                CalculationHeight(mousePos.y);

                //タイムラインの範囲外に出た時
                CheckWidth();
                CheckHeight();

                targetImage.localPosition = new Vector3(newWidth, newPosY, 0);
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)targetImage.parent,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 currentMousePos
            );

            resizeOffset = currentMousePos - initMousePos;

            if (isResizeRight)
            {
                newWidth = initSizeDelta.x + resizeOffset.x;
            }
            else
            {
                newWidth = initSizeDelta.x - resizeOffset.x;
            }

            CalculationSize();

            //クリップの長さ変更の際に最大・最小サイズを超えないようにする
            newWidth = Mathf.Clamp(newWidth, minWidth, maxWidth);

            //タイムラインの左端、右端を超えるとき
            if (targetImage.position.x > rect_UpLeft.position.x 
                || targetImage.position.x < rect_DownRight.position.x)
            {
                if (resizeCount == 0)
                {
                    resizeCount = 1;
                }

                //リサイズ前が大きい場合
                if (targetImage.sizeDelta.x > newWidth)
                {
                    resizeCount--;
                }
                //リサイズ前が小さい場合
                else if (targetImage.sizeDelta.x < newWidth)
                {
                    resizeCount++;
                }
            }

            targetImage.sizeDelta = new Vector2(newWidth, targetImage.sizeDelta.y);        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow || isLook)
        {
            return;
        }

        if ((functionLook.FunctionLook & LookFlags.ClipAccess) == 0)
        {
            //ピボットを初期のものに
            SetPivot(targetImage, new Vector2(0, 0.5f));
            //重なった場合
            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                ClipsRect[i].localPosition = new Vector3(
                    ClipsRect[i].localPosition.x, ClipsRect[i].localPosition.y, ClipsRect[i].localPosition.z);

                if (checkOverlap.IsOverlap(targetImage, ClipsRect[i]))
                {
                    //同じオブジェクトじゃないとき
                    if (targetImage.name != Clips[i].name)
                    {
                        Debug.Log("重なった");
                        targetImage.localPosition = startPos;
                    }
                }
            }

            //クリップがタイムラインの外に出た時
            if (isOut)
            {
                targetImage.localPosition = startPos;
                isOut = false;
            }
            
            startPos = this.transform.localPosition;

            if (mode != CLIP_MODE.normal)
            {
                playSound.PlaySE(PlaySound.SE_TYPE.objMove);
                //タイムラインの左端とクリップが重なってる場合
                if (targetImage.localPosition.x < rect_UpLeft.localPosition.x)
                {
                    Debug.Log("左重なった");
                    //サイズ変更による場合
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                //タイムラインの右端とクリップが重なってる場合
                else if (targetImage.localPosition.x + targetImage.sizeDelta.x > rect_DownRight.localPosition.x)
                {
                    Debug.Log("右重なった");
                    //サイズ変更による場合
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                resizeCount = 0;
                mode = CLIP_MODE.normal;
            }
        }

    }

    private void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        size = rectTransform.rect.size;
        deltaPivot = rectTransform.pivot - pivot;
        deltaPos = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);

        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPos * rectTransform.localScale.x;
    }

    /// <summary>
    /// サイズ変更するための計算
    /// </summary>
    private void CalculationSize()
    {
        dotMove = (float)Math.Round(newWidth / onetick);
        newWidth = dotMove * onetick;
    }

    /// <summary>
    /// クリップが画面外に行った際の処理
    /// </summary>
    private void ReCalculationSize()
    {
        dotMove -= resizeCount;
        newWidth = dotMove * onetick;
        targetImage.sizeDelta = new Vector2(newWidth, targetImage.sizeDelta.y);
    }


    /// <summary>
    /// X座標の計算用
    /// </summary>
    private void CalculationWidth(float posX)
    {
        dotPosX = posX - ((float)Math.Round(posX / oneWidth) * oneWidth);
        if (dotPosX < oneWidth / 2)
        {
            newWidth = (float)Math.Round(posX / oneWidth) * oneWidth + 30f;
        }
        else
        {
            newWidth = ((float)Math.Round(posX / oneWidth) + 1) * oneWidth + 30f;
        }
        //計算にある 30 はタイムラインの枠の分
    }

    /// <summary>
    /// Y座標の計算用
    /// </summary>
    private void CalculationHeight(float posY)
    {
        dotPosY = posY - ((float)Math.Round(posY / oneHeight) * oneHeight);
        if (dotPosY < oneHeight / 2)
        {
            newPosY = (float)Math.Round(posY / oneHeight) * oneHeight - 15f;
        }
        else
        {
            newPosY = ((float)Math.Round(posY / oneHeight) + 1) * oneHeight - 15f;
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
            newWidth = rect_UpLeft.localPosition.x;
            isOut = true;
        }
        //右側
        else if (targetImage.localPosition.x + targetImage.sizeDelta.x > rect_DownRight.localPosition.x)
        {
            newWidth = rect_DownRight.localPosition.x - targetImage.sizeDelta.x;
            isOut = true;
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
            newPosY = rect_UpLeft.localPosition.y;
            isOut = true;
        }
        //下
        else if (targetImage.localPosition.y < rect_DownRight.localPosition.y)
        {
            newPosY = rect_DownRight.localPosition.y;
            isOut = true;
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
}
