using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool b_start = false;
    private string s_nowSceneName = "";

    private PlaySound playSound;

    private void Awake()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        b_start = false;

        s_nowSceneName = SceneManager.GetActiveScene().name;
        switch (s_nowSceneName)
        {
            case "Title":
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
            playSound.PlayBGM(PlaySound.BGM_TYPE.stage1);
            playSound.PlaySE(PlaySound.SE_TYPE.start);
            Time.timeScale = 0;
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
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("時間変更");
                Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            }
        }
        Debug.Log(ModeData.ModeEntity.mode);
    }


    public void OnStart()
    {
        if (!b_start)
        {
            Time.timeScale = 1;
            b_start = true;
        }
    }

    public void OnReset()
    {
        b_start = false;
        Time.timeScale = 0;
    }
}
