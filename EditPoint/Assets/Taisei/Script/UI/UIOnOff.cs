using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIOnOff : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject ToolName;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolName.SetActive(true); // マウスカーソルがUIオブジェクト上にある時、有効化
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolName.SetActive(false); // マウスカーソルがUIオブジェクトから離れた時、無効化
    }
}
