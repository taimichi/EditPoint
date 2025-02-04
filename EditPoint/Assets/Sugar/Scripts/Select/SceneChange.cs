using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    [SerializeField] string sceneName;
   
    Fade fade;          // FadeCanvas
    bool isOn = false;

    private void OnEnable()
    {
        Debug.Log("起動");
        if(sceneName=="")
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
            SceneManager.LoadScene(sceneName);
            isOn = true;
        });
    }
}
