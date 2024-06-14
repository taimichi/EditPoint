using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerChange : MonoBehaviour
{
    [SerializeField] private Text LayerNo;
    //layerNum : 0 = 7.全体, 1 = 8.layer1, 2 = 9.layer2, 3 = 10.layer3
    private int layerNum = 0;
    [SerializeField, Header("レイヤーの最大値")] private int maxLayerNum;
    [SerializeField, Header("レイヤーの最小値")] private int minLayerNum;
    private bool changeLayer = false;

    private GameObject[] Layer1AllObj;
    private GameObject[] Layer2AllObj;
    private GameObject[] Layer3AllObj;

    [SerializeField] private GameObject LayerPanel;

    // Start is called before the first frame update
    void Start()
    {
        Layer1AllObj = GetLayerAllObj(8);
        Layer2AllObj = GetLayerAllObj(9);
        Layer3AllObj = GetLayerAllObj(10);
    }

    // Update is called once per frame
    void Update()
    {
        Wheel();
        if (changeLayer)
        {
            if (!LayerPanel.activeSelf)
            {
                LayerPanel.SetActive(true);
            }
            ChangeLayer();
        }
        else
        {
            if (LayerPanel.activeSelf)
            {
                LayerPanel.SetActive(false);
            }
        }
    }

    //マウスホイール
    private void Wheel()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            layerNum++;
            if (layerNum >= maxLayerNum)
            {
                layerNum = maxLayerNum;
            }
            changeLayer = true;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            layerNum--;
            if (layerNum <= minLayerNum)
            {
                layerNum = minLayerNum;
            }
            changeLayer = true;
        }

        LayerNo.text = layerNum.ToString();
        
    }

    //レイヤー変更時
    private void ChangeLayer()
    {
        Layer1ColorChange();
        Layer2ColorChange();
        Layer3ColorChange();
    }

    //全てのオブジェクトのレイヤーを取得
    private GameObject[] GetLayerAllObj(LayerMask layerMask)
    {
        GameObject[] goArray = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> goList = new List<GameObject>();
        foreach (GameObject go in goArray)
        {
            // LayerMask bit check
            if (((1 << go.layer) & layerMask.value) != 0)
            {
                goList.Add(go);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }

    //レイヤーの番号を返す
    public int ReturnLayerNum()
    {
        return layerNum;
    }

    //レイヤー1オブジェクトの色合い変更
    private void Layer1ColorChange()
    {
        //レイヤーが全体か1を選択しているとき
        if (layerNum == 1 || layerNum == 0)
        {
            for(int i = 0; i < Layer1AllObj.Length; i++)
            {
                Layer1AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
        }
        //それ以外の時
        else
        {
            for (int i = 0; i < Layer1AllObj.Length; i++)
            {
                Layer1AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
        }
    }

    //レイヤー２オブジェクトの色合い変更
    private void Layer2ColorChange()
    {
        //レイヤーが全体か2を選択しているとき
        if (layerNum == 2 || layerNum == 0)
        {
            for(int i = 0; i < Layer2AllObj.Length; i++)
            {
                Layer1AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
        }
        //それ以外の時
        else
        {
            for (int i = 0; i < Layer2AllObj.Length; i++)
            {
                Layer2AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
        }
    }

    //レイヤー３オブジェクトの色合い変更
    private void Layer3ColorChange()
    {
        //レイヤーが全体か3を選択しているとき
        if (layerNum == 3 || layerNum == 0)
        {
            for(int i = 0; i < Layer3AllObj.Length; i++)
            {
                Layer3AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
        }
        //それ以外の時
        else
        {
            for (int i = 0; i < Layer3AllObj.Length; i++)
            {
                Layer3AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
        }
    }
}
