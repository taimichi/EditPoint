using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private RangeSelection rangeSelect;
    [SerializeField] private LayerController layerControll;
    [SerializeField] private CopyAndPaste CaP;

    //�}�E�X���W
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

    private bool b_objMove = false;

    private PlayerLayer plLayer;

    private Color startColor;

    //�I�����ɃA�E�g���C��������
    private GameObject ClickObj;
    /// <summary>
    /// �X�N���v�^�u���I�u�W�F�N�g�ł܂Ƃ߂Ă���}�e���A��
    /// "layerMaterials"�Ƃ������X�g�������Ă���
    /// </summary>
    [SerializeField] private Materials materials;

    //�������������ϐ�
    private bool b_isNoHit;
    private bool b_isGroundLayer;
    private bool b_isSpecificTag;

    void Start()
    {
        plLayer = GameObject.Find("Player").GetComponent<PlayerLayer>();   
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CaP.ReturnSetOnOff())
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

            //UI�⓮���������Ȃ��I�u�W�F�N�g�������炾�����牽�����Ȃ�
            if (b_isNoHit || b_isGroundLayer || b_isSpecificTag)
            {
                if (ClickObj != null)
                {
                    MaterialReset();
                }
                ClickObj = null;
                return;
            }

            if(ClickObj != null)
            {
                MaterialReset();
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

            if (hit2d)
            {
                //�P��
                if (!rangeSelect.ReturnSelectNow())
                {
                    ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[6];
                    Obj = hit2d.transform.gameObject;
                    v3_objPos = Obj.transform.position;

                    CopyObj = Instantiate(Obj);
                    CopyObj.transform.position = v3_objPos;
                    startColor = CopyObj.GetComponent<SpriteRenderer>().color;
                    CopyObj.GetComponent<SpriteRenderer>().color = new Color(startColor.r, startColor.g, startColor.b, 50f / 255f);

                    Obj.GetComponent<Collider2D>().isTrigger = true;
                    b_objMove = true;
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
                        b_objMove = true;
                    }
                }
            }

        }

        if (b_objMove)
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
                b_objMove = false;

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

        //�E�N���b�N�����烌�C���[�\����S�̂ɂ���
        if (Input.GetMouseButtonDown(1))
        {
            MaterialReset();
            layerControll.OutChangeLayerNum(0);
        }
    }

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

    public bool ReturnObjMove()
    {
        return b_objMove;
    }
}
