using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    [SerializeField]
    Vector3[] path =
    {
        new Vector3(0f,0f,0f),
        new Vector3(5f,0f,0f),
        new Vector3(0f,0f,0f),
    };

    [SerializeField]
    float[] pathTime =
    {
        1,
        1,
        0,
    };

    [SerializeField]
    int nowPath = 0;

    [SerializeField]
    float timer;

    private void Start()
    {
        this.transform.position = path[0];
        timer = pathTime[0];
    }

    private void Update()
    {
        // à⁄ìÆóp
        Vector3 movePos = this.transform.position;

        Vector3 dist = path[nowPath + 1] - path[nowPath];
        if (nowPath == path.Length)
        {
            dist = path[0] - path[nowPath];
        }
        Vector3 moveSpeed = dist / pathTime[nowPath];
        movePos += moveSpeed * Time.deltaTime;

        this.transform.position = movePos;



        // éûä‘ä«óù
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            nowPath++;
            if (nowPath == path.Length - 1)
            {
                nowPath = 0;
            }
            Debug.Log(nowPath + " ìûíÖ");
            timer = pathTime[nowPath];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = this.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}
