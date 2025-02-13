using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFileButton : MonoBehaviour
{
    [SerializeField] bool isEnd;
    [SerializeField] GameObject panel;
    private void OnEnable()
    {
        // はいを押したらここで終了
        if (isEnd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
        // いいえを押したらパネルを閉じる
        else
        {
            // 自身を非表示に
            panel.SetActive(false);
            this.gameObject.SetActive(false);

        }
    }
}
