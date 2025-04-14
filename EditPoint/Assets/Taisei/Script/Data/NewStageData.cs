using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StageData")]
public class NewStageData : ScriptableObject
{
    public List<string> STAGE_DATA = new List<string>();
}
