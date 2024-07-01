using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GroundChecker gc;
    Rigidbody2D rb;

    const float moveSpeed = 5.0f;
    const float jumpPower = 10;

    [Range(-1, 1)] int inputLR = 0;
    bool inputJump = false;

    Vector3 moveVelocity;

    void Start()
    {
        gc = GetComponent<GroundChecker>();
        rb = GetComponent<Rigidbody2D>();

        gc.InitCol();
    }

    void Update()
    {
        gc.CheckGround();

        MoveController(inputLR);

        PlayerInput();

    }

    void MoveController(int input)
    {
        moveVelocity = rb.velocity;
        moveVelocity.x = input * moveSpeed;
        rb.velocity = moveVelocity;
    }

    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            inputLR = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputLR = -1;
        }
        else
        {
            inputLR = 0;
        }
    }

    void AutoInput()
    {
        if (rb.velocity.x == 0)
        {

        }   
    }
}
