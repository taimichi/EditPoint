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
    void Start()
    {
        // 現在のシーン名を取得
        nowStageName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < std.STAGE_DATA.Count; i++)
        {
            // 現在シーンの次のシーン名を取得
            if(nowStageName==std.STAGE_DATA[i])
            {
                Debug.Log("DATA" + i);
                Debug.Log(std.STAGE_DATA.Count);
                // 最大値を越したら0に戻す
                if (i+1 == std.STAGE_DATA.Count)
                {
                    NextStageName = std.STAGE_DATA[0];
                }
                else
                {
                    NextStageName = std.STAGE_DATA[i + 1];
                }
            }
        }
    }

    public void NextButton()
    {
        if (Click) { return; }
        Click = true;
        fade.FadeIn(0.5f, () => SceneManager.LoadScene(NextStageName));
    }
}
