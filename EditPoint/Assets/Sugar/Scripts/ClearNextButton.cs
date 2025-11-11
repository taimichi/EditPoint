using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClearNextButton : MonoBehaviour
{
    [Header("ステージ情報"), SerializeField] NewStageData std;
    [Header("フェードオブジェクト"), SerializeField] Fade fade;

    string nowStageName;
    string NextStageName;

    bool Click = false;

    private PlaySound playSound;

    void Start()
    {
        // 現在のシーン名を取得
        nowStageName = SceneManager.GetActiveScene().name;
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < std.stageData.Length; i++)
        {
            // 現在シーンの次のシーン名を取得
            if(nowStageName==std.stageData[i].stageName)
            {
                Debug.Log("DATA" + i);
                Debug.Log(std.stageData[i].stageName.Length);
                // 最大値を越したら0に戻す
                if (i + 1 == std.stageData.Length)
                {
                    NextStageName = std.stageData[0].stageName;
                }
                else
                {
                    NextStageName = std.stageData[i + 1].stageName;
                    std.stageData[i + 1].stagelock = NewStageData.StageLock.Open;
                }
            }
        }
    }

    public void NextButton()
    {
        if (Click) { return; }
        Click = true;
        playSound.PlaySE(PlaySound.SE_TYPE.sceneChange);
        fade.FadeIn(1.5f, () => SceneManager.LoadScene(NextStageName));
    }
}
