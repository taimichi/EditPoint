using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    // パブリック変数
    public LayerMask LayerMask; // チェック用のレイヤー
    // 変数
    BoxCollider2D col;          // ボックスコライダー2D
    bool isGround;              // 地面チェック用の変数

    const float RayLength = 0.1f;
    
    // 初期化
    public void InitCol()
    {
        col = GetComponent<BoxCollider2D>();
    }

    // プレイヤーの中心位置（胸元）を取得
    public Vector3 GetCenterPos()
    {
        Vector3 pos = transform.position;
        // ボックスコライダーのオフセットから中心を計算
        pos.x += col.offset.x;
        pos.y += col.offset.y;
        return pos;
    }

    // プレイヤーの足元座標を取得
    public Vector3 GetFootPos()
    {
        Vector3 pos = GetCenterPos();
        pos.y -= col.size.y / 2;
        return pos;
    }

    // 地面に接しているかチェック
    public void CheckGround()
    {

        isGround = false;   // 一旦空中判定にしておく

        // デバッグ用に線を出す
        Vector3 foot = GetFootPos();    // 始点
        Vector3 len = -Vector3.up * RayLength; // 長さ
        float width = col.size.x / 2;   // 当たり判定の幅

        // 左端、中央、右端の順にチェックしていく
        foot.x += -width;               // 初期位置を左にずらす

        for (int no = 0; no < 3; ++no)
        {
            // 当たり判定の結果用の変数
            RaycastHit2D result;

            // レイを飛ばして、指定したレイヤーにぶつかるかチェック
            result = Physics2D.Linecast(foot, foot + len, LayerMask);

            // デバッグ表示用
            Debug.DrawLine(foot, foot + len);

            if (result.collider.gameObject.TryGetComponent<TypeAttr>(out var typeAttr))
            {
                Debug.Log(typeAttr.isGround);
            }

            // コライダーと接触したかチェック
            if (result.collider != null)
            {
                isGround = true;        // 地面と接触した
                Debug.Log("地面と接触");
                return;                 // 終了
            }
            foot.x += width;
        }
        Debug.Log("空中");
    }
    // 地面に接している変数を取得
    public bool IsGround() { return isGround; }
}
