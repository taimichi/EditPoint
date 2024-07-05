using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectMove : MonoBehaviour
{
    private Vector3 v3_pos;
    private Vector3 v3_scrWldPos;
    //取得したオブジェクトの元の座標
    private Vector3 v3_objPos;

    private GameObject Obj;
    private GameObject CopyObj;

    private bool b_ojbMove = false;

    //オブジェクトがプレイヤーの上にあるかどうか
    private bool b_plTrigger = false;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            //UIだったら何もしない
            if (EventSystem.current.IsPointerOverGameObject() || 
                hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                hit2d.collider.tag == "Player")
            {
                return;
            }


            if (hit2d)
            {
                Obj = hit2d.transform.gameObject;
                v3_objPos = Obj.transform.position;

                CopyObj = Instantiate(Obj);
                CopyObj.transform.position = v3_objPos;
                CopyObj.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 50f / 255f);

                Obj.GetComponent<Collider2D>().isTrigger = true;
                b_ojbMove = true;
            }

        }

        if (b_ojbMove)
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
                b_ojbMove = false;
                Obj.GetComponent<Collider2D>().isTrigger = false;

                Destroy(CopyObj);
                CopyObj = null;

                if (b_plTrigger)
                {
                    Obj.transform.position = v3_objPos;
                    return;
                }
                Obj = null;

            }
        }
    }

    public void CheckPlTrigger(bool trigger)
    {
        b_plTrigger = trigger;
    }

    public bool ReturnObjMove()
    {
        return b_ojbMove;
    }
}
