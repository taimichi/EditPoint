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
    /// <summary>
    /// スクリプタブルオブジェクトでまとめてあるマテリアル
    /// "layerMaterials"というリストが入っている
    /// </summary>
    [SerializeField] private Materials materials;

    private PlayerLayer plLayer;

    private enum MODE_TYPE
    {
        normal,
        copy,
        paste,
    }
    private MODE_TYPE mode;

    //条件を説明する変数
    private bool b_isNoHit;
    private bool b_isGroundLayer;
    private bool b_isSpecificTag;


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
        if (mode == MODE_TYPE.copy)
        {
            Copy();
            return;
        }
        else if(mode == MODE_TYPE.normal)
        {
            GetObj();
            return;
        }

        if(b_setOnOff && mode == MODE_TYPE.paste)
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
                        if(PasteObj.layer == LayerMask.NameToLayer("Layer1"))
                        {
                            PasteObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[3];
                        }
                        else if (PasteObj.layer == LayerMask.NameToLayer("Layer2"))
                        {
                            PasteObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[4];
                        }
                        else if(PasteObj.layer == LayerMask.NameToLayer("Layer3"))
                        {
                            PasteObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[5];
                        }
                        PasteObj.GetComponent<Collider2D>().isTrigger = false;
                        layerControll.ChangeObjectList();

                    }
                    //複数
                    else
                    {
                        for (int i = 0; i < PasteObjs.Length; i++)
                        {
                            if (PasteObjs[i].layer == LayerMask.NameToLayer("Layer1"))
                            {
                                PasteObjs[i].GetComponent<SpriteRenderer>().material = materials.layerMaterials[3];
                            }
                            else if (PasteObjs[i].layer == LayerMask.NameToLayer("Layer2"))
                            {
                                PasteObjs[i].GetComponent<SpriteRenderer>().material = materials.layerMaterials[4];
                            }
                            else if (PasteObjs[i].layer == LayerMask.NameToLayer("Layer3"))
                            {
                                PasteObjs[i].GetComponent<SpriteRenderer>().material = materials.layerMaterials[5];
                            }
                            PasteObjs[i]. GetComponent<Collider2D>().isTrigger = false;
                            layerControll.ChangeObjectList();
                        }
                    }
                    Paste();
                }
            }

            //右クリックでペーストモード解除
            if (Input.GetMouseButtonDown(1))
            {
                if (!rangeSelect.ReturnSelectNow())
                {
                    MaterialReset();
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
                mode = MODE_TYPE.normal;
                b_setOnOff = false;
                copyModeText.enabled = false;
            }
        }
        
    }

    public bool ReturnSetOnOff()
    {
        return b_setOnOff;
    }

    private void GetObj()
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

            b_isNoHit = (hit2d == false);
            b_isGroundLayer = (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Ground"));
            b_isSpecificTag = new List<string> { "Player", "RangeSelect", "UnTouch", "LayerPanel" }.Contains(hit2d.collider.tag);

            if (b_isNoHit || b_isGroundLayer || b_isSpecificTag)
            {
                if (ClickObj != null)
                {
                    MaterialReset();
                }
                ClickObj = null;
                layerControll.OutChangeLayerNum(0);
                return;
            }

            //新たなオブジェクトに更新する前に元のマテリアルに戻す
            if (ClickObj != null)
            {
                MaterialReset();
            }


            ClickObj = hit2d.collider.gameObject;

            if (ClickObj.layer == LayerMask.NameToLayer("Layer1"))
            {
                ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[6];
                layerControll.OutChangeLayerNum(1);
            }
            else if (ClickObj.layer == LayerMask.NameToLayer("Layer2"))
            {
                ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[6];
                layerControll.OutChangeLayerNum(2);
            }
            else if (ClickObj.layer == LayerMask.NameToLayer("Layer3"))
            {
                ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[6];
                layerControll.OutChangeLayerNum(3);
            }

            //コピーモードの時のみ
            if (mode == MODE_TYPE.copy)
            {
                //クリックしたオブジェクトが選択可能オブジェクトだったら
                if (ClickObj != null)
                {
                    ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[6];

                    //単体
                    if (!rangeSelect.ReturnSelectNow())
                    {
                        if (i_CopyNum > 0)
                        {
                            CopyObj = ClickObj;
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
                    mode = MODE_TYPE.paste;
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            MaterialReset();
            ClickObj = null;
        }
    }

    private void Copy()
    {
        GetObj();

        //コピーモードを解除
        if (Input.GetMouseButtonDown(1))
        {
            MaterialReset();
            mode = MODE_TYPE.normal;
            ClickObj = null;
            CopyObj = null;
            for(int i = 0; i < CopyObjs.Length; i++)
            {
                CopyObjs[i] = null;
            }
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
                PasteObj.GetComponent<Collider2D>().isTrigger = true;
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
                    PasteObjs[i].GetComponent<Collider2D>().isTrigger = true;
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
        i_CopyNum--;
        mode = MODE_TYPE.copy;
        copyModeText.enabled = true;
        copyModeText.text = "現在コピーモードです";
        if (ClickObj != null)
        {
            ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[6];

            //単体
            if (!rangeSelect.ReturnSelectNow())
            {
                if (i_CopyNum > 0)
                {
                    CopyObj = ClickObj;
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
            mode = MODE_TYPE.paste;

        }
    }

    /// <summary>
    /// マテリアルを最初の状態に戻す
    /// </summary>
    private void MaterialReset()
    {
        if (ClickObj.layer == LayerMask.NameToLayer("Layer1"))
        {
            ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[3];
        }
        else if (ClickObj.layer == LayerMask.NameToLayer("Layer2"))
        {
            ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[4];
        }
        else if (ClickObj.layer == LayerMask.NameToLayer("Layer3"))
        {
            ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[5];
        }
    }


}
