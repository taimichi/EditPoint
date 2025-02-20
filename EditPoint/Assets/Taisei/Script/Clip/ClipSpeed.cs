using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipSpeed : MonoBehaviour
{
    [SerializeField, Header("初期のクリップの長さ(700が再生速度1倍)")] private float startWidth = 140f;
    [SerializeField] private RectTransform ClipRect;    //クリップのRectTransform
    private float playSpeed;  //現在のクリップの再生速度
    private float changeSpeed;    //変更時のクリップ再生速度

    void Start()
    {
        startWidth = ClipRect.sizeDelta.x;
    }

    void Update()
    {
        changeSpeed = (float)Math.Truncate(ClipRect.sizeDelta.x / startWidth * 10) / 10;
        //再生速度が1以下の時
        if (changeSpeed <= 1) 
        {
            changeSpeed = Mathf.Abs(changeSpeed - 1) + 1;
        }
        //再生速度が1より上の時
        else
        {
            changeSpeed = Mathf.Abs(changeSpeed - 2);
        }
        playSpeed = changeSpeed;
    }

    /// <summary>
    /// 外部にクリップに設定してあるオブジェクトの再生速度の値を返す
    /// </summary>
    /// <returns>クリップに設定してるオブジェクトの再生速度</returns>
    public float ReturnPlaySpeed() => playSpeed;

    /// <summary>
    /// 外部からf_StartWidthを変更する
    /// </summary>
    /// <param name="getWidth">受け取るWidth</param>
    public void GetStartWidth(float getWidth)
    {
        startWidth = getWidth;
    }

    /// <summary>
    /// クリップの再生速度の更新
    /// </summary>
    public void UpdateSpeed(float _newSpeed)
    {
        playSpeed = _newSpeed;
    }
}
