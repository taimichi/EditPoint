using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeBar : MonoBehaviour
{
    [SerializeField] private float f_speed = 278f;
    private RectTransform barPos;
    private Vector2 v2_nowPos;
    [SerializeField, Header("時間(秒)")] private float limit = 60f;
    [SerializeField] private TimelineData data;
    private Vector2 v2_startPos;

    private float f_nowTime = 0;

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
        Debug.Log(f_nowTime + "秒");
    }

    private void FixedUpdate()
    {
        if (barPos.localPosition.x < v2_startPos.x + (limit * data.f_oneTickWidht))
        {
            barPos.localPosition = v2_nowPos;
            v2_nowPos.x += f_speed * Time.deltaTime;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// タイムバーの時間を返す
    /// </summary>
    /// <returns>タイムバーのある位置の時間(秒)</returns>
    public float ReturnTime()
    {
        return f_nowTime;
    }

    public void OnReStart()
    {
        barPos.localPosition = v2_startPos;
    }
}
