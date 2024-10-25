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
            }
            else if (!obj.activeSelf) 
            {
                obj.SetActive(true);
            }
        }
    }
}
