using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkImage : MonoBehaviour
{
    /// <summary>
    /// フェードのスピード
    /// </summary>
    private float alpha = 0.02f;
    private Image image;
    private bool change = false;
    /// <summary>
    /// 最大1
    /// </summary>
    private float max = 1f;

    private Color startColor;

    // Start is called before the first frame update
    void Start()
    {
        image = this.gameObject.GetComponent<Image>();
        startColor = image.color;
        image.color = new Color(startColor.r, startColor.g, startColor.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!change)
        {
            image.color += new Color(0, 0, 0, alpha);
            if (image.color.a >= max)
            {
                change = !change;
            }
        }
        else
        {
            image.color -= new Color(0, 0, 0, alpha);
            if (image.color.a <= 0)
            {
                change = !change;
            }
        }
    }
}
