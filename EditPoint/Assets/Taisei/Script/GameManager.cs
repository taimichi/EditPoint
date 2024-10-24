using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool b_debug = false;
    private bool b_start = false;
    private string s_nowSceneName = "";

    private PlaySound playSound;

    private QuickGuideMenu quick;

    private void Awake()
    {
        GameData.GameEntity.b_playNow = false;
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        b_start = false;

        s_nowSceneName = SceneManager.GetActiveScene().name;    //�V�[�������擾
        b_debug = false;
        switch (s_nowSceneName)
        {
            case "Title":
                b_debug = true;
                playSound.PlayBGM(PlaySound.BGM_TYPE.title_stageSelect);
                Time.timeScale = 1;
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
            playSound.PlayBGM(PlaySound.BGM_TYPE.stage1);
            playSound.PlaySE(PlaySound.SE_TYPE.start);
            Time.timeScale = 0;
        }
        else if(s_nowSceneName.Contains("Tutorial"))
        {
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
        //�f�o�b�O�p�@�\
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //timescale�������I�ɕύX
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("timescale�����ύX");
                Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            }

            //�`���[�g���A���X�e�[�W�������I�Ƀv���C������Ԃɂ���
            if (Input.GetKeyDown(KeyCode.D))
            {
                DebugOption();
                Debug.Log("�`���[�g���A������������");
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
        if (!b_start)
        {
            GameData.GameEntity.b_playNow = true;
            Time.timeScale = 1;
            b_start = true;
        }
    }

    public void OnReset()
    {
        GameData.GameEntity.b_playNow = false;
        b_start = false;
        Time.timeScale = 0;
    }
}
