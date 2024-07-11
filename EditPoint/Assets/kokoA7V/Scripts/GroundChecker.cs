using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    // パブリック変数
    public LayerMask L_LayerMask; // チェック用のレイヤー
    // 変数
    CapsuleCollider2D col;          // ボックスコライダー2D
    bool isGround;              // 地面チェック用の変数

    const float RayLength = 0.1f;

    [SerializeField] private GameObject layer1obj;
    [SerializeField] private GameObject layer2obj;
    [SerializeField] private GameObject layer3obj;

    private int i_1Index;
    private int i_2Index;
    private int i_3Index;

    [SerializeField] private PlayerLayer plLayer;

    private void Start()
    {
        i_1Index = layer1obj.transform.GetSiblingIndex() - 2;
        i_2Index = layer2obj.transform.GetSiblingIndex() - 2;
        i_3Index = layer3obj.transform.GetSiblingIndex() - 2;
    }

    private void Update()
    {
        i_1Index = layer1obj.transform.GetSiblingIndex() - 2;
        i_2Index = layer2obj.transform.GetSiblingIndex() - 2;
        i_3Index = layer3obj.transform.GetSiblingIndex() - 2;

    }

    // 初期化
    public void InitCol()
    {
        col = GetComponent<CapsuleCollider2D>();
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

            LayerCheck();

            // レイを飛ばして、指定したレイヤーにぶつかるかチェック
            result = Physics2D.Linecast(foot, foot + len, L_LayerMask);

            // デバッグ表示用
            Debug.DrawLine(foot, foot + len);

            // コライダーと接触したかチェック
            if (result.collider != null)
            {
                if (result.collider.gameObject.TryGetComponent<GroundAttr>(out var typeAttr))
                {
                    if (typeAttr.isGround)
                    {
                        isGround = true;        // 地面と接触した
                        //Debug.Log("地面と接触");
                        return;                 // 終了
                    }
                }

                //isGround = true;        // 地面と接触した
                //Debug.Log("地面と接触");
                //return;                 // 終了
            }
            foot.x += width;
        }
        Debug.Log("空中");
    }
    // 地面に接している変数を取得
    public bool IsGround() { return isGround; }

    private void LayerCheck()
    {
        switch (plLayer.ReturnPLLayer() - 1)
        {
            //レイヤー1
            case 0:
                switch (i_1Index)
                {
                    //１だけ
                    case 0:
                        SetMultipleLayerMask(new int[] {8});
                        break;

                    case 1:
                        //１と２
                        if(i_2Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 8, 9 });
                        }
                        //１と３
                        else if (i_3Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 8, 10 });
                        }
                        break;

                    //１と２と３
                    case 2:
                        SetMultipleLayerMask(new int[] { 8, 9, 10 });
                        break;
                }
                break;

            //レイヤー２
            case 1:
                switch (i_2Index)
                {
                    //２のみ
                    case 0:
                        SetMultipleLayerMask(new int[] { 9 });
                        break;

                    case 1:
                        //１と２
                        if(i_1Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 8, 9 });
                        }
                        //２と３
                        else if(i_3Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 9, 10 });
                        }
                        break;

                    //１と２と３
                    case 2:
                        SetMultipleLayerMask(new int[] { 8, 9, 10 });
                        break;
                }
                break;

            //レイヤー３
            case 2:
                switch (i_3Index)
                {
                    //３のみ
                    case 0:
                        SetMultipleLayerMask(new int[] { 10 });
                        break;

                    case 1:
                        //１と３
                        if(i_1Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 8, 10 });
                        }
                        //２と３
                        else if (i_2Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 9, 10 });
                        }
                        break;

                    //１と２と３
                    case 2:
                        SetMultipleLayerMask(new int[] { 8, 9, 10 });
                        break;
                }
                break;
        }
    }

    void SetMultipleLayerMask(int[] layers)
    {
        L_LayerMask = 0;
        foreach (int layer in layers)
        {
            L_LayerMask |= (1 << layer);
        }
    }
}
