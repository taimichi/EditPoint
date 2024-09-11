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

    [System.Obsolete]
    public void Test(float maxTime)
    {
        FuturePos = new List<Vector2>();

        Physics2D.autoSimulation = false;
        float time = 0;
        float deltaTime = Time.fixedDeltaTime;
        while (time < maxTime)
        {
            Physics2D.Simulate(deltaTime);
            FuturePos.Add(this.gameObject.transform.position);
            time += deltaTime;
        }

        Physics2D.autoSimulation = true;
    }

    public void Test2(float maxTime)
    {
        int count = 0;
        float time = 0;
        float deltaTime = Time.fixedDeltaTime;
        while (time < maxTime)
        {
            count++;
            time += deltaTime;
        }
        this.gameObject.transform.position = FuturePos[count];
        Debug.Log(FuturePos[count]);
    }
}
