using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipSpeed : MonoBehaviour
{
    [SerializeField, Header("�����̃N���b�v�̒���(700���Đ����x1�{)")] private float f_StartWidth = 700;
    [SerializeField] private RectTransform ClipRect;    //�N���b�v��RectTransform
    private float f_playSpeed;  //���݂̃N���b�v�̍Đ����x
    private float f_changeSpeed;    //�ύX���̃N���b�v�Đ����x

    void Start()
    {
        f_StartWidth = ClipRect.sizeDelta.x;
    }

    void Update()
    {
        f_changeSpeed = (float)Math.Truncate(ClipRect.sizeDelta.x / f_StartWidth * 10) / 10;
        //�Đ����x��1�ȉ��̎�
        if (f_changeSpeed <= 1) 
        {
            f_changeSpeed = Mathf.Abs(f_changeSpeed - 1) + 1;
        }
        //�Đ����x��1����̎�
        else
        {
            f_changeSpeed = Mathf.Abs(f_changeSpeed - 2);
        }
        f_playSpeed = f_changeSpeed;
    }

    /// <summary>
    /// �O���ɃN���b�v�ɐݒ肵�Ă���I�u�W�F�N�g�̍Đ����x�̒l��Ԃ�
    /// </summary>
    /// <returns>�N���b�v�ɐݒ肵�Ă�I�u�W�F�N�g�̍Đ����x</returns>
    public float ReturnPlaySpeed()
    {
        return f_playSpeed;
    }

    /// <summary>
    /// �O������f_StartWidth��ύX����
    /// </summary>
    /// <param name="getWidth">�󂯎��Width</param>
    public void GetStartWidth(float getWidth)
    {
        f_StartWidth = getWidth;
    }

    /// <summary>
    /// �N���b�v�̍Đ����x�̍X�V
    /// </summary>
    public void UpdateSpeed(float _newSpeed)
    {
        f_playSpeed = _newSpeed;
    }
}
