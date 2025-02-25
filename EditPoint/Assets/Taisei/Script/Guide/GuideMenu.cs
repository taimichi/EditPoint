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
    [SerializeField] private GameObject buttonGuide;
    [SerializeField] private GameObject otherGuide;

    [SerializeField] private GameObject LButton;
    [SerializeField] private GameObject RButton;
    [SerializeField] private GameObject clipGuide1;
    [SerializeField] private GameObject clipGuide2;

    private string key = "";

    private Dictionary<string, GameObject> guides;

    private void Awake()
    {
        guides = new Dictionary<string, GameObject>
        {
            {"Clip", clipGuide},
            {"Block",blockGuide },
            {"Copy",copyGuide },
            {"Move",moveGuide },
            {"Blower",blowerGuide },
            {"Button",buttonGuide },
            {"Other" ,otherGuide}
        };
    }


    void Start()
    {
        GuideMenuObj.SetActive(false);
    }


    public void OnCloseGuide()
    {
        key = "";
        DeactiveAll();

        GuideMenuObj.SetActive(false);
    }

    /// <summary>
    /// 操作説明メニューを開く
    /// </summary>
    public void OnOpenGuide()
    {
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
    /// クリップ操作説明
    /// </summary>
    public void OnClipGuide()
    {
        key = "Clip";
        ActiveOnOff();

        clipGuide1.SetActive(true);
        clipGuide2.SetActive(false);

        LButton.SetActive(false);
        RButton.SetActive(true);
    }

    /// <summary>
    /// ブロック生成操作説明
    /// </summary>
    public void OnBlockGuide()
    {
        key = "Block";
        ActiveOnOff();
    }

    /// <summary>
    /// 送風機操作説明
    /// </summary>
    public void OnBlowerGuide()
    {
        key = "Blower";
        ActiveOnOff();
    }

    /// <summary>
    /// コピペ操作説明
    /// </summary>
    public void OnCopyGuide()
    {
        key = "Copy";
        ActiveOnOff();
    }

    /// <summary>
    /// 移動操作説明
    /// </summary>
    public void OnMoveGuide()
    {
        key = "Move";
        ActiveOnOff();
    }

    /// <summary>
    /// ボタン操作説明
    /// </summary>
    public void OnButtonGuide()
    {
        key = "Button";
        ActiveOnOff();
    }

    public void OnOtherGuide()
    {
        key = "Other";
        ActiveOnOff();
    }

    /// <summary>
    /// keyで設定してあるオブジェクトを表示、それ以外は非表示にする
    /// </summary>
    private void ActiveOnOff()
    {
        DeactiveAll();
        foreach (var Obj in guides)
        {
            Obj.Value.SetActive(Obj.Key == key);
        }

    }

    /// <summary>
    /// 全操作方法を非表示にする
    /// </summary>
    private void DeactiveAll()
    {
        foreach (var Obj in guides)
        {
            Obj.Value.SetActive(false);
        }
    }

}
