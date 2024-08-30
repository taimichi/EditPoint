using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    public List<Vector3> path = new List<Vector3>();

    public List<float> pathTime = new List<float>();

    public float speed = 1;

    [SerializeField]
    int nowPath = 0;

    float timer;

    private void Start()
    {
        this.transform.position = path[0];
        timer = pathTime[0];

        path.Add(Vector3.zero);
        pathTime.Add(0);
    }

    private void Update()
    {
        // 移動用
        Vector3 movePos = this.transform.position;

        Vector3 dist = path[nowPath + 1] - path[nowPath];
        if (nowPath + 1 == path.Count - 1)
        {
            dist = path[0] - path[nowPath];
        }
        Vector3 moveSpeed = dist / pathTime[nowPath];
        movePos += moveSpeed * Time.deltaTime * speed;

        this.transform.position = movePos;



        // 時間管理
        timer -= Time.deltaTime * speed;

        if (timer <= 0)
        {
            nowPath++;
            if (nowPath == path.Count - 1)
            {
                nowPath = 0;
            }
            //Debug.Log(nowPath + " 到着");
            timer = pathTime[nowPath];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Playerを子オブジェクト化
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = this.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Playerが離れたら子オブジェクト解除
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}
