using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaffCredit : MonoBehaviour
{
    [SerializeField] private RectTransform rectCredit;
    private GameObject CreditPanel;
    [SerializeField] private RectTransform rectStart;
    [SerializeField] private RectTransform rectEnd;

    private float moveSpeed = 2.5f;

    private float timer = 0f;
    private const float LIMIT_TIME = 3.0f;

    #region fade
    [SerializeField] private Image fadeImage;
    private Color startColor;
    private bool isFade = false;
    private float alpha = 0;
    private float fadeSpeed = 0.02f;
    #endregion

    void Start()
    {
        rectCredit.localPosition = new Vector3(
                                                rectCredit.localPosition.x,
                                                rectStart.localPosition.y,
                                                rectCredit.localPosition.z);

        CreditPanel = this.transform.GetChild(0).gameObject;

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
            timer = 0f;
            fadeImage.color = startColor;
            alpha = 0;
            isFade = false;

        }
    }

    //�X�L�b�v�{�^�����������Ƃ�
    public void OnSkip()
    {
        CreditPanel.SetActive(false);

        rectCredit.localPosition = new Vector3(
                                                rectCredit.localPosition.x,
                                                rectStart.localPosition.y,
                                                rectCredit.localPosition.z);


    }
}
