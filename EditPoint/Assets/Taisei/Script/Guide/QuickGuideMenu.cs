using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixeye.Unity;

//ステージ初めに表示されるチュートリアル用
public class QuickGuideMenu : MonoBehaviour
{
    //チュートリアル用画像が入っているスクリプタブルオブジェクト
    [SerializeField] private GuideSpriteListData guideSprite;

    [SerializeField] private GameObject QuickGuideObj;
    [SerializeField] private GameObject GuideImage;
    private Image GuideSprite;

    [SerializeField] private GameObject LButton;        //左矢印ボタン
    [SerializeField] private GameObject RButton;        //右矢印ボタン
    [SerializeField] private GameObject CloseButton;    //閉じるボタン

    private Sprite[] sprites;   //表示用スプライト配列
    private int nowPage = 0;    //現在のページ数

    private PlaySound playSound;

    private void Awake()
    {
        GuideSprite = GuideImage.GetComponent<Image>();
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        //閉じるボタンを非表示
        CloseButton.SetActive(false);
    }

    /// <summary>
    /// 左右ボタンを使用できるようにする
    /// </summary>
    private void SetLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(true);

        //閉じるボタンを非表示
        CloseButton.SetActive(false);
    }

    /// <summary>
    /// 左右ボタンを使用不可にする
    /// </summary>
    private void CloseLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(false);

        //閉じるボタンを表示
        CloseButton.SetActive(true);
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
        //ページ数を変更
        nowPage++;
        //現在のページ数が最大数になったとき
        if(nowPage >= sprites.Length - 1)
        {
            RButton.SetActive(false);
            //閉じるボタン表示
            CloseButton.SetActive(true);
        }
        LButton.SetActive(true);

        //チュートリアル画像更新
        GuideSprite.sprite = sprites[nowPage];
    }

    /// <summary>
    /// 左矢印を押したとき
    /// </summary>
    public void OnLeftButton()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        //ページ数を更新
        nowPage--;
        //現在のページ数が最小値になったとき
        if(nowPage <= 0)
        {
            LButton.SetActive(false);
        }
        RButton.SetActive(true);

        //閉じるボタンが表示状態だったら
        if (CloseButton.activeSelf)
        {
            //閉じるボタンを非表示に
            CloseButton.SetActive(false);
        }

        //チュートリアル画像更新
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
            //テンプレート
            //case "string(表示したい操作方法の名前)":
            //    画像表示
            //    sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.xxx].GuideSprites;
            //    break;

            //クリップ
            case "Clip":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.clip].GuideSprites;
                SetLRButton();
                break;

            //ブロック生成
            case "Block":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.blockGene].GuideSprites ;
                break;

            //送風機
            case "Blower":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.blower].GuideSprites;
                break;

            //コピー＆ペースト
            case "Copy":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.copy].GuideSprites;
                break;

            //物の移動・拡縮・回転
            case "Move":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.move].GuideSprites;
                break;
                
            //動く床
            case "MoveGround":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.moveGround].GuideSprites;
                break;

            //カードキー
            case "Card":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.card].GuideSprites;
                break;

            //カット
            case "Cut":
                sprites = guideSprite.GuideSpriteDictionary[GuideSpriteListData.GUIDE.cut].GuideSprites;
                break;
        }

        //画像枚数が2枚以上の時
        if(sprites.Length >= 2)
        {
            //左右ボタンを使用可能に
            SetLRButton();
        }

        //チュートリアル画像をセット
        GuideSprite.sprite = sprites[nowPage];
    }
}
