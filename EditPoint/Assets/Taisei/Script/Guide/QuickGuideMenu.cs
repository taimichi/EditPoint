using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixeye.Unity;

//�X�e�[�W���߂ɕ\�������`���[�g���A���p
public class QuickGuideMenu : MonoBehaviour
{
    //�`���[�g���A���p�摜�������Ă���X�N���v�^�u���I�u�W�F�N�g
    [SerializeField] private GuideSpriteListData guideSprite;

    [SerializeField] private GameObject QuickGuideObj;
    [SerializeField] private GameObject GuideImage;
    private Image GuideSprite;

    [SerializeField] private GameObject LButton;        //�����{�^��
    [SerializeField] private GameObject RButton;        //�E���{�^��
    [SerializeField] private GameObject CloseButton;    //����{�^��

    private Sprite[] sprites;   //�\���p�X�v���C�g�z��
    private int nowPage = 0;    //���݂̃y�[�W��

    private PlaySound playSound;

    private void Awake()
    {
        GuideSprite = GuideImage.GetComponent<Image>();
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        //����{�^�����\��
        CloseButton.SetActive(false);
    }

    /// <summary>
    /// ���E�{�^�����g�p�ł���悤�ɂ���
    /// </summary>
    private void SetLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(true);

        //����{�^�����\��
        CloseButton.SetActive(false);
    }

    /// <summary>
    /// ���E�{�^�����g�p�s�ɂ���
    /// </summary>
    private void CloseLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(false);

        //����{�^����\��
        CloseButton.SetActive(true);
    }

    /// <summary>
    /// �K�C�h�����
    /// </summary>
    public void OnClose()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.cancell);
        nowPage = 0;
        CloseLRButton();
        GuideImage.SetActive(false);
        QuickGuideObj.SetActive(false);
    }

    /// <summary>
    /// �E���{�^������������
    /// </summary>
    public void OnRightButton()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        //�y�[�W����ύX
        nowPage++;
        //���݂̃y�[�W�����ő吔�ɂȂ����Ƃ�
        if(nowPage >= sprites.Length - 1)
        {
            RButton.SetActive(false);
            //����{�^���\��
            CloseButton.SetActive(true);
        }
        LButton.SetActive(true);

        //�`���[�g���A���摜�X�V
        GuideSprite.sprite = sprites[nowPage];
    }

    /// <summary>
    /// �������������Ƃ�
    /// </summary>
    public void OnLeftButton()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        //�y�[�W�����X�V
        nowPage--;
        //���݂̃y�[�W�����ŏ��l�ɂȂ����Ƃ�
        if(nowPage <= 0)
        {
            LButton.SetActive(false);
        }
        RButton.SetActive(true);

        //����{�^�����\����Ԃ�������
        if (CloseButton.activeSelf)
        {
            //����{�^�����\����
            CloseButton.SetActive(false);
        }

        //�`���[�g���A���摜�X�V
        GuideSprite.sprite = sprites[nowPage];
    }

    /// <summary>
    /// �ŏ��ɑ�����@��\������
    /// </summary>
    /// <param name="_key">�\��������������@</param>
    public void StartGuide(string guideName)
    {
        nowPage = 0;
        QuickGuideObj.SetActive(true);
        GuideImage.SetActive(true);
        CloseLRButton();

        switch (guideName)
        {
            //�e���v���[�g
            //case "string(�\��������������@�̖��O)":
            //    �摜�\��
            //    sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.xxx].GuideSprites;
            //    break;

            //�N���b�v
            case "Clip":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.clip].GuideSprites;
                SetLRButton();
                break;

            //�u���b�N����
            case "Block":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.blockGene].GuideSprites ;
                break;

            //�����@
            case "Blower":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.blower].GuideSprites;
                break;

            //�R�s�[���y�[�X�g
            case "Copy":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.copy].GuideSprites;
                break;

            //���̈ړ��E�g�k�E��]
            case "Move":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.move].GuideSprites;
                break;
                
            //������
            case "MoveGround":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.moveGround].GuideSprites;
                break;

            //�J�[�h�L�[
            case "Card":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.card].GuideSprites;
                break;

            //�J�b�g
            case "Cut":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.cut].GuideSprites;
                break;
        }

        //�摜������2���ȏ�̎�
        if(sprites.Length >= 2)
        {
            //���E�{�^�����g�p�\��
            SetLRButton();
        }

        //�`���[�g���A���摜���Z�b�g
        GuideSprite.sprite = sprites[nowPage];
    }
}
