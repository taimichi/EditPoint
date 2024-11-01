using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClipFunction : MonoBehaviour
{
    [SerializeField] private RectTransform Timebar;
    private int cutCount = 0;

    [SerializeField] private GetClip GetClip;

    private GameObject Clip;

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
        //再生中は編集機能をロック
        if (GameData.GameEntity.b_playNow)
        {
            return;
        }

        Clip = GetClip.ReturnGetClip();
        RectTransform clipRect = Clip.GetComponent<RectTransform>();

        //カット機能を使うのはクリップとタイムバーが重なってる時のみ
        if(IsOverlapping(clipRect, Timebar))
        {
            cutCount++;

            grandParentRect = clipRect.parent.parent.GetComponent<RectTransform>();

            //選択したクリップの左端の座標
            Vector3 leftEdge = grandParentRect.InverseTransformPoint(clipRect.position) 
                + new Vector3(clipRect.rect.width * clipRect.pivot.x, 0, 0);
            //左端からの長さ
            float dis = Timebar.localPosition.x - leftEdge.x;

            //サイズを調整
            dis = ((float)Math.Round(dis / TimelineData.TimelineEntity.f_oneResize)) * TimelineData.TimelineEntity.f_oneResize;
            
            //タイムバーから右端までの長さ
            float newDis = clipRect.rect.width - dis;


            //カットした後の左側のクリップ
            clipRect.sizeDelta = new Vector2(dis, clipRect.rect.height);

            //右端取得
            float rightEdge = clipRect.anchoredPosition.x + (clipRect.rect.width * (1 - clipRect.pivot.x));

            //カットした時の右側用
            GameObject newClip = Instantiate(Clip, clipRect.localPosition, Quaternion.identity, this.gameObject.transform);
            newClip.name = Clip.name + "(CutClip)";
            RectTransform newClipRect = newClip.GetComponent<RectTransform>();
            newClipRect.sizeDelta = new Vector2(newDis, newClipRect.rect.height);

            newClipRect.localPosition = new Vector2(rightEdge, clipRect.localPosition.y);

        }
    }

}
