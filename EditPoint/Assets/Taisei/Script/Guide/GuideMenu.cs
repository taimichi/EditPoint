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

    private void Awake()
    {
        GuideSprite = GuideImage.GetComponent<Image>();
        canvas = this.GetComponent<Canvas>();
        canvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }


    void Start()
    {
        CloseLRButton();
        GuideImage.SetActive(false);
        GuideMenuObj.SetActive(false);
    }

    /// <summary>
    /// ���������ʂ����
    /// </summary>
    public void OnCloseGuide()
    {
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
        GuideMenuObj.SetActive(true);
    }

    /// <summary>
    /// �����{�^�����������Ƃ�
    /// </summary>
    public void OnLButton()
    {
        nowPage--;
        if (nowPage <= 0)
        {
            LButton.SetActive(false);
        }
        RButton.SetActive(true);

        GuideSprite.sprite = sprites[nowPage];
    }

    /// <summary>
    /// �E���{�^�����������Ƃ�
    /// </summary>
    public void OnRButton()
    {
        nowPage++;
        if(nowPage >= sprites.Length -1)
        {
            RButton.SetActive(false);
        }
        LButton.SetActive(true);

        GuideSprite.sprite = sprites[nowPage];
    }

    /// <summary>
    /// �N���b�v�������
    /// </summary>
    public void OnClipGuide()
    {
        sprites = ClipGuide;

        SetLRButton();
        SetImage();

    }

    /// <summary>
    /// �u���b�N�����������
    /// </summary>
    public void OnBlockGuide()
    {
        sprites = BlockGeneGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �����@�������
    /// </summary>
    public void OnBlowerGuide()
    {
        sprites = BlowerGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �R�s�y�������
    /// </summary>
    public void OnCopyGuide()
    {
        sprites = CopyGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �ړ��E�T�C�Y�E�p�x�ύX�������
    /// </summary>
    public void OnMoveGuide()
    {
        sprites = MoveGuide;
        SetImage();
        SetLRButton();

    }

    /// <summary>
    /// �{�^���������
    /// </summary>
    public void OnButtonGuide()
    {
        sprites = ButtonGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �폜�{�^������
    /// </summary>
    public void OnDeleteGuide()
    {
        sprites = DeleteGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �^�C�����C������
    /// </summary>
    public void OnTimelineGuide()
    {
        sprites = TimelineGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// ����������
    /// </summary>
    public void OnMoveGroundGuide()
    {
        sprites = MoveGroundGuide;
        SetImage();
        SetLRButton();

    }

    /// <summary>
    /// �J�[�h�L�[����
    /// </summary>
    public void OnCardGuide()
    {
        sprites = CardGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �J�b�g����
    /// </summary>
    public void OnCutGuide()
    {
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
    }

    /// <summary>
    /// ���E�{�^�������
    /// </summary>
    private void CloseLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(false);
    }

}
