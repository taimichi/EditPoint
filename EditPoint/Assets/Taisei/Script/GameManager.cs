using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�ҏW���[�h
    private bool editMode;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            editMode = editMode == false ? true : false;
        }
    }

    public bool ReturnEditMode()
    {
        return editMode;
    }
}
