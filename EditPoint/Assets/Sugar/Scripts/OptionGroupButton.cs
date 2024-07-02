using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionGroupButton : MonoBehaviour
{
    [SerializeField] Fade fade; // FadeCanvas
    string StageName;

    private void Start()
    {
        fade.FadeIn(0.01f, () => {
            fade.FadeOut(0.5f);
        });
        StageName = SceneManager.GetActiveScene().name;
    }
    public void GroupSettings()
    {
        Debug.Log("A_Button");
    }
    public void GroupRestart() // シーンリロード
    {
        Debug.Log("B_Button");
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene(StageName);
        });
    }
    public void GroupTitle() // タイトルシーンへ
    {
        Debug.Log("C_Button");
        fade.FadeIn(0.5f, () => { 
            SceneManager.LoadScene("TitleScene");
        });
    }
}
