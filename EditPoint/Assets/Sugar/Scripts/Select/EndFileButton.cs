using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFileButton : MonoBehaviour
{
    [SerializeField] bool isEnd;
    [SerializeField] GameObject panel;
    private void OnEnable()
    {
        // �͂����������炱���ŏI��
        if (isEnd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
        }
        // ����������������p�l�������
        else
        {
            // ���g���\����
            panel.SetActive(false);
            this.gameObject.SetActive(false);

        }
    }
}
