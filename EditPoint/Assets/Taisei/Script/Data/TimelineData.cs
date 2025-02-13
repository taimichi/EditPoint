using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TimeLineData")]
public class TimelineData : ScriptableObject
{
    public const string PATH = "TimelineData";
    private static TimelineData _timelineEntity;
    public static TimelineData TimelineEntity
    {
        get
        {
            if (_timelineEntity == null)
            {
                _timelineEntity = Resources.Load<TimelineData>(PATH);
                if (_timelineEntity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _timelineEntity;
        }
    }


    [Header("1回にサイズ変更する量")] public float oneResize = 70f;
    [Header("1目盛りの幅(ローカル座標基準)")] public float oneTickWidht = 70f;
    [Header("1ラインの高さ(ローカル座標基準)")] public float oneTickHeight = 75f;
}
