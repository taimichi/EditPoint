using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lording : MonoBehaviour
{
    [SerializeField] private GameObject LordingPanel;   //���[�f�B���O��ʂ�Panel
    [SerializeField] private Slider slider;             //�ǂݍ��ݗ���\������
    private AsyncOperation async;                       //�񓯊�����Ŏg�p����AsyncOperation

    private void Start()
    {
        LordingPanel.SetActive(false);
    }

    /// <summary>
    /// ���[�f�B���O��ʂ��g���ăV�[���J�ڂ�����
    /// </summary>
    /// <param name="sceneName">�J�ڐ�̃V�[����</param>
    public void LordScene(string sceneName)
    {
        LordingPanel.SetActive(true);
        StartCoroutine(Lord(sceneName));
    }

    /// <summary>
    /// ���[�h�̃R���[�`���@
    /// </summary>
    IEnumerator Lord(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);

        //�ǂݍ��݂��I���܂Ői�����X���C�_�[�ɕ\��������
        while (!async.isDone)
        {
            //async.progress�̒l��0�`1�̊Ԃɕ␳
            float progressVal = Mathf.Clamp01(async.progress / 0.9f);
            slider.value = progressVal;
            yield return null;
        }
    }
}
