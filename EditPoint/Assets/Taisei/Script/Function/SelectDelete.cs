using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDelete : MonoBehaviour
{
    [SerializeField] private GameObject DeleteButton;
    private RectTransform buttonRect;
    private GetClip getClip;
    private ObjectScaleEditor objectScale;
    private GameObject SelectObj;
    private Camera UIcamera;

    /// <summary>
    /// false = clip/true = object
    /// </summary>
    private bool isTrigger;                 //�N���b�v���I�u�W�F�N�g���ǂ���

    private void Awake()
    {
        UIcamera = GameObject.Find("UICamera").GetComponent<Camera>();

        buttonRect = DeleteButton.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (DeleteButton.activeSelf)
        {
            //�{�^�����I�u�W�F�N�g�̈ʒu�Ɉړ�
            //�N���b�v�̏ꍇ
            if (!isTrigger)
            {
                RectTransform rect = SelectObj.GetComponent<RectTransform>();
                Vector3[] newPos = new Vector3[4];
                rect.GetWorldCorners(newPos);

                buttonRect.position = RectTransformUtility.WorldToScreenPoint(UIcamera, newPos[2]);
            }
            //�I�u�W�F�N�g�̏ꍇ
            else
            {
                Bounds bounds = SelectObj.GetComponent<SpriteRenderer>().bounds;
                Vector3 newPos = new Vector3(bounds.max.x, bounds.max.y, 0);

                buttonRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, newPos);
            }
        }
    }

    /// <summary>
    /// �{�^����\�����邩��\���ɂ��邩
    /// </summary>
    public void SetActiveButton(bool set)
    {
        //��\���ɂ���Ƃ�
        if (!set)
        {
            //�I�u�W�F�N�g����ɂ���
            SelectObj = null;
        }
        DeleteButton.SetActive(set);
    }

    //�f���[�g�{�^�����������Ƃ�
    public void OnDelete()
    {
        Debug.Log("�폜");
        //�N���b�v�폜
        if (!isTrigger)
        {
            getClip.ClipDestroy();
        }
        //�I�u�W�F�N�g�폜
        else
        {
            objectScale.ObjectDelete();
        }
        SelectObj = null;
        DeleteButton.SetActive(false);
    }

    /// <summary>
    /// �f���[�g�{�^�����N������
    /// </summary>
    /// <param name="trigger">false=�N���b�v/true=�I�u�W�F�N�g</param>
    /// <param name="obj">�I�������I�u�W�F�N�g</param>
    public void ButtonActive(bool trigger, GameObject obj)
    {
        SelectObj = obj;
        isTrigger = trigger;

        if(SelectObj != null)
        {
            SetActiveButton(true);
        }

        PosCalculation();
    }

    /// <summary>
    /// �f���[�g�{�^���̈ʒu���v�Z
    /// </summary>
    private void PosCalculation()
    {
        //�{�^�����I�u�W�F�N�g�̈ʒu�Ɉړ�
        //�N���b�v�̏ꍇ
        if (!isTrigger)
        {
            RectTransform rect = SelectObj.GetComponent<RectTransform>();
            Vector3[] newPos = new Vector3[4];
            rect.GetWorldCorners(newPos);

            buttonRect.position = RectTransformUtility.WorldToScreenPoint(UIcamera, newPos[2]);
        }
        //�I�u�W�F�N�g�̏ꍇ
        else
        {
            Bounds bounds = SelectObj.GetComponent<SpriteRenderer>().bounds;
            Vector3 newPos = new Vector3(bounds.max.x, bounds.max.y, 0);

            buttonRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, newPos);
        }
    }

    //�X�N���v�g���擾
    public void Get_getClip(GameObject obj)
    {
        getClip = obj.GetComponent<GetClip>();
    }
    public void Get_objectScaleEditor(GameObject obj)
    {
        objectScale = obj.GetComponent<ObjectScaleEditor>();
    }


}
