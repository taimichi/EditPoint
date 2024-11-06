using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClearNextButton : MonoBehaviour
{
    [Header("�X�e�[�W���"), SerializeField] StageDataBase std;
    [Header("�t�F�[�h�I�u�W�F�N�g"), SerializeField] Fade fade;

    string nowStageName;
    string NextStageName;

    bool Click = false;
    void Start()
    {
        // ���݂̃V�[�������擾
        nowStageName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < std.STAGE_DATA.Count; i++)
        {
            // ���݃V�[���̎��̃V�[�������擾
            if(nowStageName==std.STAGE_DATA[i].StageSceneName)
            {
                // �ő�l���z������0�ɖ߂�
                if(i == std.STAGE_DATA.Count - 1)
                {
                    i = 0;
                }
                NextStageName = std.STAGE_DATA[i + 1].StageSceneName;
            }
        }
    }

    public void NextButton()
    {
        if (Click) { return; }
        Click = true;
        fade.FadeIn(0.5f, () => SceneManager.LoadScene(NextStageName));
    }
}
