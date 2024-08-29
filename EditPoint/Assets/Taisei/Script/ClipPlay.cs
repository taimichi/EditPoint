using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipPlay : MonoBehaviour
{
    private RectTransform rect_timeBar;
    [SerializeField] private RectTransform rect_Clip;

    private float f_timer = 0;
    /// <summary>
    /// クリップを再生するかどうか
    /// </summary>
    private bool b_clipPlay = false;

    void Start()
    {
        //タイムバーのRectTransformを取得
        rect_timeBar = GameObject.Find("Timebar").GetComponent<RectTransform>();
    }

    void Update()
    {
        if (IsOverlapping(rect_Clip, rect_timeBar))
        {
            Debug.Log("UIオブジェクトが接触しています");
            b_clipPlay = true;
        }
        else
        {
            Debug.Log("UIオブジェクトが接触していません");
            b_clipPlay = false;
        }

        //クリップ再生中の処理
        if (b_clipPlay)
        {
            f_timer += Time.deltaTime;

        }
        //クリップ再生してないときの処理
        else
        {
            f_timer = 0f;
        }
    }

    public float ReturnClipTime()
    {
        return f_timer;
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
}
