using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGroup : MonoBehaviour
{
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

    // �A�j���[�V�������
    int state = 3;
   
    // �N���X
    ClassUIAnim UAnim;

    bool click = false;
    void Start()
    {
        // �C���X�^���X����
        UAnim = new ClassUIAnim();
    }

    private void Update()
    {
        if (click)
        {
            UICONTROLL();
        }
        Debug.Log(state);
    }

    void UICONTROLL()
    {
        switch (state)
        {
            case 1: // �O���[�v��\��
                if (TargetRct.anchoredPosition.y <= startPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, -spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = startPos;
                    state=10; //�@�����̏I��
                }
                break;
            case 3: // �O���[�v�\��
                if (TargetRct.anchoredPosition.y >= TargetPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    state=10; //�@�����̏I��
                }
                break;
            case 10:
                click = false;
                state = TargetRct.anchoredPosition == startPos ? 3 : 1;
                break;
        }

    }

    public void BUTTONCHECK()
    {
        click = true;
    }
}
