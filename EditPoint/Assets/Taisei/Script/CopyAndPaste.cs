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
    //�I�����Ɣ�I�����ɕύX����}�e���A��
    //0 = �f�t�H���g�}�e���A��
    //1 = �I�����̃A�E�g���C���}�e���A��
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
        //�y�[�X�g�E�I�u�W�F�N�g�ړ���Ԃ���Ȃ��Ƃ�
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

            //UI�̏ザ��Ȃ�������
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //�N���b�N������J�[�\���̈ʒu�ɃI�u�W�F�N�g��u��
                if (Input.GetMouseButtonDown(0) && !plLayer.ReturnPlTrigger())
                {
                    //�P��
                    if (!rangeSelect.ReturnSelectNow())
                    {
                        PasteObj.GetComponent<SpriteRenderer>().material = materials[0];
                        layerControll.ChangeObjectList();

                    }
                    //����
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
                    //Debug.Log("�����܂���");
                }
            }

            //�E�N���b�N�Ńy�[�X�g���[�h����
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
        //�N���b�N�����Ƃ��ɑI�������I�u�W�F�N�g�̃��C���[�ɕύX
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

            //�N���b�N�����I�u�W�F�N�g���I���\�I�u�W�F�N�g��������
            if (ClickObj != null)
            {
                ClickObj.GetComponent<SpriteRenderer>().material = materials[1];

                //�P��
                if (!rangeSelect.ReturnSelectNow())
                {
                    if (i_CopyNum > 0)
                    {
                        CopyObj = hit2d.collider.gameObject;
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
                b_pasteMode = true;
                b_copyMode = false;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            b_copyMode = false;
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
        b_copyMode = true;
        copyModeText.enabled = true;
        copyModeText.text = "���݃R�s�[���[�h�ł�";
    }


}
