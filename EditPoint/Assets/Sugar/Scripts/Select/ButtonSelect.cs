using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;    // UI
using UnityEngine.SceneManagement;

public class ButtonSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // カーソルが触れたらこのオブジェクトをtrue
    [SerializeField] GameObject img;
    [SerializeField] GameObject fileObj;

    // ここでどのボタン機能か判断
    public enum SelectButton
    {
        File,FileClose,Bg,Title,End,Yes_No,Option,OptionClose,etc,
        /*ゲームシーン中に使うやつ*/
        Guide,Game,Select
    }
    [Header("ボタン機能の種類を選ぶ"),SerializeField] SelectButton kindButton;

    //ステージセレクトのロック用
    //false=使用不可 / true=使用可能
    private bool isLock = true;
    [SerializeField] private GameObject LockPanel;



    // UIのインターフェース
    #region interface
    // UI上にカーソルが触れているか
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 表示
        img.SetActive(true);
    }
    // 離れた場合
    public void OnPointerExit(PointerEventData eventData)
    {
        // 非表示
        img.SetActive(false);
    }
    #endregion

    void Start()
    {
        //ステージセレクトボタンの時
        if(kindButton == SelectButton.File)
        {
            SceneChange sc = fileObj.GetComponent<SceneChange>();
            sc.SarchStage();
            isLock = sc.ReturnIsLock();
            //ロック状態の時
            if (!isLock)
            {
                LockPanel.SetActive(true);
            }
            else
            {
                LockPanel.SetActive(false);
            }
        }
    }

    void Update()
    {
        // 選ばれた状態でのみ
        if(img.activeSelf)
        {
            // クリックしたらそのファイルを起動
            if (Input.GetMouseButtonDown(0))
            {
                CheckKind(kindButton);
                img.SetActive(false);
            }
        }

    }

    // どのボタンを使うか判断
    private void CheckKind(SelectButton select)
    {
        switch(select)
        {
            case SelectButton.File:
                File();
                break;
            case SelectButton.FileClose:
                FileClose();
                break;
            case SelectButton.Bg:
                Bg();
                break;
            case SelectButton.Title:
                Title();
                break;
            case SelectButton.End:
                End();
                break;
            case SelectButton.Yes_No:
                Yes_No();
                break;
            case SelectButton.Option:
                Option();
                break;
            case SelectButton.OptionClose:
                OptionClose();
                break;

            case SelectButton.Guide:
                Guide();
                break;
            case SelectButton.Game:
                Game();
                break;
            case SelectButton.Select:
                Select();
                break;
        }
    }

    #region Function

    // 設定を閉じる
    private void OptionClose()
    {
        GameObject obj = GameObject.Find("AudioCanvas");
        obj.GetComponent<SoundMenu>().CloseWindow();

    }
    // 設定
    private void Option()
    {
        GameObject obj = GameObject.Find("AudioCanvas");
        obj.GetComponent<SoundMenu>().OpenWindow();
    }

    // はい、いいえのボタン
    private void Yes_No()
    {
        // 本当に終わるのか再確認を表示後の処理を実行
        fileObj.SetActive(true);
    }

    // 終了ボタン
    private void End()
    {
        // 本当に終わるのか再確認を表示
        fileObj.SetActive(true);
    }

    // タイトルへ戻る
    private void Title()
    {
        string sceneName="Title";

        Fade fade;          // FadeCanvas
        bool isOn = false;
        if (isOn)
        {
            return;
        }
        fade = GameObject.Find("GameFade").GetComponent<Fade>();
        // フェード
        fade.FadeIn(0.5f, () =>
        {
            SceneManager.LoadScene(sceneName);
            isOn = true;
        });
    }

    // 背景変更ボタン
    private void Bg()
    {
        GameObject obj = GameObject.Find("DeskTopBg");
        obj.GetComponent<ImageLoader>().LoadImage();
    }

    // ステージパネルを閉じるこの時にファイルの位置を元に戻す
    private void FileClose()
    {
        fileObj.SetActive(false);

        // ファイルオブジェクト全てを基準座標に戻す
        DraggableImage[] targets = FindObjectsOfType<DraggableImage>();
        if (targets.Length > 0)
        {
            foreach (DraggableImage target in targets)
            {
                target.OriginalPos();
            }
        }
    }

    // ファイル機能
    private void File()
    {
        if (isLock)
        {
            fileObj.SetActive(true);
        }
    }
    #endregion

    #region GameFunc
    // ガイドを開く
    private void Guide()
    {
        GameObject obj = GameObject.Find("GuideCanvas");
        obj.GetComponent<GuideMenu>().OnOpenGuide();
    }
    // メニューを閉じる
    private void Game()
    {
        fileObj.SetActive(false);
    }
    // セレクトシーンに移行
    private void Select()
    {
        GameObject obj = GameObject.Find("PlayBack");
        obj.GetComponent<ToolButton>().SelectScene();
    }
    #endregion
}
