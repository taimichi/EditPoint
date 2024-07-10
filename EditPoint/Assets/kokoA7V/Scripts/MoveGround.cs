using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    [SerializeField]
    List<Vector3> path = new List<Vector3>();

    [SerializeField]
    List<float> pathTime = new List<float>();

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
        // �ړ��p
        Vector3 movePos = this.transform.position;

        Vector3 dist = path[nowPath + 1] - path[nowPath];
        if (nowPath == path.Count)
        {
            dist = path[0] - path[nowPath];
        }
        Vector3 moveSpeed = dist / pathTime[nowPath];
        movePos += moveSpeed * Time.deltaTime;

        this.transform.position = movePos;



        // ���ԊǗ�
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            nowPath++;
            if (nowPath == path.Count - 1)
            {
                nowPath = 0;
            }
            //Debug.Log(nowPath + " ����");
            timer = pathTime[nowPath];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player���q�I�u�W�F�N�g��
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = this.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Player�����ꂽ��q�I�u�W�F�N�g����
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}
