using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //編集モード
    private bool editMode;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("時間変更");
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
    }

    public bool ReturnEditMode()
    {
        return editMode;
    }
}
