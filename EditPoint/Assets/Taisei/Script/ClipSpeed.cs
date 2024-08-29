using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipSpeed : MonoBehaviour
{
    [SerializeField, Header("初期のクリップの長さ(700が再生速度1倍)")] private float f_StartWidth = 700;
    [SerializeField] private RectTransform ClipRect;
    private float f_playSpeed;

    void Start()
    {
        f_StartWidth = ClipRect.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        f_playSpeed = ClipRect.sizeDelta.x / f_StartWidth;
    }

    /// <summary>
    /// 外部にクリップに設定してあるオブジェクトの再生速度の値を返す
    /// </summary>
    /// <returns>クリップに設定してるオブジェクトの再生速度　戻り値:0.5～2</returns>
    public float ReturnPlaySpeed()
    {
        return f_playSpeed;
    }
}
