using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;    // UI
using UnityEngine.SceneManagement;

public class ButtonSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // �J�[�\�����G�ꂽ�炱�̃I�u�W�F�N�g��true
    [SerializeField] GameObject img;
    [SerializeField] GameObject fileObj;

    // �����łǂ̃{�^���@�\�����f
    enum SelectButton
    {
        File,FileClose,Bg,Title,End,Yes_No,Option,OptionClose,etc
    }
    [Header("�{�^���@�\�̎�ނ�I��"),SerializeField] SelectButton kindButton;

    // UI�̃C���^�[�t�F�[�X
    #region interface
    // UI��ɃJ�[�\�����G��Ă��邩
    public void OnPointerEnter(PointerEventData eventData)
    {
        // �\��
        img.SetActive(true);
    }
    // ���ꂽ�ꍇ
    public void OnPointerExit(PointerEventData eventData)
    {
        // ��\��
        img.SetActive(false);
    }
    #endregion

    void Update()
    {
        // �I�΂ꂽ��Ԃł̂�
        if(img.activeSelf)
        {
            // �N���b�N�����炻�̃t�@�C�����N��
            if (Input.GetMouseButtonDown(0))
            {
                CheckKind(kindButton);
                img.SetActive(false);
            }
        }
    }

    // �ǂ̃{�^�����g�������f
    private void CheckKind(SelectButton select)
    {
        switch(select)
        {
            case SelectButton.File:
                File();
                break;
            case SelectButton.FileClose:
                FileClose();
                break;
            case SelectButton.Bg:
                Bg();
                break;
            case SelectButton.Title:
                Title();
                break;
            case SelectButton.End:
                End();
                break;
            case SelectButton.Yes_No:
                Yes_No();
                break;
            case SelectButton.Option:
                Option();
                break;
            case SelectButton.OptionClose:
                OptionClose();
                break;
        }
    }

    #region Function

    // �ݒ�����
    private void OptionClose()
    {
        GameObject obj = GameObject.Find("AudioCanvas");
        obj.GetComponent<SoundMenu>().CloseWindow();

    }
    // �ݒ�
    private void Option()
    {
        GameObject obj = GameObject.Find("AudioCanvas");
        obj.GetComponent<SoundMenu>().OpenWindow();
    }

    // �͂��A�������̃{�^��
    private void Yes_No()
    {
        // �{���ɏI���̂��Ċm�F��\����̏��������s
        fileObj.SetActive(true);
    }

    // �I���{�^��
    private void End()
    {
        // �{���ɏI���̂��Ċm�F��\��
        fileObj.SetActive(true);
    }

    // �^�C�g���֖߂�
    private void Title()
    {
        string sceneName="Title";

        Fade fade;          // FadeCanvas
        bool isOn = false;
        if (isOn)
        {
            return;
        }
        fade = GameObject.Find("GameFade").GetComponent<Fade>();
        // �t�F�[�h
        fade.FadeIn(0.5f, () =>
        {
            SceneManager.LoadScene(sceneName);
            isOn = true;
        });
    }

    // �w�i�ύX�{�^��
    private void Bg()
    {
        GameObject obj = GameObject.Find("DeskTopBg");
        obj.GetComponent<ImageLoader>().LoadImage();
    }

    // �X�e�[�W�p�l������邱�̎��Ƀt�@�C���̈ʒu�����ɖ߂�
    private void FileClose()
    {
        fileObj.SetActive(false);

        // �t�@�C���I�u�W�F�N�g�S�Ă�����W�ɖ߂�
        DraggableImage[] targets = FindObjectsOfType<DraggableImage>();
        if (targets.Length > 0)
        {
            foreach (DraggableImage target in targets)
            {
                target.OriginalPos();
            }
        }
    }

    // �t�@�C���@�\
    private void File()
    {
        fileObj.SetActive(true);
    }
    #endregion

}
