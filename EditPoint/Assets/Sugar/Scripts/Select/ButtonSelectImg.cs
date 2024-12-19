using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;    // UI

public class ButtonSelectImg : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // �J�[�\�����G�ꂽ�炱�̃I�u�W�F�N�g��true
    [SerializeField] GameObject img;
    [SerializeField] GameObject fileObj;

    // UI��ɃJ�[�\�����G��Ă��邩
    public void OnPointerEnter(PointerEventData eventData)
    {
        // �\��
        img.SetActive(true);
    }
    // ���ꂽ�ꍇ
    public void OnPointerExit(PointerEventData eventData)
    {
        // ��\��
        img.SetActive(false);
    }

    void Update()
    {
        // �I�΂ꂽ��Ԃł̂�
        if(img.activeSelf)
        {
            // �N���b�N�����炻�̃t�@�C�����N��
            if (Input.GetMouseButtonDown(0))
            {
                fileObj.SetActive(true);
                img.SetActive(false);
            }
        }
    }
}
