using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestClear : MonoBehaviour
{
    [SerializeField] Fade F_canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextButton()
    {
        // フェード
        F_canvas.FadeIn(0.5f, () => {
            SceneManager.LoadScene("Select");
        });
    }
}
