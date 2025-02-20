using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{
    /*----その他変数（コンポーネントとかスクリプト）----*/
    [SerializeField]
    Fade fade;          // FadeCanvas

    private PlaySound playSound;
    [SerializeField] private GameObject audioPanel;

    [SerializeField] private GameObject LordUI;
    [SerializeField] private Slider Slider;
    private AsyncOperation async;

    private void Start()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
    }

    public void STARTBUTTON()
    {
        // フェード
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene("Select");
        });
    }

    private void Update()
    {
        if (audioPanel.activeSelf) { return; }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            playSound.PlaySE(PlaySound.SE_TYPE.enter);
            // フェード
            fade.FadeIn(0.5f, () => {
                SceneManager.LoadScene("Talk");
                StartCoroutine("LordData");
            });

        }
    }

    IEnumerator LordData()
    {
        // シーンの読み込みをする
        async = SceneManager.LoadSceneAsync("Load1");

        //　読み込みが終わるまで進捗状況をスライダーの値に反映させる
        while (!async.isDone)
        {
            var progressVal = Mathf.Clamp01(async.progress / 0.9f);
            Slider.value = progressVal;
            yield return null;
        }
    }
}
}
