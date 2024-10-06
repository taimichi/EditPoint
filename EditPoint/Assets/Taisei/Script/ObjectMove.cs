using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectMove : MonoBehaviour
{

    //マウス座標
    private Vector3 v3_pos;
    private Vector3 v3_scrWldPos;
    //取得したオブジェクトの元の座標
    private Vector3 v3_objPos;

    //単体移動
    private GameObject Obj;
    private GameObject CopyObj;

    private bool b_objMove = false;

    //選択時にアウトラインをつける
    private GameObject ClickObj;
    /// <summary>
    /// スクリプタブルオブジェクトでまとめてあるマテリアル
    /// "layerMaterials"というリストが入っている
    /// </summary>
    [SerializeField] private Materials materials;

    //条件を簡略化する変数
    private bool b_isNoHit;
    private bool b_isSpecificTag = false;

    private bool b_objSetMode = false;

    private PlaySound playSound;

    void Start()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
    }


    void Update()
    {
        if (!b_objSetMode)
        {
            if (Input.GetMouseButtonDown(0) && 
                (ModeData.ModeEntity.mode == ModeData.Mode.normal || ModeData.ModeEntity.mode == ModeData.Mode.blowerControll))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                b_isNoHit = (hit2d == false);
                if (!b_isNoHit)
                {
                    b_isSpecificTag = new List<string> { "Player", "UnTouch", "Marcker", "MoveGround" }.Contains(hit2d.collider.tag);
                }

                //UIや動かしたくないオブジェクトだったらだったら何もしない
                if (b_isNoHit || b_isSpecificTag)
                {
                    if (ClickObj != null)
                    {
                        if (ClickObj.name.Contains("Blower"))
                        {
                            ClickObj.transform.GetChild(0).GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
                        }
                        else
                        {
                            ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
                        }
                    }
                    ClickObj = null;
                    return;
                }

                if (ClickObj != null)
                {
                    if (ClickObj.name.Contains("Blower"))
                    {
                        ClickObj.transform.GetChild(0).GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
                    }
                    else
                    {
                        ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
                    }
                }


                ClickObj = hit2d.collider.gameObject;
                if (ClickObj.transform.parent != null && ClickObj.transform.parent.gameObject.name.Contains("Blower"))
                {
                    ClickObj = hit2d.collider.transform.parent.gameObject;

                }
                playSound.PlaySE(PlaySound.SE_TYPE.select);

                if (hit2d)
                {
                    if (ClickObj.name.Contains("Blower"))
                    {
                        ClickObj.transform.GetChild(0).GetComponent<SpriteRenderer>().material = materials.layerMaterials[1];
                    }
                    else
                    {
                        ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[1];
                    }
                    Obj = ClickObj;
                    v3_objPos = Obj.transform.position;


                    if (Obj.name.Contains("Blower"))
                    {
                        Obj.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = true;
                    }
                    else
                    {
                        Obj.GetComponent<Collider2D>().isTrigger = true;
                    }

                    b_objMove = true;
                    ModeData.ModeEntity.mode = ModeData.Mode.moveANDdirect;
                }

            }

            if (b_objMove && ModeData.ModeEntity.mode==ModeData.Mode.moveANDdirect)
            {
                if (Input.GetMouseButton(0))
                {
                    v3_pos = Input.mousePosition;
                    v3_pos.z = 10;

                    v3_scrWldPos = Camera.main.ScreenToWorldPoint(v3_pos);
                    Obj.transform.position = v3_scrWldPos;

                }
                else if (Input.GetMouseButtonUp(0))
                {
                    playSound.PlaySE(PlaySound.SE_TYPE.objMove);
                    b_objMove = false;
                    if (Obj.name.Contains("Blower"))
                    {
                        Obj.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = false;
                    }
                    else
                    {
                        Obj.GetComponent<Collider2D>().isTrigger = false;
                    }
                    Obj = null;
                    ModeData.ModeEntity.mode = ModeData.Mode.normal;
                }
            }
        }

        //deleteキーで選択してるオブジェクトを消す
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (ClickObj != null)
            {
                Destroy(ClickObj);
            }
        }
    }


    public void ObjSetMode(bool _modeTrigger)
    {
        b_objSetMode = _modeTrigger;
    }

    public bool ReturnObjMove()
    {
        return b_objMove;
    }
}
