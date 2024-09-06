using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TimeData")]
public class TimeData : ScriptableObject
{
    public bool b_DragMode = false;     //�^�C���o�[��G���Ĉړ����Ă��邩�ǂ���  
    public float f_nowTime = 0f;        //�S�̂̌��݂̌o�ߎ���
}