using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDelete : MonoBehaviour
{
    private Canvas deleteCanvas;
    private RectTransform canvasRect;

    [SerializeField] private GameObject DeleteButton;
    private RectTransform buttonRect;
    private GetClip getClip;
    private ObjectScaleEditor objectScale;
    private GameObject SelectObj;
    private Camera UIcamera;

    /// <summary>
    /// false = clip, true = object
    /// </summary>
    private bool isTrigger;                 //�N���b�v���I�u�W�F�N�g���ǂ���

    private void Awake()
    {
        deleteCanvas = this.gameObject.GetComponent<Canvas>();
        canvasRect = this.gameObject.GetComponent<RectTransform>();

        UIcamera = GameObject.Find("UICamera").GetComponent<Camera>();
        //deleteCanvas.worldCamera = UIcamera;

        buttonRect = DeleteButton.GetComponent<RectTransform>();
    }

    /// <summary>
    /// �{�^����\�����邩��\���ɂ��邩
    /// </summary>
    public void SetActiveButton(bool set)
    {
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
        DeleteButton.SetActive(false);
    }

    public void GetSelectObject(bool trigger, GameObject obj)
    {
        SetActiveButton(true);
        SelectObj = obj;
        isTrigger = trigger;

        //�{�^�����I�u�W�F�N�g�̈ʒu�Ɉړ�
        //�N���b�v�̏ꍇ
        if (!isTrigger)
        {
            buttonRect.position = RectTransformUtility.WorldToScreenPoint(UIcamera, SelectObj.transform.position);
        }
        //�I�u�W�F�N�g�̏ꍇ
        else
        {
            buttonRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, SelectObj.transform.position);
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
