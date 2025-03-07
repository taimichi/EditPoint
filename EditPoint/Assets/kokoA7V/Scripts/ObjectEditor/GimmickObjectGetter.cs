using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickObjectGetter : MonoBehaviour
{
    [SerializeField]
    GameObject ObjectScaleEditor;

    ObjectScaleEditor ose;

    [SerializeField]
    GameObject editObj;

    private void Start()
    {
        ose = ObjectScaleEditor.GetComponent<ObjectScaleEditor>();
        ObjectScaleEditor.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            GimmickObjGet(false);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GimmickObjGet(true);
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g�̑I�������@�����ō�or�E�N���b�N�𔻕�
    /// �� false/�E true
    /// </summary>
    /// <param name="trigger">true/false</param>
    private void GimmickObjGet(bool trigger)
    {

        // ObjectScaleEdditor�p�̃u���b�N�N���b�N���擾�v���O����
        if (editObj == null)
        {
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(MousePos(), Vector2.zero))
            {
                // �R�s�y���͂������Ȃ��悤��
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Gimmick") && ModeData.ModeEntity.mode != ModeData.Mode.copy�@&& ModeData.ModeEntity.mode != ModeData.Mode.paste)
                {
                    ObjectScaleEditor.SetActive(true);
                    ose.GetObjTransform(hit.collider.gameObject, trigger);
                    editObj = hit.collider.gameObject;
                }
            }
        }
        else
        {
            bool isEditCancel = true;
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(MousePos(), Vector2.zero))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Handle") || hit.collider.gameObject == editObj)
                {
                    isEditCancel = false;
                }
            }

            if (isEditCancel)
            {
                editObj = null;
                ose.DeleteButtonChange(false);
                ObjectScaleEditor.SetActive(false);
            }

        }
    }

    private Vector3 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }
}
