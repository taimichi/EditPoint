using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 force;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        force = new Vector3(7.5f, 0, 0);
        rb.AddForce(force);

    }

}
