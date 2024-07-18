using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearGroupButton : MonoBehaviour
{
    [SerializeField] Fade F_canvas;
    public void NextButton()
    {
        // フェード
        F_canvas.FadeIn(0.5f, () => {
            SceneManager.LoadScene("Select");
        });
    }
}
