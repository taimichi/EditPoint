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

    [SerializeField, Header("�y�[�X�g�ł����")] private int i_PasteNum = 1;
    [SerializeField, Header("�J�b�g�ł����")] private int i_CutNum = 1;

    //�I�u�W�F�N�g�̃y�[�X�g��ړ�����Ƃ���true�ɂ���
    private bool b_setOnOff = false;
    //�I����Ԃ��ǂ���
    private bool b_choiseOnOff = false;

    private Vector3 v3_mousePos;    //�}�E�X�̍��W
    private Vector3 v3_scrWldPos;   //�}�E�X�̍��W�����[���h���W��

    //�y�[�X�g�ł��邩�ǂ���
    //false=�ݒu�\ true=�ݒu�s��
    private bool b_checkPeast = false;

    //�y�[�X�g���̃I�u�W�F�N�g
    private GameObject PasteObj;
    private string s_pasteObjName;
    //�y�[�X�g��
    private int i_PasteCnt = 0;

    //�I�������I�u�W�F�N�g
    private GameObject ClickObj;
    //�I�����Ɣ�I�����ɕύX����}�e���A��
    //0 = �f�t�H���g�}�e���A��
    //1 = �I�����̃A�E�g���C���}�e���A��
    [SerializeField] private Material[] materials = new Material[2];



    void Start()
    {
        ChoiseObj = null;
        CutObj = null;
        PasteObj = null;
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
                if (i_PasteCnt > 0)
                {
                    PasteObj.SetActive(true);
                }
                v3_mousePos = Input.mousePosition;
                v3_mousePos.z = 10;

                v3_scrWldPos = Camera.main.ScreenToWorldPoint(v3_mousePos);
                PasteObj.transform.position = v3_scrWldPos;
                
                //�N���b�N������J�[�\���̈ʒu�ɃI�u�W�F�N�g��u��
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
                    Debug.Log("�����܂���");
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
        if (i_CutNum > 0)
        {
            CutObj = ChoiseObj;
            ChoiseObj = null;
            CutObj.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255 / 255f, 50f / 255f);
            i_PasteCnt = 0;
            PasteObj = null;
        }
    }

    //�y�[�X�g����Ƃ�
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

            //�J�[�\���������I�ɉ�ʒ����Ɉړ�(����ǉ��\��)
        }
    }

    public void CheckPasteTrigger(bool trigger)
    {
        b_checkPeast = trigger;
    }

}
