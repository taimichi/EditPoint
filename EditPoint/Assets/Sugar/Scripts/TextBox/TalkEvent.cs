using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkEvent : MonoBehaviour
{
    #region field
    // キャラを表示する場所
    [SerializeField] Transform[] charaPos;
    // 登場させるキャラ
    [SerializeField] GameObject[] charaObj;

    // どのボックスを使うか
    [SerializeField] GameObject[] useTextBox;
    [SerializeField] Text[] nameText;
    [SerializeField] Text[] talkText;

    [SerializeField] GameObject CanvasFolder;


    [SerializeField] GameObject SelectButtonBox;
    // 生成するキャラを保存する
    GameObject gObj;

    // 会話番号
    int num=0;

    bool select = false;

    // Switchで使う
    int Snum=0;

    // 受け取った会話と名前を保存
    string resName;
    string resTalk;

    // 一文字ごとの表示にかかる時間
    [SerializeField] float delay = 0.3f; 

    // 会話データ
    TalkData tData;

    // enumでどのボックスを使うか選べるように
    enum SelectBox
    {
        Up,
        Center,
        Down,
    }

    SelectBox sBox;

    // enumでどのポジションにするか選べるように
    enum SelectPos
    {
        Left,
        Cenetr,
        Right,
    }
    // 生成の時に使う
    enum CharaName
    {
        AD,
    }
    #endregion
    void Start()
    {
        // インスタンス生成
        tData = new TalkData();
    }

    public int ParamSnum
    {
        set
        {
            // 最初は0
            Snum = value;
        }
    }

    void Update()
    {
        Debug.Log(resTalk);
        switch (Snum)
        {
            case 0: // 会話番号1のセット
                // 初期化
                ClearManager(true,true,true);

                sBox = SelectBox.Down;

                // 会話と名前のセット
                DataSet(num, CharaName.AD);
                
                // どの段のテキストボックスを使うか
                UseTalkBox(sBox);

                // キャラの表示
                gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);
                
                Snum++;
                break;
            case 1: // 会話送り
                if(Input.GetMouseButtonDown(0))
                {
                    if (talkText[(int)sBox].text == resTalk)
                    {
                        Snum++;
                        resTalk = "";
                        num++;
                    }

                    // 一文字ずつ送るコルーチンの停止
                    StopAllCoroutines();
                    // 全文表示
                    talkText[(int)sBox].text = resTalk;
                                       
                }
                break;
            case 2: // 会話番号2のセット
                // 初期化
                ClearManager(true, true, true);

                // 使うテキストボックスを変更するならこれで今まで
                // 使ってたものを非表示に
                ActiveFalse();

                // どの段を使うか指定
                sBox = SelectBox.Up;

                // 会話と名前のセット
                DataSet(num, CharaName.AD);

                // どの段のテキストボックスを使うか
                UseTalkBox(sBox);

                // キャラの表示
                gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Right].position, Quaternion.identity);

                Snum++;
                break;
            case 3:// 選択肢
                if (Input.GetMouseButtonDown(0))
                {
                    StopAllCoroutines();
                    // 全文表示
                    talkText[(int)sBox].text = resTalk;

                    resTalk = "";
                    SelectButtonBox.SetActive(true);
                }
                break;
            case 4: // Yesルート 会話番号3のセット
                    // 初期化
                ClearManager(true, true, true);
                // 使うテキストボックスを変更するならこれで今まで
                // 使ってたものを非表示に
                ActiveFalse();
                sBox = SelectBox.Down;

                // 会話と名前のセット
                DataSet(num, CharaName.AD);

                // どの段のテキストボックスを使うか
                UseTalkBox(sBox);

                // キャラの表示
                gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);

                Snum=6;
                break;
            case 5:// Noルート
                Snum=2;
                break;
            case 6:
                if (Input.GetMouseButtonDown(0))
                {
                    if (talkText[(int)sBox].text == resTalk)
                    {
                        Snum++;
                        resTalk = "";
                        num++;
                    }
                    // 一文字ずつ送るコルーチンの停止
                    StopAllCoroutines();
                    // 全文表示
                    talkText[(int)sBox].text = resTalk;
                }
                break;
            case 7:// 会話番号4のセット
                // 初期化
                ClearManager(true, true, true);
                // 使うテキストボックスを変更するならこれで今まで
                // 使ってたものを非表示に
                ActiveFalse();
                sBox = SelectBox.Down;

                // 会話と名前のセット
                DataSet(num, CharaName.AD);

                // どの段のテキストボックスを使うか
                UseTalkBox(sBox);

                // キャラの表示
                gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);
                Snum = 999;
                break;
            case 999:
                if (Input.GetMouseButtonDown(0))
                {
                    // 一文字ずつ送るコルーチンの停止
                    StopAllCoroutines();
                    // 全文表示
                    talkText[(int)sBox].text = resTalk;
                    Snum++;
                    resTalk = "";
                }
                break;
            case 1000: // 終了
                if (Input.GetMouseButtonDown(0))
                {
                    StopAllCoroutines();
                    ActiveFalse();
                    Snum=0;
                    num=0;
                    resTalk = "";
                    ClearManager(true, true, true);
                    CanvasFolder.SetActive(false);
                }
                break;
        }
    }

    public void YesButton()
    {
        select = true;
        SelectButtonBox.SetActive(false);
        num++;
        Snum++;
    }

    public void NoButton()
    {
        select = false;
        SelectButtonBox.SetActive(false);
        num += 2;
        Snum+=2;
    }

    IEnumerator RevealText(SelectBox selectBox)
    {
        // 一文字ずつ表示
        foreach (char c in resTalk)
        {
            talkText[(int)selectBox].text += c;
            yield return new WaitForSecondsRealtime(delay); // 指定時間待機
        }
    }
    /// <summary>
    /// どの位置のテキストボックスを使うのか
    /// </summary>
    /// <param name="selectBox">Up Center Downから選択</param>
    private void UseTalkBox(SelectBox selectBox)
    {
        // 表示してない時のみに実行
        if (!useTextBox[(int)selectBox].activeSelf)
        {
            useTextBox[(int)selectBox].SetActive(true);
        }

        nameText[(int)selectBox].text = resName;

        StartCoroutine(RevealText(selectBox));
    }
    /// <summary>
    /// talkのデータをセット
    /// </summary>
    /// <param name="num">会話番号</param>
    /// <param name="charaName">名前</param>
    private void DataSet(int num,CharaName charaName)
    {
        // 会話セット
        resTalk = tData.SendText(num);
        // 名前セット
        resName = tData.SendName((int)charaName);
    }

    /// <summary>
    /// 次のメッセージを入れるためTextの中身を消す
    /// </summary>
    /// <param name="talk">talkの中身を削除するならtrue</param>
    /// <param name="name">nameの中身を削除するならtrue</param>
    /// <param name="chara">charaを削除するならtrue</param>
    private void ClearManager(bool talk,bool name,bool chara)
    {
        // 次のメッセージを入れるためTextの中身を消す
        // talkの中身
        if (talk)
        {
            for (int i = 0; i < talkText.Length; i++)
            {
                talkText[i].text = "";
            }
        }
        // nameの中身
        if (name)
        {
            for (int i = 0; i < nameText.Length; i++)
            {
                nameText[i].text = "";
            }
        }
        // キャラを消去する
        if(chara)
        {
            Destroy(gObj);
        }
    }

    /// <summary>
    /// 使うテキストボックスを変更する時に現在のボックスを非表示に
    /// </summary>
    private void ActiveFalse()
    {
        for(int i=0;i<useTextBox.Length;i++)
        {
            // もし表示状態なら非表示に
            if (useTextBox[i].activeSelf)
            {
                useTextBox[i].SetActive(false);
            }
        }
    }
}
