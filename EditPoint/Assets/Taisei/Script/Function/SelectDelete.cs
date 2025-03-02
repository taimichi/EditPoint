using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDelete : MonoBehaviour
{
    [SerializeField] private GameObject DeleteButton;
    private RectTransform buttonRect;
    private GetClip getClip;
    private ObjectScaleEditor objectScale;
    private GameObject SelectObj;
    private Camera UIcamera;

    /// <summary>
    /// false = clip/true = object
    /// </summary>
    private bool isTrigger;                 //クリップかオブジェクトかどうか

    private void Awake()
    {
        UIcamera = GameObject.Find("UICamera").GetComponent<Camera>();

        buttonRect = DeleteButton.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (DeleteButton.activeSelf)
        {
            //ボタンをオブジェクトの位置に移動
            //クリップの場合
            if (!isTrigger)
            {
                RectTransform rect = SelectObj.GetComponent<RectTransform>();
                Vector3[] newPos = new Vector3[4];
                rect.GetWorldCorners(newPos);

                buttonRect.position = RectTransformUtility.WorldToScreenPoint(UIcamera, newPos[2]);
            }
            //オブジェクトの場合
            else
            {
                Bounds bounds = SelectObj.GetComponent<SpriteRenderer>().bounds;
                Vector3 newPos = new Vector3(bounds.max.x, bounds.max.y, 0);

                buttonRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, newPos);
            }
        }
    }

    /// <summary>
    /// ボタンを表示するか非表示にするか
    /// </summary>
    public void SetActiveButton(bool set)
    {
        //非表示にするとき
        if (!set)
        {
            //オブジェクトを空にする
            SelectObj = null;
        }
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
        SelectObj = null;
        DeleteButton.SetActive(false);
    }

    /// <summary>
    /// デリートボタンを起動する
    /// </summary>
    /// <param name="trigger">false=クリップ/true=オブジェクト</param>
    /// <param name="obj">選択したオブジェクト</param>
    public void ButtonActive(bool trigger, GameObject obj)
    {
        SelectObj = obj;
        isTrigger = trigger;

        if(SelectObj != null)
        {
            SetActiveButton(true);
        }

        PosCalculation();
    }

    /// <summary>
    /// デリートボタンの位置を計算
    /// </summary>
    private void PosCalculation()
    {
        //ボタンをオブジェクトの位置に移動
        //クリップの場合
        if (!isTrigger)
        {
            RectTransform rect = SelectObj.GetComponent<RectTransform>();
            Vector3[] newPos = new Vector3[4];
            rect.GetWorldCorners(newPos);

            buttonRect.position = RectTransformUtility.WorldToScreenPoint(UIcamera, newPos[2]);
        }
        //オブジェクトの場合
        else
        {
            Bounds bounds = SelectObj.GetComponent<SpriteRenderer>().bounds;
            Vector3 newPos = new Vector3(bounds.max.x, bounds.max.y, 0);

            buttonRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, newPos);
        }
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
