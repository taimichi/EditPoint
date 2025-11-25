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
    public bool isStartTalk = false;

    /// <summary>
    /// エンディングを流したかどうか
    /// </summary>
    public bool isEnding = false;

    [System.Flags]
    public enum CLEARTALK_FRAG
    {
        none = 0,
        stage1 = 1 << 0,
        stage2 = 1 << 1,
        stage3 = 1 << 2,
        stage4 = 1 << 3,
    }
    [EnumFlags] public CLEARTALK_FRAG talkFrags = CLEARTALK_FRAG.none;
}
