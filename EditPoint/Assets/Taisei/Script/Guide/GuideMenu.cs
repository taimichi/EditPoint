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
    /// 操作説明画面を閉じる
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
    /// 操作説明メニューを開く
    /// </summary>
    public void OnOpenGuide()
    {
        GuideMenuObj.SetActive(true);
    }

    /// <summary>
    /// 左矢印ボタンを押したとき
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
    /// 右矢印ボタンを押したとき
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
    /// クリップ操作説明
    /// </summary>
    public void OnClipGuide()
    {
        sprites = ClipGuide;

        SetLRButton();
        SetImage();

    }

    /// <summary>
    /// ブロック生成操作説明
    /// </summary>
    public void OnBlockGuide()
    {
        sprites = BlockGeneGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// 送風機操作説明
    /// </summary>
    public void OnBlowerGuide()
    {
        sprites = BlowerGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// コピペ操作説明
    /// </summary>
    public void OnCopyGuide()
    {
        sprites = CopyGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// 移動・サイズ・角度変更操作説明
    /// </summary>
    public void OnMoveGuide()
    {
        sprites = MoveGuide;
        SetImage();
        SetLRButton();

    }

    /// <summary>
    /// ボタン操作説明
    /// </summary>
    public void OnButtonGuide()
    {
        sprites = ButtonGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// 削除ボタン説明
    /// </summary>
    public void OnDeleteGuide()
    {
        sprites = DeleteGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// タイムライン説明
    /// </summary>
    public void OnTimelineGuide()
    {
        sprites = TimelineGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// 動く床説明
    /// </summary>
    public void OnMoveGroundGuide()
    {
        sprites = MoveGroundGuide;
        SetImage();
        SetLRButton();

    }

    /// <summary>
    /// カードキー説明
    /// </summary>
    public void OnCardGuide()
    {
        sprites = CardGuide;
        SetImage();
        CloseLRButton();

    }

    /// <summary>
    /// カット説明
    /// </summary>
    public void OnCutGuide()
    {
        sprites = CutGuide;
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
    }

    /// <summary>
    /// 左右ボタンを閉じる
    /// </summary>
    private void CloseLRButton()
    {
        LButton.SetActive(false);
        RButton.SetActive(false);
    }

}
