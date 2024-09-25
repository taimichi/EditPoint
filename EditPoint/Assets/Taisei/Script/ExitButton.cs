using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    private Fade fade;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("FadeCanvas"))
        {
            fade = GameObject.Find("FadeCanvas").GetComponent<Fade>();
        }
        else
        {
            fade = GameObject.Find("GameFade").GetComponent<Fade>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// タイトルシーンへ戻る
    /// </summary>
    public void OnExitButton()
    {
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene("Title");
        });
    }
}
