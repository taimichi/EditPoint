using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TimeData")]
public class TimeData : ScriptableObject
{
    public bool b_DragMode = false;     //タイムバーを触って移動しているかどうか  
    public float f_nowTime = 0f;        //全体の現在の経過時間
}