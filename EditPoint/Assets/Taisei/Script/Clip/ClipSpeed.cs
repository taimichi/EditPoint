using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipSpeed : MonoBehaviour
{
    [SerializeField, Header("初期のクリップの長さ(700が再生速度1倍)")] private float startWidth = 140f;
    [SerializeField] private RectTransform ClipRect;    //クリップのRectTransform
    private float playSpeed = 1f;  //現在のクリップの再生速度
    private float changeSpeed;    //変更時のクリップ再生速度
    private const float MIN_SPEED = 0.1f;
    private const float MAX_SPEED = 2.0f; 

    void Start()
    {
        startWidth = ClipRect.sizeDelta.x;
    }

    void Update()
    {
        changeSpeed = startWidth - ClipRect.sizeDelta.x;
        
        if(changeSpeed > 0)
        {
            changeSpeed = Mathf.Abs(changeSpeed);
            //加速
            float test = changeSpeed / TimelineData.TimelineEntity.oneResize;
            playSpeed = (0.1f * test) + 1;

            if(playSpeed >= MAX_SPEED)
            {
                playSpeed = MAX_SPEED;
            }
        }
        else if (changeSpeed < 0)
        {
            changeSpeed = Mathf.Abs(changeSpeed);
            //減速
            float test = changeSpeed / TimelineData.TimelineEntity.oneResize;
            playSpeed = 1 - (0.1f * test);

            //最低速を下回った場合
            if(playSpeed <= MIN_SPEED)
            {
                playSpeed = MIN_SPEED;
            }
        }
        else
        {
            playSpeed = 1f;
        }
    }

    /// <summary>
    /// 外部にクリップに設定してあるオブジェクトの再生速度の値を返す
    /// </summary>
    /// <returns>クリップに設定してるオブジェクトの再生速度</returns>
    public float ReturnPlaySpeed() => playSpeed;

    /// <summary>
    /// 外部からf_StartWidthを変更する
    /// </summary>
    /// <param name="_getWidth">受け取るWidth</param>
    public void GetStartWidth(float _getWidth)
    {
        startWidth = _getWidth;
    }

    /// <summary>
    /// クリップの再生速度の更新
    /// </summary>
    public void UpdateSpeed(float _newSpeed)
    {
        playSpeed = _newSpeed;
    }
}
