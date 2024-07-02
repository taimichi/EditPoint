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

    [SerializeField, Header("�y�[�X�g�ł����")] private int PasteNum = 1;
    [SerializeField, Header("�J�b�g�ł����")] private int CutNum = 1;

    //�I�u�W�F�N�g�̃y�[�X�g��ړ�����Ƃ���true�ɂ���
    private bool setOnOff = false;
    //�I����Ԃ��ǂ���
    private bool choiseOnOff = false;

    private Vector3 pos;
    private Vector3 scrWldPos;

    //�y�[�X�g�ł��邩�ǂ���
    //false=�ݒu�\ true=�ݒu�s��
    private bool checkPeast = false;

    //�y�[�X�g���̃I�u�W�F�N�g
    private GameObject PasteObj;
    private string pasteObjName;

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
            if (!setOnOff)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        Debug.Log("UI����");
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
                pos = Input.mousePosition;
                pos.z = 10;

                scrWldPos = Camera.main.ScreenToWorldPoint(pos);
                PasteObj.transform.position = scrWldPos;
                if (Input.GetMouseButtonDown(0) && !checkPeast)
                {
                    pasteObjName = CutObj.name;
                    PasteObj.name = pasteObjName;
                    Destroy(CutObj);

                    PasteObj.GetComponent<Collider2D>().isTrigger = false;
                    layerChange.ChangeObjectList();
                    setOnOff = false;

                    CutObj = null;
                    pasteObjName = null;

                }
                else if (checkPeast)
                {
                    Debug.Log("�����܂���");
                }
            }
        }
    }

    public bool ReturnSetOnOff()
    {
        return setOnOff;
    }

    //�J�b�g�{�^������������
    public void OnCut()
    {
        if (CutNum > 0)
        {
            CutObj = ChoiseObj;
            ChoiseObj = null;
            CutObj.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255 / 255f, 50f / 255f);
        }
    }

    //�y�[�X�g����Ƃ�
    public void OnPaste()
    {
        if (PasteNum > 0)
        {
            setOnOff = true;
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
        checkPeast = trigger;
    }

}
