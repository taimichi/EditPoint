using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Test_UIAnim : MonoBehaviour
{
    // �������^�[�Q�b�gUI
    [SerializeField]
    RectTransform TargetRct;
    [SerializeField]
    Transform tf;

    // �����̍��W�l
    [SerializeField]
    Vector2 startPos = new Vector2(0, 0);
    // �ڕW�̍��W
    [SerializeField]
    Vector2 TargetPos = new Vector2(0, 0);
    [SerializeField]
    Vector3 startRot = new Vector3(0,0,0);
    // ��]�l
    [SerializeField]
    float rotX, rotY, rotZ = 0;



    // ���W�ύX���̑��x
    [SerializeField]
    float spdX=0, spdY=0;

    // UI�̕��ƍ���
    [SerializeField]
    float Width=1000, Height=400;
    // �X�P�[���̃T�C�Y�ύX���̑��x
    [SerializeField]
    float sclX = 0, sclY = 0;

    // �t�F�[�h�̎���
    [SerializeField]
    Image ImgCol;
    [SerializeField]
    Text TextCol;
    // �t�F�[�h���x
    [SerializeField]
    float FadeSpd = 0.05f;
    // �A�j���[�V�����̏��
    [SerializeField] // �f�o�b�O�p�ɐG���悤�ɂ���
    int state = 0;

    // �\������
    [SerializeField]
    float _time = 5f;
    float Settime;
    // �N���X
    ClassUIAnim UAnim;
    void Start()
    {
        // �C���X�^���X����
        UAnim = new ClassUIAnim();
        Settime = _time;
    }

    void Update()
    {
        // �A�j���[�V�������s
        anim();
    }

    void anim()
    {
// case 0-2�܂ł��e�L�X�g�{�b�N�X�Ɏg���\��̃A�j���[�V����
        switch (state)
        {
            case 0: // ������
                Setup();
                break;
            case 1: // �ړ�
                if (TargetRct.anchoredPosition.y >= TargetPos.y)
                {�@
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    state++;
                }
                break;
            case 2: // �g��
                if (TargetRct.sizeDelta.x <= Width)
                {
                    TargetRct = UAnim.anim_SclChange(TargetRct, sclX, sclY);
                }
                else
                {
                    TargetRct.sizeDelta = new Vector2(Width,Height);
                   
                    state++;
                }
                break;

            case 3: // �������s���Ȃ��B����𕪂��Ă邾���ł�
                break;

// case4-6�܂ŉ�ʏ㕔��UI���҂傱��Əo�Ă���
            case 4: // ������
                Setup();
                break;
            case 5: // ���UI�{�^�����\�������
                if (TargetRct.anchoredPosition.y >= TargetPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    state++;
                }
                break;
                
            case 6: // �������s���Ȃ��B����𕪂��Ă邾���ł�
                break;

// case7-9�܂ŃC���[�W�ƃe�L�X�g�̓���g�����t�F�[�h�ōŏ��ɓ���
            case 7:
                Setup();
                break;
            case 8:
                if (TargetRct.anchoredPosition.x >= TargetPos.x)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    state++;
                }
                break;
            case 9:
                TimeCount();
                break;
            case 10: // �t�F�[�h�A�E�g�J�n
                if (TextCol.color.a >= 0.0f)
                {
                    TextCol = UAnim.anim_Fade_T(TextCol, -FadeSpd);
                    ImgCol = UAnim.anim_Fade_I(ImgCol, -FadeSpd);
                }
                else
                {
                    state++;
                }
                break;

            case 11:
                break;

// case11-13�܂ŃV���v���ȃt�F�[�h
            case 12: // �t�F�[�h�C���J�n
                if (TextCol.color.a <= 1.0f)
                {
                    TextCol = UAnim.anim_Fade_T(TextCol, FadeSpd);
                    ImgCol = UAnim.anim_Fade_I(ImgCol, FadeSpd);
                }
                else 
                {
                    state++;
                }
                break;
            case 13: // �\�����Ă��
                TimeCount();
                break;
            case 14:
                if (TextCol.color.a >= 0.0f)
                {
                    TextCol = UAnim.anim_Fade_T(TextCol, -FadeSpd);
                    ImgCol = UAnim.anim_Fade_I(ImgCol, -FadeSpd);
                }
                else
                {
                    state++;
                }
                break;

            case 15:
                break;

            case 16:
                // ��]�����l
                tf.rotation = Quaternion.Euler(startRot.x, startRot.y, startRot.z);
                state++;
                break;
            case 17:
                if (startRot.z <= 0)
                {
                    startRot.z += rotZ;
                    UAnim.T_anim_rotation(tf, startRot.x, startRot.y, startRot.z);
                }
                else
                {
                    state++;
                }
                break;

            default:
                break;
        }
    }
    void Setup()
    {
        TargetRct = UAnim.anim_Start(TargetRct, startPos);
        state++;
    }
    void TimeCount()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            state++;
            _time = Settime;
        }
    }
}
