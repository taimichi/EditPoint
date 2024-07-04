using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerController : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private CutAndPaste cp;

    [SerializeField] private Text LayerNo;
    //現在選択しているレイヤー番号
    //layerNum : 0 = 7.全体, 1 = 8.layer1, 2 = 9.layer2, 3 = 10.layer3
    private int i_layerNum = 0;
    //最後に選択したレイヤー番号 1～3
    private int lastLayerNum = 1;
    [SerializeField, Header("レイヤーの最大値")] private int i_maxLayerNum;
    [SerializeField, Header("レイヤーの最小値")] private int i_minLayerNum;
    private bool b_changeLayer = false;

    //order in layerの値
    private int i_orderInLayer1Num = 5;
    private int i_orderInLayer2Num = 6;
    private int i_orderInLayer3Num = 7;

    private GameObject[] GroundLayer;
    private GameObject[] Layer1AllObj;
    private GameObject[] Layer2AllObj;
    private GameObject[] Layer3AllObj;

    [SerializeField] private GameObject LayerPanel;

    [SerializeField, Header("レイヤー表示順用　レイヤー1番")] private GameObject Layer1Rep;
    [SerializeField, Header("レイヤー表示順用　レイヤー2番")] private GameObject Layer2Rep;
    [SerializeField, Header("レイヤー表示順用　レイヤー3番")] private GameObject Layer3Rep;

    void Start()
    {
        LayerPanel.SetActive(false);
        GroundLayer = GetLayerAllObj(LayerMask.NameToLayer("Ground"));
        Layer1AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer1"));
        Layer2AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer2"));
        Layer3AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer3"));

        ChangeLayer();
    }

    void Update()
    {
        //編集モードがONの時
        if (gm.ReturnEditMode() == true)
        {
            if (!cp.ReturnSetOnOff())
            {
                Wheel();
                if (b_changeLayer)
                {
                    if (!LayerPanel.activeSelf)
                    {
                        LayerPanel.SetActive(true);
                    }
                }
                else
                {
                    if (LayerPanel.activeSelf)
                    {
                        LayerPanel.SetActive(false);
                    }
                }
                LayerReplacement();
                ChangeLayer();

            }
            else
            {

            }
        }
        LayerNo.text = i_layerNum.ToString();
    }

    //マウスホイール
    private void Wheel()
    {
        if (Input.mouseScrollDelta.y > 0 && Input.GetKey(KeyCode.R))
        {
            i_layerNum++;
            if (i_layerNum >= i_maxLayerNum)
            {
                i_layerNum = i_maxLayerNum;
            }
            b_changeLayer = true;
        }
        else if (Input.mouseScrollDelta.y < 0 && Input.GetKey(KeyCode.R))
        {
            i_layerNum--;
            if (i_layerNum <= i_minLayerNum)
            {
                i_layerNum = i_minLayerNum;
            }
            b_changeLayer = true;
        }        
    }

    //レイヤー変更時
    private void ChangeLayer()
    {
        GroundLayerChange();
        Layer1ColorChange();
        Layer2ColorChange();
        Layer3ColorChange();
        if (i_layerNum == 0)
        {
            LayerPanel.SetActive(false);
        }

        if (i_layerNum != 0)
        {
            lastLayerNum = i_layerNum;
        }
    }

    //全てのオブジェクトのレイヤーを取得
    private GameObject[] GetLayerAllObj(LayerMask layerMask)
    {
        GameObject[] goArray = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> goList = new List<GameObject>();
        
        for(int i = 0; i < goArray.Length; i++)
        {
            if(goArray[i].layer == layerMask)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }

    //その他のレイヤーオブジェクトの色合い変更
    private void GroundLayerChange()
    {
        if (i_layerNum == 0)
        {
            for(int i = 0; i < GroundLayer.Length; i++)
            {
                GroundLayer[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
        }
        else
        {
            for(int i = 0; i < GroundLayer.Length; i++)
            {
                GroundLayer[i].GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
    }

    //レイヤー1オブジェクトの色合い変更
    private void Layer1ColorChange()
    {
        //オブジェクトが配列に存在するときのみ
        if (Layer1AllObj != null)
        {
            //レイヤーが全体か1を選択しているとき
            if (i_layerNum == 0)
            {
                for(int i = 0; i < Layer1AllObj.Length; i++)
                {
                    //
                    if(Layer1AllObj[i] != null)
                    {
                        Layer1AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = i_orderInLayer1Num;
                    }
                }
            }
            else if (i_layerNum == 1)
            {
                for (int i = 0; i < Layer1AllObj.Length; i++)
                {
                    if(Layer1AllObj[i] != null)
                    {
                        Layer1AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
                    }
                }
            }
            //それ以外の時
            else
            {
                for (int i = 0; i < Layer1AllObj.Length; i++)
                {
                    if (Layer1AllObj[i] != null)
                    {
                        Layer1AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
                    }
                }
            }
        }
    }

    //レイヤー２オブジェクトの色合い変更
    private void Layer2ColorChange()
    {
        //オブジェクトが配列に存在するときのみ
        if (Layer2AllObj != null)
        {
            //レイヤーが全体か2を選択しているとき
            if(i_layerNum == 0)
            {
                for (int i = 0; i < Layer2AllObj.Length; i++)
                {
                    Layer2AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = i_orderInLayer2Num;
                }
            }
            else if (i_layerNum == 2)
            {
                for (int i = 0; i < Layer2AllObj.Length; i++)
                {
                    Layer2AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
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
    }

    //レイヤー３オブジェクトの色合い変更
    private void Layer3ColorChange()
    {
        //オブジェクトが配列に存在するときのみ
        if (Layer3AllObj != null)
        {
            //レイヤーが全体か3を選択しているとき
            if (i_layerNum == 0)
            {
                for (int i = 0; i < Layer3AllObj.Length; i++)
                {
                    Layer3AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = i_orderInLayer3Num;
                }

            }
            else if (i_layerNum == 3)
            {
                for (int i = 0; i < Layer3AllObj.Length; i++)
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

    //レイヤーの表示順を変えた時
    private void LayerReplacement()
    {
        //レイヤー1
        switch (Layer1Rep.transform.GetSiblingIndex() - 2)
        {
            case 0:
                i_orderInLayer1Num = 7;
                break;

            case 1:
                i_orderInLayer1Num = 6;
                break;

            case 2:
                i_orderInLayer1Num = 5;
                break;
        }

        //レイヤー2
        switch (Layer2Rep.transform.GetSiblingIndex() - 2)
        {
            case 0:
                i_orderInLayer2Num = 7;
                break;

            case 1:
                i_orderInLayer2Num = 6;
                break;

            case 2:
                i_orderInLayer2Num = 5;
                break;
        }

        //レイヤー3
        switch (Layer3Rep.transform.GetSiblingIndex() - 2)
        {
            case 0:
                i_orderInLayer3Num = 7;
                break;

            case 1:
                i_orderInLayer3Num = 6;
                break;

            case 2:
                i_orderInLayer3Num = 5;
                break;
        }
    }


    //現在のレイヤー番号を返す
    public int ReturnLayreNum()
    {
        return i_layerNum;
    }
    //最後に選択したレイヤー番号を返す
    public int ReturnLastLayerNum()
    {
        return lastLayerNum;
    }

    //外部からのレイヤー変更
    public void OutChangeLayerNum(int outNum)
    {
        b_changeLayer = true;
        i_layerNum = outNum;
        if (outNum != 0)
        {
            LayerPanel.SetActive(true);
        }
        ChangeLayer();
    }

    //ペースト時にレイヤーごとに分けたオブジェクト配列を上書き
    public void ChangeObjectList()
    {
        Layer1AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer1"));
        Layer2AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer2"));
        Layer3AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer3"));
    }

    //各レイヤーの表示非表示
    //レイヤー１
    public void Layer1Active()
    {
        if(Layer1AllObj != null)
        {
            for(int i = 0; i < Layer1AllObj.Length; i++)
            {
                Layer1AllObj[i].SetActive(Layer1AllObj[i].activeSelf == false ? true : false);
            }
        }
    }
    //レイヤー２
    public void Layer2Active()
    {
        if (Layer2AllObj != null)
        {
            for(int i = 0; i < Layer2AllObj.Length; i++)
            {
                Layer2AllObj[i].SetActive(Layer2AllObj[i].activeSelf == false ? true : false);
            }
        }
    }
    //レイヤー３
    public void Layer3Active()
    {
        if (Layer3AllObj != null)
        {
            for(int i = 0; i < Layer3AllObj.Length; i++)
            {
                Layer3AllObj[i].SetActive(Layer3AllObj[i].activeSelf == false ? true : false);
            }
        }
    }

    //ペースト時にオブジェクトのレイヤーに変更
    public void PasteChangeLayer(LayerMask layerMask)
    {
        LayerPanel.SetActive(true);
        switch (layerMask)
        {
            case 8:
                i_layerNum = 1;
                ChangeLayer();
                break;

            case 9:
                i_layerNum = 2;
                ChangeLayer();
                break;

            case 10:
                i_layerNum = 3;
                ChangeLayer();
                break;
        }
    }

    

}
