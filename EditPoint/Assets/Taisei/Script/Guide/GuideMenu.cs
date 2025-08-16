using UnityEngine;
using UnityEngine.UI;

public class GuideMenu : MonoBehaviour
{
    //�`���[�g���A���p�摜�������Ă���X�N���v�^�u���I�u�W�F�N�g
    [SerializeField, Header("�`���[�g���A���p�摜")] private GuideSpriteListData guideSprite;

    private Canvas canvas;
    [SerializeField] private GameObject GuideMenuObj;

    //�摜��\������Image�I�u�W�F�N�g
    [SerializeField] private GameObject GuideImage;
    private Image GuideSprite;

    [SerializeField] private GameObject LButton;    //�����{�^��
    [SerializeField] private GameObject RButton;    //�E���{�^��

    private Sprite[] sprites;                       //�\���p�̃X�v���C�g������z��
    private int nowPage = 0;                        //���݂̃y�[�W��

    //���ݕ\�����Ă���y�[�W��
    [SerializeField] private Text PageNum;

    private PlaySound playSound;


    void Start()
    {
        //Image�擾
        GuideSprite = GuideImage.GetComponent<Image>();
        //�L�����o�X�擾
        canvas = this.GetComponent<Canvas>();
        //�k���`�F�b�N
        if(GameObject.Find("UICamera") != null)
        {
            //UI�J�������擾
            canvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        }
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        //���E�{�^�����\��
        CloseLRButton();

        //�`���[�g���A���p�I�u�W�F�N�g���\��
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
        //�y�[�W���ƃX�v���C�g�z���������
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
        //��������p�I�u�W�F�N�g��\��
        GuideMenuObj.SetActive(true);
    }

    /// <summary>
    /// �����{�^�����������Ƃ�
    /// </summary>
    public void OnLButton()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        nowPage--;
        //�y�[�W����0�ȉ��ɂȂ�Ƃ�
        if (nowPage <= 0)
        {
            //����ɑO�̃y�[�W�ɖ߂�Ȃ��悤�����{�^�����\��
            LButton.SetActive(false);
            //�y�[�W����0��
            nowPage = 0;
        }
        RButton.SetActive(true);

        //�`���[�g���A���p�摜���X�V
        GuideSprite.sprite = sprites[nowPage];
        //�y�[�W�����X�V
        PageNum.text = (nowPage + 1).ToString() + " / " + sprites.Length;
    }

    /// <summary>
    /// �E���{�^�����������Ƃ�
    /// </summary>
    public void OnRButton()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        nowPage++;
        //�y�[�W�����ő�l�ȏ�ɂȂ�Ƃ�
        if(nowPage >= sprites.Length -1)
        {
            //���̃y�[�W�ɐi�܂Ȃ��悤�E���{�^�����\��
            RButton.SetActive(false);
            //�y�[�W�����ő�l��
            nowPage = sprites.Length - 1;
        }
        LButton.SetActive(true);

        //�`���[�g���A���p�摜���X�V
        GuideSprite.sprite = sprites[nowPage];
        //�y�[�W�����X�V
        PageNum.text = (nowPage + 1).ToString() + " / " + sprites.Length;
    }

    /// <summary>
    /// �X�v���C�g��ݒ�
    /// </summary>
    /// <param name="_guide">�ݒ肷��X�v���C�g�̓��e</param>
    private void SetGuideSprite(GuideSpriteListData.GUIDE _guide)
    {
        //�\���p�̃X�v���C�g�ɕ\���������`���[�g���A���p�摜��ݒ�
        sprites = guideSprite.GuideSpriteDictionary[_guide].GuideSprites;
    }

    /// <summary>
    /// �N���b�v�������
    /// </summary>
    public void OnClipGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.clip);
        SetLRButton();
        SetImage();

    }

    /// <summary>
    /// �u���b�N�����������
    /// </summary>
    public void OnBlockGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.blockGene);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �����@�������
    /// </summary>
    public void OnBlowerGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.blower);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �R�s�y�������
    /// </summary>
    public void OnCopyGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.copy);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �ړ��E�T�C�Y�E�p�x�ύX�������
    /// </summary>
    public void OnMoveGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.move);
        SetImage();
        SetLRButton();

    }

    /// <summary>
    /// �{�^���������
    /// </summary>
    public void OnButtonGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.button);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �폜�{�^������
    /// </summary>
    public void OnDeleteGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.delete);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �^�C�����C������
    /// </summary>
    public void OnTimelineGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.timeline);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// ����������
    /// </summary>
    public void OnMoveGroundGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.moveGround);
        SetImage();
        SetLRButton();

    }

    /// <summary>
    /// �J�[�h�L�[����
    /// </summary>
    public void OnCardGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.card);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// �J�b�g����
    /// </summary>
    public void OnCutGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.cut);
        SetImage();
        CloseLRButton();
    }

    public void OnOtherGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.other);
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
