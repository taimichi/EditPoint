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

    /// <summary>
    /// �ȗ����p
    /// </summary>
    private Dictionary<string, GameObject> QuickGuides;

    private void Awake()
    {
        QuickGuides = new Dictionary<string, GameObject>
        {
            {"Clip", Clip },
            {"Block", Block },
            {"Copy", Copy },
            {"Blower", Blower },
            {"Move", Move }
        };
    }

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

    /// <summary>
    /// �ŏ��ɑ�����@��\������
    /// </summary>
    /// <param name="_key">�\��������������@</param>
    public void StartGuide(string _key)
    {
        QuickGuideObj.SetActive(true);
        DeactiveAll();  //��U���ׂĔ�\����
        //����̃I�u�W�F�N�g�����\��
        foreach(var Obj in QuickGuides)
        {
            Obj.Value.SetActive(Obj.Key == _key);
        }

        switch (_key)
        {
            case "Clip":
                TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.clip;
                break;

            case "Block":
                TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.block;
                break;

            case "Copy":
                TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.copy;
                break;

            case "Blower":
                TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.blower;
                break;

            case "Move":
                TutorialData.TutorialEntity.frags |= TutorialData.Tutorial_Frags.move;
                break;
        }
    }

    /// <summary>
    /// �S������@���\���ɂ���
    /// </summary>
    private void DeactiveAll()
    {
        foreach(var Obj in QuickGuides)
        {
            Obj.Value.SetActive(false);
        }
    }


}
