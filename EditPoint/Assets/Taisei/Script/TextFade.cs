using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    [SerializeField] private Text text;
    ClassUIAnim uIAnim;
    int num = 0;
    // Start is called before the first frame update
    void Start()
    {
        uIAnim = new ClassUIAnim();
    }

    // Update is called once per frame
    void Update()
    {
        switch (num)
        {
            case 0:
                uIAnim.anim_Fade_T(text, -0.02f);
                if (text.color.a <= 0.0f)
                {
                    num = 1;
                }
                break;

            case 1:
                uIAnim.anim_Fade_T(text, 0.02f);
                if (text.color.a >= 1.0f)
                {
                    num = 0;
                }
                break;
        }
    }
}
