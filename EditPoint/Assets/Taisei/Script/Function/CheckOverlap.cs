using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI同士が重なっているかチェックするため用
/// </summary>
public class CheckOverlap
{
    // メモ　Rectクラスに関して
    // RectクラスはBoxを作るクラス
    // Rect(左上X、左上Y、横幅、縦幅)として使う
    // よって、Rectの座標(0,0)は左上の座標となる

    /// <summary>
    /// UI同士が重なっているかチェック
    /// </summary>
    /// <returns>重なっている=true 重なっていない=false</returns>
    public bool IsOverlap(RectTransform rect1, RectTransform rect2)
    {
        // RectTransformの境界をワールド座標で取得
        Rect rect1World = GetWorldRect(rect1);
        Rect rect2World = GetWorldRect(rect2);

        // 境界が重なっているかどうかをチェック
        return rect1World.Overlaps(rect2World);
    }

    /// <summary>
    /// ワールド座標での境界を取得
    /// </summary>
    /// <returns>ワールド座標でのRectTransform</returns>
    public Rect GetWorldRect(RectTransform rt)
    {
        //四隅のワールド座標を入れる配列
        Vector3[] corners = new Vector3[4];
        //RectTransformの四隅のワールド座標を取得
        rt.GetWorldCorners(corners);

        return new Rect(corners[0], corners[2] - corners[0]);
    }

}
