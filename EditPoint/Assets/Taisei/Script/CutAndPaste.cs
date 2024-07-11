using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CutAndPaste : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private RangeSelection rangeSelect;
    private GameObject ChoiseObj;
    private GameObject CutObj;

    [SerializeField] private LayerController layerChange;

    [SerializeField, Header("�y�[�X�g�ł����")] private int i_PasteNum = 1;
    [SerializeField, Header("�J�b�g�ł����")] private int i_CutNum = 1;

    //�I�u�W�F�N�g�̃y�[�X�g��ړ�����Ƃ���true�ɂ���
    private bool b_setOnOff = false;

    private Vector3 v3_mousePos;    //�}�E�X�̍��W
    private Vector3 v3_scrWldPos;   //�}�E�X�̍��W�����[���h���W��

    //�y�[�X�g���̃I�u�W�F�N�g
    private GameObject PasteObj;
    private string s_pasteObjName;
    //�y�[�X�g��
    private int i_PasteCnt = 0;

    //�����p
    private GameObject[] CutObjs;
    private GameObject[] PasteObjs;
    private string[] s_pasteObjNames;
    private Vector3[] v3_offset;

    //�I�������I�u�W�F�N�g
    private GameObject ClickObj;
    //�I�����Ɣ�I�����ɕύX����}�e���A��
    //0 = �f�t�H���g�}�e���A��
    //1 = �I�����̃A�E�g���C���}�e���A��
    [SerializeField] private Material[] materials = new Material[2];

    private PlayerLayer plLayer;

    private Color startColor;

    void Start()
    {
        plLayer = GameObject.Find("Player").GetComponent<PlayerLayer>();
        ChoiseObj = null;
        CutObj = null;
        PasteObj = null;

        i_CutNum = 0;
        i_PasteNum = 0;
    }

    void Update()
    {
        //�ҏW���[�h��ON�̎�
        if (gm.ReturnEditMode() == true)
        {
            //�y�[�X�g�E�I�u�W�F�N�g�ړ���Ԃ���Ȃ��Ƃ�
            if (!b_setOnOff)
            {
                //�N���b�N�����Ƃ��ɑI�������I�u�W�F�N�g�̃��C���[�ɕύX
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                    if (hit2d == false
                        || hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                        hit2d.collider.tag == "Player" ||
                        hit2d.collider.tag == "RangeSelect" || 
                        hit2d.collider.tag == "UnTouch" || 
                        hit2d.collider.tag == "LayerPanel" || 
                        hit2d.collider.tag == "CutObj")
                    {
                        if (EventSystem.current.IsPointerOverGameObject())
                        {
                            return;
                        }
                        //Debug.Log("�Ȃ�");
                        ChoiseObj = null;
                        if (ClickObj != null)
                        {
                            ClickObj.GetComponent<SpriteRenderer>().material = materials[0];
                        }
                        ClickObj = null;
                        Debug.Log("�Ȃɂ��Ȃ�");
                        layerChange.OutChangeLayerNum(0);
                        return;
                    }

                    if (ClickObj != null)
                    {
                        ClickObj.GetComponent<SpriteRenderer>().material = materials[0];
                    }
                    if (hit2d == false || hit2d.collider.tag == "LayerPanel")
                    {
                    }
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

                    //�N���b�N�����I�u�W�F�N�g���I���\�I�u�W�F�N�g��������
                    if (ClickObj != null)
                    {
                        ClickObj.GetComponent<SpriteRenderer>().material = materials[1];

                    }
                }
            }
            //�y�[�X�g�E�I�u�W�F�N�g�ړ���Ԃ̎�
            else
            {
                v3_mousePos = Input.mousePosition;
                v3_mousePos.z = 10;

                v3_scrWldPos = Camera.main.ScreenToWorldPoint(v3_mousePos);

                if (!rangeSelect.ReturnSelectNow())
                {
                    if (i_PasteCnt > 0)
                    {
                        PasteObj.SetActive(true);
                    }
                    PasteObj.transform.position = v3_scrWldPos;
                }
                else
                {
                    for(int i = 0; i < PasteObjs.Length; i++)
                    {
                        if (i_PasteCnt > 0)
                        {
                            PasteObjs[i].SetActive(true);
                        }
                        PasteObjs[i].transform.position = v3_scrWldPos + v3_offset[i];
                    }
                }

                //�N���b�N������J�[�\���̈ʒu�ɃI�u�W�F�N�g��u��
                if (Input.GetMouseButtonDown(0) && !plLayer.ReturnPlTrigger())
                {
                    //�P��
                    if (!rangeSelect.ReturnSelectNow())
                    {
                        if (i_PasteCnt > 0)
                        {
                            s_pasteObjName = CutObj.name + "(Clone" + i_PasteCnt + ")";
                        }
                        else
                        {
                            s_pasteObjName = CutObj.name;
                        }

                        PasteObj.name = s_pasteObjName;
                        CutObj.tag = "Untagged";
                        CutObj.SetActive(false);

                        PasteObj.GetComponent<Collider2D>().isTrigger = false;
                        PasteObj.GetComponent<SpriteRenderer>().material = materials[0];
                        layerChange.ChangeObjectList();
                        b_setOnOff = false;

                        s_pasteObjName = "";
                    }
                    //����
                    else
                    {
                        s_pasteObjNames = new string[PasteObjs.Length];
                        for (int i = 0; i < PasteObjs.Length; i++)
                        {
                            if (i_PasteCnt > 0)
                            {
                                s_pasteObjNames[i] = CutObjs[i].name + "(Clone" + i_PasteCnt + ")";
                            }
                            else
                            {
                                s_pasteObjNames[i] = CutObjs[i].name;
                            }

                            PasteObjs[i].name = s_pasteObjNames[i];
                            CutObjs[i].tag = "Untagged";
                            CutObjs[i].SetActive(false);

                            PasteObjs[i].GetComponent<Collider2D>().isTrigger = false;
                            PasteObjs[i].GetComponent<SpriteRenderer>().material = materials[0];
                            layerChange.ChangeObjectList();
                            b_setOnOff = false;

                            s_pasteObjNames[i] = "";
                        }
                    }

                }
                else if (plLayer.ReturnPlTrigger())
                {
                    //Debug.Log("�����܂���");
                }
            }
        }
    }

    public bool ReturnSetOnOff()
    {
        return b_setOnOff;
    }

    //�J�b�g�{�^������������
    public void OnCut()
    {
        //�P��
        if (!rangeSelect.ReturnSelectNow())
        {
            if (i_CutNum > 0)
            {
                CutObj = ChoiseObj;
                ChoiseObj = null;
                startColor = CutObj.GetComponent<SpriteRenderer>().color;
                CutObj.tag = "CutObj";
                CutObj.GetComponent<SpriteRenderer>().color = new Color(startColor.r, startColor.g, startColor.b, 50f / 255f);
                i_PasteCnt = 0;
                PasteObj = null;
                i_CutNum++;
            }
        }
        //����
        else
        {
            if (i_CutNum > 0)
            {
                CutObjs = rangeSelect.ReturnRangeSelectObj();
                v3_offset = new Vector3[CutObjs.Length];
                PasteObjs = new GameObject[CutObjs.Length];
                for (int i = 0; i < CutObjs.Length; i++)
                {
                    startColor = CutObjs[i].GetComponent<SpriteRenderer>().color;
                    CutObjs[i].tag = "CutObj";
                    CutObjs[i].GetComponent<SpriteRenderer>().color = new Color(startColor.r, startColor.g, startColor.b, 50f / 255f);
                    v3_offset[i] = CutObjs[i].transform.position - CutObjs[0].transform.position;
                    PasteObjs[i] = null;

                }
                i_PasteCnt = 0;
                i_CutNum++;
            }
        }
    }

    //�y�[�X�g����Ƃ�
    public void OnPaste()
    {
        //�P��
        if (!rangeSelect.ReturnSelectNow())
        {
            if (i_PasteNum > 0)
            {
                b_setOnOff = true;
                i_PasteCnt++;
                PasteObj = Instantiate(CutObj);
                PasteObj.GetComponent<Collider2D>().isTrigger = true;
                PasteObj.GetComponent<SpriteRenderer>().color = new Color(startColor.r, startColor.g, startColor.b, 255f / 255f);
                CutObj.GetComponent<SpriteRenderer>().sortingOrder = 5;

                layerChange.PasteChangeLayer(CutObj.layer);

                i_PasteNum++;
                //�J�[�\���������I�ɉ�ʒ����Ɉړ�(����ǉ��\��)
            }
        }
        //����
        else
        {
            if (i_PasteNum > 0)
            {
                b_setOnOff = true;
                i_PasteCnt++;
                for (int i = 0; i < CutObjs.Length; i++)
                {
                    PasteObjs[i] = Instantiate(CutObjs[i]);
                    PasteObjs[i].GetComponent<Collider2D>().isTrigger = true;
                    PasteObjs[i].GetComponent<SpriteRenderer>().color = new Color(startColor.r, startColor.g, startColor.b, 255f / 255f);
                    CutObjs[i].GetComponent<SpriteRenderer>().sortingOrder = 5;

                    layerChange.PasteChangeLayer(CutObjs[i].layer);
                }
                i_PasteNum++;
            }
        }
    }

}
