using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���J��
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour
{
    /*----���̑��ϐ��i�R���|�[�l���g�Ƃ��X�N���v�g�j----*/
    [SerializeField]
    Fade fade;          // FadeCanvas

    private PlaySound playSound;
    [SerializeField] private GameObject audioPanel;

    private void Start()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
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
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            playSound.PlaySE(PlaySound.SE_TYPE.enter);
            // �t�F�[�h
            fade.FadeIn(0.5f, () => {
                SceneManager.LoadScene("Talk");
            });

        }
    }
}
