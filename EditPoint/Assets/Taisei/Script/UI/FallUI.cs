using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallUI : MonoBehaviour
{
    [SerializeField] private GameObject FallCanvas;
    [SerializeField] private GameObject FadeInImage;    //�t�F�[�h�C���p�摜
    private RectTransform InRect;                       //�t�F�[�h�C���摜��RectTransform
    [SerializeField] private GameObject FadeOutImage;   //�t�F�[�h�A�E�g�p�摜
    private RectTransform OutRect;                      //�t�F�[�h�A�E�g�摜��RectTransform

    private Vector2 InStartSize;                        //�t�F�[�h�C���摜�̏����T�C�Y
    private Vector2 OutStartSize;                       //�t�F�[�h�A�E�g�摜�̏����T�C�Y

    private Vector3 InStartPos;                         //�t�F�[�h�C���摜�̏������W
    private Vector3 OutStartPos;                        //�t�F�[�h�A�E�g�摜�̏������W

    private float fadeTimer = 0f;                       //�t�F�[�h�^�C�}�[
    private const float MAX_FADETIME = 0.3f;            //�t�F�[�h�̍ő厞��

    private const float MAX_SIZE = 2750f;               //�ő�T�C�Y(����)
    private const float MIN_SIZE = 540f;                //�ŏ��T�C�Y(����)

    private float offset = 0;                           //�ύX����T�C�Y��
    private float fadeSpeedSize;                        //�T�C�Y�ύX�̃X�s�[�h

    private const float MAX_POSY = 300f;                //�����̍ő�l
    private const float MIN_POSY = -800f;               //�����̍ŏ��l

    private float dis = 0;                              //�ύX���鋗��
    private float fadeSpeedPos;                         //�ړ��X�s�[�h

    private bool startFade = false;                     //�t�F�[�h���J�n���邩�ǂ���
    /// <summary>
    /// false = �Ó]  true = ���]
    /// </summary>
    private bool fade = false;                          //�Ó]�����]��

    private PlaySound playSound;

    void Start()
    {
        offset = MAX_SIZE - MIN_SIZE;                   //�T�C�Y�������߂�
        dis = MAX_POSY - MIN_POSY;                      //���������߂�

        fadeSpeedSize = offset / MAX_FADETIME;          //�T�C�Y�ύX�̑��x�����߂�
        fadeSpeedPos = dis / MAX_FADETIME;              //Y���W�̈ړ����x�����߂�

        InRect = FadeInImage.GetComponent<RectTransform>();
        OutRect = FadeOutImage.GetComponent<RectTransform>();

        InStartSize = InRect.sizeDelta;                     //�t�F�[�h�C���摜�̏����T�C�Y���擾
        InStartPos = FadeInImage.transform.localPosition;   //�t�F�[�h�C���摜�̏������W���擾
        OutStartSize = OutRect.sizeDelta;                   //�t�F�[�h�A�E�g�摜�̏����T�C�Y���擾
        OutStartPos = FadeOutImage.transform.localPosition; //�t�F�[�h�A�E�g�摜�̏������W���擾

        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

    }

    void Update()
    {
        //�t�F�[�h�������J�n�\���ǂ���
        if (startFade)
        {
            //�Ó]����
            if (!fade)
            {
                Fall_FadeIn();
            }
            //���]����
            else
            {
                Fall_FadeOut();
            }
        }
    }

    /// <summary>
    /// �Ó]
    /// </summary>
    private void Fall_FadeIn()
    {
        InRect.sizeDelta += new Vector2(0, fadeSpeedSize * Time.deltaTime);         //�T�C�Y�X�V
        InRect.localPosition += new Vector3(0, fadeSpeedPos * Time.deltaTime, 0);   //�ʒu�X�V
        //�t�F�[�h���Ԃ��ő�l�𒴂�����
        if (fadeTimer >= MAX_FADETIME)
        {
            //�摜�؂�ւ��@�t�F�[�h�C���摜���\���A�t�F�[�h�A�E�g�摜��\��
            FadeInImage.SetActive(false);
            FadeOutImage.SetActive(true);
            fade = true;                    //�t�F�[�h�𖾓]�ɐ؂�ւ�
            fadeTimer = 0;                  //�^�C�}�[�����Z�b�g
        }
        fadeTimer += Time.deltaTime;
    }

    /// <summary>
    /// ���]
    /// </summary>
    private void Fall_FadeOut()
    {
        OutRect.sizeDelta -= new Vector2(0, fadeSpeedSize * Time.deltaTime);        //�T�C�Y�X�V
        OutRect.localPosition += new Vector3(0, fadeSpeedPos * Time.deltaTime, 0);  //�ʒu�X�V
        //�t�F�[�h���Ԃ��ő�l�𒴂�����
        if (fadeTimer >= MAX_FADETIME)
        {
            //�摜�؂�ւ��@�t�F�[�h�A�E�g�摜���\���A�t�F�[�h�C���摜��\��
            FadeInImage.SetActive(true);
            FadeOutImage.SetActive(false);

            //�摜�T�C�Y�Ȃǂ����ꂼ�ꏉ���T�C�Y�ɖ߂�(�ēx�g����悤�ɂ��邽��)
            InRect.sizeDelta = InStartSize;
            FadeInImage.transform.localPosition = InStartPos;
            OutRect.sizeDelta = OutStartSize;
            FadeOutImage.transform.localPosition = OutStartPos;

            fade = false;           //�t�F�[�h���Ó]�ɐ؂�ւ�
            startFade = false;      //�t�F�[�h���������s�s�\�ɂ���
            fadeTimer = 0;          //�^�C�}�[�����Z�b�g

            //�L�����o�X���\����
            FallCanvas.SetActive(false);
        }
        fadeTimer += Time.deltaTime;
    }

    /// <summary>
    /// �t�F�[�h�J�n�p�̊֐�
    /// </summary>
    public void FadeStart()
    {
        FallCanvas.SetActive(true);
        startFade = true;
        playSound.PlaySE(PlaySound.SE_TYPE.fall);
    }
}
