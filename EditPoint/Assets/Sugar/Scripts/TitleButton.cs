using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���J��

public class TitleButton : MonoBehaviour
{
    /*----���̑��ϐ��i�R���|�[�l���g�Ƃ��X�N���v�g�j----*/
    [SerializeField]
    Fade fade;          // FadeCanvas
    public void STARTBUTTON()
    {
        // �t�F�[�h
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene("Select");
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // �t�F�[�h
            fade.FadeIn(0.5f, () => {
                SceneManager.LoadScene("Select");
            });

        }
    }
}
