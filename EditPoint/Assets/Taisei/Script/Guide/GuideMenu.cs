using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideMenu : MonoBehaviour
{
    [SerializeField] private GameObject GuideMenuObj;
    private float nowTimeScale = 0;

    [SerializeField] private GameObject clipGuide;
    [SerializeField] private GameObject blockGuide;
    [SerializeField] private GameObject copyGuide;
    [SerializeField] private GameObject moveGuide;
    [SerializeField] private GameObject blowerGuide;

    [SerializeField] private GameObject LButton;
    [SerializeField] private GameObject RButton;
    [SerializeField] private GameObject clipGuide1;
    [SerializeField] private GameObject clipGuide2;


    void Start()
    {
        //GuideMenuObj.SetActive(false);
    }


    public void OnCloseGuide()
    {
        Time.timeScale = nowTimeScale;

        blockGuide.SetActive(false);
        clipGuide.SetActive(false);
        blowerGuide.SetActive(false);
        copyGuide.SetActive(false);
        moveGuide.SetActive(false);

        GuideMenuObj.SetActive(false);
    }

    public void OnOpenGuide()
    {
        nowTimeScale = Time.timeScale;
        Time.timeScale = 0;
        GuideMenuObj.SetActive(true);
    }

    public void OnLButton()
    {
        clipGuide1.SetActive(true);
        clipGuide2.SetActive(false);

        LButton.SetActive(false);
        RButton.SetActive(true);
    }

    public void OnRButton()
    {
        clipGuide1.SetActive(false);
        clipGuide2.SetActive(true);

        LButton.SetActive(true);
        RButton.SetActive(false);

    }

    /// <summary>
    /// �N���b�v�������
    /// </summary>
    public void OnClipGuide()
    {
        clipGuide.SetActive(true);

        clipGuide1.SetActive(true);
        clipGuide2.SetActive(false);

        LButton.SetActive(false);
        RButton.SetActive(true);


        blockGuide.SetActive(false);
        blowerGuide.SetActive(false);
        copyGuide.SetActive(false);
        moveGuide.SetActive(false);
    }

    /// <summary>
    /// �u���b�N�����������
    /// </summary>
    public void OnBlockGuide()
    {
        blockGuide.SetActive(true);

        clipGuide.SetActive(false);
        blowerGuide.SetActive(false);
        copyGuide.SetActive(false);
        moveGuide.SetActive(false);
    }

    /// <summary>
    /// �����@�������
    /// </summary>
    public void OnBlowerGuide()
    {
        blowerGuide.SetActive(true);

        clipGuide.SetActive(false);
        blockGuide.SetActive(false);
        copyGuide.SetActive(false);
        moveGuide.SetActive(false);
    }

    /// <summary>
    /// �R�s�y�������
    /// </summary>
    public void OnCopyGuide()
    {
        copyGuide.SetActive(true);

        clipGuide.SetActive(false);
        blockGuide.SetActive(false);
        blowerGuide.SetActive(false);
        moveGuide.SetActive(false);
    }

    /// <summary>
    /// �ړ��������
    /// </summary>
    public void OnMoveGuide()
    {
        moveGuide.SetActive(true);

        clipGuide.SetActive(false);
        blockGuide.SetActive(false);
        blowerGuide.SetActive(false);
        copyGuide.SetActive(false);
    }
}
