using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkStart : MonoBehaviour
{
    [SerializeField] Fade fade;          // FadeCanvas
    [SerializeField] GameObject TalkCanvas;
    [SerializeField] GameObject fadeObj;
    void Start()
    {
        // 非表示状態だったら表示する
        if (!fadeObj.activeSelf) {fadeObj.SetActive(true); }
        // フェード機能を使ったら非表示に
        fade.FadeIn(0.7f, () => {
            Time.timeScale = 0;    // 時間停止
            TalkCanvas.SetActive(true); // 会話イベントの始まり
            fadeObj.SetActive(false);
        });
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartTalk();
        }

        if(!TalkCanvas.activeSelf)
        {
            Time.timeScale = 1.0f;
        }
    }

    public void StartTalk()
    {
        // 非表示状態だったら表示する
        if (!fadeObj.activeSelf) { fadeObj.SetActive(true); }
        // フェード機能を使ったら非表示に
        fade.FadeIn(0.7f, () => {
            Time.timeScale = 0;    // 時間停止
            TalkCanvas.SetActive(true); // 会話イベントの始まり
            fadeObj.SetActive(false);
        });
    }
}
