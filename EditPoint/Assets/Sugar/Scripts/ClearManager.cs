using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���̃X�N���v�g�ł̓N���A�L�����o�X��
// �^�C�~���O���w�肷��
public class ClearManager : MonoBehaviour
{
    #region field
    // �N���X
    ClassUIAnim UAnim;

    // �t�F�[�h��������
    [SerializeField]
    Image ClearPanel;

    [SerializeField]
    RectTransform BtnGroup;

    // �����̍��W�l
    [SerializeField]
    Vector2 startPos = new Vector2(0, 0);

    // �ڕW�̍��W
    [SerializeField]
    Vector2 TargetPos = new Vector2(0, 0);

    [SerializeField]
    GameObject panel;

    float sizeA=0;

    // ���W�ύX���̑��x
    [SerializeField]
    float spdX = 0, spdY = 0;

    // �t�F�[�h���x
    [SerializeField]
    float Fadespd=0.1f;

    // �����̏��������邽��
    int num = 0;
    #endregion
    void Start()
    {
        // �C���X�^���X����
        UAnim = new ClassUIAnim();
    }

    void Update()
    {
        UICONTROLL();
    }
    void UICONTROLL()
    {
        switch (num)
        {
            case 0:
                // �������W��
                BtnGroup.anchoredPosition = startPos;
                num++;
                break;
            case 1:
                if (ClearPanel.color.a < 0.9f)
                {
                    ClearPanel = UAnim.anim_Fade_I(ClearPanel, Fadespd);
                    sizeA += 200;
                    ClearPanel.rectTransform.sizeDelta = new Vector2(sizeA,sizeA);
                }
                else
                    num++;
                break;
            case 2:
                if (BtnGroup.anchoredPosition.y <= TargetPos.y)
                    BtnGroup = UAnim.anim_PosChange(BtnGroup, spdX, spdY);
                else
                {
                    BtnGroup.anchoredPosition = TargetPos;
                    Destroy(panel);
                    num++;
                }
                break;
        }

    }
}
