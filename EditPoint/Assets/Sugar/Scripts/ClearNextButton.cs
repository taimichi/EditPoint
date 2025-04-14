using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClearNextButton : MonoBehaviour
{
    [Header("�X�e�[�W���"), SerializeField] NewStageData std;
    [Header("�t�F�[�h�I�u�W�F�N�g"), SerializeField] Fade fade;

    string nowStageName;
    string NextStageName;

    bool Click = false;

    private PlaySound playSound;

    void Start()
    {
        // ���݂̃V�[�������擾
        nowStageName = SceneManager.GetActiveScene().name;
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < std.STAGE_DATA.Count; i++)
        {
            // ���݃V�[���̎��̃V�[�������擾
            if(nowStageName==std.STAGE_DATA[i])
            {
                Debug.Log("DATA" + i);
                Debug.Log(std.STAGE_DATA.Count);
                // �ő�l���z������0�ɖ߂�
                if (i+1 == std.STAGE_DATA.Count)
                {
                    NextStageName = std.STAGE_DATA[0];
                }
                else
                {
                    NextStageName = std.STAGE_DATA[i + 1];
                }
            }
        }
    }

    public void NextButton()
    {
        if (Click) { return; }
        Click = true;
        playSound.PlaySE(PlaySound.SE_TYPE.sceneChange);
        fade.FadeIn(1.5f, () => SceneManager.LoadScene(NextStageName));
    }
}
