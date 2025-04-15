using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaffCredit : MonoBehaviour
{
    [SerializeField] private RectTransform rectCredit;  //�N���W�b�g��RectTransform
    private GameObject CreditPanel;                     //�N���W�b�g�̔w�i
    [SerializeField] private RectTransform rectStart;   //�J�n�ʒu
    [SerializeField] private RectTransform rectEnd;     //�I���ʒu

    private float moveSpeed = 2.5f;                     //�N���W�b�g�̓�������

    private float timer = 0f;                           //�Ō�~�܂��Ă��鎞�Ԃ̌v���p
    private const float LIMIT_TIME = 3.0f;              //�Ō�~�܂鎞��

    #region fade
    [SerializeField] private Image fadeImage;
    private Color startColor;
    private bool isFade = false;
    private float alpha = 0;
    private float fadeSpeed = 0.02f;
    #endregion

    void Start()
    {
        //�����ʒu��ݒ�
        rectCredit.localPosition = new Vector3(
                                                rectCredit.localPosition.x,
                                                rectStart.localPosition.y,
                                                rectCredit.localPosition.z);

        //�w�i�擾
        CreditPanel = this.transform.GetChild(0).gameObject;

        //�����̃J���[�l��ݒ�
        startColor = fadeImage.color;
    }

    void Update()
    {
        //�N���W�b�g�p�l�����\����ԂɂȂ�����
        if (CreditPanel.activeSelf)
        {
            //�N���W�b�g�̍Ōオ�ŏI�n�_�ɍs������
            if(rectCredit.localPosition.y >= rectEnd.localPosition.y)
            {
                if(timer >= LIMIT_TIME)
                {
                    StartFade();
                    return;
                }
                timer += Time.deltaTime;
            }
            //��������Ȃ��Ƃ�
            else
            {
                //�N���W�b�g����������
                rectCredit.localPosition += Vector3.up * moveSpeed;
            }
        }
        else
        {
            //�����ʒu��
            rectCredit.localPosition = new Vector3(
                                                    rectCredit.localPosition.x,
                                                    rectStart.localPosition.y,
                                                    rectCredit.localPosition.z);
        }
    }

    /// <summary>
    /// �t�F�[�h����
    /// </summary>
    private void StartFade()
    {
        if (!isFade)
        {
            //�A���t�@�l��ύX
            alpha += fadeSpeed;
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            if (alpha >= 1f)
            {
                isFade = true;
            }
        }
        else
        {
            CreditPanel.SetActive(false);
            //�X�^�[�g�n�_�ɖ߂�
            rectCredit.localPosition = new Vector3(
                                    rectCredit.localPosition.x,
                                    rectStart.localPosition.y,
                                    rectCredit.localPosition.z);
            //������Ԃɖ߂�
            timer = 0f;
            fadeImage.color = startColor;
            alpha = 0;
            isFade = false;

        }
    }

    //�X�L�b�v�{�^�����������Ƃ�
    public void OnSkip()
    {
        //�w�i������
        CreditPanel.SetActive(false);

        //�����ʒu�ɖ߂�
        rectCredit.localPosition = new Vector3(
                                                rectCredit.localPosition.x,
                                                rectStart.localPosition.y,
                                                rectCredit.localPosition.z);
    }
}
