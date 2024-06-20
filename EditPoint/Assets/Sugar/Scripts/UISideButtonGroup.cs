using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISideButtonGroup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    // �N���X
    ClassUIAnim UAnim;

    // �A�j���[�V�����̏��
    [SerializeField] // �f�o�b�O�p�ɐG���悤�ɂ���
    int state = 0;

    private void Start()
    {
        UAnim = new ClassUIAnim();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        state = 1;
        Debug.Log("�G�ꂽ");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        state = 3;
        Debug.Log("���ꂽ");
    }

    void Update()
    {
        anim();
    }

    void anim()
    {
        switch (state)
        {
            case 0: //������
                Setup();
                break;

            case 1:
                if (TargetRct.anchoredPosition.x >= TargetPos.x)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    state++; //�@�����̏I��
                }
                break;

            case 3:
                if (TargetRct.anchoredPosition.x <= startPos.x)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, -spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = startPos;
                    state++;
                }
                break;

        }
    }
    void Setup()
    {
        TargetRct = UAnim.anim_Start(TargetRct, startPos);
    }
}
