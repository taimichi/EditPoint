using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    // GameObject.Find���Ƃ킩���G�Gbykoko20240926
    [SerializeField]
    private Fade fade;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("FadeCanvas"))
        {
            fade = GameObject.Find("FadeCanvas").GetComponent<Fade>();
        }
        else if(GameObject.Find("GameFade"))
        {
            fade = GameObject.Find("GameFade").GetComponent<Fade>();
        }
        else
        {
            // else����܂�bykoko20240926
            Debug.Log("canvas�Ȃ����`");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �^�C�g���V�[���֖߂�
    /// </summary>
    public void OnExitButton()
    {
        // �ǂ���瓮���ĂȂ����ۂ������I���R�s���Ibykoko20240926
        Debug.Log("osareta");
        if (fade != null)
        {

            Debug.Log("fade");
            fade.FadeIn(0.5f, () =>
            {
                SceneManager.LoadScene("Title");
            });
        }
        else
        {
            Debug.Log("no fade");
            SceneManager.LoadScene("Title");
        }
    }
}
