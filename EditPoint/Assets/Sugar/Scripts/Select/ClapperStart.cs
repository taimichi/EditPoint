using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���J��

public class ClapperStart : MonoBehaviour
{
    // �������Ώ�
    [SerializeField] GameObject obj;

    [SerializeField] Vector3 strPos;
    [SerializeField] Vector3 endpos;

    [SerializeField] float spdY = -1.0f;

    [SerializeField] Fade fade;          // FadeCanvas

    [SerializeField] Animator animator;

    float ofsY;
    private int num = 999;
    [SerializeField]bool isSelectMode = false;
    string name;

    int count = 0;

    private void Start()
    {
        // �I���V�[������Ȃ��Ȃ�ŏ�����X�^�[�g
        if (!isSelectMode) { num = 0; }
    }
    // Update is called once per frame
    void Update()
    {
        switch (num)
        {
            case 0: // �����l�ɃZ�b�g
                obj.transform.position = strPos;
                ofsY = strPos.y;
                num++;
                break;
            case 1: // �ړ�
                ofsY += spdY;
                obj.transform.position = new Vector2(0, ofsY);
                if (obj.transform.position.y <= endpos.y)
                {
                    num++;
                }
                break;
            case 2: // �A�j���[�V�����N��
                animator.Play("Scene1");
                break;
            case 3: // �V�[���J�� 
                if(!isSelectMode)
                {
                    count++;
                    if (count >= 60)
                    {
                        Destroy(obj);
                    }
                    return;
                }
                count++;
                if (count <= 60) { return; }
                num++;
                SceneManager.LoadScene(name);
                //fade.FadeIn(0.5f, () => {

                //});
                break;
        }
    }

    public string SceneName
    {
        set
        {
            // �V�[���l�[���̎擾
            name = value;
            num = 0; // �l���Z�b�g���ꂽ�瓮���o��
            Debug.Log(name);
        }
    }

    public void EndAnim()
    {
        num++;
    }
        
}
