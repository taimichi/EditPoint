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


    [Header("1��ɃT�C�Y�ύX�����")] public float oneResize = 70f;
    [Header("1�ڐ���̕�(���[�J�����W�)")] public float oneTickWidht = 70f;
    [Header("1���C���̍���(���[�J�����W�)")] public float oneTickHeight = 75f;
}
