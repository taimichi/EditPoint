using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipSpeed : MonoBehaviour
{
    [SerializeField, Header("初期のクリップの長さ(700が再生速度1倍)")] private float f_StartWidth = 700;
    [SerializeField] private RectTransform ClipRect;    //クリップのRectTransform
    private float f_playSpeed;  //現在のクリップの再生速度
    private float f_changeSpeed;    //変更時のクリップ再生速度

    void Start()
    {
        f_StartWidth = ClipRect.sizeDelta.x;
    }

    void Update()
    {
        f_changeSpeed = (float)Math.Truncate(ClipRect.sizeDelta.x / f_StartWidth * 10) / 10;
        //再生速度が1以下の時
        if (f_changeSpeed <= 1) 
        {
            f_changeSpeed = Mathf.Abs(f_changeSpeed - 1) + 1;
        }
        //再生速度が1より上の時
        else
        {
            f_changeSpeed = Mathf.Abs(f_changeSpeed - 2);
        }
        f_playSpeed = f_changeSpeed;
    }

    /// <summary>
    /// 外部にクリップに設定してあるオブジェクトの再生速度の値を返す
    /// </summary>
    /// <returns>クリップに設定してるオブジェクトの再生速度</returns>
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

    /// <summary>
    /// クリップの再生速度の更新
    /// </summary>
    public void UpdateSpeed(float _newSpeed)
    {
        f_playSpeed = _newSpeed;
    }
}
