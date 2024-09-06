using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollbarOnlyScrollRect : ScrollRect
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        // スクロールバー上でのみスクロールできるようにする
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
        // スクロールバー上かどうかを確認
        return eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Scrollbar>() != null;
    }
}
