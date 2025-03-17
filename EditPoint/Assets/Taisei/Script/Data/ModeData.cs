using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ModeData")]
public class ModeData : ScriptableObject
{
    public const string PATH = "ModeData";
    private static ModeData _modeEntity;
    public static ModeData ModeEntity
    {
        get
        {
            if (_modeEntity == null)
            {
                _modeEntity = Resources.Load<ModeData>(PATH);
                if (_modeEntity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _modeEntity;
        }
    }

    public enum Mode
    {
        normal,
        moveANDdirect,
        copy,
        paste,
        create,

    }

    public Mode mode = Mode.normal;

}
