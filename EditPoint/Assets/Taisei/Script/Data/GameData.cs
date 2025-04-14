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
    /// 再生ボタンを押して、再生をしているか
    /// </summary>
    public bool isPlayNow = false;

    /// <summary>
    /// タイムバーリセットをしたかどうか
    /// </summary>
    public bool isTimebarReset = false;

    /// <summary>
    /// 動画時間が終わったかどうか
    /// </summary>
    public bool isLimitTime = false;

    /// <summary>
    /// ステージクリアしたかどうか
    /// </summary>
    public bool isClear = false;

    /// <summary>
    /// 最初に会話したかどうか
    /// </summary>
    public bool isTalk = false;

}
