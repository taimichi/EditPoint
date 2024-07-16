using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CopyAndPaste : MonoBehaviour
{
    [SerializeField] private Text copyModeText;

    [SerializeField] private RangeSelection rangeSelect;
    //コピー元のオブジェクト
    private GameObject CopyObj;

    [SerializeField] private LayerController layerControll;

    [SerializeField, Header("ペーストできる回数")] private int i_PasteNum = 1000;
    [SerializeField, Header("コピーできる回数")] private int i_CopyNum = 1000;

    //オブジェクトのペーストや移動するときにtrueにする
    private bool b_setOnOff = false;

    //ペースト時のオブジェクト
    private GameObject PasteObj;

    //複数用
    private GameObject[] CopyObjs;
    private GameObject[] PasteObjs;
    private Vector3[] v3_offset;    //オブジェクト同士の距離を保存

    //マウスの座標関連
    private Vector3 v3_mousePos;
    private Vector3 v3_scrWldPos;

    //選択したオブジェクト
    private GameObject ClickObj;
    //選択時と非選択時に変更するマテリアル
    //0 = デフォルトマテリアル
    //1 = 選択時のアウトラインマテリアル
    [SerializeField] private Material[] materials = new Material[2];

    private PlayerLayer plLayer;

    private bool b_copyMode = false;
    private bool b_pasteMode = false;

    void Start()
    {
        plLayer = GameObject.Find("Player").GetComponent<PlayerLayer>();
        CopyObj = null;
        PasteObj = null;
        copyModeText.enabled = false;
    }

    void Update()
    {
        //ペースト・オブジェクト移動状態じゃないとき
        if (b_copyMode)
        {
            Copy();
            return;
        }

        if(b_setOnOff && b_pasteMode)
        {
            v3_mousePos = Input.mousePosition;
            v3_mousePos.z = 10;

            v3_scrWldPos = Camera.main.ScreenToWorldPoint(v3_mousePos);

            if (!rangeSelect.ReturnSelectNow())
            {
                PasteObj.transform.position = v3_scrWldPos;
            }
            else
            {
                for (int i = 0; i < PasteObjs.Length; i++)
                {
                    PasteObjs[i].transform.position = v3_scrWldPos + v3_offset[i];
                }
            }

            //UIの上じゃなかったら
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //クリックしたらカーソルの位置にオブジェクトを置く
                if (Input.GetMouseButtonDown(0) && !plLayer.ReturnPlTrigger())
                {
                    //単体
                    if (!rangeSelect.ReturnSelectNow())
                    {
                        PasteObj.GetComponent<SpriteRenderer>().material = materials[0];
                        layerControll.ChangeObjectList();

                    }
                    //複数
                    else
                    {
                        for (int i = 0; i < PasteObjs.Length; i++)
                        {
                            PasteObjs[i].GetComponent<SpriteRenderer>().material = materials[0];
                            layerControll.ChangeObjectList();

                        }
                    }
                    Paste();

                }
                else if (plLayer.ReturnPlTrigger())
                {
                    //Debug.Log("おけません");
                }
            }

            //右クリックでペーストモード解除
            if (Input.GetMouseButtonDown(1))
            {
                if (!rangeSelect.ReturnSelectNow())
                {
                    Destroy(PasteObj);
                    PasteObj = null;
                    layerControll.ChangeObjectList();
                }
                else
                {
                    for (int i = 0; i < PasteObjs.Length; i++)
                    {
                        Destroy(PasteObjs[i]);
                        PasteObjs[i] = null;
                        layerControll.ChangeObjectList();
                    }
                }
                b_pasteMode = false;
                b_setOnOff = false;
                copyModeText.enabled = false;
            }
        }
        
    }

    public bool ReturnSetOnOff()
    {
        return b_setOnOff;
    }

    private void Copy()
    {
        //クリックしたときに選択したオブジェクトのレイヤーに変更
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (hit2d == false
                || hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                hit2d.collider.tag == "Player" ||
                hit2d.collider.tag == "RangeSelect" ||
                hit2d.collider.tag == "UnTouch" ||
                hit2d.collider.tag == "LayerPanel")
            {
                if (ClickObj != null)
                {
                    ClickObj.GetComponent<SpriteRenderer>().material = materials[0];
                }
                ClickObj = null;
                layerControll.OutChangeLayerNum(0);
                return;
            }

            if (ClickObj != null)
            {
                ClickObj.GetComponent<SpriteRenderer>().material = materials[0];
            }
            if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer1"))
            {
                ClickObj = hit2d.collider.gameObject;
                layerControll.OutChangeLayerNum(1);
            }
            else if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer2"))
            {
                ClickObj = hit2d.collider.gameObject;
                layerControll.OutChangeLayerNum(2);
            }
            else if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer3"))
            {
                ClickObj = hit2d.collider.gameObject;
                layerControll.OutChangeLayerNum(3);
            }

            //クリックしたオブジェクトが選択可能オブジェクトだったら
            if (ClickObj != null)
            {
                ClickObj.GetComponent<SpriteRenderer>().material = materials[1];

                //単体
                if (!rangeSelect.ReturnSelectNow())
                {
                    if (i_CopyNum > 0)
                    {
                        CopyObj = hit2d.collider.gameObject;
                        PasteObj = null;
                    }
                }
                //複数
                else
                {
                    if (i_CopyNum > 0)
                    {
                        CopyObjs = rangeSelect.ReturnRangeSelectObj();
                        v3_offset = new Vector3[CopyObjs.Length];
                        PasteObjs = new GameObject[CopyObjs.Length];
                        for (int i = 0; i < CopyObjs.Length; i++)
                        {
                            v3_offset[i] = CopyObjs[i].transform.position - CopyObjs[0].transform.position;
                            PasteObjs[i] = null;

                        }
                    }
                }
                //ペーストモードに移行
                Paste();
                b_pasteMode = true;
                b_copyMode = false;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            b_copyMode = false;
        }
    }

    //ペースト
    private void Paste()
    {
        copyModeText.text = "現在ペーストモードです";
        //単体
        if (!rangeSelect.ReturnSelectNow())
        {
            if (i_PasteNum > 0)
            {
                PasteObj = Instantiate(CopyObj);
                CopyObj.GetComponent<SpriteRenderer>().sortingOrder = 5;

                layerControll.PasteChangeLayer(CopyObj.layer);

                i_PasteNum--;
                //カーソルを強制的に画面中央に移動(今後追加予定)
            }
        }
        //複数
        else
        {
            if (i_PasteNum > 0)
            {
                for (int i = 0; i < CopyObjs.Length; i++)
                {
                    PasteObjs[i] = Instantiate(CopyObjs[i]);
                    CopyObjs[i].GetComponent<SpriteRenderer>().sortingOrder = 5;

                    layerControll.PasteChangeLayer(CopyObjs[i].layer);
                }
                i_PasteNum--;
            }
        }
        b_setOnOff = true;
    }

    //コピーボタンを押した時
    public void OnCopy()
    {
        b_copyMode = true;
        copyModeText.enabled = true;
        copyModeText.text = "現在コピーモードです";
    }


}
