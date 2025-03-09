using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearGroupButton : MonoBehaviour
{
    [SerializeField] Fade F_canvas;

    public void NextButton()
    {
        PlaySound playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        playSound.PlaySE(PlaySound.SE_TYPE.sceneChange);
        // フェード
        F_canvas.FadeIn(1.5f, () => {
            SceneManager.LoadScene("Select");
        });
    }
}
