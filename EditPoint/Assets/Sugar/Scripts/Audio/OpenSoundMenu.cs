using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSoundMenu : MonoBehaviour
{
    [SerializeField] GameObject obj;
    void Start()
    {
        obj.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (obj.activeSelf) 
            {
                obj.SetActive(false);
                Time.timeScale = 1;
            }
            else if (!obj.activeSelf) 
            {
                obj.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
