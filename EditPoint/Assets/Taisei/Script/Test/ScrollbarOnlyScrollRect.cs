using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollbarOnlyScrollRect : ScrollRect
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        // �X�N���[���o�[��ł̂݃X�N���[���ł���悤�ɂ���
        if (IsPointerOverScrollbar(eventData))
        {
            base.OnBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (IsPointerOverScrollbar(eventData))
        {
            base.OnDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (IsPointerOverScrollbar(eventData))
        {
            base.OnEndDrag(eventData);
        }
    }

    private bool IsPointerOverScrollbar(PointerEventData eventData)
    {
        // �X�N���[���o�[�ォ�ǂ������m�F
        return eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Scrollbar>() != null;
    }
}
