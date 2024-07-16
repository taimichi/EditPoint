using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class StageSelect : MonoBehaviour
{
    // FadeCanvas
    [SerializeField] 
    Fade fade;          

    // �������Ώ�
    [SerializeField] 
    RectTransform[] targetRct;

    // Rect�z��Ɏg���ԍ�
    int rctState = 0;

    // �z��̍ő�l�ƍŏ��l
    int max;
    int min;

    // Rect���ړ��I���܂Ń{�^�����@�\�����Ȃ�
    bool Click = true;
    
    // �����̍��W�l
    [SerializeField]
    Vector2 CenterStartPos = new Vector2(0, 0);
    [SerializeField]
    Vector2 LeftStartPos = new Vector2(0, 0);
    [SerializeField]
    Vector2 RightStartPos = new Vector2(0, 0);

    // �ڕW�̍��W
    [SerializeField]
    Vector2 CenterTargetPos = new Vector2(0, 0);
    [SerializeField]
    Vector2 LeftTargetPos = new Vector2(0, 0);
    [SerializeField]
    Vector2 RighttargetPos = new Vector2(0, 0);

    // ���W�ύX���̑��x
    [SerializeField]
    float spdX = 0, spdY = 0;

    // �A�j���[�V�������
    int state = 100;

    // �N���X
    ClassUIAnim UAnim;

    // Text���y�[�W��������₷��
    [SerializeField] Text page;

    int Nowpage_INT=1;
    
    string ALLpage;
    string NOWpage;

    void Start()
    {
        max = targetRct.Length - 1;
        min = 0;

        // �y�[�W�̍ő吔
        ALLpage = targetRct.Length.ToString();

        UAnim = new ClassUIAnim();
    }

    // Update is called once per frame
    void Update()
    {
        // ���݂̃y�[�W
        // rctState�͔z��Ŏg�����ߐ������[�P�����̂Ł{�P����
        NOWpage = Nowpage_INT.ToString();

        // �e�L�X�g�ɔ��f
        page.text = NOWpage + "/" + ALLpage;

        UICon();
    }

    // Rect�𓮂�������
    void UICon()
    {
        switch (state)
        {
            case 0:
                if (targetRct[rctState].anchoredPosition.x >= CenterStartPos.x)
                {
                    targetRct[rctState] = UAnim.anim_PosChange(targetRct[rctState], -spdX, spdY);
                    if (rctState == max) 
                    {
                        targetRct[min] = UAnim.anim_PosChange(targetRct[min], -spdX, spdY);
                    }
                    else
                    {
                        targetRct[rctState+1] = UAnim.anim_PosChange(targetRct[rctState+1], -spdX, spdY);
                    }
                }
                else
                {
                    targetRct[rctState].anchoredPosition = CenterStartPos;
                    Click = true;
                    state++;
                }
                break;
            case 10:
                if (targetRct[rctState].anchoredPosition.x <= CenterStartPos.x)
                {
                    targetRct[rctState] = UAnim.anim_PosChange(targetRct[rctState], spdX, spdY);
                    if (rctState == min)
                    {
                        targetRct[max] = UAnim.anim_PosChange(targetRct[max], spdX, spdY);
                    }
                    else
                    {
                        targetRct[rctState-1] = UAnim.anim_PosChange(targetRct[rctState - 1], spdX, spdY);
                    }
                }
                else
                {
                    targetRct[rctState].anchoredPosition = CenterStartPos;
                    Click = true;
                    state++;
                }
                break;
        }
    }    

    // ���{�^��
    public void LButton()
    {
        // �ړ����Ȃ�return
        if (!Click) { return; }

        // �ő�l���z���Ȃ��悤��
        if (rctState == min)
        {
            rctState = max;
            Nowpage_INT = targetRct.Length;
        }
        else
        {
            rctState--;
            Nowpage_INT--;
        }

        // �A�j���[�V��������
        state = 0;

        // �����������E�������W�Ɉړ�
        targetRct[rctState].anchoredPosition = RightStartPos;

        Click = false;
    }

    // �E�{�^��
    public void RButton()
    {
        // �ړ����Ȃ�return
        if (!Click) { return; }

        // �������Ώ�Rect�̌v�Z
        if (rctState == max)
        {
            rctState = min;
            Nowpage_INT = 1;
        }
        else
        {
            rctState++;
            Nowpage_INT++;
        }

        // �A�j���[�V��������
        state = 10;

        // �������������������W�Ɉړ�
        targetRct[rctState].anchoredPosition = LeftStartPos;

        Click = false;
    }

    // ��������X�e�[�W�{�^��

    // eventSystem�^�̕ϐ���錾�@�C���X�y�N�^�[��EventSystem���A�^�b�`���Ď擾���Ă���
    [SerializeField] private EventSystem eventSystem;

    //GameObject�^�̕ϐ���錾�@�{�^���I�u�W�F�N�g�����锠
    private GameObject button_ob;

    //GameObject�^�̕ϐ���錾�@�e�L�X�g�I�u�W�F�N�g�����锠
    private GameObject NameText_ob;

    //Text�^�̕ϐ���錾�@�e�L�X�g�R���|�[�l���g�����锠
    private Text name_text;

    public void StageButton(string Scenename)
    {
        button_ob = eventSystem.currentSelectedGameObject;

        //�{�^���̎q�̃e�L�X�g�I�u�W�F�N�g�𖼑O�w��Ŏ擾 ���̏ꍇText100�Ɩ��O���t���Ă���e�L�X�g�I�u�W�F�N�g��T��
        NameText_ob = button_ob.transform.Find("Text").gameObject;

        //�e�L�X�g�I�u�W�F�N�g�̃e�L�X�g���擾
        name_text = NameText_ob.GetComponent<Text>();

        if (name_text.text == "Rock") { return; }
        // �t�F�[�h
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene(Scenename);
        });
    }
}
