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
        PathReset();
    }

    private void Update()
    {
        if (GameData.GameEntity.isPlayNow)
        {
            EnemyMove();
        }

        if (GameData.GameEntity.isTimebarReset)
        {
            PathReset();
        }
    }

    private void PathReset()
    {
        nowPath = 0;
        this.transform.position = pathPosition[nowPath];
        timer = pathTime[nowPath];
    }

    private void EnemyMove()
    {
        // 時間管理
        timer -= Time.deltaTime * speed;

        // 移動用
        Vector3 movePos = this.transform.position;

        // 方向を計算
        Vector3 dist;
        if (nowPath == pathPosition.Count - 1)
        {
            dist = pathPosition[0] - pathPosition[nowPath];
        }
        else
        {
            dist = pathPosition[nowPath + 1] - pathPosition[nowPath];
        }
        Vector3 moveSpeed = dist / pathTime[nowPath];

        movePos += moveSpeed * Time.deltaTime * speed;

        this.transform.position = movePos;  // 座標更新

        // パス更新
        if (timer <= 0)
        {
            if (nowPath == pathPosition.Count - 1)
            {
                nowPath = 0;
            }
            else
            {
                nowPath++;
            }
            timer = pathTime[nowPath];
        }

        // 動いてる方向に見た目変更
        Vector3 dir = this.transform.localScale;
        dir.x = Mathf.Sign(dist.x) * -1;
        this.transform.localScale = dir;
    }

}
