using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData
{
    //  会話文はここでセット
    private string[] talk= { 
        "最初の文字だよ",
        "次の文字だよ",
    };
    // 会話キャラの名前
    private string[] name =
    {
        "AD",
    };

    /// <summary>
    /// メッセージを送る
    /// </summary>
    /// <param name="num">何番目の会話をするか</param>
    /// <returns>メッセージデータを送る</returns>
    public string SendText(int num)
    {
        return talk[num];
    }
    /// <summary>
    /// 名前を送る
    /// </summary>
    /// <param name="num">キャラの番号</param>
    /// <returns>キャラの名前を送る</returns>
    public string SendName(int num)
    {
        return name[num];
    }
}
