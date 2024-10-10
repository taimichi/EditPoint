using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipSpeed : MonoBehaviour
{
    [SerializeField, Header("�����̃N���b�v�̒���(700���Đ����x1�{)")] private float f_StartWidth = 700;
    [SerializeField] private RectTransform ClipRect;
    private float f_playSpeed;
    private float f_changeSpeed;

    void Start()
    {
        f_StartWidth = ClipRect.sizeDelta.x;
    }

    void Update()
    {
        f_changeSpeed = (float)Math.Truncate(ClipRect.sizeDelta.x / f_StartWidth * 10) / 10;
        if (f_changeSpeed <= 1) 
        {
            f_changeSpeed = Mathf.Abs(f_changeSpeed - 1) + 1;
        }
        else
        {
            f_changeSpeed = Mathf.Abs(f_changeSpeed - 2);
        }
        f_playSpeed = f_changeSpeed;
    }

    /// <summary>
    /// �O���ɃN���b�v�ɐݒ肵�Ă���I�u�W�F�N�g�̍Đ����x�̒l��Ԃ�
    /// </summary>
    /// <returns>�N���b�v�ɐݒ肵�Ă�I�u�W�F�N�g�̍Đ����x�@�߂�l:0.5�`2</returns>
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
}
