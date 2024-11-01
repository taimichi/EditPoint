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

    private bool b_ResizeRight;  // 右側をリサイズ中かどうかのフラグ

    private Vector2 v2_size;
    private Vector2 v2_deltaPivot;
    private Vector3 v2_deltaPos;

    [SerializeField, Header("クリップの最小サイズ")] private float f_minSize = 350;
    [SerializeField, Header("クリップの最大サイズ")] private float f_maxSize = 1400;
    private float f_newSize;

    [SerializeField, Header("左右端の範囲")] private float f_edgeRange = 10f;

    private float f_dotMove = 0;
    private float f_onetick;            //サイズ変更時、1回にサイズ変更する量

    private Vector2 v2_mousePos;        //マウスの座標
    private Vector2 v2_newPos;          //新しい座標
    private float f_dotWidth = 0f;
    private float f_newWidth;           //新しい横の移動位置
    private float f_dotHeight = 0f;
    private float f_newHeight;          //新しいタテの移動位置
    private float f_oneWidth;           //移動時、一回に移動する量　横
    private float f_oneHeight;          //移動時、一回に移動する量　縦

    private int i_resizeCount = 0;

    private RectTransform rect_outLeft;    //タイムラインの左端
    private RectTransform rect_outRight;   //タイムラインの右端

    private GameObject[] Clips;
    private RectTransform[] ClipsRect;

    private Vector3 v3_beforePos;

    [SerializeField] private bool b_Lock = false;

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
        rect_outLeft = GameObject.Find("LeftOutLine").GetComponent<RectTransform>();
        rect_outRight = GameObject.Find("RightOutLine").GetComponent<RectTransform>();

        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        CalculationWidth(targetImage.localPosition.x);
        CalculationHeight(targetImage.localPosition.y);
        CheckWidth();
        CheckHeight();
        v2_newPos = new Vector2(f_newWidth, f_newHeight);
        targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);
    }
    private void Start()
    {
        if (this.gameObject.tag == "CreateClip")
        {
            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                //他のクリップと重なった場合
                if (CheckOverrap(targetImage, ClipsRect[i]))
                {
                    Debug.Log("重なった");
                    //重なったクリップの下に移動
                    f_newHeight = ClipsRect[i].localPosition.y - f_oneHeight;
                    CheckHeight();

                    //クリップが一番下で重なった場合
                    if (ClipsRect[i].localPosition.y <= TimelineData.TimelineEntity.f_timelineEndDown)
                    {
                        f_newHeight = 0 * f_oneHeight - 15f;

                        //重なったクリップの右端の座標を取得
                        float rightEdge = ClipsRect[i].anchoredPosition.x + (ClipsRect[i].rect.width * (1 - ClipsRect[i].pivot.x));

                        f_newWidth = rightEdge + f_oneWidth;

                        CalculationWidth(f_newWidth);
                        CalculationHeight(f_newHeight);

                        v2_newPos = new Vector2(f_newWidth, f_newHeight);
                        targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);
                        for(int j = 0; j < 5; j++)
                        {
                            if (CheckOverrap(targetImage, ClipsRect[j]))
                            {
                                f_newHeight -= f_oneHeight;
                                v2_newPos = new Vector2(f_newWidth, f_newHeight);
                                targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);
                            }
                        }
                    }
                    v2_newPos = new Vector2(f_newWidth, f_newHeight);
                    targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);
                }
            }

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
        if (GameData.GameEntity.b_playNow)
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
                b_ResizeRight = false;
                mode = CLIP_MODE.resize;
            }
            else if (Mathf.Abs(localMousePos.x - (targetImage.rect.width * (1 - targetImage.pivot.x))) <= f_edgeRange)
            {
                // 右端
                SetPivot(targetImage, new Vector2(0, 0.5f));
                b_ResizeRight = true;
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
        if (GameData.GameEntity.b_playNow)
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
                v2_newPos = new Vector2(f_newWidth, f_newHeight);

                targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);


                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)targetImage.parent,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 currentMousePos
            );

            v2_resizeOffset = currentMousePos - v2_initMousePos;

            if (b_ResizeRight)
            {
                f_newSize = v2_initSizeDelta.x + v2_resizeOffset.x;
            }
            else
            {
                f_newSize = v2_initSizeDelta.x - v2_resizeOffset.x;
            }

            CalculationSize();

            f_newSize = Mathf.Clamp(f_newSize, f_minSize, f_maxSize);

            if (CheckOverrap(targetImage, rect_outLeft) || CheckOverrap(targetImage, rect_outRight))
            {
                if (i_resizeCount == 0)
                {
                    i_resizeCount = 1;
                }

                //リサイズ前が大きい場合
                if (targetImage.sizeDelta.x > f_newSize)
                {
                    i_resizeCount--;

                    Debug.Log("い");
                }
                //リサイズ前が小さい場合
                else if (targetImage.sizeDelta.x < f_newSize)
                {
                    Debug.Log("あ");

                    i_resizeCount++;
                }
            }

            targetImage.sizeDelta = new Vector2(f_newSize, targetImage.sizeDelta.y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.b_playNow)
        {
            return;
        }

        if (!b_Lock)
        {
            //重なった場合
            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                if (CheckOverrap(targetImage, ClipsRect[i]))
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
                //左端とクリップが重なってる場合
                if (CheckOverrap(targetImage, rect_outLeft))
                {
                    //サイズ変更による場合
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                //右端とクリップが重なってる場合
                else if (CheckOverrap(targetImage, rect_outRight))
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
    /// クリップが画面外に行ってしまった場合用
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
            f_newWidth = (float)Math.Round(posX / f_oneWidth) * f_oneWidth - 30f;
        }
        else
        {
            f_newWidth = ((float)Math.Round(posX / f_oneWidth) + 1) * f_oneWidth - 30f;
        }
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
    }

    /// <summary>
    /// クリップがタイムラインの左右の範囲外に出た時
    /// </summary>
    private void CheckWidth()
    {
        if (f_newWidth < TimelineData.TimelineEntity.f_timelineEndLeft)
        {
            f_newWidth = TimelineData.TimelineEntity.f_timelineEndLeft;
        }
        else if (f_newWidth > TimelineData.TimelineEntity.f_timelineEndRight)
        {
            f_newWidth = TimelineData.TimelineEntity.f_timelineEndRight;
        }
    }

    /// <summary>
    /// クリップがタイムラインの上下の範囲外に出た時
    /// </summary>
    private void CheckHeight()
    {
        if (f_newHeight > TimelineData.TimelineEntity.f_timelineEndUp)
        {
            f_newHeight = TimelineData.TimelineEntity.f_timelineEndUp;
        }
        else if (f_newHeight < TimelineData.TimelineEntity.f_timelineEndDown)
        {
            f_newHeight = TimelineData.TimelineEntity.f_timelineEndDown;
        }
    }


    /// <summary>
    /// クリップと端が重なっているかをチェック
    /// </summary>
    /// <param name="clipRect">クリップのRectTransform</param>
    /// <param name="edgeRect">端のRectTransform</param>
    /// <returns>重なっている=true 重なっていない=false</returns>
    private bool CheckOverrap(RectTransform clipRect, RectTransform edgeRect)
    {
        // RectTransformの境界をワールド座標で取得
        Rect rect1World = GetWorldRect(clipRect);
        Rect rect2World = GetWorldRect(edgeRect);

        // 境界が重なっているかどうかをチェック
        return rect1World.Overlaps(rect2World);
    }

    /// <summary>
    /// ワールド座標での境界を取得
    /// </summary>
    /// <param name="rt">取得するRectTransform</param>
    /// <returns>ワールド座標でのRectTransform</returns>
    private Rect GetWorldRect(RectTransform rt)
    {
        //四隅のワールド座標を入れる配列
        Vector3[] corners = new Vector3[4];
        //RectTransformの四隅のワールド座標を取得
        rt.GetWorldCorners(corners);

        return new Rect(corners[0], corners[2] - corners[0]);
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
