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

    // update内に入れてinputLRの値を変えることで横移動可能
    public void MoveLR(int input)
    {
        Vector3 moveVelocity = rb.velocity;

        // 横移動
        moveVelocity.x = input * moveSpeed;

        // 斜め移動時の浮き上がり防止
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
