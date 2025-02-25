using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSE : MonoBehaviour
{
    private PlaySound playSound;

    private void Awake()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
    }


    /// <summary>
    /// 決定ボタンのサウンド
    /// </summary>
    public void PlaySE_Enter()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.enter);
    }

    /// <summary>
    /// 移動ボタンのサウンド
    /// </summary>
    public void PlaySE_Move()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.move);
    }

    /// <summary>
    /// ツールボタンのサウンド
    /// </summary>
    public void PlaySE_Tool()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.toolButton);
    }

    /// <summary>
    /// 開発中のサウンド
    /// </summary>
    public void PlaySE_Develop()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.develop);
    }
}
