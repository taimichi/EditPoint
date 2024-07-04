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

    private Vector3 v3_pos;
    private Vector3 v3_scrWldPos;

    //�y�[�X�g�ł��邩�ǂ���
    //false=�ݒu�\ true=�ݒu�s��
    private bool b_checkPeast = false;

    //�y�[�X�g���̃I�u�W�F�N�g
    private GameObject PasteObj;
    private string s_pasteObjName;

    void Start()
    {
        ChoiseObj = null;
        CutObj = null;
    }

    void Update()
    {
        //�ҏW���[�h��ON�̎�
        if (gm.ReturnEditMode() == true)
        {
            //�y�[�X�g�E�I�u�W�F�N�g�ړ���Ԃ���Ȃ��Ƃ�
            if (!b_setOnOff)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                    if (EventSystem.current.IsPointerOverGameObject() && 
                        //���C���[�̃I�u�W�F�N�g����Ȃ��Ƃ�
                        (hit2d.collider.gameObject.layer != LayerMask.NameToLayer("Layer1") &&
                         hit2d.collider.gameObject.layer != LayerMask.NameToLayer("Layer2") &&
                         hit2d.collider.gameObject.layer != LayerMask.NameToLayer("Layer3")))
                    {
                        return;
                    }

                    if (hit2d == false)
                    {
                        layerChange.OutChangeLayerNum(0);
                    }
                    else if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer1"))
                    {
                        layerChange.OutChangeLayerNum(1);
                    }
                    else if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer2"))
                    {
                        layerChange.OutChangeLayerNum(2);
                    }
                    else if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer3"))
                    {
                        layerChange.OutChangeLayerNum(3);
                    }

                    if (hit2d)
                    {
                        ChoiseObj = hit2d.transform.gameObject;
                    }
                }
            }
            //�y�[�X�g�E�I�u�W�F�N�g�ړ���Ԃ̎�
            else
            {
                v3_pos = Input.mousePosition;
                v3_pos.z = 10;

                v3_scrWldPos = Camera.main.ScreenToWorldPoint(v3_pos);
                PasteObj.transform.position = v3_scrWldPos;
                
                //�N���b�N������J�[�\���̈ʒu�ɃI�u�W�F�N�g��u��
                if (Input.GetMouseButtonDown(0) && !b_checkPeast)
                {
                    s_pasteObjName = CutObj.name;
                    PasteObj.name = s_pasteObjName;
                    Destroy(CutObj);

                    PasteObj.GetComponent<Collider2D>().isTrigger = false;
                    layerChange.ChangeObjectList();
                    b_setOnOff = false;

                    CutObj = null;
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
