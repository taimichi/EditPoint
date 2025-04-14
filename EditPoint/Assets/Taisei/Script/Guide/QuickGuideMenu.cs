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
    /// 左右ボタンを使用できるようにする
    /// </summary>
    private void SetLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(true);
    }

    /// <summary>
    /// 左右ボタンを非表示にする
    /// </summary>
    private void CloseLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(false);
    }

    /// <summary>
    /// ガイドを閉じる
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
    /// 右矢印ボタンを押した時
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
    /// 左矢印を押したとき
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
    /// 最初に操作方法を表示する
    /// </summary>
    /// <param name="_key">表示したい操作方法</param>
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
