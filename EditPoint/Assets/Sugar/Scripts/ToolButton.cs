using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���J��

public class ToolButton : MonoBehaviour
{
    /*----string�ϐ�----*/
    string S_stageName;   // �X�e�[�W���擾�p�ϐ�         

    /*----���̑��ϐ��i�R���|�[�l���g�Ƃ��X�N���v�g�j----*/
    [SerializeField]
    Fade fade;          // FadeCanvas

    [SerializeField] GameObject clpObj;
    [SerializeField] ClapperStart clapper;

    private void Start()
    {
     /* fade.cutoutRange = 1;
        // �V�[���̎n�܂�Ńt�F�[�h
        fade.FadeOut(1.0f);
     */

        // ���݂̃V�[�������擾(�V�[�������[�h�Ɏg��)
        S_stageName = SceneManager.GetActiveScene().name;
    }

    /*----�������牺�Ƀ{�^���̏����̒��g���L��----*/

    public void GroupSettings() // �ݒ�{�^��
    {
       
    }
    public void GroupRestart() // �V�[�������[�h�{�^���i��蒼���j
    {
        Time.timeScale = 1;
        // �t�F�[�h
        //clpObj.SetActive(true);
        //clapper.SceneName = S_stageName;
        SceneManager.LoadScene(S_stageName);
        //    fade.FadeIn(0.5f, () => {
        //        SceneManager.LoadScene(S_stageName);
        //    });
    }
    public void GroupTitle() // �^�C�g���V�[���ցi[2024.07/05]���̂Ƃ���^�C�g�������X�e�[�W�I���ɖ߂��\������j
    {
        // �t�F�[�h
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene("Title");
        });
    }

    public void BackSelect()
    {
        Time.timeScale = 1;
        // �t�F�[�h
        clpObj.SetActive(true);
        clapper.SceneName = "Select";
    }
}
