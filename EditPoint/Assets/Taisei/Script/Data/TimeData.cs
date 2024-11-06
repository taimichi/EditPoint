using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TimeData")]
public class TimeData : ScriptableObject
{
    public const string PATH = "TimeData";
    private static TimeData _timeEntity;
    public static TimeData TimeEntity
    {
        get
        {
            if (_timeEntity == null)
            {
                _timeEntity = Resources.Load<TimeData>(PATH);
                if (_timeEntity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _timeEntity;
        }
    }


    public bool b_DragMode = false;     //�^�C���o�[��G���Ĉړ����Ă��邩�ǂ���  
    public float f_nowTime = 0f;        //�S�̂̌��݂̌o�ߎ���
}