using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TimeLineData")]
public class TimelineData : ScriptableObject
{
    [Header("1�ڐ���̕�")] public float f_oneTickWidht = 140f;
    [Header("1���C���̍���")] public float f_oneTickHeight = 59.5f;

}
