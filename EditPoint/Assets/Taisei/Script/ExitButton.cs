using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    // GameObject.Findだとわからん；；bykoko20240926
    [SerializeField]
    private Fade fade;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("FadeCanvas"))
        {
            fade = GameObject.Find("FadeCanvas").GetComponent<Fade>();
        }
        else if(GameObject.Find("GameFade"))
        {
            fade = GameObject.Find("GameFade").GetComponent<Fade>();
        }
        else
        {
            // else入れますbykoko20240926
            Debug.Log("canvasないお～");
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
        // どうやら動いてないっぽいっす！理由不明！bykoko20240926
        Debug.Log("osareta");
        if (fade != null)
        {

            Debug.Log("fade");
            fade.FadeIn(0.5f, () =>
            {
                SceneManager.LoadScene("Title");
            });
        }
        else
        {
            Debug.Log("no fade");
            SceneManager.LoadScene("Title");
        }
    }
}
