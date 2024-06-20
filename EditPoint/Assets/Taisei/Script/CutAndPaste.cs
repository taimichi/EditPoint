using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutAndPaste : MonoBehaviour
{
    private GameObject ChoiseObj;
    private GameObject CutObj;

    [SerializeField] private LayerChange layerChange;

    [SerializeField, Header("�y�[�X�g�ł����")] private int PasteNum = 1;
    [SerializeField, Header("�J�b�g�ł����")] private int CutNum = 1;

    //�I�u�W�F�N�g�̃y�[�X�g��ړ�����Ƃ���true�ɂ���
    private bool setOnOff = false;
    //�I����Ԃ��ǂ���
    private bool choiseOnOff = false;

    private Vector3 pos;
    private Vector3 scrWldPos;

    void Start()
    {
        ChoiseObj = null;
        CutObj = null;
    }

    void Update()
    {
        //�y�[�X�g�E�I�u�W�F�N�g�ړ���Ԃ���Ȃ��Ƃ�
        if (!setOnOff)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                if (hit2d != false && hit2d.collider.gameObject.layer == LayerMask.NameToLayer("UI"))
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
            CutObj.transform.position = scrWldPos;
            if (Input.GetMouseButtonDown(0))
            {
                setOnOff = false;
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
            CutObj.SetActive(false);
        }
    }

    //�y�[�X�g����Ƃ�
    public void OnPaste()
    {
        if (PasteNum > 0)
        {
            setOnOff = true;
            CutObj.SetActive(true);
            switch (layerChange.ReturnLayreNum())
            {
                case 1:
                    CutObj.layer = LayerMask.NameToLayer("Layer1");
                    break;

                case 2:
                    CutObj.layer = LayerMask.NameToLayer("Layer2");
                    break;

                case 3:
                    CutObj.layer = LayerMask.NameToLayer("Layer3");
                    break;
            }

            //�J�[�\���������I�ɉ�ʒ����Ɉړ�(����ǉ��\��)
        }
    }

}
