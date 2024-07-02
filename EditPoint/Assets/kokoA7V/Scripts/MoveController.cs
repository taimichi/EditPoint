using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController
{

    Rigidbody2D rb;

    const float moveSpeed = 5.0f;
    const float jumpPower = 10;

    [Range(-1, 1)]
    public int inputLR = 0;
    public bool inputJump = false;

    Vector3 moveVelocity;

    public MoveController(Rigidbody2D _rb)
    {
        rb = _rb;
    }

    // update���ɓ����inputLR�̒l��ς��邱�Ƃŉ��ړ��\
    public void MoveLR()
    {
        moveVelocity = rb.velocity;
        moveVelocity.x = inputLR * moveSpeed;
        rb.velocity = moveVelocity;
    }
}
