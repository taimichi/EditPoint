using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�ҏW���[�h
    private bool editMode;

    void Start()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("���ԕύX");
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
    }

    public bool ReturnEditMode()
    {
        return editMode;
    }
}
