using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool isDragging = false;
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private Canvas canvas; // UIの座標系を取得するためのCanvas

    public RectTransform targetImage; // 目標地点のImageのRectTransform
    public float snapDistance = 50f; // スナップする距離の閾値

    // ここに表示する予定のキャンバスオブジェクトを渡す
    [SerializeField] LoadingProgressBar load;
    // ロードを表示する
    [SerializeField] GameObject LoadObj;
    // 表示するステージパネル
    [SerializeField] GameObject StagePanel;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        // 親のCanvasを取得
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvasが見つかりません！このスクリプトはCanvas内のUIオブジェクトでのみ機能します。");
        }
        Vector2 checkPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);  // チェックしたい位置
        CheckUIObjectAtPosition(checkPosition, canvas);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && canvas != null)
        {
            // UIの座標変換を考慮して位置を更新
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );

            rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // 目標のImageとの距離を計算
        float distance = Vector2.Distance(rectTransform.anchoredPosition, targetImage.anchoredPosition);

        // しきい値内なら目標Imageの位置にスナップ
        if (distance < snapDistance)
        {
            rectTransform.anchoredPosition = targetImage.anchoredPosition;
            LoadObj.SetActive(true);
            load.SetObj = StagePanel;
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition; // 元の位置に戻る
        }
    }

    // 指定された座標が特定のRectTransform内にあるかチェック
    public static void CheckUIObjectAtPosition(Vector2 position, Canvas canvas, float tolerance = 0.1f)
    {
        // Canvas内のすべてのUIオブジェクトを取得
        RectTransform[] rectTransforms = canvas.GetComponentsInChildren<RectTransform>();

        foreach (var rectTransform in rectTransforms)
        {
            // RectTransformがスクリーン座標系に基づいているかを確認
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                position,
                canvas.worldCamera,
                out localPosition
            );

            // そのUIオブジェクトのRect内に指定位置が含まれているかチェック
            if (rectTransform.rect.Contains(localPosition))
            {
                // 位置が含まれていればオブジェクト名を出力
                Debug.Log("指定位置にオブジェクトがあります: " + rectTransform.gameObject.name);
            }
        }
    }

    /// <summary>
    /// 元の位置に戻す
    /// </summary>
    public void OriginalPos()
    {
        rectTransform.anchoredPosition = originalPosition; // 元の位置に戻る
    }
}
