using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region field
    [Header("カーソルを合わせた時に表示するパネル"),SerializeField] GameObject panel;
    #endregion
    void Start()
    {
        panel.SetActive(false);
    }

    #region Interface
    // UI上にカーソルが触れているか
    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
    }

    // UIから離れた場合
    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
    #endregion
}
