using UnityEngine;
using UnityEngine.EventSystems;

public class SelectManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform slotContainer; // �X���b�g�̐e�I�u�W�F�N�g
    public float slotWidth = 100f; // �X���b�g1�̕�
    public int visibleSlotCount = 5; // �\�������X���b�g��

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
        // �q�I�u�W�F�N�g���`�F�b�N���āA�\���O�ɏo���X���b�g�𔽑Α��ɍĔz�u
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
        // ���݂̈ʒu����ł��߂��X���b�g�ɃX�i�b�v����
        float nearestSlotX = Mathf.Round(slotContainer.anchoredPosition.x / slotWidth) * slotWidth;
        slotContainer.anchoredPosition = new Vector2(nearestSlotX, slotContainer.anchoredPosition.y);
    }
}
