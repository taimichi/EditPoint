using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClipFunction : MonoBehaviour
{
    [SerializeField] private RectTransform Timebar; //タイムバーのRectTransform

    [SerializeField] private GetClip GetClip;   //選択したクリップを取得するスクリプト

    private GameObject Clip;

    private RectTransform grandParentRect;

    private float old_maxTime = 0f; //カット前のクリップの最大時間
    private float new_maxTime = 0f; //カットした後のクリップの最大時間

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
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        Clip = GetClip.ReturnGetClip();
        Clip.transform.GetChild(0).gameObject.SetActive(false);
        RectTransform clipRect = Clip.GetComponent<RectTransform>();
        ClipPlay clipPlay = Clip.GetComponent<ClipPlay>();

        //カット機能を使うのはクリップとタイムバーが重なってる時のみ
        if(IsOverlapping(clipRect, Timebar))
        {
            old_maxTime = clipPlay.ReturnMaxTime();

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
            //カットしたクリップの長さを調整
            newClipRect.sizeDelta = new Vector2(newDis, newClipRect.rect.height);
            //カットしたクリップの位置を調整
            newClipRect.localPosition = new Vector2(rightEdge + 0.1f, clipRect.localPosition.y);

            //一旦片方のクリップのオブジェクトとの紐づけを解除
            ClipPlay newClipPlay = newClip.GetComponent<ClipPlay>();
            newClipPlay.DestroyConnectObj();

            //クリップと紐づけられたオブジェクトを取得
            List<GameObject> newConnectObj = clipPlay.ReturnConnectObj();

            //クリップの長さと速さの初期値を設定
            //クリップ(左)
            ClipSpeed clipSpeed = Clip.GetComponent<ClipSpeed>();
            clipSpeed.GetStartWidth(dis);   // 長さ
            clipSpeed.UpdateSpeed(1f);      // 速さ
            //クリップ(右)
            ClipSpeed newClipSpeed = newClip.GetComponent<ClipSpeed>();
            newClipSpeed.GetStartWidth(newDis); //長さ
            newClipSpeed.UpdateSpeed(1f);       //速さ

            newClipPlay.CalculationMaxTime();
            new_maxTime = newClipPlay.ReturnMaxTime();
            newClipPlay.UpdateStartTime(old_maxTime - new_maxTime);

            //クリップに紐づけられたオブジェクトを複製して、新しいクリップと紐づけ
            for (int i = 0; i < newConnectObj.Count; i++)
            {
                GameObject obj = Instantiate(newConnectObj[i]);
                //動く床だった時
                if(TryGetComponent<MoveGround>(out var test))
                {
                    test.GetClipTime_Auto(old_maxTime - new_maxTime);
                }
                newClipPlay.OutGetObj(obj);
            }
        }
    }

}
