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
            playSound.PlaySE(PlaySound.SE_TYPE.start);

            backGround = GameObject.Find("BackGroundImage").GetComponent<Image>();

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
                            quick.StartGuide("Clip");
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

                    case string name when name.Contains("Move"):
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
                        if((TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.cut) == 0)
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
                                          TutorialData.Tutorial_Frags.move | TutorialData.Tutorial_Frags.cut |
                                          TutorialData.Tutorial_Frags.card | TutorialData.Tutorial_Frags.moveGround);
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
            GameData.GameEntity.isTimebarReset = false;
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
        if(KeyScripts.Count != 0)
        {
            for(int i= 0; i < KeyScripts.Count; i++)
            {
                KeyScripts[i].KeyReset();
            }
        }
    }

    public void AddKeyList(KeyController _keyController)
    {
        KeyScripts.Add(_keyController);
    }
}
