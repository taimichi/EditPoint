using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData
{
    //  会話文はここでセット
    private string[] talk= {
        "…。",
        "この世界が開かれた気配！？もしかして！！！画面の前に誰かいますね！？",
        "この時を待ってました！",
        "どうも、はじめまして！この世界のアシスタントロボット、「エディ」と申します！",
    };
    // 会話キャラの名前
    private string[] name =
    {
        "エディ",
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
