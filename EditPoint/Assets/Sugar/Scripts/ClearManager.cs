using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���̃X�N���v�g�ł̓N���A�L�����o�X��
// �^�C�~���O���w�肷��
public class ClearManager : MonoBehaviour
{
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

    // ���W�ύX���̑��x
    [SerializeField]
    float spdX = 0, spdY = 0;

    // �t�F�[�h���x
    [SerializeField]
    float Fadespd=0.1f;

    int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        // �C���X�^���X����
        UAnim = new ClassUIAnim();
    }

    // Update is called once per frame
    void Update()
    {
        UICONTROLL();
    }
    void UICONTROLL()
    {
        switch (state)
        {
            case 0:
                // �������W��
                BtnGroup.anchoredPosition = startPos;
                state++;
                break;
            case 1:
                if (ClearPanel.color.a < 0.5f)
                    ClearPanel = UAnim.anim_Fade_I(ClearPanel, Fadespd);
                else
                    state++;
                break;
            case 2:
                if (BtnGroup.anchoredPosition.x >= TargetPos.x)
                    BtnGroup = UAnim.anim_PosChange(BtnGroup, spdX, spdY);
                else
                {
                    BtnGroup.anchoredPosition = TargetPos;
                    Destroy(panel);
                    state++;
                }
                break;
        }

    }
}
