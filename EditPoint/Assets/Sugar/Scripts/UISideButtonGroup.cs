using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISideButtonGroup : MonoBehaviour//IPointerExitHandler //IPointerEnterHandler 
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
    int state;

    private void Start()
    {
        UAnim = new ClassUIAnim();
    }

    public int Statereceive
    {
        set
        {
            state = value;
        }
    }
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    state = 1;
    //    Debug.Log("�G�ꂽ");
    //}

    //public void OptionGroupButton()
    //{
    //    // ���ɖڕW�ɂ���Ȃ瓮�삵�Ȃ�
    //    if (TargetRct.anchoredPosition == TargetPos) { return; }

    //    state = 1;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    // UI�������ĂȂ��Ȃ瓮��Ȃ�
    //    if (TargetRct.anchoredPosition==startPos) { return; }
    //    state = 3;
    //    Debug.Log("���ꂽ");
    //}

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
                if (TargetRct.anchoredPosition.y >= TargetPos.y)
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
                if (TargetRct.anchoredPosition.y <= startPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, -spdX, -spdY);
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
