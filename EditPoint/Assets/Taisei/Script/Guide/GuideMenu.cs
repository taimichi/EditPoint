using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixeye.Unity;

public class GuideMenu : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private GameObject GuideMenuObj;

    [SerializeField] private GameObject GuideImage;
    private Image GuideSprite;

    [SerializeField] private GameObject LButton;
    [SerializeField] private GameObject RButton;

    #region Sprite
    [Foldout("Sprite")] [SerializeField] private Sprite[] ClipGuide;
    [Foldout("Sprite")] [SerializeField] private Sprite[] BlockGeneGuide;
    [Foldout("Sprite")] [SerializeField] private Sprite[] CopyGuide;
    [Foldout("Sprite")] [SerializeField] private Sprite[] MoveGuide;
    [Foldout("Sprite")] [SerializeField] private Sprite[] DeleteGuide;
    [Foldout("Sprite")] [SerializeField] private Sprite[] TimelineGuide;
    [Foldout("Sprite")] [SerializeField] private Sprite[] ButtonGuide;
    [Foldout("Sprite")] [SerializeField] private Sprite[] BlowerGuide;
    [Foldout("Sprite")] [SerializeField] private Sprite[] MoveGroundGuide;
    [Foldout("Sprite")] [SerializeField] private Sprite[] CardGuide;
    [Foldout("Sprite")] [SerializeField] private Sprite[] CutGuide;
    #endregion

    private Sprite[] sprites;
    private int nowPage = 0;

    //���ݕ\�����Ă���y�[�W��
    [SerializeField] private Text PageNum;

    private PlaySound playSound;


    void Start()
    {
        GuideSprite = GuideImage.GetComponent<Image>();
        canvas = this.GetComponent<Canvas>();
        if(GameObject.Find("UICamera") != null)
        {
            canvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        }
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();


        CloseLRButton();
        GuideImage.SetActive(false);
        GuideMenuObj.SetActive(false);

        PageNum.enabled = false;
    }

    /// <summary>
    /// ���������ʂ����
    /// </summary>
    public void OnCloseGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.cancell);

        CloseLRButton();
        sprites = null;
        nowPage = 0;

        GuideImage.SetActive(false);
        GuideMenuObj.SetActive(false);
    }

    /// <summary>
    /// ����������j���[���J��
    /// </summary>
    public void OnOpenGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        GuideMenuObj.SetActive(true);
    }

    /// <summary>
    /// �����{�^�����������Ƃ�
    /// </summary>
    public void OnLButton()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        nowPage--;
        if (nowPage <= 0)
        {
            LButton.SetActive(false);
            nowPage = 0;
        }
        RButton.SetActive(true);

        GuideSprite.sprite = sprites[nowPage];

        PageNum.text = (nowPage + 1).ToString() + " / " + sprites.Length;
    }

    /// <summary>
    /// �E���{�^�����������Ƃ�
    /// </summary>
    public void OnRButton()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        nowPage++;
        if(nowPage >= sprites.Length -1)
        {
            RButton.SetActive(false);
            nowPage = sprites.Length - 1;
        }
        LButton.SetActive(true);

        GuideSprite.sprite = sprites[nowPage];

        PageNum.text = (nowPage + 1).ToString() + " / " + sprites.Length;
    }

    /// <summary>
    /// �N���b�v�������
    /// </summary>
    public void OnClipGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        sprites = ClipGuide;
        SetLRButton();
        SetImage();

    }

    /// <summary>
    /// �u���b�N�����������
    /// </summary>
    public void OnBlockGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        sprites = BlockGeneGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �����@�������
    /// </summary>
    public void OnBlowerGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        sprites = BlowerGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �R�s�y�������
    /// </summary>
    public void OnCopyGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        sprites = CopyGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �ړ��E�T�C�Y�E�p�x�ύX�������
    /// </summary>
    public void OnMoveGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        sprites = MoveGuide;
        SetImage();
        SetLRButton();

    }

    /// <summary>
    /// �{�^���������
    /// </summary>
    public void OnButtonGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        sprites = ButtonGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �폜�{�^������
    /// </summary>
    public void OnDeleteGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        sprites = DeleteGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �^�C�����C������
    /// </summary>
    public void OnTimelineGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        sprites = TimelineGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// ����������
    /// </summary>
    public void OnMoveGroundGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        sprites = MoveGroundGuide;
        SetImage();
        SetLRButton();

    }

    /// <summary>
    /// �J�[�h�L�[����
    /// </summary>
    public void OnCardGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        sprites = CardGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �J�b�g����
    /// </summary>
    public void OnCutGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        sprites = CutGuide;
        SetImage();
        CloseLRButton();
    }

    /// <summary>
    /// �摜��ݒ肷��
    /// </summary>
    private void SetImage()
    {
        nowPage = 0;
        GuideImage.SetActive(true);
        GuideSprite.sprite = sprites[nowPage];
    }

    /// <summary>
    /// ���E�{�^�����Z�b�g����
    /// </summary>
    private void SetLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(true);

        PageNum.enabled = true;
        PageNum.text = (nowPage + 1).ToString() + " / " + sprites.Length;
    }

    /// <summary>
    /// ���E�{�^�������
    /// </summary>
    private void CloseLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(false);

        PageNum.enabled = false;
    }


}
