using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomVerticalSlider : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform topLimit;  // 上限（スライダー）
    public RectTransform bottomLimit;  // 下限（スライダー）
    public RectTransform targetTopLimit;  // 対象オブジェクトの上限
    public RectTransform targetBottomLimit;  // 対象オブジェクトの下限
    public RectTransform targetRect;  // 対象の RectTransform
    public float scrollSpeed = 10f;  // マウスホイールの感度

    // 移動速度を調整するためのスケーリングファクター
    public float speedMultiplier = 2f; // スライダーの移動速度を調整

    private RectTransform handle; // ハンドル（自身の RectTransform）
    private bool isDragging = false;
    private float currentYPosition;  // 現在のスライダー位置
    private Vector2 pointerOffset;  // クリック位置のオフセット
    private Vector2 initialMousePosition; // クリックした時のマウスの位置

    void Start()
    {
        handle = GetComponent<RectTransform>();
        currentYPosition = handle.anchoredPosition.y;  // 初期位置を設定
    }

    // クリックした位置にスライダーを移動
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;

        // マウス位置からスライダーの位置を引いてオフセットを計算
        RectTransformUtility.ScreenPointToLocalPointInRectangle(handle.parent as RectTransform, eventData.position, eventData.pressEventCamera, out pointerOffset);
        initialMousePosition = eventData.position;
    }

    // マウスボタンを離したらドラッグ解除
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    // ドラッグ処理
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // 現在のマウスの移動量を計算して、移動速度を調整
        float mouseDeltaY = eventData.position.y - initialMousePosition.y;

        // スピード調整：移動量を速度スケールで調整
        float adjustedDeltaY = mouseDeltaY * speedMultiplier;

        // 新しいY位置を計算
        currentYPosition += adjustedDeltaY;
        currentYPosition = Mathf.Clamp(currentYPosition, bottomLimit.anchoredPosition.y, topLimit.anchoredPosition.y);

        // ハンドルを新しい位置に移動
        handle.anchoredPosition = new Vector2(handle.anchoredPosition.x, currentYPosition);

        // 新しいY座標を記録して次のドラッグに備える
        initialMousePosition = eventData.position;

        // スライダーの位置に基づいて対象の位置を更新
        UpdateTargetRectPosition(currentYPosition);
    }

    void Update()
    {
        // マウスホイールでスライダーを動かす
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            currentYPosition += scroll * scrollSpeed * 10f;
            currentYPosition = Mathf.Clamp(currentYPosition, bottomLimit.anchoredPosition.y, topLimit.anchoredPosition.y);
            HandleSliderMovement(currentYPosition);
        }
    }

    // スライダーの位置を動かす
    private void HandleSliderMovement(float yPosition)
    {
        handle.anchoredPosition = new Vector2(handle.anchoredPosition.x, yPosition);

        // スライダーの位置に基づいて対象の位置を更新
        UpdateTargetRectPosition(yPosition);
    }

    // 対象の RectTransform をスライダーの位置に基づいて動かす
    private void UpdateTargetRectPosition(float clampedY)
    {
        // スライダーの範囲内での割合を計算
        float normalizedSliderPosition = (clampedY - bottomLimit.anchoredPosition.y) / (topLimit.anchoredPosition.y - bottomLimit.anchoredPosition.y);

        // 対象の RectTransform の範囲を設定
        float targetMinY = targetBottomLimit.anchoredPosition.y;
        float targetMaxY = targetTopLimit.anchoredPosition.y;

        // 対象の Y 座標をスライダーの割合に基づいて動かす
        float targetY = Mathf.Lerp(targetMinY, targetMaxY, normalizedSliderPosition);
        targetRect.anchoredPosition = new Vector2(targetRect.anchoredPosition.x, targetY);
    }
}
