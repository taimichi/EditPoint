using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���J��
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class TitleButton : MonoBehaviour
{
    /*----���̑��ϐ��i�R���|�[�l���g�Ƃ��X�N���v�g�j----*/
    [SerializeField]
    Fade fade;          // FadeCanvas

    private PlaySound playSound;
    [SerializeField] private GameObject audioPanel;

    [SerializeField] private GameObject MenuBG;
    [SerializeField] private GameObject Menu;
    private Image MenuImage;
    [SerializeField] private GameObject MenuFunction;

    [SerializeField] private GameObject CreditPanel;

    private void Start()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        MenuImage = Menu.GetComponent<Image>();
    }

    public void STARTBUTTON()
    {
        // �t�F�[�h
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene("Select");
        });
    }

    private void Update()
    {
        if (audioPanel.activeSelf) { return; }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //���N���b�Nor�E�N���b�N�Ŏ��̃V�[����
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                playSound.PlaySE(PlaySound.SE_TYPE.enter);
                playSound.PlaySE(PlaySound.SE_TYPE.sceneChange);
                // �t�F�[�h
                fade.FadeIn(1.5f, () => {
                    SceneManager.LoadSceneAsync("Talk");
                });
            }
        }
    }

    IEnumerator OpenMenuAnim()
    {
        MenuImage.DOFade(1, 0);
        MenuImage.DOFillAmount(1f, 0.2f);

        yield return new WaitUntil(() => MenuImage.fillAmount == 1);

        MenuFunction.SetActive(true);
    }

    //���j���[���J��
    public void OnMenuOpen()
    {
        MenuBG.SetActive(true);
        Menu.SetActive(true);

        StartCoroutine(OpenMenuAnim());
    }

    IEnumerator CloseMenuAnim()
    {
        MenuImage.DOFade(0, 0.2f);
        yield return new WaitUntil(() => MenuImage.color.a == 0);
        Menu.SetActive(false);
        MenuBG.SetActive(false);
        MenuImage.fillAmount = 0;
    }

    //���j���[�����
    public void OnMenuClose()
    {
        MenuFunction.SetActive(false);
        StartCoroutine(CloseMenuAnim());
    }

    //�X�^�b�t�N���W�b�g
    public void OnCredit()
    {
        CreditPanel.SetActive(true);
    }
}

