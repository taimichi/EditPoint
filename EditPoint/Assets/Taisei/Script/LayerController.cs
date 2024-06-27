using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerController : MonoBehaviour
{
    [SerializeField] private CutAndPaste cp;

    [SerializeField] private Text LayerNo;
    //現在選択しているレイヤー番号
    //layerNum : 0 = 7.全体, 1 = 8.layer1, 2 = 9.layer2, 3 = 10.layer3
    private int layerNum = 0;
    //最後に選択したレイヤー番号 1～3
    private int lastLayerNum = 1;
    [SerializeField, Header("レイヤーの最大値")] private int maxLayerNum;
    [SerializeField, Header("レイヤーの最小値")] private int minLayerNum;
    private bool changeLayer = false;

    private GameObject[] GroundLayer;
    private GameObject[] Layer1AllObj;
    private GameObject[] Layer2AllObj;
    private GameObject[] Layer3AllObj;

    [SerializeField] private GameObject LayerPanel;

    // Start is called before the first frame update
    void Start()
    {
        LayerPanel.SetActive(false);
        GroundLayer = GetLayerAllObj(LayerMask.NameToLayer("Ground"));
        Layer1AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer1"));
        Layer2AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer2"));
        Layer3AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer3"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!cp.ReturnSetOnOff())
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
        else
        {

        }
    }

    //マウスホイール
    private void Wheel()
    {
        if (Input.mouseScrollDelta.y > 0 && Input.GetKey(KeyCode.R))
        {
            layerNum++;
            if (layerNum >= maxLayerNum)
            {
                layerNum = maxLayerNum;
            }
            changeLayer = true;
        }
        else if (Input.mouseScrollDelta.y < 0 && Input.GetKey(KeyCode.R))
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
        GroundLayerChange();
        Layer1ColorChange();
        Layer2ColorChange();
        Layer3ColorChange();
        if (layerNum == 0)
        {
            LayerPanel.SetActive(false);
        }

        if (layerNum != 0)
        {
            lastLayerNum = layerNum;
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
        if (layerNum == 0)
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
            if (layerNum == 1 || layerNum == 0)
            {
                for (int i = 0; i < Layer1AllObj.Length; i++)
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
    }

    //レイヤー２オブジェクトの色合い変更
    private void Layer2ColorChange()
    {
        //オブジェクトが配列に存在するときのみ
        if (Layer2AllObj != null)
        {
            //レイヤーが全体か2を選択しているとき
            if (layerNum == 2 || layerNum == 0)
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
            if (layerNum == 3 || layerNum == 0)
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

    //現在のレイヤー番号を返す
    public int ReturnLayreNum()
    {
        return layerNum;
    }
    //最後に選択したレイヤー番号を返す
    public int ReturnLastLayerNum()
    {
        return lastLayerNum;
    }

    //外部からのレイヤー変更
    public void OutChangeLayerNum(int outNum)
    {
        changeLayer = true;
        layerNum = outNum;
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
}
