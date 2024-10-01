using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoSignal : MonoBehaviour
{
    private float timer = 0f;
    private float maxTime = 1.5f;

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}

        if (timer >= maxTime)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        timer += Time.fixedDeltaTime;
    }

    public void OnReLode()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
