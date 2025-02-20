using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaleEditor : MonoBehaviour
{
    // ���C��΂��Ƃ��̃n���h���擾�p���C���[�}�X�N
    LayerMask handle_LayerMask;

    // ���g�����X�t�H�[��
    [SerializeField]
    Vector2 objPosition = new Vector2(0, 0);
    [SerializeField]
    float objRotation = 0;
    [SerializeField]
    Vector2 objScale = new Vector2(1, 1);

    [SerializeField]
    GameObject[] handle = new GameObject[10];

    // UR = 0
    // UL = 1
    // DR = 2
    // DL = 3
    // R = 4
    // L = 5
    // U = 6
    // D = 7
    // Rot = 8
    // Body = 9

    //�n���h���T�C�Y�����p
    [SerializeField]
    float handleSize = 0.1f;

    // �}�E�X�h���b�O�J�n�ʒu
    Vector2 clickStartPos;

    // �N���b�N���Ƀn���h����͂�ł������ۂ�
    bool isHandleGrab = false;

    // �擾�����n���h�����
    float nowHandlePriority = 0;
    Vector2 scaleSign;
    HandleType nowHandleType;

    // ���z�I�u�W�F�N�g
    [SerializeField]
    GameObject virtualObject;

    [SerializeField]
    GameObject handleParent;

    // �ҏW�Ώ�
    public GameObject editObject;

    private void Start()
    {
        handle_LayerMask = LayerMask.NameToLayer("Handle");

        //virtualObject.transform.position = editObject.transform.position;
        //virtualObject.transform.localEulerAngles = new Vector3(0, 0, editObject.transform.localEulerAngles.z);
        //virtualObject.transform.localScale = editObject.transform.localScale;
    }

    private void Update()
    {
        virtualObject.GetComponent<VirtualObjectCollisionChecker>().nowEditObject = editObject;

        // �n���h���|�W�V����
        handle[0].transform.localPosition = new Vector2(objScale.x / 2, objScale.y / 2);
        handle[1].transform.localPosition = new Vector2(-objScale.x / 2, objScale.y / 2);
        handle[2].transform.localPosition = new Vector2(objScale.x / 2, -objScale.y / 2);
        handle[3].transform.localPosition = new Vector2(-objScale.x / 2, -objScale.y / 2);

        handle[4].transform.localPosition = new Vector2(objScale.x / 2, 0);
        handle[5].transform.localPosition = new Vector2(-objScale.x / 2, 0);
        handle[6].transform.localPosition = new Vector2(0, objScale.y / 2);
        handle[7].transform.localPosition = new Vector2(0, -objScale.y / 2);

        handle[8].transform.localPosition = new Vector2(0, objScale.y / 2 + 0.5f);

        handle[9].transform.localPosition = Vector2.zero;


        // �n���h���X�P�[��
        handle[0].transform.localScale = new Vector2(2 * handleSize, 2 * handleSize);
        handle[1].transform.localScale = new Vector2(2 * handleSize, 2 * handleSize);
        handle[2].transform.localScale = new Vector2(2 * handleSize, 2 * handleSize);
        handle[3].transform.localScale = new Vector2(2 * handleSize, 2 * handleSize);

        handle[4].transform.localScale = new Vector2(handleSize, objScale.y);
        handle[5].transform.localScale = new Vector2(handleSize, objScale.y);
        handle[6].transform.localScale = new Vector2(objScale.x, handleSize);
        handle[7].transform.localScale = new Vector2(objScale.x, handleSize);

        handle[8].transform.localScale = new Vector2(2 * handleSize, 2 * handleSize);

        handle[9].transform.localScale = objScale;


        // �n���h���܂Ƃ߂̃|�W�V�����ƃ��[�e�[�V����
        // �X�P�[���͂�����Ȃ�
        handleParent.transform.position = objPosition;
        handleParent.transform.localEulerAngles = new Vector3(0, 0, objRotation);

        // �����߂�
        objPosition = virtualObject.gameObject.transform.position;
        objRotation = virtualObject.gameObject.transform.localEulerAngles.z;
        objScale = virtualObject.gameObject.transform.localScale;

        // �}�E�X�N���b�N�A�n���h���擾
        if (Input.GetMouseButtonDown(0))
        {
            nowHandlePriority = 0;
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(MousePos(), Vector2.zero, handle_LayerMask))
            {
                // �n���h�����ǂ����`�F�b�N
                if (hit.collider.gameObject.TryGetComponent<HandleSign>(out var _handleSign))
                {
                    // �O���u�t���O�`�F�b�N
                    clickStartPos = MousePos();
                    isHandleGrab = true;

                    // �v���C�I���e�B����ԍ������̂��擾
                    if (_handleSign.priority > nowHandlePriority)
                    {
                        scaleSign = _handleSign.handleSign;
                        nowHandleType = _handleSign.handleType;
                        nowHandlePriority = _handleSign.priority;
                    }
                }
            }
        }

        Vector2 nowMousePos = MousePos();

        // �ύX�O
        Vector2 mouseVec = nowMousePos - clickStartPos;

        // �p�x�v�Z��
        float mouseRad = Mathf.Atan2(mouseVec.y, mouseVec.x);
        float editRad = mouseRad - objRotation * Mathf.Deg2Rad;
        
        // �ύX��
        Vector2 editVec = new Vector2(mouseVec.magnitude * Mathf.Cos(editRad), mouseVec.magnitude * Mathf.Sin(editRad));

        Vector2 rotMouseVec = nowMousePos - objPosition;
        float rotRad = Mathf.Atan2(rotMouseVec.y, rotMouseVec.x);

        // �}�E�X�h���b�O�A�I�u�W�F�N�g�傫���ύX��
        if (Input.GetMouseButton(0))
        {
            if (isHandleGrab)
            {
                // ��]�n���h�����ۂ�
                if (nowHandleType == HandleType.rot)
                {
                    virtualObject.transform.localEulerAngles = new Vector3(0, 0, rotRad * Mathf.Rad2Deg - 90);
                }
                else if (nowHandleType == HandleType.body)
                {
                    virtualObject.transform.position = (Vector2)editObject.transform.position + mouseVec;
                }
                else
                {
                    //Vector3 absSign = new Vector3(Mathf.Abs(scaleSign.x), Mathf.Abs(scaleSign.y), 1);
                    virtualObject.transform.position = (Vector2)editObject.transform.position + mouseVec / 2;
                    virtualObject.transform.localScale = (Vector2)editObject.transform.localScale + editVec * scaleSign;
                }
            }
        }

        // �}�E�X�N���b�N�����A���I�u�W�F�N�g�̃T�C�Y�ɕύX
        if (Input.GetMouseButtonUp(0))
        {
            if (isHandleGrab)
            {
                // virtual�����I�u�W�F�N�g�ɐڐG���Ă��邩�`�F�b�N
                // �ڐG���ĂȂ��ꍇ�̂ݑ���A���Ă��烊�Z�b�g
                if (virtualObject.GetComponent<VirtualObjectCollisionChecker>().isCollision == false)
                {
                    // ��]�n���h�����ۂ�
                    if (nowHandleType == HandleType.rot)
                    {
                        editObject.transform.localEulerAngles = new Vector3(0, 0, virtualObject.transform.localEulerAngles.z);
                    }
                    else if (nowHandleType == HandleType.body)
                    {
                        editObject.transform.position = virtualObject.transform.position;
                    }
                    else
                    {
                        editObject.transform.position = virtualObject.transform.position;
                        editObject.transform.localScale = virtualObject.transform.localScale;
                    }


                    objPosition = editObject.transform.position;
                    objRotation = editObject.transform.localEulerAngles.z;
                    objScale = editObject.transform.localScale;
                }
                else
                {
                    virtualObject.transform.position = editObject.transform.position;
                    virtualObject.transform.localEulerAngles = new Vector3(0, 0, editObject.transform.localEulerAngles.z);
                    virtualObject.transform.localScale = editObject.transform.localScale;
                }

            }

            // �t���O����
            isHandleGrab = false;
            nowHandleType = HandleType.def;

        }
        
    }

    // �}�E�X�|�W�V�����擾
    private Vector3 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }

    public void GetObjTransform(GameObject _editObj)
    {
        editObject = _editObj;
        //objPosition = _editObj.transform.position;
        //objRotation = _editObj.transform.localEulerAngles.z;
        //objScale = _editObj.transform.localScale;

        virtualObject.transform.position = _editObj.transform.position;
        virtualObject.transform.localEulerAngles = new Vector3(0, 0, _editObj.transform.localEulerAngles.z);
        virtualObject.transform.localScale = _editObj.transform.localScale;
    }

}
