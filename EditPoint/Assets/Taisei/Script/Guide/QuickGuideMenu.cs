using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixeye.Unity;

public class QuickGuideMenu : MonoBehaviour
{
    //チュートリアル用画像が入っているスクリプタブルオブジェクト
    [SerializeField] private GuideSpriteListData guideSprite;

    [SerializeField] private GameObject QuickGuideObj;
    [SerializeField] private GameObject GuideImage;
    private Image GuideSprite;

    [SerializeField] private GameObject LButton;        //左矢印ボタン
    [SerializeField] private GameObject RButton;        //右矢印ボタン

    private Sprite[] sprites;   //表示用スプライト配列
    private int nowPage = 0;    //現在のページ数

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
