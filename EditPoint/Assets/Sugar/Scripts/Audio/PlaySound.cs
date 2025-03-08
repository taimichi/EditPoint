using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [Header("再生する音をここに入れる")]
    [SerializeField] AudioClip[] clip;
    [Header("再生するBGMをここに入れる")]
    [SerializeField] AudioClip[] clipMusic;
    // 再生
    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource SE;

    public enum SE_TYPE
    { 
        enter,      //決定
        move,       //移動
        select,     //オブジェクト選択
        objMove,    //クリップ、オブジェクト移動
        cut,        //カット
        clipGene,   //クリップ生成
        toolButton, //ツールボタンクリック
        katinko,    //カチンコ
        blockGene,  //ブロック生成
        start,      //スタート
        gool,       //ゴール
        copy,       //コピー
        paste,      //ペースト
        cancell,    //モード解除
        itemGet,    //アイテム入手
        death,      //死亡
        develop,    //開発中
        fall,       //落下
    }

    public enum BGM_TYPE
    {
        title_stageSelect,
        noon,
        talk,
        evening,
        night
    }

    public void PlayBGM(BGM_TYPE _bgm)
    {
        BGM.clip = clipMusic[(int)_bgm];
        BGM.Play();
    }

    public void StopBGM()
    {
        BGM.Stop();
    }

    /// <summary>
    /// SEを再生するよ
    /// </summary>
    /// <param name="i">再生するSE番号</param>
    public void PlaySE(SE_TYPE tYPE)
    {
        SE.PlayOneShot(clip[(int)tYPE]);
    }
}
