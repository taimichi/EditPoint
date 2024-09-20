using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [Header("�Đ����鉹�������ɓ����")]
    [SerializeField] AudioClip[] clip;
    [Header("�Đ�����BGM�������ɓ����")]
    [SerializeField] AudioClip[] clipMusic;
    // �Đ�
    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource SE;

    public enum TYPE
    { 
        a,
        b,
        c
    }

    public void PlayBGM(int i)
    {
        BGM.clip = clipMusic[i];
        BGM.Play();
    }

    public void StopBGM()
    {
        BGM.Stop();
    }

    /// <summary>
    /// SE���Đ������
    /// </summary>
    /// <param name="i">�Đ�����SE�ԍ�</param>
    public void PlaySE(TYPE tYPE)
    {
        SE.PlayOneShot(clip[(int)tYPE]);
    }
}
