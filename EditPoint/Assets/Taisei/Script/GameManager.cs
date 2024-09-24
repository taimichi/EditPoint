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
                break;

            case "Talk":
                playSound.PlayBGM(PlaySound.BGM_TYPE.talk);
                break;

            case "Select":
                playSound.PlayBGM(PlaySound.BGM_TYPE.title_stageSelect);
                break;

            case "Stage1":
                playSound.PlayBGM(PlaySound.BGM_TYPE.stage1);
                playSound.PlaySE(PlaySound.SE_TYPE.start);
                Time.timeScale = 0;
                break;

            case "Stage2":
                playSound.PlayBGM(PlaySound.BGM_TYPE.stage1);
                playSound.PlaySE(PlaySound.SE_TYPE.start);
                Time.timeScale = 0;
                break;

            case "Stage3":
                playSound.PlayBGM(PlaySound.BGM_TYPE.stage1);
                playSound.PlaySE(PlaySound.SE_TYPE.start);
                Time.timeScale = 0;
                break;


        }

    }

    void Start()
    {

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
