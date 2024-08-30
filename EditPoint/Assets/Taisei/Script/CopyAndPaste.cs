using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CopyAndPaste : MonoBehaviour
{
    [SerializeField] private Text copyModeText;

    [SerializeField] private RangeSelection rangeSelect;
    //�R�s�[���̃I�u�W�F�N�g
    private GameObject CopyObj;

    [SerializeField] private LayerController layerControll;

    [SerializeField, Header("�y�[�X�g�ł����")] private int i_PasteNum = 1000;
    [SerializeField, Header("�R�s�[�ł����")] private int i_CopyNum = 1000;

    //�I�u�W�F�N�g�̃y�[�X�g��ړ�����Ƃ���true�ɂ���
    private bool b_setOnOff = false;

    //�y�[�X�g���̃I�u�W�F�N�g
    private GameObject PasteObj;

    //�����p
    private GameObject[] CopyObjs;
    private GameObject[] PasteObjs;
    private Vector3[] v3_offset;    //�I�u�W�F�N�g���m�̋�����ۑ�

    //�}�E�X�̍��W�֘A
    private Vector3 v3_mousePos;
    private Vector3 v3_scrWldPos;

    //�I�������I�u�W�F�N�g
    private GameObject ClickObj;
    /// <summary>
    /// �X�N���v�^�u���I�u�W�F�N�g�ł܂Ƃ߂Ă���}�e���A��
    /// "layerMaterials"�Ƃ������X�g�������Ă���
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

    //�������������ϐ�
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
        //�y�[�X�g�E�I�u�W�F�N�g�ړ���Ԃ���Ȃ��Ƃ�
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

            //UI�̏ザ��Ȃ�������
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //�N���b�N������J�[�\���̈ʒu�ɃI�u�W�F�N�g��u��
                if (Input.GetMouseButtonDown(0) && !plLayer.ReturnPlTrigger())
                {
                    //�P��
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
                    //����
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

            //�E�N���b�N�Ńy�[�X�g���[�h����
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
        //�N���b�N�����Ƃ��ɑI�������I�u�W�F�N�g�̃��C���[�ɕύX
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

            //�V���ȃI�u�W�F�N�g�ɍX�V����O�Ɍ��̃}�e���A���ɖ߂�
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

            //�R�s�[���[�h�̎��̂�
            if (mode == MODE_TYPE.copy)
            {
                //�N���b�N�����I�u�W�F�N�g���I���\�I�u�W�F�N�g��������
                if (ClickObj != null)
                {
                    ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[6];

                    //�P��
                    if (!rangeSelect.ReturnSelectNow())
                    {
                        if (i_CopyNum > 0)
                        {
                            CopyObj = ClickObj;
                            PasteObj = null;
                        }
                    }
                    //����
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
                    //�y�[�X�g���[�h�Ɉڍs
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

        //�R�s�[���[�h������
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

    //�y�[�X�g
    private void Paste()
    {
        copyModeText.text = "���݃y�[�X�g���[�h�ł�";
        //�P��
        if (!rangeSelect.ReturnSelectNow())
        {
            if (i_PasteNum > 0)
            {
                PasteObj = Instantiate(CopyObj);
                PasteObj.GetComponent<Collider2D>().isTrigger = true;
                CopyObj.GetComponent<SpriteRenderer>().sortingOrder = 5;

                layerControll.PasteChangeLayer(CopyObj.layer);

                i_PasteNum--;
                //�J�[�\���������I�ɉ�ʒ����Ɉړ�(����ǉ��\��)
            }
        }
        //����
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

    //�R�s�[�{�^������������
    public void OnCopy()
    {
        i_CopyNum--;
        mode = MODE_TYPE.copy;
        copyModeText.enabled = true;
        copyModeText.text = "���݃R�s�[���[�h�ł�";
        if (ClickObj != null)
        {
            ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[6];

            //�P��
            if (!rangeSelect.ReturnSelectNow())
            {
                if (i_CopyNum > 0)
                {
                    CopyObj = ClickObj;
                    PasteObj = null;
                }
            }
            //����
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
            //�y�[�X�g���[�h�Ɉڍs
            Paste();
            mode = MODE_TYPE.paste;

        }
    }

    /// <summary>
    /// �}�e���A�����ŏ��̏�Ԃɖ߂�
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
