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
            Debug.LogError(this.gameObject + "��Rigidbody2D�����Ă�������");
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

    /// <summary>
    /// �^�C���o�[���蓮�œ��������Ƃ��A�v���C���[�̍��W���^�C���o�[�̎��Ԃ̎��̈ʒu�ɓ��������߂̌v�Z
    /// </summary>
    public void FutureCalculation(float maxTime)
    {
        Physics2D.autoSimulation = false;
        float time = 0f;
        float deltaTime = Time.fixedDeltaTime;

        while (time < maxTime)
        {
            Physics2D.Simulate(deltaTime);
            time += deltaTime;
        }



        Physics2D.autoSimulation = true;

    }

    /// <summary>
    /// �^�C���o�[���蓮�Ŋ����߂����Ƃ��̌v�Z
    /// </summary>
    public void BackToTheFutureCalculation(float _time)
    {

    }

}
