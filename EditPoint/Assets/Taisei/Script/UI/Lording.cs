using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lording : MonoBehaviour
{
    [SerializeField] private GameObject LordingPanel;   //ローディング画面のPanel
    [SerializeField] private Slider slider;             //読み込み率を表示する
    private AsyncOperation async;                       //非同期動作で使用するAsyncOperation
    [SerializeField, Tooltip("最低ロード表示時間")] private float minLoadTime = 2f;

    private void Start()
    {
        LordingPanel.SetActive(false);
    }

    /// <summary>
    /// ローディング画面を使ってシーン遷移をする
    /// </summary>
    /// <param name="sceneName">遷移先のシーン名</param>
    public void LordScene(string sceneName)
    {
        LordingPanel.SetActive(true);
        StartCoroutine(Lord(sceneName));
    }

    /// <summary>
    /// ロードのコルーチン　
    /// </summary>
    IEnumerator Lord(string sceneName)
    {
        //ロードが開始した時間
        float startTime = Time.time;

        async = SceneManager.LoadSceneAsync(sceneName);
        
        //ロードが終わってもシーンのアクティブ化をしないようにする
        async.allowSceneActivation = false;

        //読み込みが終わるまで進捗をスライダーに表示させる
        while (async.progress < 0.9f)
        {
            //async.progressの値を0～1の間に補正
            float progressVal = Mathf.Clamp01(async.progress / 0.9f);
            slider.value = progressVal;
            yield return null;
        }

        //ロード完了待機
        while (Time.time - startTime < minLoadTime)
        {
            yield return null;
        }

        //シーンのアクティブ化を許可
        async.allowSceneActivation = true;
    }
}
