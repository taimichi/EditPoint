using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TimeLineData")]
public class TimelineData : ScriptableObject
{
    [Header("1回にサイズ変更する量")] public float f_oneResize = 140f;
    [Header("1目盛りの幅(ローカル座標)")] public float f_oneTickWidht = 140f;
    [Header("1ラインの高さ(ローカル座標)")] public float f_oneTickHeight = 74f;
    [Header("タイムラインの左端のX座標(ローカル座標)")] public float f_timelineEndLeft = -166f;
    [Header("タイムラインの右端のX座標(ローカル座標)")] public float f_timelineEndRight = 83134f;
    [Header("タイムラインの上のY座標(ローカル座標)")] public float f_timelineEndUp = 2f;
    [Header("タイムラインの下のY座標(ローカル座標)")] public float f_timelineEndDown = -294f;
}
