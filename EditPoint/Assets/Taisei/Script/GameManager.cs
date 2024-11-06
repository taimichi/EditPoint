using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string s_nowSceneName = "";

    private TimeBar timeBar;

    private PlaySound playSound;

    private QuickGuideMenu quick;

    private void Awake()
    {
        GameData.GameEntity.b_playNow = false;
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        s_nowSceneName = SceneManager.GetActiveScene().name;    //シーン名を取得
        switch (s_nowSceneName)
        {
            case "Title":
                playSound.PlayBGM(PlaySound.BGM_TYPE.title_stageSelect);
                break;

            case "Talk":
                playSound.PlayBGM(PlaySound.BGM_TYPE.talk);
                break;

            case "Select":
                playSound.PlayBGM(PlaySound.BGM_TYPE.title_stageSelect);
                break;
        }

        if (s_nowSceneName.Contains("Stage"))
        {
            timeBar = GameObject.Find("Timebar").GetComponent<TimeBar>();
            playSound.PlayBGM(PlaySound.BGM_TYPE.stage1);
            playSound.PlaySE(PlaySound.SE_TYPE.start);
        }
        else if(s_nowSceneName.Contains("Tutorial"))
        {
            timeBar = GameObject.Find("Timebar").GetComponent<TimeBar>();
            quick = GameObject.Find("GuideCanvas").GetComponent<QuickGuideMenu>();

            playSound.PlayBGM(PlaySound.BGM_TYPE.stage1);
            playSound.PlaySE(PlaySound.SE_TYPE.start);
            Time.timeScale = 0;

            switch (s_nowSceneName)
            {
                case string name when name.Contains("Clip1"):
                    if((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.clip) == 0)
                    {
                        quick.StartGuide("Clip");
                    }
                    break;

                case string name when name.Contains("Block1"):
                    if ((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.block) == 0)
                    {
                        quick.StartGuide("Block");
                    }
                    break;

                case string name when name.Contains("Copy1"):
                    if((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.copy) == 0)
                    {
                        quick.StartGuide("Copy");
                    }
                    break;

                case string name when name.Contains("Blower1"):
                    if((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.blower) == 0)
                    {
                        quick.StartGuide("Blower");
                    }
                    break;

                case string name when name.Contains("Move1"):
                    if((TutorialData.TutorialEntity.frags & TutorialData.Tutorial_Frags.move) == 0)
                    {
                        quick.StartGuide("Move");
                    }
                    break;

            }
        }

    }

    void Start()
    {
        ModeData.ModeEntity.mode = ModeData.Mode.normal;

    }

    void Update()
    {
        //デバッグ用機能
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //timescaleを強制的に変更
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("timescale強制変更");
                Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            }

            //チュートリアルステージを強制的にプレイした状態にする
            if (Input.GetKeyDown(KeyCode.D))
            {
                DebugOption();
                Debug.Log("チュートリアル情報を初期化");
            }
        }

        Debug.Log(ModeData.ModeEntity.mode);
    }

    private void DebugOption()
    {
        TutorialData.TutorialEntity.frags &= TutorialData.Tutorial_Frags.clip;
        TutorialData.TutorialEntity.frags &= TutorialData.Tutorial_Frags.block;
        TutorialData.TutorialEntity.frags &= TutorialData.Tutorial_Frags.copy;
        TutorialData.TutorialEntity.frags &= TutorialData.Tutorial_Frags.blower;
        TutorialData.TutorialEntity.frags &= TutorialData.Tutorial_Frags.move;
        TutorialData.TutorialEntity.frags &= TutorialData.Tutorial_Frags.button;
        TutorialData.TutorialEntity.frags &= TutorialData.Tutorial_Frags.other;
    }

    public void OnStart()
    {
        if (!GameData.GameEntity.b_playNow)
        {
            timeBar.OnReStart();
            GameData.GameEntity.b_playNow = true;
        }
    }

    public void OnReset()
    {
        GameData.GameEntity.b_playNow = false;
    }
}
