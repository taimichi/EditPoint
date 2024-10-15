using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region field
    [Header("�J�[�\�������킹�����ɕ\������p�l��"),SerializeField] GameObject panel;
    #endregion
    void Start()
    {
        panel.SetActive(false);
    }

    #region Interface
    // UI��ɃJ�[�\�����G��Ă��邩
    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
    }

    // UI���痣�ꂽ�ꍇ
    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
    #endregion
}
