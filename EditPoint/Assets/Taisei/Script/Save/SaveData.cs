[System.Serializable]
public class SaveData
{
    //セーブデータが存在するか
    public bool isDataExistence;

    //チュートリアルプレイ情報
    public TutorialData.Tutorial_Frags tutorialFrag;

    //会話テキスト情報
    public bool isStartTalk;
    public GameData.CLEARTALK_FRAG clearTalk;

    //エンディング情報
    public bool isEnding;

    //ステージのクリア情報
    public NewStageData.STAGE_DATA[] stageDatas;

}
