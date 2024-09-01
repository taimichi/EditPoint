using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkStart : MonoBehaviour
{
    [SerializeField] Fade fade;          // FadeCanvas
    [SerializeField] GameObject TalkCanvas;
    [SerializeField] GameObject fadeObj;

    [SerializeField] bool isDebug = false;

    private void Start()
    {
        if (isDebug)
        {
            StartTalk();
        }
    }

    public void StartTalk()
    {
        // ��\����Ԃ�������\������
        if (!fadeObj.activeSelf) { fadeObj.SetActive(true); }
        //// �t�F�[�h�@�\���g�������\����
        //fade.FadeIn(0.7f, () => {
        //    Time.timeScale = 0;    // ���Ԓ�~
        //    TalkCanvas.SetActive(true); // ��b�C�x���g�̎n�܂�
        //    fadeObj.SetActive(false);
        //});

        Time.timeScale = 0;    // ���Ԓ�~
        TalkCanvas.SetActive(true); // ��b�C�x���g�̎n�܂�
        fadeObj.SetActive(false);


    }
}
