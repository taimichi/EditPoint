using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipSpeed : MonoBehaviour
{
    [SerializeField, Header("�����̃N���b�v�̒���(700���Đ����x1�{)")] private float startWidth = 140f;
    [SerializeField] private RectTransform ClipRect;    //�N���b�v��RectTransform
    private float playSpeed;  //���݂̃N���b�v�̍Đ����x
    private float changeSpeed;    //�ύX���̃N���b�v�Đ����x
    private const float MIN_SPEED = 0.1f;

    void Start()
    {
        startWidth = ClipRect.sizeDelta.x;
    }

    void Update()
    {
        changeSpeed = ClipRect.sizeDelta.x / startWidth;
        //�Đ����x��1�ȉ��̎�
        if (changeSpeed <= 1) 
        {
            //���x�����Ȃ�
            changeSpeed = Mathf.Abs(changeSpeed - 1) + 1;
        }
        //�Đ����x��1����̎�
        else
        {
            //���x�x���Ȃ�
            changeSpeed = Mathf.Abs(changeSpeed - 2);
            if(changeSpeed <= 0)
            {
                changeSpeed = MIN_SPEED;
            }
        }
        playSpeed = changeSpeed;
        Debug.Log(playSpeed + " " + this.gameObject.name);
    }

    /// <summary>
    /// �O���ɃN���b�v�ɐݒ肵�Ă���I�u�W�F�N�g�̍Đ����x�̒l��Ԃ�
    /// </summary>
    /// <returns>�N���b�v�ɐݒ肵�Ă�I�u�W�F�N�g�̍Đ����x</returns>
    public float ReturnPlaySpeed() => playSpeed;

    /// <summary>
    /// �O������f_StartWidth��ύX����
    /// </summary>
    /// <param name="getWidth">�󂯎��Width</param>
    public void GetStartWidth(float getWidth)
    {
        startWidth = getWidth;
    }

    /// <summary>
    /// �N���b�v�̍Đ����x�̍X�V
    /// </summary>
    public void UpdateSpeed(float _newSpeed)
    {
        playSpeed = _newSpeed;
    }
}
