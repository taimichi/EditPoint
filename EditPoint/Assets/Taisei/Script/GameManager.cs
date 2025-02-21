using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //���݂̃V�[����
    private string nowSceneName = "";

    private TimeBar timeBar;

    private PlaySound playSound;

    private QuickGuideMenu quick;

    //�^�C�g����ʂ̂�(�f�o�b�O�p)
    private bool isDebug = false;

    private void Awake()
    {
        //�e�t���O�����Z�b�g
        GameData.GameEntity.isPlayNow = false;
        GameData.GameEntity.isClear = false;
        GameData.GameEntity.isLimitTime = false;

        //AudioCanvas���擾
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        nowSceneName = SceneManager.GetActiveScene().name;    //�V�[�������擾

        //�f�o�b�O�p�t���O�����Z�b�g
        isDebug = false;
    }

    void Start()
    {
        //�`���[�g���A���ȊO�̃X�e�[�W�̂Ƃ�
        if (nowSceneName.Contains("Stage"))
        {
            timeBar = GameObject.Find("Timebar").GetComponent<TimeBar>();
            playSound.PlayBGM(PlaySound.BGM_TYPE.stage1);
            playSound.PlaySE(PlaySound.SE_TYPE.start);
        }
        //�`���[�g���A���X�e�[�W�̎�
        else if (nowSceneName.Contains("Tutorial"))
        {
            timeBar = GameObject.Find("Timebar").GetComponent<TimeBar>();
            quick = GameObject.Find("GuideCanvas").GetComponent<QuickGuideMenu>();

            playSound.PlayBGM(PlaySound.BGM_TYPE.stage1);
            playSound.PlaySE(PlaySound.SE_TYPE.start);

            var frags = TutorialData.TutorialEntity.frags;

            //�`���[�g���A���X�e�[�W�����߂ăv���C����Ƃ��A
            //���ꂼ��̑�����@���n�߂ɕ\��
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
        //����ȊO�̂Ƃ�
        else
        {
            switch (nowSceneName)
            {
                case "Title":
                    playSound.PlayBGM(PlaySound.BGM_TYPE.title_stageSelect);
                    isDebug = true; //�f�o�b�O�p�@�\���g�p�\�ɂ���
                    break;

                case "Talk":
                    playSound.PlayBGM(PlaySound.BGM_TYPE.talk);
                    //�t�F�[�h�A�E�g����
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
        //�f�o�b�O�p�@�\
        //�^�C�g����ʂ̎��̂ݎg�p�\
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //�`���[�g���A���X�e�[�W�������I�Ƀv���C������Ԃɂ���
            if (Input.GetKeyDown(KeyCode.D) && isDebug)
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
        TutorialData.TutorialEntity.frags &= (TutorialData.Tutorial_Frags.clip | TutorialData.Tutorial_Frags.block |
                                          TutorialData.Tutorial_Frags.copy | TutorialData.Tutorial_Frags.blower |
                                          TutorialData.Tutorial_Frags.move | TutorialData.Tutorial_Frags.button |
                                          TutorialData.Tutorial_Frags.other);
    }

    /// <summary>
    /// �X�^�[�g�{�^�����������Ƃ�
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
    /// �^�C���o�[���Z�b�g��������
    /// </summary>
    public void OnReset()
    {
        GameData.GameEntity.isPlayNow = false;
        GameData.GameEntity.isTimebarReset = true;
        GameData.GameEntity.isLimitTime = false;
    }
}
