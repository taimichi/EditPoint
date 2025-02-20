using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController
{
    Rigidbody2D rb;

    const float moveSpeed = 2.5f;

    public MoveController(Rigidbody2D _rb)
    {
        rb = _rb;
    }

    // update���ɓ����inputLR�̒l��ς��邱�Ƃŉ��ړ��\
    public void MoveLR(int input)
    {
        Vector3 moveVelocity = rb.velocity;

        // ���ړ�
        moveVelocity.x = input * moveSpeed;

        // �΂߈ړ����̕����オ��h�~
        if(input == 0)
        {
            if (moveVelocity.y > 0)
            {
                moveVelocity.y = 0;
            }
        }

        rb.velocity = moveVelocity;
    }
}
