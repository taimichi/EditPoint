using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioData : MonoBehaviour
{
    // �V�[���؂�ւ��Ă��j��
    // ����Ȃ��悤�ɂ���
    public static AudioData instance;

    // ���ʂ̃f�[�^
    // �������ʂ͂P �ˁ@BGM������ƃf�J�������̂�0.15�ɒ������܂���by��������
    private float volumeMASTER=1.0f;
    private float volumeBGM=0.15f;
    private float volumeSE=0.2f;

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
