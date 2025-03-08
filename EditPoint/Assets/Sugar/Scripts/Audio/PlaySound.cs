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

    public enum SE_TYPE
    { 
        enter,      //����
        move,       //�ړ�
        select,     //�I�u�W�F�N�g�I��
        objMove,    //�N���b�v�A�I�u�W�F�N�g�ړ�
        cut,        //�J�b�g
        clipGene,   //�N���b�v����
        toolButton, //�c�[���{�^���N���b�N
        katinko,    //�J�`���R
        blockGene,  //�u���b�N����
        start,      //�X�^�[�g
        gool,       //�S�[��
        copy,       //�R�s�[
        paste,      //�y�[�X�g
        cancell,    //���[�h����
        itemGet,    //�A�C�e������
        death,      //���S
        develop,    //�J����
        fall,       //����
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
    /// SE���Đ������
    /// </summary>
    /// <param name="i">�Đ�����SE�ԍ�</param>
    public void PlaySE(SE_TYPE tYPE)
    {
        SE.PlayOneShot(clip[(int)tYPE]);
    }
}
