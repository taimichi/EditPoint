using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移

public class TitleButton : MonoBehaviour
{
    /*----その他変数（コンポーネントとかスクリプト）----*/
    [SerializeField]
    Fade fade;          // FadeCanvas
    public void STARTBUTTON()
    {
        // フェード
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene("Select");
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // フェード
            fade.FadeIn(0.5f, () => {
                SceneManager.LoadScene("Select");
            });

        }
    }
}
