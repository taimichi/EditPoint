using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private RangeSelection rangeSelect;

    private Vector3 v3_pos;
    private Vector3 v3_scrWldPos;
    //�擾�����I�u�W�F�N�g�̌��̍��W
    private Vector3 v3_objPos;

    //�P�̈ړ�
    private GameObject Obj;
    private GameObject CopyObj;

    //�����ړ�
    private GameObject[] Objs;
    private GameObject[] CopyObjs; 
    private Vector3[] v3_objsPos;
    private Vector3[] v3_offset;    //��I�u�W�F�N�g����̋���
    private bool b_plTriggers = false;  //�����̃I�u�W�F�N�g�̂�������v���C���[�ɐG��Ă��邩�ǂ���

    private bool b_ojbMove = false;

    private PlayerLayer plLayer;

    private Color startColor;

    void Start()
    {
        plLayer = GameObject.Find("Player").GetComponent<PlayerLayer>();   
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            //UI�������牽�����Ȃ�
            if (hit2d == false
                || hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                hit2d.collider.tag == "Player" ||
                hit2d.collider.tag == "RangeSelect" || 
                hit2d.collider.tag == "UnTouch")
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                return;
            }


            if (hit2d)
            {
                //�P��
                if (!rangeSelect.ReturnSelectNow())
                {
                    Obj = hit2d.transform.gameObject;
                    v3_objPos = Obj.transform.position;

                    CopyObj = Instantiate(Obj);
                    CopyObj.transform.position = v3_objPos;
                    startColor = CopyObj.GetComponent<SpriteRenderer>().color;
                    CopyObj.GetComponent<SpriteRenderer>().color = new Color(startColor.r, startColor.g, startColor.b, 50f / 255f);

                    Obj.GetComponent<Collider2D>().isTrigger = true;
                    b_ojbMove = true;
                }
                //����
                else
                {
                    Objs = rangeSelect.ReturnRangeSelectObj();
                    CopyObjs = new GameObject[Objs.Length];
                    v3_objsPos = new Vector3[Objs.Length];
                    v3_offset = new Vector3[Objs.Length];
                    for(int i = 0; i < Objs.Length; i++)
                    {
                        v3_objsPos[i] = Objs[i].transform.position;
                        v3_offset[i] = Objs[i].transform.position - Objs[0].transform.position;
                        CopyObjs[i] = Instantiate(Objs[i]);
                        CopyObjs[i].transform.position = v3_objsPos[i];
                        startColor = CopyObjs[i].GetComponent<SpriteRenderer>().color;
                        CopyObjs[i].GetComponent<SpriteRenderer>().color= new Color(startColor.r, startColor.g, startColor.b, 50f / 255f);

                        Objs[i].GetComponent<Collider2D>().isTrigger = true;
                        b_ojbMove = true;
                    }
                }
            }

        }

        if (b_ojbMove)
        {
            if (Input.GetMouseButton(0))
            {
                v3_pos = Input.mousePosition;
                v3_pos.z = 10;

                v3_scrWldPos = Camera.main.ScreenToWorldPoint(v3_pos);
                //�P��
                if (!rangeSelect.ReturnSelectNow())
                {
                    Obj.transform.position = v3_scrWldPos;
                }
                //����
                else
                {
                    for(int i = 0; i < Objs.Length; i++)
                    {
                        Objs[i].transform.position = v3_scrWldPos + v3_offset[i];
                    }
                }

            }
            else if (Input.GetMouseButtonUp(0))
            {
                b_ojbMove = false;

                //�P��
                if (!rangeSelect.ReturnSelectNow())
                {
                    Obj.GetComponent<Collider2D>().isTrigger = false;

                    Destroy(CopyObj);
                    CopyObj = null;

                    if (plLayer.ReturnPlTrigger())
                    {
                        Obj.transform.position = v3_objPos;
                        return;
                    }
                    Obj = null;

                }
                //����
                else
                {
                    if (plLayer.ReturnPlTrigger())
                    {
                        b_plTriggers = true;
                    }
                    else
                    {
                        b_plTriggers = false;
                    }
                    for(int i = 0; i < Objs.Length; i++)
                    {
                        Objs[i].GetComponent<Collider2D>().isTrigger = false;

                        Destroy(CopyObjs[i]);

                        if (b_plTriggers)
                        {
                            Objs[i].transform.position = v3_objsPos[i];
                        }
                    }
                    b_plTriggers = false;
                }
            }
        }
    }

    public bool ReturnObjMove()
    {
        return b_ojbMove;
    }
}
