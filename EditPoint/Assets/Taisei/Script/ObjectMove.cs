using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private LayerMask lm;


    //�I�u�W�F�N�g���ړ�������ۂ̃}�E�X���W
    private Vector3 v3_scrWldPos;

    //�N���b�N�����ʒu�ƃI�u�W�F�N�g�̍��W�Ƃ̍�
    private Vector3 v3_mousePos;
    private Vector3 v3_offset;

    //�P�̈ړ�
    private GameObject Obj;

    private bool b_objMove = false;

    //�I�����ɃA�E�g���C��������
    private GameObject ClickObj;
    /// <summary>
    /// �X�N���v�^�u���I�u�W�F�N�g�ł܂Ƃ߂Ă���}�e���A��
    /// "layerMaterials"�Ƃ������X�g�������Ă���
    /// </summary>
    [SerializeField] private Materials materials;

    //�������ȗ�������ϐ�
    private bool b_isNoHit;
    private bool b_isSpecificTag = false;

    private bool b_objSetMode = false;

    private PlaySound playSound;

    void Start()
    {
        if (GameObject.Find("AudioCanvas") != null)
        {
            playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        }
        else
        {
            Debug.Log("audio�Ȃ�");
        }
    }


    void Update()
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.b_playNow)
        {
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            return;
        }

        if (!b_objSetMode)
        {
            if (Input.GetMouseButtonDown(0) && 
                (ModeData.ModeEntity.mode == ModeData.Mode.normal || ModeData.ModeEntity.mode == ModeData.Mode.moveANDdirect))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, Mathf.Infinity, lm);

                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                b_isNoHit = (hit2d == false);
                if (!b_isNoHit)
                {
                    b_isSpecificTag = new List<string> { "Player", "UnTouch", "Marcker", "MoveGround" }.Contains(hit2d.collider.tag);
                }

                //UI�⓮���������Ȃ��I�u�W�F�N�g�������炾�����牽�����Ȃ�
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
                v3_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                v3_offset = ClickObj.transform.position - v3_mousePos;
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
                    v3_scrWldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    v3_scrWldPos.z = 10;
                    Obj.transform.position = v3_scrWldPos + v3_offset;

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

        //delete�L�[�őI�����Ă�I�u�W�F�N�g������
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

    public GameObject ReturnClickObj()
    {
        return ClickObj;
    }
}
