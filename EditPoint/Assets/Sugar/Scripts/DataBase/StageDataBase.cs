using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stageの情報をデータベースにまとめておく
[CreateAssetMenu(fileName = "StageDataBase", menuName = "CreateStgaeDataBase")]
public class StageDataBase : ScriptableObject
{
    public List<StageData> STAGE_DATA = new List<StageData>();
}
