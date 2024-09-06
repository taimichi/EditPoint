using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class TimeBar : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private float f_speed = 278f;
    private RectTransform barPos;
    private Vector2 v2_nowPos;
    [SerializeField, Header("時間(秒)")] private float limit = 60f;
    [SerializeField] private TimelineData timelineData;
    private Vector2 v2_startPos;

    [SerializeField] private TimeData timeData;

    private float f_nowTime = 0;
    private bool b_dragMode = false;

    private void Awake()
    {
        timeData.b_DragMode = b_dragMode;
        timeData.f_nowTime = f_nowTime;
    }

    void Start()
    {
        barPos = this.gameObject.GetComponent<RectTransform>();
        v2_nowPos = barPos.localPosition;
        v2_startPos = barPos.localPosition;
    }

    void Update()
    {
        float f_distance = this.transform.localPosition.x - v2_startPos.x;
        f_nowTime = (float)Math.Truncate(f_distance / f_speed * 10) / 10;
        timeData.f_nowTime = f_nowTime;
        //Debug.Log(f_nowTime + "秒");
    }

    private void FixedUpdate()
    {
        if (barPos.localPosition.x < v2_startPos.x + (limit * timelineData.f_oneTickWidht))
        {
            barPos.localPosition = v2_nowPos;
            v2_nowPos.x += f_speed * Time.deltaTime;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    //ドラッグ開始するとき
    public void OnBeginDrag(PointerEventData eventData)
    {
        b_dragMode = true;
        timeData.b_DragMode = b_dragMode;
    }

    //ドラッグ中
    public void OnDrag(PointerEventData eventData)
    {
        //マウス座標取得
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)barPos.parent,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 v2_mousePos
        );

        v2_mousePos.y = v2_startPos.y;
        if (v2_mousePos.x <= v2_startPos.x)
        {
            v2_mousePos.x = v2_startPos.x;
        }
        barPos.localPosition = v2_mousePos;
        v2_nowPos = barPos.localPosition;
    }

    //ドラッグ終了時
    public void OnEndDrag(PointerEventData eventData)
    {
        b_dragMode = false;
        timeData.b_DragMode = b_dragMode;
    }

    /// <summary>
    /// ボタンを押したときタイムバーの位置を0秒の場所に戻す
    /// </summary>
    public void OnReStart()
    {
        Time.timeScale = 0;
        barPos.localPosition = v2_startPos;
        v2_nowPos = v2_startPos;
    }

    /// <summary>
    /// タイムバーがドラッグ中かどうかの判定を返す
    /// </summary>
    /// <returns>true=ドラッグ中　false=ドラッグしてない</returns>
    public bool ReturenDragMode()
    {
        return b_dragMode;
    }
}
