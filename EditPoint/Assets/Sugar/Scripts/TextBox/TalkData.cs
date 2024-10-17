using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData
{
    //  会話文はここでセット
    private string[] talk= {
        "…。",
        "この{せ}世{かい}界が{ひら}開かれた{け}気{はい}配！？もしかして！！！\n#{が}画{めん}面の{まえ}前に{だれ}誰かいますね！？",
        "この{とき}時を{ま}待ってました！",
        "どうも、はじめまして！\n#この{せ}世{かい}界のアシスタントロボット、エディです！",
        "あなたには{いま}今からこの{せ}世{かい}界が{かか}抱えている{ふ}不{じょ}条{うり}理を\n#{ただ}糾してもらいます、{ただ}正してもらいます、{ただ}質してもらいます！",
        "そうと{き}決まればさっそくこの{どう}動{が}画ファイルを{えら}選んでください！\n#その{さき}先でもわたしがしっかりとアシストしますよ！",
        "{へん}返{とう}答してるじゃないですか！",
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
