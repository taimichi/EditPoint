using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pixeye.Unity;

public class GameManager : MonoBehaviour
{
    //現在のシーン名
    private string nowSceneName = "";

    private TimeBar timeBar;

    private PlaySound playSound;

    private QuickGuideMenu quick;

    //タイトル画面のみ(デバッグ用)
    private bool isDebug = false;

    //背景
    private Image backGround;
    //背景画像
    [Foldout("Sprite"), SerializeField] private Sprite[] sprites;

    private List<KeyController> KeyScripts = new List<KeyController>();

    private float playSpeed = 1f;

    private Button speedChangeButton;
    private Text speedText;

    [SerializeField] private GameObject fadeObj;

    private AllTexts allText;

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

        //ステージシーンの時
        if (!nowSceneName.Contains("Stage"))
        {
            fadeObj.SetActive(false);
        }
    }

    void Start()
    {
        //fps値を60に固定
        Application.targetFrameRate = 60;

        Time.timeScale = 1;

        //ステージシーンのとき
        if (nowSceneName.Contains("Stage"))
        {
            timeBar = GameObject.Find("Timebar").GetComponent<TimeBar>();
            playSound.PlaySE(PlaySound.SE_TYPE.start);

            backGround = GameObject.Find("BackGroundImage").GetComponent<Image>();

            GameObject SpeedChangeObj = GameObject.Find("SpeedChange");
            speedChangeButton = SpeedChangeObj.GetComponent<Button>();
            speedChangeButton.onClick.AddListener(OnChangePlaySpeed);
            speedText = GameObject.Find("SpeedText").GetComponent<Text>();
            speedText.text = "×" + playSpeed.ToString();

            fadeObj.GetComponent<Fade>().FadeOut(1.0f, () => {
                fadeObj.SetActive(false); }
            );

            switch (nowSceneName)
            {
                case string name when name.Contains("Stage1"):
                    backGround.sprite = sprites[0];
                    playSound.PlayBGM(PlaySound.BGM_TYPE.noon);

                    break;

                case string name when name.Contains("Stage2"):
                    backGround.sprite = sprites[0];
                    playSound.PlayBGM(PlaySound.BGM_TYPE.noon);

                    break;

                case string name when name.Contains("Stage3"):
                    backGround.sprite = sprites[1];
                    playSound.PlayBGM(PlaySound.BGM_TYPE.evening);

                    break;

                case string name when name.Contains("Stage4"):
                    backGround.sprite = sprites[2];
                    playSound.PlayBGM(PlaySound.BGM_TYPE.night);

                    break;
            }

            //チュートリアルステージの時
            if (nowSceneName.Contains("Tutorial"))
            {
                quick = GameObject.Find("GuideCanvas").GetComponent<QuickGuideMenu>();

                //チュートリアルステージを初めてプレイするとき、
                //それぞれの操作方法を始めに表示
                switch (nowSceneName)
                {
                    case string name when name.Contains("Clip"):
                        if ((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.clip) == 0)
                        {
                            //チュートリアルを表示
                            quick.StartGuide("Clip");
                            //フラグをオンにする
                            TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.clip;
                        }
                        break;

                    case string name when name.Contains("Block"):
                        if ((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.block) == 0)
                        {
                            quick.StartGuide("Block");
                            TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.block;
                        }
                        break;

                    case string name when name.Contains("Copy"):
                        if ((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.copy) == 0)
                        {
                            quick.StartGuide("Copy");
                            TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.copy;
                        }
                        break;

                    case string name when name.Contains("Blower"):
                        if ((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.blower) == 0)
                        {
                            quick.StartGuide("Blower");
                            TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.blower;
                        }
                        break;

                    case string name when name.Contains("Transform"):
                        if ((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.move) == 0)
                        {
                            quick.StartGuide("Move");
                            TutorialData.TutorialEntity.frags  |= TutorialData.Tutorial_Frags.move;
                        }
                        break;

                    case string name when name.Contains("MoveGround"):
                        if((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.moveGround) == 0)
                        {
                            quick.StartGuide("MoveGround");
                            TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.moveGround;
                        }
                        break;

                    case string name when name.Contains("Card"):
                        if((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.card) == 0)
                        {
                            quick.StartGuide("Card");
                            TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.card;
                        }
                        break;

                    case string name when name.Contains("Cut"):
                        if((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.cut) == 0)
                        {
                            quick.StartGuide("Cut");
                            TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.cut;
                        }
                        break;

                }
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
                    fade.FadeOut(1.0f);
                    break;

                case "Select":
                    playSound.PlayBGM(PlaySound.BGM_TYPE.title_stageSelect);
                    Fade fade2 = GameObject.Find("GameFade").GetComponent<Fade>();
                    fade2.FadeOut(1.0f);

                    allText = GameObject.Find("TalkCanvas").GetComponent<AllTexts>();
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
            //強制的に初期状態にする
            if (Input.GetKeyDown(KeyCode.D) && isDebug)
            {
                DebugOption_Reset();
                GameData.GameEntity.isTimebarReset = false;
                Debug.Log("チュートリアル情報を初期化");
                playSound.PlaySE(PlaySound.SE_TYPE.develop);
            }
            //全ステージを開放する
            else if(Input.GetKeyDown(KeyCode.O) && isDebug)
            {
                DebugOption_Open();
                Debug.Log("全ステージを開放");
                playSound.PlaySE(PlaySound.SE_TYPE.develop);
            }
        }

        //ステージセレクトの時
        if(nowSceneName == "Select")
        {
            #region デバッグ用 会話のみ発生
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    allText.SetAllTexts(AllTexts.TEXT_MESSAGE.clear_stage1);
            //}
            //if (Input.GetKeyDown(KeyCode.Y))
            //{
            //    allText.SetAllTexts(AllTexts.TEXT_MESSAGE.clear_stage2);
            //}
            //if (Input.GetKeyDown(KeyCode.U))
            //{
            //    allText.SetAllTexts(AllTexts.TEXT_MESSAGE.clear_stage3);
            //}
            //if (Input.GetKeyDown(KeyCode.I))
            //{
            //    allText.SetAllTexts(AllTexts.TEXT_MESSAGE.clear_stage4);
            //}

            //if (Input.GetKeyDown(KeyCode.Return))
            //{
            //    for (int i = 0; i < NewStageData.StageEntity.stageData.Length; i++)
            //    {
            //        NewStageData.StageEntity.stageData[i].stagelock = NewStageData.StageLock.Open;
            //    }
            //}
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    for (int i = 0; i < NewStageData.StageEntity.stageData.Length; i++)
            //    {
            //        NewStageData.StageEntity.stageData[i].stagelock = NewStageData.StageLock.Lock;
            //    }
            //}

            #endregion

            //一度も全ステクリア会話が発生していないとき
            if ((GameData.GameEntity.talkFrags & GameData.CLEARTALK_FRAG.stage4) == 0)
            {
                //全ステージクリアしたかどうか
                bool isAllClear = false;

                //全ステージクリアをしたかチェック
                for (int i = 0; i < NewStageData.StageEntity.stageData.Length; i++)
                {
                    //１つでもクリアしてないところがあった時
                    if(NewStageData.StageEntity.stageData[i].stagelock != NewStageData.StageLock.Open)
                    {
                        //ループを抜ける
                        isAllClear = false;
                        break;
                    }
                    //全ステージクリア
                    isAllClear = true;
                }

                //全ステージクリアしてた時
                if (isAllClear)
                {

                    //会話発生
                    allText.SetAllTexts(AllTexts.TEXT_MESSAGE.clear_stage4);
                    //１回以上会話発生した判定に
                    GameData.GameEntity.talkFrags |= GameData.CLEARTALK_FRAG.stage4;
                }
            }
            //全ステージクリア会話後
            else
            {
                //エンディングが流れてないとき
                if (!GameData.GameEntity.isEnding)
                {
                    if(allText.ReturnTalkState() == TextBoxSystem.TALK_STATE.end)
                    {
                        allText.ResetTalkState();
                        StaffCredit credit = GameObject.Find("CreditCanvas").GetComponent<StaffCredit>();
                        credit.OnStartCredit();
                        GameData.GameEntity.isEnding = true;
                    }
                }
            }

        }
    }

    #region デバッグ機能
    /// <summary>
    /// デバッグ用機能　チュートリアルステージをプレイしたかどうかの情報を初期化する
    /// </summary>
    private void DebugOption_Reset()
    {
        //全フラグをオフにする
        TutorialData.TutorialEntity.frags = TutorialData.Tutorial_Frags.None;

        //ステージ1-1以外のステージをロック状態に
        for(int i = 0; i < NewStageData.StageEntity.stageData.Length; i++)
        {
            //ステージ1以外をロック状態に
            if (i != 0)
            {
                NewStageData.StageEntity.stageData[i].stagelock = NewStageData.StageLock.Lock;
            }
        }
    }

    /// <summary>
    /// デバッグ用機能　全ステージを開放する
    /// </summary>
    private void DebugOption_Open()
    {
        //ステージ1-1以外のステージをロック状態に
        for (int i = 0; i < NewStageData.StageEntity.stageData.Length; i++)
        {
            NewStageData.StageEntity.stageData[i].stagelock = NewStageData.StageLock.Open;
        }
    }
    #endregion

    /// <summary>
    /// スタートボタンを押したとき
    /// </summary>
    public void OnStart()
    {
        if (!GameData.GameEntity.isPlayNow)
        {
            timeBar.OnReStart();
            GameData.GameEntity.isPlayNow = true;
            GameData.GameEntity.isTimebarReset = false;
            Time.timeScale = playSpeed;
        }
    }

    /// <summary>
    /// タイムバーリセットをした時
    /// </summary>
    public void OnReset()
    {
        Time.timeScale = 1;
        GameData.GameEntity.isPlayNow = false;
        GameData.GameEntity.isTimebarReset = true;
        GameData.GameEntity.isLimitTime = false;
        if(KeyScripts.Count != 0)
        {
            for(int i= 0; i < KeyScripts.Count; i++)
            {
                KeyScripts[i].KeyReset();
            }
        }
    }

    /// <summary>
    /// 再生時のスピード変更
    /// </summary>
    public void OnChangePlaySpeed()
    {
        switch (playSpeed)
        {
            case 1.0f:
                playSpeed = 1.5f;
                break;

            case 1.5f:
                playSpeed = 2.0f;
                break;

            case 2.0f:
                playSpeed = 2.5f;
                break;

            case 2.5f:
                playSpeed = 1.0f;
                break;
        }
        speedText.text = "×" + playSpeed.ToString();
        Time.timeScale = playSpeed;
    }

    public void AddKeyList(KeyController _keyController)
    {
        KeyScripts.Add(_keyController);
    }
}
