using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageResizer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform targetImage;  //クリップのRectTransform
    private Vector2 initialSizeDelta;
    private Vector2 initialMousePosition;

    private Vector3 center;
    private Vector2 offset;

    private bool isResizingRight;  // 右側をリサイズ中かどうかのフラグ

    private Vector2 size;
    private Vector2 deltaPivot;
    private Vector3 deltaPosition;

    [SerializeField, Header("クリップの最小サイズ")] private float f_minSize = 350;
    [SerializeField, Header("クリップの最大サイズ")] private float f_maxSize = 1400;
    private float f_newSize;

    [SerializeField, Header("左右端の範囲")] private float f_edgeRange = 10f;

    private float f_dotMove = 0;
    [SerializeField] private TimelineData timelineData;
    private float f_onetick;

    private Vector3 v3_mousePos;
    private Vector3 v3_offset;
    private float f_dotWidth = 0f;
    private float f_dotHeight = 0f;
    private float f_oneWidth;
    private float f_oneHeight;

    private int i_resizeCount = 0;

    private RectTransform rect_outLeft;    //タイムラインの左端
    private RectTransform rect_outRight;   //タイムラインの右端

    private enum CLIP_MODE
    {
        normal,
        resize,
        move,
    }
    private CLIP_MODE mode = CLIP_MODE.normal;


    private void Awake()
    {
        //リサイズ用
        f_onetick = timelineData.f_oneTickWidht;

        //クリップ移動用
        f_oneWidth = timelineData.f_oneTickWidht;
        f_oneHeight = timelineData.f_oneTickHeight;

        //タイムラインの端のRectTransform取得
        rect_outLeft = GameObject.Find("LeftOutLine").GetComponent<RectTransform>();
        rect_outRight = GameObject.Find("RightOutLine").GetComponent<RectTransform>();
    }
    private void Start()
    {
        center = targetImage.TransformPoint(targetImage.rect.center);
    }

    private void Update()
    {
        center = targetImage.TransformPoint(targetImage.rect.center);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialSizeDelta = targetImage.sizeDelta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetImage,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localMousePosition
        );

        // マウス位置が画像の右端か左端かをチェック
        if (Mathf.Abs(localMousePosition.x - (-targetImage.rect.width * targetImage.pivot.x)) <= f_edgeRange)
        {
            // 左端
            SetPivot(targetImage, new Vector2(1, 0.5f));
            isResizingRight = false;
            mode = CLIP_MODE.resize;
        }
        else if (Mathf.Abs(localMousePosition.x - (targetImage.rect.width * (1 - targetImage.pivot.x))) <= f_edgeRange)
        {
            // 右端
            SetPivot(targetImage, new Vector2(0, 0.5f));
            isResizingRight = true;
            mode = CLIP_MODE.resize;
        }
        else
        {
            // 端以外の場合はリサイズを無効化、クリップ移動モードにする
            mode = CLIP_MODE.move;
        }

        if (mode == CLIP_MODE.resize)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)targetImage.parent,
                eventData.position,
                eventData.pressEventCamera,
                out initialMousePosition
            );
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (mode != CLIP_MODE.resize)
        {
            if (targetImage == null)
            {
                mode = CLIP_MODE.normal;
                return;
            }

            //クリップ移動処理
            v3_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v3_mousePos.z = 0;

            v3_offset = targetImage.transform.position - center;

            //ドット移動用
            CalculationHeight();
            CalculationWidth();

            targetImage.transform.position = v3_mousePos + v3_offset;

            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)targetImage.parent,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 currentMousePosition
        );

        offset = currentMousePosition - initialMousePosition;

        if (isResizingRight)
        {
            f_newSize = initialSizeDelta.x + offset.x;
        }
        else
        {
            f_newSize = initialSizeDelta.x - offset.x;
        }

        CalculationSize();

        f_newSize = Mathf.Clamp(f_newSize, f_minSize, f_maxSize);

        if(IsOverlapping(targetImage,rect_outLeft) || IsOverlapping(targetImage, rect_outRight))
        {
            if(i_resizeCount == 0)
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

        center = targetImage.TransformPoint(targetImage.rect.center);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(mode != CLIP_MODE.normal)
        {
            //左端とクリップが重なってる場合
            if (IsOverlapping(targetImage, rect_outLeft))
            {
                //クリップ移動による場合
                if(mode == CLIP_MODE.move)
                {

                }
                //サイズ変更による場合
                else if(mode == CLIP_MODE.resize)
                {
                    ReCalculationSize();
                }
            }
            //右端とクリップが重なってる場合
            else if (IsOverlapping(targetImage, rect_outRight))
            {
                //クリップ移動による場合
                if (mode == CLIP_MODE.move)
                {

                }
                //サイズ変更による場合
                else if (mode == CLIP_MODE.resize)
                {
                    ReCalculationSize();
                }
            }
            i_resizeCount = 0;
            mode = CLIP_MODE.normal;
        }

    }

    private void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        size = rectTransform.rect.size;
        deltaPivot = rectTransform.pivot - pivot;
        deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);

        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition * rectTransform.localScale.x;
    }

    /// <summary>
    /// サイズをドット移動するための計算
    /// </summary>
    private void CalculationSize()
    {
        f_dotMove = Mathf.Round(f_newSize / f_onetick);
        f_newSize = f_dotMove * f_onetick;
    }

    private void ReCalculationSize()
    {
        f_dotMove -= i_resizeCount;
        f_newSize = f_dotMove * f_onetick;
        targetImage.sizeDelta = new Vector2(f_newSize, targetImage.sizeDelta.y);
    }


    /// <summary>
    /// X座標の計算用
    /// </summary>
    private void CalculationWidth()
    {

    }

    /// <summary>
    /// Y座標の計算用
    /// </summary>
    private void CalculationHeight()
    {

    }


    /// <summary>
    /// クリップと端が重なっているかをチェック
    /// </summary>
    /// <param name="rect1">クリップのRectTransform</param>
    /// <param name="rect2">端のRectTransform</param>
    /// <returns>重なっている=true 重なっていない=false</returns>
    private bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        // RectTransformの境界をワールド座標で取得
        Rect rect1World = GetWorldRect(rect1);
        Rect rect2World = GetWorldRect(rect2);

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

}
