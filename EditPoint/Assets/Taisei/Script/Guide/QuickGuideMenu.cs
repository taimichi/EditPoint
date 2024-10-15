using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickGuideMenu : MonoBehaviour
{
    [SerializeField] private GameObject QuickGuideObj;
    [SerializeField] private GameObject Clip;
    [SerializeField] private GameObject Block;
    [SerializeField] private GameObject Copy;
    [SerializeField] private GameObject Blower;
    [SerializeField] private GameObject Move;

    [SerializeField] private GameObject ClipImage1;
    [SerializeField] private GameObject ClipImage2;
    [SerializeField] private GameObject LButton;
    [SerializeField] private GameObject RButton;

    public void OnClose()
    {
        QuickGuideObj.SetActive(false);
    }

    public void OnRightButton()
    {
        ClipImage1.SetActive(false);
        ClipImage2.SetActive(true);

        LButton.SetActive(true);
        RButton.SetActive(false);
    }

    public void OnLeftButton()
    {
        ClipImage1.SetActive(true);
        ClipImage2.SetActive(false);

        LButton.SetActive(false);
        RButton.SetActive(true);
    }
}
