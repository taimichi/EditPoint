using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMoveController : MonoBehaviour
{
    Rigidbody2D rb;

    Vector2 run = Vector2.zero;

    bool inputFlic = false;
    Vector2 flic = Vector2.zero;

    private List<Vector2> FuturePos = new List<Vector2>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError(this.gameObject + "‚ÉRigidbody2D‚ð‚Â‚¯‚Ä‚­‚¾‚³‚¢");
        }
    }

    public void MoveUpdate()
    {
        Vector2 moveVelocity = rb.velocity;

        if (inputFlic)
        {
            moveVelocity = flic;
            inputFlic = false;
        }
        else
        {
            moveVelocity.x = flic.x;
        }

        if (rb.gravityScale != 0)
        {
            moveVelocity.x += run.x;
        }
        else
        {
            moveVelocity += run;
        }

        rb.velocity = moveVelocity;
    }

    public void Run(Vector2 _runSpeed)
    {
        run = _runSpeed;
    }

    public void Flic(Vector2 _flicPower)
    {
        flic = _flicPower;
        inputFlic = true;
    }

    public void Friction(float _frictionPower)
    {
        if (flic != Vector2.zero)
        {
            flic *= _frictionPower;
        }
    }

    public void ResetMove()
    {
        Vector2 resetVerocity = new Vector2(0, 0);
        rb.velocity = resetVerocity;
    }


}
