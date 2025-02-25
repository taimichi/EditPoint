using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Sliderにアクセスするために必要

public class LoadingProgressBar : MonoBehaviour
{
    public Slider progressBar; // 進行状況バー

    public float loadTime = 5f; // ロード完了までの時間（秒）


    private GameObject SetCanvasObj;

    private void OnEnable()
    {
        // 初期状態では進行状況を0に設定
        progressBar.value = 0f;

        // スライダーを操作不可にしてハンドルを非表示にする
        SetSliderInactive();

        // 非同期に進行状況を更新するコルーチンを開始
        StartCoroutine(LoadProcess());
    }

    private void SetSliderInactive()
    {
        // スライダーを無効化
        progressBar.interactable = false;

        //// スライダーがインタラクションされないようにする
        //GraphicRaycaster raycaster = progressBar.GetComponentInParent<GraphicRaycaster>();
        //if (raycaster != null)
        //{
        //    raycaster.enabled = false;
        //}
    }

    private IEnumerator LoadProcess()
    {
        float timeElapsed = 0f;

        while (timeElapsed < loadTime)
        {
            // 経過時間を計算し、進行状況を更新
            timeElapsed += Time.deltaTime;
            progressBar.value = timeElapsed / loadTime; // 進行状況（0-1の範囲）

            yield return null; // 次のフレームまで待機
        }

        // ロード完了後に進行状況バーを100%に設定
        progressBar.value = 1f;

        if (SetCanvasObj == null) { Debug.Log("セットされてないよ"); }
        SetCanvasObj.SetActive(true);
        this.gameObject.SetActive(false);
        Debug.Log("ロード完了！");

        yield break;
    }

    /// <summary>
    /// 表示するUI
    /// </summary>
    public GameObject SetObj
    {
        set { SetCanvasObj = value; }
    }
}
