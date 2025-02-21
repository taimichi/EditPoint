using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //現在のシーン名
    private string nowSceneName = "";

    private TimeBar timeBar;

    private PlaySound playSound;

    private QuickGuideMenu quick;

    //タイトル画面のみ(デバッグ用)
    private bool isDebug = false;

    private void Awake()
    {
        //各フラグをリセット
        GameData.GameEntity.isPlayNow = false;
        GameData.GameEntity.isClear = false;
        GameData.GameEntity.isLimitTime = false;

        //AudioCanvasを取得
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        nowSceneName = SceneManager.GetActiveScene().name;    //シーン名を取得

        //デバッグ用フラグをリセット
        isDebug = false;
    }

    void Start()
    {
        //チュートリアル以外のステージのとき
        if (nowSceneName.Contains("Stage"))
        {
            timeBar = GameObject.Find("Timebar").GetComponent<TimeBar>();
            playSound.PlayBGM(PlaySound.BGM_TYPE.stage1);
            playSound.PlaySE(PlaySound.SE_TYPE.start);
        }
        //チュートリアルステージの時
        else if (nowSceneName.Contains("Tutorial"))
        {
            timeBar = GameObject.Find("Timebar").GetComponent<TimeBar>();
            quick = GameObject.Find("GuideCanvas").GetComponent<QuickGuideMenu>();

            playSound.PlayBGM(PlaySound.BGM_TYPE.stage1);
            playSound.PlaySE(PlaySound.SE_TYPE.start);

            var frags = TutorialData.TutorialEntity.frags;

            //チュートリアルステージを初めてプレイするとき、
            //それぞれの操作方法を始めに表示
            switch (nowSceneName)
            {
                case string name when name.Contains("Clip"):
                    if ((frags & TutorialData.Tutorial_Frags.clip) == 0)
                    {
                        quick.StartGuide("Clip");
                    }
                    break;

                case string name when name.Contains("Block"):
                    if ((frags & TutorialData.Tutorial_Frags.block) == 0)
                    {
                        quick.StartGuide("Block");
                    }
                    break;

                case string name when name.Contains("Copy"):
                    if ((frags & TutorialData.Tutorial_Frags.copy) == 0)
                    {
                        quick.StartGuide("Copy");
                    }
                    break;

                case string name when name.Contains("Blower"):
                    if ((frags & TutorialData.Tutorial_Frags.blower) == 0)
                    {
                        quick.StartGuide("Blower");
                    }
                    break;

                case string name when name.Contains("Move"):
                    if ((frags & TutorialData.Tutorial_Frags.move) == 0)
                    {
                        quick.StartGuide("Move");
                    }
                    break;

            }
        }
        //それ以外のとき
        else
        {
            switch (nowSceneName)
            {
                case "Title":
                    playSound.PlayBGM(PlaySound.BGM_TYPE.title_stageSelect);
                    isDebug = true; //デバッグ用機能を使用可能にする
                    break;

                case "Talk":
                    playSound.PlayBGM(PlaySound.BGM_TYPE.talk);
                    //フェードアウト処理
                    Fade fade = GameObject.Find("GameFade").GetComponent<Fade>();
                    fade.FadeOut(0.5f);
                    break;

                case "Select":
                    playSound.PlayBGM(PlaySound.BGM_TYPE.title_stageSelect);
                    break;
            }
        }

        ModeData.ModeEntity.mode = ModeData.Mode.normal;
    }

    void Update()
    {
        //デバッグ用機能
        //タイトル画面の時のみ使用可能
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //チュートリアルステージを強制的にプレイした状態にする
            if (Input.GetKeyDown(KeyCode.D) && isDebug)
            {
                DebugOption();
                GameData.GameEntity.isTimebarReset = false;
                Debug.Log("チュートリアル情報を初期化");
            }
        }
    }

    /// <summary>
    /// デバッグ用機能　チュートリアルステージをプレイしたかどうかの情報を初期化する
    /// </summary>
    private void DebugOption()
    {
        TutorialData.TutorialEntity.frags &= (TutorialData.Tutorial_Frags.clip | TutorialData.Tutorial_Frags.block |
                                          TutorialData.Tutorial_Frags.copy | TutorialData.Tutorial_Frags.blower |
                                          TutorialData.Tutorial_Frags.move | TutorialData.Tutorial_Frags.button |
                                          TutorialData.Tutorial_Frags.other);
    }

    /// <summary>
    /// スタートボタンを押したとき
    /// </summary>
    public void OnStart()
    {
        if (!GameData.GameEntity.isPlayNow)
        {
            timeBar.OnReStart();
            GameData.GameEntity.isPlayNow = true;
        }
    }

    /// <summary>
    /// タイムバーリセットをした時
    /// </summary>
    public void OnReset()
    {
        GameData.GameEntity.isPlayNow = false;
        GameData.GameEntity.isTimebarReset = true;
        GameData.GameEntity.isLimitTime = false;
    }
}
