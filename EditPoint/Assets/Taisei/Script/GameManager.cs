using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool b_start = false;

    private void Awake()
    {
        b_start = false;
    }

    void Start()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        //デバッグ用機能
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("時間変更");
                Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            }
        }
    }


    public void OnStart()
    {
        if (!b_start)
        {
            Time.timeScale = 1;
            b_start = true;
        }
    }

    public void OnReset()
    {
        b_start = false;
    }
}
