using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "StageInfo", menuName = "CreateStageInfo")]
public class StageData : ScriptableObject
{
    public string StageName;  // �X�e�[�W��
    public Sprite StageImage;  // �X�e�[�W�̃T���l
    public string infomation_Stage; // ���(�X�e�[�W�̐���)
    public string StageSceneName;
    public StageData(StageData stagedata)
    {
        this.StageImage = stagedata.StageImage;
        this.StageName = stagedata.StageName;
        this.infomation_Stage = stagedata.infomation_Stage;
        this.StageSceneName = stagedata.StageSceneName;
    }
}