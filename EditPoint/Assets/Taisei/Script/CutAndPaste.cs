using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CutAndPaste : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    private GameObject ChoiseObj;
    private GameObject CutObj;

    [SerializeField] private LayerController layerChange;

    [SerializeField, Header("ペーストできる回数")] private int i_PasteNum = 1;
    [SerializeField, Header("カットできる回数")] private int i_CutNum = 1;

    //オブジェクトのペーストや移動するときにtrueにする
    private bool b_setOnOff = false;
    //選択状態かどうか
    private bool b_choiseOnOff = false;

    private Vector3 v3_mousePos;    //マウスの座標
    private Vector3 v3_scrWldPos;   //マウスの座標をワールド座標に

    //ペーストできるかどうか
    //false=設置可能 true=設置不可
    private bool b_checkPeast = false;

    //ペースト時のオブジェクト
    private GameObject PasteObj;
    private string s_pasteObjName;
    //ペースト回数
    private int i_PasteCnt = 0;

    //選択したオブジェクト
    private GameObject ClickObj;
    //選択時と非選択時に変更するマテリアル
    //0 = デフォルトマテリアル
    //1 = 選択時のアウトラインマテリアル
    [SerializeField] private Material[] materials = new Material[2];



    void Start()
    {
        ChoiseObj = null;
        CutObj = null;
        PasteObj = null;
    }

    void Update()
    {
        //編集モードがONの時
        if (gm.ReturnEditMode() == true)
        {
            //ペースト・オブジェクト移動状態じゃないとき
            if (!b_setOnOff)
            {
                //クリックしたときに選択したオブジェクトのレイヤーに変更
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                    if (EventSystem.current.IsPointerOverGameObject()
                        //|| 
                        //hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || 
                        //hit2d.collider.tag == "Player" || 
                        //hit2d.collider.tag == "RangeSelect"
                        )
                    {
                        ClickObj.GetComponent<SpriteRenderer>().material = materials[0];
                        ClickObj = null;
                        return;
                    }

                    if (ClickObj != null)
                    {
                        ClickObj.GetComponent<SpriteRenderer>().material = materials[0];
                    }
                    //if (hit2d == false)
                    //{
                    //    ClickObj = null;
                    //    layerChange.OutChangeLayerNum(0);
                    //}
                    if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer1"))
                    {
                        ClickObj = hit2d.collider.gameObject;
                        layerChange.OutChangeLayerNum(1);
                    }
                    else if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer2"))
                    {
                        ClickObj = hit2d.collider.gameObject;
                        layerChange.OutChangeLayerNum(2);
                    }
                    else if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer3"))
                    {
                        ClickObj = hit2d.collider.gameObject;
                        layerChange.OutChangeLayerNum(3);
                    }

                    if (hit2d)
                    {
                        ChoiseObj = hit2d.transform.gameObject;
                    }

                    //クリックしたオブジェクトが選択可能オブジェクトだったら
                    if (ClickObj != null)
                    {
                        ClickObj.GetComponent<SpriteRenderer>().material = materials[1];

                    }
                }
            }
            //ペースト・オブジェクト移動状態の時
            else
            {
                if (i_PasteCnt > 0)
                {
                    PasteObj.SetActive(true);
                }
                v3_mousePos = Input.mousePosition;
                v3_mousePos.z = 10;

                v3_scrWldPos = Camera.main.ScreenToWorldPoint(v3_mousePos);
                PasteObj.transform.position = v3_scrWldPos;
                
                //クリックしたらカーソルの位置にオブジェクトを置く
                if (Input.GetMouseButtonDown(0) && !b_checkPeast)
                {
                    if (i_PasteCnt > 0)
                    {
                        s_pasteObjName = CutObj.name + "(Clone" + i_PasteCnt + ")";
                    }
                    else
                    {
                        s_pasteObjName = CutObj.name;
                    }
                    i_PasteCnt++;

                    PasteObj.name = s_pasteObjName;
                    CutObj.SetActive(false);

                    PasteObj.GetComponent<Collider2D>().isTrigger = false;
                    layerChange.ChangeObjectList();
                    b_setOnOff = false;

                    s_pasteObjName = null;

                }
                else if (b_checkPeast)
                {
                    Debug.Log("おけません");
                }
            }
        }
    }

    public bool ReturnSetOnOff()
    {
        return b_setOnOff;
    }

    //カットボタンを押した時
    public void OnCut()
    {
        if (i_CutNum > 0)
        {
            CutObj = ChoiseObj;
            ChoiseObj = null;
            CutObj.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255 / 255f, 50f / 255f);
            i_PasteCnt = 0;
            PasteObj = null;
        }
    }

    //ペーストするとき
    public void OnPaste()
    {
        if (i_PasteNum > 0)
        {
            b_setOnOff = true;
            PasteObj = Instantiate(CutObj);
            PasteObj.GetComponent<Collider2D>().isTrigger = true;
            PasteObj.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            CutObj.GetComponent<SpriteRenderer>().sortingOrder = 5;

            layerChange.PasteChangeLayer(CutObj.layer);

            //カーソルを強制的に画面中央に移動(今後追加予定)
        }
    }

    public void CheckPasteTrigger(bool trigger)
    {
        b_checkPeast = trigger;
    }

}
