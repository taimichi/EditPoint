using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGroup : MonoBehaviour
{
    enum SetUI
    {
        display,
        hide,
        CHECK
    }

    // �������^�[�Q�b�gUI
    [SerializeField]
    RectTransform TargetRct;

    // �����̍��W�l
    [SerializeField]
    Vector2 startPos = new Vector2(0, 0);

    // �ڕW�̍��W
    [SerializeField]
    Vector2 TargetPos = new Vector2(0, 0);

    // ���W�ύX���̑��x
    [SerializeField]
    float spdX = 0, spdY = 0;

    // �Ώۂ�Rect�̈ʒu����\������̂���\���ɂ���̂��`�F�b�N����
    SetUI set =SetUI.CHECK ;
   
    // �N���X
    ClassUIAnim UAnim;

    bool isClick = false;
    void Start()
    {
        // �C���X�^���X����
        UAnim = new ClassUIAnim();

        set = TargetRct.anchoredPosition == startPos ? SetUI.hide : SetUI.display;
    }

    private void Update()
    {
        if (isClick)
        {
            UICONTROLL();
        }
    }

    void UICONTROLL()
    {
        switch (set)
        {
            case SetUI.display: // �O���[�v�\��
                if (TargetRct.anchoredPosition.y >= startPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, -spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = startPos;
                    set=SetUI.CHECK; //�@�����̏I��
                }
                break;
            case SetUI.hide: // �O���[�v��\��
                if (TargetRct.anchoredPosition.y <= TargetPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    set = SetUI.CHECK; //�@�����̏I��
                }
                break;
            case SetUI.CHECK:
                isClick = false;

                // ���Ƀ{�^���������ꂽ�Ƃ��ɕ\������̂�
                // �\��/��\���ɂ���̂��`�F�b�N���ăZ�b�g
                set = TargetRct.anchoredPosition == startPos ? SetUI.hide : SetUI.display;
                break;
        }

    }

    /// <summary>
    /// �{�^�����̓`�F�b�N
    /// </summary>
    public void BUTTONCHECK()
    {
        isClick = true;
    }
}
