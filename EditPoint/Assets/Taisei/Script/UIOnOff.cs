using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIOnOff : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject ToolName;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolName.SetActive(true); // �}�E�X�J�[�\����UI�I�u�W�F�N�g��ɂ��鎞�A�L����
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolName.SetActive(false); // �}�E�X�J�[�\����UI�I�u�W�F�N�g���痣�ꂽ���A������
    }
}
