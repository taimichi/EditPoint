using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemyController : MonoBehaviour
{
    [SerializeField]
    public List<Vector3> pathPosition = new List<Vector3>();

    [SerializeField]
    public List<float> pathTime = new List<float>();

    [SerializeField]
    float speed = 1;

    [SerializeField]
    int nowPath = 0;

    float timer = 0;

    private void Start()
    {
        this.transform.position = pathPosition[nowPath];
        timer = pathTime[nowPath];

        pathPosition.Add(Vector3.zero);
        pathTime.Add(0);
    }

    private void Update()
    {
        // ���ԊǗ�
        timer -= Time.deltaTime * speed;

        // �ړ��p
        Vector3 movePos = this.transform.position;

        // �������v�Z
        Vector3 dist = pathPosition[nowPath + 1] - pathPosition[nowPath];
        if (nowPath + 1 == pathPosition.Count - 1)
        {
            dist = pathPosition[0] - pathPosition[nowPath];
        }
        Vector3 moveSpeed = dist / pathTime[nowPath];

        movePos += moveSpeed * Time.deltaTime * speed;
        
        this.transform.position = movePos;  // ���W�X�V

        // �p�X�X�V
        if (timer <= 0)
        {
            nowPath++;
            if (nowPath == pathPosition.Count - 1)
            {
                nowPath = 0;
            }
            timer = pathTime[nowPath];
        }
    }

}
