using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GroundChecker gc;
    //Rigidbody2D rb;

    //MoveController mc;

    Vector3 scale;

    [SerializeField]
    float moveSpeed = 0.1f;

    int inputLR = 0;
    Vector3 movePos;

    void Start()
    {
        gc = GetComponent<GroundChecker>();
        //rb = GetComponent<Rigidbody2D>();

        //mc = new MoveController(rb);

        gc.InitCol();

        inputLR = 1;
    }

    void Update()
    {
        gc.CheckGround();

        //mc.MoveLR();

        //PlayerInput();

        AutoInput();

        TestMove();
    }

    void TestMove()
    {
        movePos = this.transform.position;
        if (inputLR != 0)
        {
            movePos.x += inputLR * moveSpeed;
        }
        this.transform.position = movePos;

        scale = this.transform.localScale;
        if (inputLR != 0)
        {
            scale.x = Mathf.Abs(scale.x) * inputLR;
        }
        this.transform.localScale = scale;
    }

    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            //mc.inputLR = 1;
            inputLR = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //mc.inputLR = -1;
            inputLR = -1;
        }
        else
        {
            //mc.inputLR = 0;
            inputLR = 0;
        }
    }

    void AutoInput()
    {
        bool isHit = false;

        float RayLength = 2;
        Vector3 center = gc.GetCenterPos();    // �n�_
        Vector3 len = Vector3.right * RayLength * transform.localScale.x; // ����

        // �����蔻��̌��ʗp�̕ϐ�
        RaycastHit2D result;

        // ���C���΂��āA�w�肵�����C���[�ɂԂ��邩�`�F�b�N
        result = Physics2D.Linecast(center, center + len, gc.LayerMask);

        // �f�o�b�O�\���p
        Debug.DrawLine(center, center + len);

        // �R���C�_�[�ƐڐG�������`�F�b�N
        if (result.collider != null)
        {
            isHit = true;
            Debug.Log("�Ԃ�����");
        }
        else
        {
            Debug.Log("������");
        }

        // �����؂�ւ�
        if (isHit)
        {
            inputLR *= -1;
            isHit = false;
        }
    }
}
