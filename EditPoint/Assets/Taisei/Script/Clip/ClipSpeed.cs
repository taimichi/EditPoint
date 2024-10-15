using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipSpeed : MonoBehaviour
{
    [SerializeField, Header("初期のクリップの長さ(700が再生速度1倍)")] private float f_StartWidth = 700;
    [SerializeField] private RectTransform ClipRect;
    private float f_playSpeed;
    private float f_changeSpeed;

    void Start()
    {
        f_StartWidth = ClipRect.sizeDelta.x;
    }

    void Update()
    {
        f_changeSpeed = (float)Math.Truncate(ClipRect.sizeDelta.x / f_StartWidth * 10) / 10;
        if (f_changeSpeed <= 1) 
        {
            f_changeSpeed = Mathf.Abs(f_changeSpeed - 1) + 1;
        }
        else
        {
            f_changeSpeed = Mathf.Abs(f_changeSpeed - 2);
        }
        f_playSpeed = f_changeSpeed;
    }

    /// <summary>
    /// 外部にクリップに設定してあるオブジェクトの再生速度の値を返す
    /// </summary>
    /// <returns>クリップに設定してるオブジェクトの再生速度　戻り値:0.5～2</returns>
    public float ReturnPlaySpeed()
    {
        return f_playSpeed;
    }

    /// <summary>
    /// 外部からf_StartWidthを変更する
    /// </summary>
    /// <param name="getWidth">受け取るWidth</param>
    public void GetStartWidth(float getWidth)
    {
        f_StartWidth = getWidth;
    }
}
