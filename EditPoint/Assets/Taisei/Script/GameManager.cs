using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //���݂̃V�[����
    private string s_nowSceneName = "";

    private TimeBar timeBar;

    private PlaySound playSound;

    private QuickGuideMenu quick;

    //�^�C�g����ʂ̂�(�f�o�b�O�p)
    private bool b_debug = false;

    private void Awake()
    {
        GameData.GameEntity.isPlayNow = false;
        GameData.GameEntity.isClear = false;
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        s_nowSceneName = SceneManager.GetActiveScene().name;    //�V�[�������擾
        b_debug = false;
        switch (s_nowSceneName)
        {
            case "Title":
                playSound.PlayBGM(PlaySound.BGM_TYPE.title_stageSelect);
                b_debug = true;
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
            //�`���[�g���A���X�e�[�W�������I�Ƀv���C������Ԃɂ���
            if (Input.GetKeyDown(KeyCode.D) && b_debug)
            {
                DebugOption();
                GameData.GameEntity.isTimebarReset = false;
                Debug.Log("�`���[�g���A������������");
            }
        }
    }

    /// <summary>
    /// �f�o�b�O�p�@�\�@�`���[�g���A���X�e�[�W���v���C�������ǂ����̏�������������
    /// </summary>
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
        if (!GameData.GameEntity.isPlayNow)
        {
            timeBar.OnReStart();
            GameData.GameEntity.isPlayNow = true;
        }
    }

    public void OnReset()
    {
        GameData.GameEntity.isPlayNow = false;
        GameData.GameEntity.isTimebarReset = true;
        GameData.GameEntity.isLimitTime = false;
    }
}
