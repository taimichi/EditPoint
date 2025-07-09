using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixeye.Unity;

public class QuickGuideMenu : MonoBehaviour
{
    //�`���[�g���A���p�摜�������Ă���X�N���v�^�u���I�u�W�F�N�g
    [SerializeField] private GuideSpriteListData guideSprite;

    [SerializeField] private GameObject QuickGuideObj;
    [SerializeField] private GameObject GuideImage;
    private Image GuideSprite;

    [SerializeField] private GameObject LButton;        //�����{�^��
    [SerializeField] private GameObject RButton;        //�E���{�^��

    private Sprite[] sprites;   //�\���p�X�v���C�g�z��
    private int nowPage = 0;    //���݂̃y�[�W��

    private PlaySound playSound;

    private void Awake()
    {
        GuideSprite = GuideImage.GetComponent<Image>();
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
    }

    /// <summary>
    /// ���E�{�^�����g�p�ł���悤�ɂ���
    /// </summary>
    private void SetLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(true);
    }

    /// <summary>
    /// ���E�{�^�����\���ɂ���
    /// </summary>
    private void CloseLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(false);
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
        nowPage++;
        if(nowPage >= sprites.Length - 1)
        {
            RButton.SetActive(false);
        }
        LButton.SetActive(true);

        GuideSprite.sprite = sprites[nowPage];
    }

    /// <summary>
    /// �������������Ƃ�
    /// </summary>
    public void OnLeftButton()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        nowPage--;
        if(nowPage <= 0)
        {
            LButton.SetActive(false);
        }
        RButton.SetActive(true);

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
            case "Clip":
                sprites = guideSprite.GuideSprites[GuideSpriteListData.GUIDE.clip].GuideSprites;
                SetLRButton();
                break;

            case "Block":
                sprites = guideSprite.GuideSprites[GuideSpriteListData.GUIDE.blockGene].GuideSprites ;
                break;

            case "Blower":
                sprites = guideSprite.GuideSprites[GuideSpriteListData.GUIDE.blower].GuideSprites;
                break;

            case "Copy":
                sprites = guideSprite.GuideSprites[GuideSpriteListData.GUIDE.copy].GuideSprites;
                break;

            case "Move":
                sprites = guideSprite.GuideSprites[GuideSpriteListData.GUIDE.move].GuideSprites;
                SetLRButton();
                break;

            case "MoveGround":
                sprites = guideSprite.GuideSprites[GuideSpriteListData.GUIDE.moveGround].GuideSprites;
                SetLRButton();
                break;

            case "Card":
                sprites = guideSprite.GuideSprites[GuideSpriteListData.GUIDE.card].GuideSprites;
                break;

            case "Cut":
                sprites = guideSprite.GuideSprites[GuideSpriteListData.GUIDE.cut].GuideSprites;
                break;
        }

        GuideSprite.sprite = sprites[nowPage];
    }

    public bool IsCheckActive() => QuickGuideObj.activeSelf;
}
