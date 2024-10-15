using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipFunction : MonoBehaviour
{
    private enum MODE_TYPE
    {
        normal,
        cut,
        delete
    }
    private MODE_TYPE mode = MODE_TYPE.normal;

    [SerializeField] private RectTransform Timebar;
    private int cutCount = 0;

    [SerializeField] private TimelineData timelineData;
    [SerializeField] private GetClip GetClip;

    private GameObject Clip;
    private GameObject CutedClip;

    private RectTransform grandParentRect;

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// クリップとタイムバーが重なっているかをチェック
    /// </summary>
    /// <param name="rect1">クリップのRectTransform</param>
    /// <param name="rect2">タイムバーのRectTransform</param>
    /// <returns>重なっている=true 重なっていない=false</returns>
    private bool IsOverlapping(RectTransform rect1, RectTransform rect2)
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
    /// <param name="rt">取得するRectTransform</param>
    /// <returns>ワールド座標でのRectTransform</returns>
    private Rect GetWorldRect(RectTransform rt)
    {
        //四隅のワールド座標を入れる配列
        Vector3[] corners = new Vector3[4];
        //RectTransformの四隅のワールド座標を取得
        rt.GetWorldCorners(corners);

        return new Rect(corners[0], corners[2] - corners[0]);
    }


    /// <summary>
    /// カット機能　ボタンで呼び出す
    /// </summary>
    public void OnCut()
    {
        Clip = GetClip.ReturnGetClip();
        RectTransform clipRect = Clip.GetComponent<RectTransform>();

        //カット機能を使うのはクリップとタイムバーが重なってる時のみ
        if(IsOverlapping(clipRect, Timebar))
        {
            mode = MODE_TYPE.cut;
            cutCount++;

            grandParentRect = clipRect.parent.parent.GetComponent<RectTransform>();

            Vector3 leftEdge = grandParentRect.InverseTransformPoint(clipRect.position) 
                + new Vector3(clipRect.rect.width * clipRect.pivot.x, 0, 0);
            float dis = Timebar.localPosition.x - leftEdge.x;
        }



    }

    /// <summary>
    /// クリップ削除機能　ボタンで呼び出す
    /// </summary>
    public void OnDelete()
    {
        mode = MODE_TYPE.delete;
    }
}
