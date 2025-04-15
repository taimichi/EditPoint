using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixeye.Unity;

public class QuickGuideMenu : MonoBehaviour
{
    [SerializeField] private GameObject QuickGuideObj;
    [SerializeField] private GameObject GuideImage;
    private Image GuideSprite;

    [SerializeField] private GameObject LButton;
    [SerializeField] private GameObject RButton;

    #region Sprite
    [Foldout("Sprite")] [SerializeField] private Sprite[] clip;
    [Foldout("Sprite")] [SerializeField] private Sprite[] block;
    [Foldout("Sprite")] [SerializeField] private Sprite[] blower;
    [Foldout("Sprite")] [SerializeField] private Sprite[] copy;
    [Foldout("Sprite")] [SerializeField] private Sprite[] move;
    [Foldout("Sprite")] [SerializeField] private Sprite[] moveGround;
    [Foldout("Sprite")] [SerializeField] private Sprite[] card;
    [Foldout("Sprite")] [SerializeField] private Sprite[] cut;
    #endregion

    private Sprite[] sprites;
    private int nowPage = 0;

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
                sprites = clip;
                SetLRButton();
                break;

            case "Block":
                sprites = block;
                break;

            case "Blower":
                sprites = blower;
                break;

            case "Copy":
                sprites = copy;
                break;

            case "Move":
                sprites = move;
                SetLRButton();
                break;

            case "MoveGround":
                sprites = moveGround;
                SetLRButton();
                break;

            case "Card":
                sprites = card;
                break;

            case "Cut":
                sprites = cut;
                break;
        }

        GuideSprite.sprite = sprites[nowPage];
    }

    public bool IsCheckActive() => QuickGuideObj.activeSelf;
}
