using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float speed = 2.0f;        // 上下の移動速度
    public float height = 0.1f;       // 移動の高さ範囲

    private Vector3 startPos;

    void Start()
    {
        // 初期位置を保存
        startPos = transform.position;
    }

    void Update()
    {
        // オブジェクトのY座標を変更して上下に動かす
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}