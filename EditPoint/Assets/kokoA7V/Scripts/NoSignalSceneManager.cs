using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoSignalSceneManager : MonoBehaviour
{
    public string SceneName;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}