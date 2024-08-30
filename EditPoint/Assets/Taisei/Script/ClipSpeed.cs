using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClipSpeed : MonoBehaviour
{
    [SerializeField, Header("�����̃N���b�v�̒���(700���Đ����x1�{)")] private float f_StartWidth = 700;
    [SerializeField] private RectTransform ClipRect;
    private float f_playSpeed;

    void Start()
    {
        f_StartWidth = ClipRect.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        f_playSpeed = ClipRect.sizeDelta.x / f_StartWidth;
    }

    /// <summary>
    /// �O���ɃN���b�v�ɐݒ肵�Ă���I�u�W�F�N�g�̍Đ����x�̒l��Ԃ�
    /// </summary>
    /// <returns>�N���b�v�ɐݒ肵�Ă�I�u�W�F�N�g�̍Đ����x�@�߂�l:0.5�`2</returns>
    public float ReturnPlaySpeed()
    {
        return f_playSpeed;
    }
}
