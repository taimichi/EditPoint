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
        // �������ꍇ�Ɏ擾
        if (GameObject.Find("AudioDataObj"))
        {
            data = GameObject.Find("AudioDataObj").GetComponent<AudioData>();

            // �o�[�̐ݒ�
            Mslider.value = data.Master;
            Bslider.value = data.BGM;
            Sslider.value = data.SE;

            // ���ʐݒ�
            AudioListener.volume = data.Master;
            BGM.volume = data.BGM;
            SE.volume = data.SE;
        }
        
    }
    // �X���C�_�[����l����艹�ʐݒ�
    // ���ʐݒ肵���猳�̃f�[�^�ɏ��𑗐M
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
