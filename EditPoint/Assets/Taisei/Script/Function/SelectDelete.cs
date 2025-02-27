using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDelete : MonoBehaviour
{
    private Canvas deleteCanvas;
    private RectTransform canvasRect;

    [SerializeField] private GameObject DeleteButton;
    private RectTransform buttonRect;
    private GetClip getClip;
    private ObjectScaleEditor objectScale;
    private GameObject SelectObj;
    private Camera UIcamera;

    /// <summary>
    /// false = clip, true = object
    /// </summary>
    private bool isTrigger;                 //クリップかオブジェクトかどうか

    private void Awake()
    {
        deleteCanvas = this.gameObject.GetComponent<Canvas>();
        canvasRect = this.gameObject.GetComponent<RectTransform>();

        UIcamera = GameObject.Find("UICamera").GetComponent<Camera>();

        buttonRect = DeleteButton.GetComponent<RectTransform>();
    }

    /// <summary>
    /// ボタンを表示するか非表示にするか
    /// </summary>
    public void SetActiveButton(bool set)
    {
        DeleteButton.SetActive(set);
    }

    //デリートボタンを押したとき
    public void OnDelete()
    {
        Debug.Log("削除");
        //クリップ削除
        if (!isTrigger)
        {
            getClip.ClipDestroy();
        }
        //オブジェクト削除
        else
        {
            objectScale.ObjectDelete();
        }
        DeleteButton.SetActive(false);
    }

    public void GetSelectObject(bool trigger, GameObject obj)
    {
        SetActiveButton(true);
        SelectObj = obj;
        isTrigger = trigger;

        //ボタンをオブジェクトの位置に移動
        var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, SelectObj.transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out var newPos);
        buttonRect.localPosition = newPos;
    }

    //スクリプトを取得
    public void Get_getClip(GameObject obj)
    {
        getClip = obj.GetComponent<GetClip>();
    }
    public void Get_objectScaleEditor(GameObject obj)
    {
        objectScale = obj.GetComponent<ObjectScaleEditor>();
    }


}
