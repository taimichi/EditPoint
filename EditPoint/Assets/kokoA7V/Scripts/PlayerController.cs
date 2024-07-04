using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GroundChecker gc;
    Rigidbody2D rb;

    //MoveController mc;

    Vector3 scale;

    int inputLR = 0;
    Vector3 movePos;

    void Start()
    {
        gc = GetComponent<GroundChecker>();
        rb = GetComponent<Rigidbody2D>();

        //mc = new MoveController(rb);

        gc.InitCol();
    }

    void Update()
    {
        gc.CheckGround();

        //mc.MoveLR();

        PlayerInput();



        movePos = this.transform.position;
        if (inputLR != 0)
        {
            movePos.x += inputLR * 0.1f;
        }
        this.transform.position = movePos;

        //Debug.Log(rb.velocity);

        //scale = this.transform.localScale;
        //if (mc.inputLR != 0)
        //{
        //    scale.x = Mathf.Abs(scale.x) * mc.inputLR;
        //}
        //this.transform.localScale = scale;

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
        if (rb.velocity.x == 0)
        {

        }   
    }
}
