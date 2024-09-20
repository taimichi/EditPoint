using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioData : MonoBehaviour
{
    // シーン切り替えても破棄
    // されないようにする
    public static AudioData instance;

    // 音量のデータ
    // 初期音量は１
    private float volumeMASTER=1.0f;
    private float volumeBGM=1.0f;
    private float volumeSE=1.0f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        CheckInstance();
    }
    void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float Master
    {
        set { volumeMASTER = value; }
        get { return volumeMASTER; }
    }
    public float BGM
    {
        set { volumeBGM = value; }
        get { return volumeBGM; }
    }
    public float SE
    {
        set { volumeSE = value; }
        get { return volumeSE; }
    }
}
