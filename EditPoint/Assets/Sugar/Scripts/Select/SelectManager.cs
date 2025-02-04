using UnityEngine;
using UnityEngine.EventSystems;

public class SelectManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform slotContainer; // スロットの親オブジェクト
    public float slotWidth = 100f; // スロット1つの幅
    public int visibleSlotCount = 5; // 表示されるスロット数

    private Vector2 initialPosition;
    private Vector2 lastMousePosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = slotContainer.anchoredPosition;
        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - lastMousePosition;
        slotContainer.anchoredPosition += new Vector2(delta.x, 0);
        lastMousePosition = eventData.position;

        UpdateSlotPositions();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapToNearestSlot();
    }

    private void UpdateSlotPositions()
    {
        // 子オブジェクトをチェックして、表示外に出たスロットを反対側に再配置
        for (int i = 0; i < slotContainer.childCount; i++)
        {
            RectTransform slot = slotContainer.GetChild(i) as RectTransform;
            float slotPositionX = slotContainer.anchoredPosition.x + slot.anchoredPosition.x;

            if (slotPositionX < -visibleSlotCount * slotWidth / 2f)
            {
                slot.anchoredPosition += new Vector2(slotWidth * slotContainer.childCount, 0);
            }
            else if (slotPositionX > visibleSlotCount * slotWidth / 2f)
            {
                slot.anchoredPosition -= new Vector2(slotWidth * slotContainer.childCount, 0);
            }
        }
    }

    private void SnapToNearestSlot()
    {
        // 現在の位置から最も近いスロットにスナップする
        float nearestSlotX = Mathf.Round(slotContainer.anchoredPosition.x / slotWidth) * slotWidth;
        slotContainer.anchoredPosition = new Vector2(nearestSlotX, slotContainer.anchoredPosition.y);
    }
}
