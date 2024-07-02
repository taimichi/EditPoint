using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GroundChecker gc;
    Rigidbody2D rb;

    MoveController mc;

    void Start()
    {
        gc = GetComponent<GroundChecker>();
        rb = GetComponent<Rigidbody2D>();

        mc = new MoveController(rb);

        gc.InitCol();
    }

    void Update()
    {
        gc.CheckGround();

        mc.MoveLR();

        PlayerInput();
    }

    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            mc.inputLR = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            mc.inputLR = -1;
        }
        else
        {
            mc.inputLR = 0;
        }
    }

    void AutoInput()
    {
        if (rb.velocity.x == 0)
        {

        }   
    }
}
