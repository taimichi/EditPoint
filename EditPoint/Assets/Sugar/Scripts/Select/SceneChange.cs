using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// ステージ番号の構造体 <br/>
    /// worldNum = ステージ"N-n"のNの部分 <br/>
    /// stageNum = ステージ"N-n"のnの部分 <br/>
    /// 両方の数値が0の場合はステージなし
    /// </summary>
    [System.Serializable]
    public struct STAGA_NUMBER
    {
        public int worldNum;    //ワールド番号
        public int stageNum;    //ステージ番号
    }
    public STAGA_NUMBER stageAddress;

    string sceneName;
   
    Fade fade;          // FadeCanvas
    bool isOn = false;

    Lording lording;

    private NewStageData stageDataScript;
    private bool isLock = true; //false=ロックされてる / true=ロックされてない

    private void Start()
    {
        lording = GameObject.FindWithTag("Lording").GetComponent<Lording>();
        SarchStage();
    }

    private void OnEnable()
    {
        Debug.Log("起動");
        if(stageAddress.worldNum == 0 && stageAddress.stageNum == 0)
        {
            this.gameObject.SetActive(false);
            return;
        }
        if (isOn)
        {
            return;
        }
        fade = GameObject.Find("GameFade").GetComponent<Fade>();
        // フェード
        fade.FadeIn(0.5f, () => {
            //SceneManager.LoadScene(sceneName);
            isOn = true;
            lording.LordScene(sceneName);
        });
    }

    /// <summary>
    /// ステージデータ群から設定してあるステージを探す
    /// </summary>
    public void SarchStage()
    {
        stageDataScript = NewStageData.StageEntity;
        //ステージシーン名をステージデータから探し取得
        if (stageAddress.worldNum != 0 && stageAddress.stageNum != 0)
        {
            for (int i = 0; i < stageDataScript.stageData.Length; i++)
            {
                //ステージデータのワールド番号と設定してあるワールド番号が同じとき
                if (stageDataScript.stageData[i].worldNum == stageAddress.worldNum)
                {
                    //ステージデータのステージ番号と設定してあるステージ番号が同じとき
                    if (stageDataScript.stageData[i].stageNum == stageAddress.stageNum)
                    {
                        //ステージデータからステージシーン名を取得
                        sceneName = stageDataScript.stageData[i].stageName;
                        if(stageDataScript.stageData[i].stagelock == NewStageData.StageLock.Lock)
                        {
                            isLock = false;
                        }
                        break;
                    }
                }
            }
        }
    }

    public bool ReturnIsLock() => isLock;
}
