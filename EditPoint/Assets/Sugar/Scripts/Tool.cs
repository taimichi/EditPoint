using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tool : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region field
    /*
      �c�[���{�b�N�X�̕\����\���ɍ��킹�ăT�C�Y�ύX
    */
    // Pos
    const float posYDisp = 0;
    const float posYHide = 400;
    // Height
    const float heightDisp = 900;
    const float heightHide = 50;

    // �\����Ԃł���\���ł��Œ�
    const float posX = 0;
    const float width = 400;

    // ����ŕ\����\���̔���
    [SerializeField] Toggle toggle;

    // �Ώۂ�Rect(�c�[���o�[)
    [SerializeField] RectTransform rctTool;
    [SerializeField] RectTransform hitBox;

    // �}�E�X���W�Ɉړ�����
    [SerializeField] RectTransform rctGroup;

    // ���̑��\����Ԃ�ς���Obj
    [SerializeField] GameObject[] obj;

    // Canvas
    [SerializeField] Canvas canvas;

    // �}�E�X�̃X�N���[�����W���擾
    Vector3 mouseScreenPos;

    // UI�ɐG�ꂽ��
    private bool isCheck = false;

    // Canvas���W�����߂�̂Ɏg��
    Vector2 localPoint;
    #endregion


    void Update()
    {
        DispOrHide();

        // �}�E�X���W�����߂� 
        mouseScreenPos = Input.mousePosition;

        // UI�𓮂�������
        if (isCheck)
        {
            if (Input.GetMouseButton(0))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform, mouseScreenPos, canvas.worldCamera, out localPoint);

                rctGroup.anchoredPosition = localPoint+new Vector2(0,-posYHide);
            }
        }
    }

    #region Method
    #region Interface
    // UI��ɃJ�[�\�����G��Ă��邩
    public void OnPointerEnter(PointerEventData eventData)
    {
        isCheck = true;
    }

    // UI���痣�ꂽ�ꍇ
    public void OnPointerExit(PointerEventData eventData)
    {
        isCheck = false;
    }
    #endregion

    /// <summary>
    /// Toggle�Ƀ`�F�b�N�������Ă��邩���`�F�b�N
    /// </summary>
    private bool IsOn()
    {
        return toggle.isOn;
    }

    /// <summary>
    /// Rect�̃T�C�Y�ύX���s������
    /// </summary>
    private void DispOrHide()
    {
        if(IsOn()) // �\�����
        {
            rctTool.anchoredPosition = new Vector2(posX, posYDisp);
            rctTool.sizeDelta = new Vector2(width,heightDisp);

            hitBox.anchoredPosition = new Vector2(posX, posYDisp);
            hitBox.sizeDelta = new Vector2(width, heightDisp);

            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].SetActive(true);
            }
        }
        else // ��\�����
        {
            rctTool.anchoredPosition = new Vector2(posX, posYHide);
            rctTool.sizeDelta = new Vector2(width, heightHide);

            hitBox.anchoredPosition = new Vector2(posX, posYHide);
            hitBox.sizeDelta = new Vector2(width, heightHide);
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].SetActive(false);
            }
        }
    }
    #endregion
}
