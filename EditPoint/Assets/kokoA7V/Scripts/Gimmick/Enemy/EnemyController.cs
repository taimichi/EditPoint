using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    GameObject target;

    [SerializeField]
    float moveSpeed = 10;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // ˆÚ“®
        Vector3 moveVelocity = rb.velocity;

        Vector3 distance = target.transform.position - this.transform.position;
        float radian = Mathf.Atan2(distance.y, distance.x);
        moveVelocity.x = Mathf.Cos(radian) * moveSpeed;
        moveVelocity.y = Mathf.Sin(radian) * moveSpeed;

        rb.velocity = moveVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.TakeDamage(1);

        }
    }
}

