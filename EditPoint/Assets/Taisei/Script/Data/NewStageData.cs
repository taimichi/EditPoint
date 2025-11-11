using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StageData")]
public class NewStageData : ScriptableObject
{
    public const string PATH = "StageData";
    private static NewStageData _stageEntity;
    public static NewStageData StageEntity
    {
        get
        {
            if (_stageEntity == null)
            {
                _stageEntity = Resources.Load<NewStageData>(PATH);
                if (_stageEntity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _stageEntity;
        }
    }

    /// <summary>
    /// ステージのロック状態を管理
    /// </summary>
    public enum StageLock
    {
        Open,
        Lock,
    }

    [System.Serializable]
    /// <summary>
    /// ステージデータ用の構造体
    /// </summary>
    public struct STAGE_DATA
    {
        public string stageName;    //ステージのシーン名
        public int worldNum;        //ワールド番号
        public int stageNum;        //ステージ番号
        public StageLock stagelock;
    }

    //ステージデータ構造体の配列
    public STAGE_DATA[] stageData;
}
