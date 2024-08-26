using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelect : MonoBehaviour
{
    // Rect�𓮂������̔��f�p
    enum RectMove
    {
        rightFeed, // �E���菈��
        leftFeed,  // �����菈��
        etc        // �I������
    }

// �ϐ�
#region variable
    // �C���X�y�N�^�[���Őݒ肷�����
#region Inspector
    // FadeCanvas
    // Text�ō����y�[�W��������₷������
    [SerializeField] 
    Text page;

    [SerializeField]
    Fade fade;

    // �������Ώ�
    [SerializeField]
    RectTransform[] targetRct;

    // �����̍��W�l
    // �����A���A�E
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

    // eventSystem�^�̕ϐ���錾�@�C���X�y�N�^�[��EventSystem���A�^�b�`���Ď擾���Ă���
    [SerializeField] private EventSystem eventSystem;
#endregion

    // Rect�z��Ɏg���ԍ�
    int rctNum = 0;

    // �z��̍ő�l�ƍŏ��l
    int max;
    int min;
    // �y�[�W�̌v�Z�Ɏg���ő�ƍŏ��̒l
    int pMax;
    int pMin;

    // ���݂̃y�[�W���J�E���g�p
    int pageCount = 1;

    // Rect���ړ��I���܂Ń{�^�����@�\�����Ȃ�
    bool isClick = true;
  
    // �S�̃y�[�W�ƌ��݃y�[�W�̕���
    string allPage;
    string nowPage;

    /// <summary>
    /// enum�ϐ�
    /// </summary>
    RectMove rMove;

    // UI�𓮂����N���X
    /// <summary>
    /// �T�C�Y�ύX��|�W�V�����ړ����̏����̃��\�b�h������N���X
    /// </summary>
    ClassUIAnim UAnim;

    // �{�^���I�u�W�F�N�g�����锠
    private GameObject button_ob;

    // �e�L�X�g�I�u�W�F�N�g�����锠
    private GameObject NameText_ob;

    // �e�L�X�g�R���|�[�l���g�����锠
    private Text name_text;

#endregion
// ���\�b�h
#region Method
    void Start()
    {
        // �y�[�W�̌v�Z�p
        pMax = targetRct.Length;
        pMin = targetRct.Length / targetRct.Length;

        // �z��Ɏg�������̂ŗv�f������l��-1
        max = targetRct.Length - 1;
        min = 0;

        // �y�[�W�̍ő吔
        allPage = targetRct.Length.ToString();

        // �C���X�^���X����
        UAnim = new ClassUIAnim();
    }

    void Update()
    {
        // ���݂̃y�[�W�J�E���g����string�ɕϊ�
        nowPage = pageCount.ToString();

        // �e�L�X�g�ɔ��f
        page.text = nowPage + "/" + allPage;

        // Rect����
        UICon();
    }

    /// <summary>
    /// Rect�𓮂�������
    /// </summary>
    void UICon()
    {
        switch (rMove)
        {
            case RectMove.rightFeed: // �E����

                // �������Ώۂ�Rect�ƖڕW���W�i�Z���^�[�j���r
                if (targetRct[rctNum].anchoredPosition.x >= CenterStartPos.x)
                {
                    // �������Ώۂ�Rect��ڕW���W�i�Z���^�[�j�ɋ߂Â���
                    UAnim.anim_PosChange(targetRct[rctNum], -spdX, spdY);

                    // �Z���^�[�ɂ��łɂ���UI���J�����̊O��
                    // �Z���^�[�ɓ������\���Rct�z��̎��̗v�f�𓮂���
                    // �ő�͒����Ȃ��悤��
                    if (rctNum == min) 
                    {
                        UAnim.anim_PosChange(targetRct[max], -spdX, spdY);
                    }
                    else
                    {
                        UAnim.anim_PosChange(targetRct[rctNum-1], -spdX, spdY);
                    }
                }
                // �������I�������w��̏ꏊ�ɂ��ꂪ�Ȃ��悤�ɃZ�b�g
                else
                {
                    targetRct[rctNum].anchoredPosition = CenterStartPos;

                    // �ړ����I������̂Ŏ��̃{�^�����͂��󂯕t����
                    isClick = true;

                    // �����I��
                    rMove = RectMove.etc;
                }
                break;
            case RectMove.leftFeed: // ������

                // �������Ώۂ�Rect�ƖڕW���W�i�Z���^�[�j���r
                if (targetRct[rctNum].anchoredPosition.x <= CenterStartPos.x)
                {
                    // �������Ώۂ�Rect��ڕW���W�i�Z���^�[�j�ɋ߂Â���
                    UAnim.anim_PosChange(targetRct[rctNum], spdX, spdY);

                    // �Z���^�[�ɂ��łɂ���UI���J�����̊O��
                    // �Z���^�[�ɓ������\���Rct�z��̈�O�̗v�f�𓮂���
                    // �ő�͒����Ȃ��悤��
                    if (rctNum == max)
                    {
                        UAnim.anim_PosChange(targetRct[min], spdX, spdY);
                    }
                    else
                    {
                        UAnim.anim_PosChange(targetRct[rctNum + 1], spdX, spdY);
                    }
                }
                else
                {
                    targetRct[rctNum].anchoredPosition = CenterStartPos;

                    // �ړ����I������̂Ŏ��̃{�^�����͂��󂯕t����
                    isClick = true;

                    // �����I��
                    rMove = RectMove.etc;
                }
                break;
        }
    }

    /// <summary>
    /// �����{�^���̃��\�b�h
    /// </summary>
    public void LButton()
    {
        // �ړ����Ȃ�return
        if (!isClick) { return; }
        // �ő�l���z���Ȃ��悤��
        if (rctNum == min)
        {
            rctNum = max;
        }
        else
        {
            rctNum--;
        }

        // �z��̗v�f���𒴂��Ȃ��悤�ɂ���
        if (pageCount == pMin)
        {
            // �ŏ��l�ł���1�����߂�
            // min���Ɣz��p�ɂ��Ă���̂�0�ɂȂ��Ă��܂�����
            pageCount = pMax;
        }
        else
        {
            pageCount--;
        }
        
        // �����������E�������W�Ɉړ�
        targetRct[rctNum].anchoredPosition = LeftStartPos;

        // �A�j���[�V��������
        rMove = RectMove.leftFeed;

        // �ړ����
        isClick = false;
    }
    
    /// <summary>
    /// �E���{�^���̃��\�b�h
    /// </summary>
    public void RButton()
    {
        // �ړ����Ȃ�return
        if (!isClick) { return; }


        // �������Ώ�Rect�̌v�Z
        if (rctNum == max)
        {
            rctNum = min;
        }
        else
        {
            rctNum++;

        }

        // �z��̗v�f���𒴂��Ȃ��悤�ɂ���
        if (pageCount==pMax)
        {
            // �ŏ��l�ł���1�����߂�
            // min���Ɣz��p�ɂ��Ă���̂�0�ɂȂ��Ă��܂�����
            pageCount = pMin;
        }
        else
        {
            pageCount++;
        }

        // �������������������W�Ɉړ�
        targetRct[rctNum].anchoredPosition = RightStartPos;

        // �A�j���[�V��������
        rMove = RectMove.rightFeed;

        // �ړ����
        isClick = false;
    }

    /// <summary>
    /// �X�e�[�W�ɑJ�ڂ���{�^���̃��\�b�h
    /// </summary>
    /// <param name="Scenename"> �J�ڐ�̃X�e�[�W��������</param>
    public void StageButton(string Scenename)
    {
        button_ob = eventSystem.currentSelectedGameObject;

        //�{�^���̎q�̃e�L�X�g�I�u�W�F�N�g�𖼑O�w��Ŏ擾 ���̏ꍇText100�Ɩ��O���t���Ă���e�L�X�g�I�u�W�F�N�g��T��
        NameText_ob = button_ob.transform.Find("Text").gameObject;

        //�e�L�X�g�I�u�W�F�N�g�̃e�L�X�g���擾
        name_text = NameText_ob.GetComponent<Text>();

        // �����X�e�[�W����Rock�Ȃ�V�[���J�ڂ����Ȃ�
        if (name_text.text == TypeName.Rock) { return; }

        // �t�F�[�h
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene(Scenename);
        });
    }
#endregion
}

// StageButton�Ŏg��
/// <summary>
/// ����̃X�e�[�W���ɑ΂��ăV�[���J�ڂ��Ȃ��悤�ɐ�������
/// </summary>
public static class TypeName
{
    public static string Rock = "Rock";
}
