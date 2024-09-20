using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderSet : MonoBehaviour
{
    AudioData data;
    [SerializeField] Slider Mslider;
    [SerializeField] Slider Bslider;
    [SerializeField] Slider Sslider;

    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource SE;

    private void Start()
    {
        // 見つけた場合に取得
        if (GameObject.Find("AudioDataObj"))
        {
            data = GameObject.Find("AudioDataObj").GetComponent<AudioData>();

            // バーの設定
            Mslider.value = data.Master;
            Bslider.value = data.BGM;
            Sslider.value = data.SE;

            // 音量設定
            AudioListener.volume = data.Master;
            BGM.volume = data.BGM;
            SE.volume = data.SE;
        }
        
    }
    // スライダーから値を取り音量設定
    // 音量設定したら元のデータに情報を送信
    public void SliderMaster()
    {
        AudioListener.volume = Mslider.value;
        data.Master = Mslider.value;
    }

    public void SliderBGM()
    {
        BGM.volume = Bslider.value;
        data.BGM = Bslider.value;
    }

    public void SliderSE()
    {
        SE.volume = Sslider.value;
        data.SE = Sslider.value;
    }
}
