using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stage�̏����f�[�^�x�[�X�ɂ܂Ƃ߂Ă���
[CreateAssetMenu(fileName = "StageDataBase", menuName = "CreateStgaeDataBase")]
public class StageDataBase : ScriptableObject
{
    public List<StageData> STAGE_DATA = new List<StageData>();
}
