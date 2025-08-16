using UnityEngine;
using UnityEngine.UI;

public class GuideMenu : MonoBehaviour
{
    //チュートリアル用画像が入っているスクリプタブルオブジェクト
    [SerializeField, Header("チュートリアル用画像")] private GuideSpriteListData guideSprite;

    private Canvas canvas;
    [SerializeField] private GameObject GuideMenuObj;

    //画像を表示するImageオブジェクト
    [SerializeField] private GameObject GuideImage;
    private Image GuideSprite;

    [SerializeField] private GameObject LButton;    //左矢印ボタン
    [SerializeField] private GameObject RButton;    //右矢印ボタン

    private Sprite[] sprites;                       //表示用のスプライトを入れる配列
    private int nowPage = 0;                        //現在のページ数

    //現在表示しているページ数
    [SerializeField] private Text PageNum;

    private PlaySound playSound;


    void Start()
    {
        //Image取得
        GuideSprite = GuideImage.GetComponent<Image>();
        //キャンバス取得
        canvas = this.GetComponent<Canvas>();
        //ヌルチェック
        if(GameObject.Find("UICamera") != null)
        {
            //UIカメラを取得
            canvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        }
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        //左右ボタンを非表示
        CloseLRButton();

        //チュートリアル用オブジェクトを非表示
        GuideImage.SetActive(false);
        GuideMenuObj.SetActive(false);

        PageNum.enabled = false;
    }

    /// <summary>
    /// 操作説明画面を閉じる
    /// </summary>
    public void OnCloseGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.cancell);

        CloseLRButton();
        //ページ数とスプライト配列を初期化
        sprites = null;
        nowPage = 0;

        GuideImage.SetActive(false);
        GuideMenuObj.SetActive(false);
    }

    /// <summary>
    /// 操作説明メニューを開く
    /// </summary>
    public void OnOpenGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        //操作説明用オブジェクトを表示
        GuideMenuObj.SetActive(true);
    }

    /// <summary>
    /// 左矢印ボタンを押したとき
    /// </summary>
    public void OnLButton()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        nowPage--;
        //ページ数が0以下になるとき
        if (nowPage <= 0)
        {
            //さらに前のページに戻らないよう左矢印ボタンを非表示
            LButton.SetActive(false);
            //ページ数を0に
            nowPage = 0;
        }
        RButton.SetActive(true);

        //チュートリアル用画像を更新
        GuideSprite.sprite = sprites[nowPage];
        //ページ数を更新
        PageNum.text = (nowPage + 1).ToString() + " / " + sprites.Length;
    }

    /// <summary>
    /// 右矢印ボタンを押したとき
    /// </summary>
    public void OnRButton()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        nowPage++;
        //ページ数が最大値以上になるとき
        if(nowPage >= sprites.Length -1)
        {
            //次のページに進まないよう右矢印ボタンを非表示
            RButton.SetActive(false);
            //ページ数を最大値に
            nowPage = sprites.Length - 1;
        }
        LButton.SetActive(true);

        //チュートリアル用画像を更新
        GuideSprite.sprite = sprites[nowPage];
        //ページ数を更新
        PageNum.text = (nowPage + 1).ToString() + " / " + sprites.Length;
    }

    /// <summary>
    /// スプライトを設定
    /// </summary>
    /// <param name="_guide">設定するスプライトの内容</param>
    private void SetGuideSprite(GuideSpriteListData.GUIDE _guide)
    {
        //表示用のスプライトに表示したいチュートリアル用画像を設定
        sprites = guideSprite.GuideSpriteDictionary[_guide].GuideSprites;
    }

    /// <summary>
    /// クリップ操作説明
    /// </summary>
    public void OnClipGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.clip);
        SetLRButton();
        SetImage();

    }

    /// <summary>
    /// ブロック生成操作説明
    /// </summary>
    public void OnBlockGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.blockGene);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// 送風機操作説明
    /// </summary>
    public void OnBlowerGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.blower);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// コピペ操作説明
    /// </summary>
    public void OnCopyGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.copy);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// 移動・サイズ・角度変更操作説明
    /// </summary>
    public void OnMoveGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.move);
        SetImage();
        SetLRButton();

    }

    /// <summary>
    /// ボタン操作説明
    /// </summary>
    public void OnButtonGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.button);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// 削除ボタン説明
    /// </summary>
    public void OnDeleteGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.delete);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// タイムライン説明
    /// </summary>
    public void OnTimelineGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.timeline);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// 動く床説明
    /// </summary>
    public void OnMoveGroundGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.moveGround);
        SetImage();
        SetLRButton();

    }

    /// <summary>
    /// カードキー説明
    /// </summary>
    public void OnCardGuide()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
        SetGuideSprite(GuideSpriteListData.GUIDE.card);
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// カット説明
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
    /// 画像を設定する
    /// </summary>
    private void SetImage()
    {
        nowPage = 0;
        GuideImage.SetActive(true);
        GuideSprite.sprite = sprites[nowPage];
    }

    /// <summary>
    /// 左右ボタンをセットする
    /// </summary>
    private void SetLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(true);

        PageNum.enabled = true;
        PageNum.text = (nowPage + 1).ToString() + " / " + sprites.Length;
    }

    /// <summary>
    /// 左右ボタンを閉じる
    /// </summary>
    private void CloseLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(false);

        PageNum.enabled = false;
    }


}
