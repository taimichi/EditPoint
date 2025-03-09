using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipSpeed : MonoBehaviour
{
    [SerializeField, Header("�����̃N���b�v�̒���(700���Đ����x1�{)")] private float startWidth = 140f;
    [SerializeField] private RectTransform ClipRect;    //�N���b�v��RectTransform
    private float playSpeed = 1f;  //���݂̃N���b�v�̍Đ����x
    private float changeSpeed;    //�ύX���̃N���b�v�Đ����x
    private const float MIN_SPEED = 0.1f;
    private const float MAX_SPEED = 2.0f; 

    void Start()
    {
        startWidth = ClipRect.sizeDelta.x;
    }

    void Update()
    {
        changeSpeed = startWidth - ClipRect.sizeDelta.x;
        
        if(changeSpeed > 0)
        {
            changeSpeed = Mathf.Abs(changeSpeed);
            //����
            float test = changeSpeed / TimelineData.TimelineEntity.oneResize;
            playSpeed = (0.1f * test) + 1;

            if(playSpeed >= MAX_SPEED)
            {
                playSpeed = MAX_SPEED;
            }
        }
        else if (changeSpeed < 0)
        {
            changeSpeed = Mathf.Abs(changeSpeed);
            //����
            float test = changeSpeed / TimelineData.TimelineEntity.oneResize;
            playSpeed = 1 - (0.1f * test);

            //�Œᑬ����������ꍇ
            if(playSpeed <= MIN_SPEED)
            {
                playSpeed = MIN_SPEED;
            }
        }
        else
        {
            playSpeed = 1f;
        }
    }

    /// <summary>
    /// �O���ɃN���b�v�ɐݒ肵�Ă���I�u�W�F�N�g�̍Đ����x�̒l��Ԃ�
    /// </summary>
    /// <returns>�N���b�v�ɐݒ肵�Ă�I�u�W�F�N�g�̍Đ����x</returns>
    public float ReturnPlaySpeed() => playSpeed;

    /// <summary>
    /// �O������f_StartWidth��ύX����
    /// </summary>
    /// <param name="_getWidth">�󂯎��Width</param>
    public void GetStartWidth(float _getWidth)
    {
        startWidth = _getWidth;
    }

    /// <summary>
    /// �N���b�v�̍Đ����x�̍X�V
    /// </summary>
    public void UpdateSpeed(float _newSpeed)
    {
        playSpeed = _newSpeed;
    }
}
