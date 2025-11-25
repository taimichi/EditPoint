using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearGroupButton : MonoBehaviour
{
    [SerializeField] Fade F_canvas;
    private NewStageData stageData;

    private void Start()
    {
        stageData = NewStageData.StageEntity;

        string nowStageName = SceneManager.GetActiveScene().name;
        string NextStageName = "";

        //次のシーン名を取得
        for (int i = 0; i < stageData.stageData.Length; i++)
        {
            // 現在シーンの次のシーン名を取得
            if (nowStageName == stageData.stageData[i].stageName)
            {
                Debug.Log("DATA" + i);
                Debug.Log(stageData.stageData[i].stageName.Length);
                stageData.stageData[i].isStageClear = true;
                // 最大値を越したら0に戻す
                if (i + 1 == stageData.stageData.Length)
                {
                    NextStageName = stageData.stageData[0].stageName;
                }
                else
                {
                    NextStageName = stageData.stageData[i + 1].stageName;
                    //次のステージが新しいワールドの1ステージ目
                    //かつ
                    //ステージがロック状態だった時
                    if (stageData.stageData[i + 1].stageNum == 1 && stageData.stageData[i + 1].stagelock == NewStageData.StageLock.Lock)
                    {
                        GameObject NextButton = GameObject.Find("Next");
                        if (NextButton != null)
                        {
                            NextButton.SetActive(false);
                        }
                    }
                    stageData.stageData[i + 1].stagelock = NewStageData.StageLock.FirstUnLock;
                }
            }
        }

    }

    public void NextButton()
    {
        PlaySound playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        playSound.PlaySE(PlaySound.SE_TYPE.sceneChange);

        // フェード
        F_canvas.FadeIn(1.5f, () => {
            SceneManager.LoadScene("Select");
        });
    }
}
