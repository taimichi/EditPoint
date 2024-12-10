using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData")]
public class GameData : ScriptableObject
{
    public const string PATH = "GameData";
    private static GameData _gameEntity;
    public static GameData GameEntity
    {
        get
        {
            if (_gameEntity == null)
            {
                _gameEntity = Resources.Load<GameData>(PATH);
                if (_gameEntity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _gameEntity;
        }
    }

    /// <summary>
    /// �Đ��{�^���������āA�Đ������Ă��邩
    /// </summary>
    public bool b_playNow = false;

    /// <summary>
    /// �^�C���o�[���Z�b�g���������ǂ���
    /// </summary>
    public bool b_timebarReset = false;

    /// <summary>
    /// ���掞�Ԃ��I�������
    /// </summary>
    public bool b_limitTime = false;

}
