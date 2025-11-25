using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    //json変換するデータのクラス
    [HideInInspector] public SaveData data;

    //jsonファイルのパス
    private string filePath;
    //jsonファイル名
    private string fileName = "SaveData.json";

    //フォルダのパス
    private string folderPath;
    //フォルダ名
    private string folderName = "SaveData";

    private void Awake()
    {
        //フォルダのパスを取得
        folderPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, folderName);

        //フォルダがなかった場合、フォルダ作成
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("フォルダがなかったため作成しました：" + folderPath);
        }

        //jsonファイルのパスを取得
        filePath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, folderName, fileName);

        //ファイルがなかった場合、ファイルを作成
        if (!File.Exists(filePath))
        {
            Save(data);
            Debug.Log("データ作成:" + filePath);

            data.isDataExistence = false;
        }

        Debug.Log("ロード");
        //ファイルを読み込み格納
        data = Load(filePath);
    }

    /// <summary>
    /// データを保存する
    /// </summary>
    /// <param name="data">保存するSaveData型のデータ</param>
    public void Save(SaveData data)
    {
        // jsonとして変換
        string json = JsonUtility.ToJson(data);                 
        // ファイル書き込み指定
        StreamWriter wr = new StreamWriter(filePath, false);    
        // json変換した情報を書き込み
        wr.WriteLine(json);                                     
        //ファイルを閉じる
        wr.Close();
    }

    /// <summary>
    /// jsonファイルを読み込んで、dataに格納
    /// </summary>
    /// <param name="path">読みこむjsonファイルのパス</param>
    private SaveData Load(string path)
    {
        // ファイル読み込み指定
        StreamReader rd = new StreamReader(path);
        // ファイル内容全て読み込む
        string json = rd.ReadToEnd();
        // ファイル閉じる
        rd.Close();                                             

        // jsonファイルを型に戻して返す
        return JsonUtility.FromJson<SaveData>(json);
    }

    private void OnDestroy()
    {
        Save(data);
    }
}
