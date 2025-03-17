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

    [SerializeField]
    private LayerMask returnLayerMask;
    private int inputLR = 0;

    private Vector3 startSize;


    private void Start()
    {
        startSize = this.transform.localScale;
        PathReset();
    }

    private void Update()
    {
        if (GameData.GameEntity.isPlayNow)
        {
            EnemyMove();
            AutoInverse();
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
        this.transform.localScale = startSize;
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

        if(dist.x > 0)
        {
            inputLR = 1;
        }
        else if (dist.x < 0)
        {
            inputLR = -1;
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

    /// <summary>
    /// 反転処理
    /// </summary>
    private void AutoInverse()
    {
        bool isHit = false;

        float rayLength = 1.0f;
        float rayWidth = 0.7f;
        Vector3 center = this.transform.position;    // 始点
        Vector3 len = Vector3.right * rayLength * inputLR; // 長さ

        // カプセルコライダーの都合上、レイの位置が悪かったので調整
        Vector2 centerOffset = new Vector2(0.2f, 0f);
        center.x += centerOffset.x * inputLR;
        center.y += centerOffset.y;

        center.y += rayWidth;

        for (int i = 0; i < 3; i++)
        {
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(center, Vector2.right, len.x, returnLayerMask))
            {
                if (hit.collider.gameObject.TryGetComponent<GroundAttr>(out var typeAttr))
                {
                    //Debug.Log("あたったやつ:" + hit.collider);
                    if (typeAttr.isGround)
                    {
                        isHit = true;
                        //Debug.Log("はんてーん");
                    }
                }
            }

            // デバッグ表示用
            Debug.DrawLine(center, center + len);

            center.y -= rayWidth;
        }

        Vector3 scale = this.transform.localScale;

        // 向き切り替え
        if (isHit)
        {
            scale.x = Mathf.Abs(scale.x) * inputLR;
            if (nowPath == pathPosition.Count - 1)
            {
                nowPath = 0;
            }
            else
            {
                nowPath++;
            }
            timer = pathTime[nowPath];

            isHit = false;
        }
        this.transform.localScale = scale;
    }

}
