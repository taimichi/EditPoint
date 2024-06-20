using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassUIAnim
{
    // 動かすUI,初期座標,どこまで動かすかの目標,XとYの位置
     public RectTransform anim_Start(RectTransform rct,Vector2 strPos)
    {
        rct.anchoredPosition = strPos;
        return rct;
    }
    public RectTransform anim_PosChange(RectTransform rct,float spdX,float spdY)
    {
        Debug.Log("PosChange");
        rct.anchoredPosition +=new Vector2( spdX,spdY);
        return rct;
    }
    public RectTransform anim_SclChange(RectTransform rct,float sclX,float sclY)
    {
        rct.sizeDelta += new Vector2(sclX, sclY);
        return rct;
    }

    // イメージのフェード
    public Image anim_Fade_I(Image Col,float fadeSpd)
    {
        Col.color +=new Color(0,0,0,fadeSpd);
        return Col;
    }
    // テキストのフェード
    public Text anim_Fade_T(Text Col, float fadeSpd)
    {
        Col.color += new Color(0, 0, 0, fadeSpd);
        return Col;
    }
    // 回転
    public Transform anim_rotation(Transform tf,float x,float y,float z)
    {
        //tf.localEulerAngles += new Vector3(x, y, z);
        tf.rotation = Quaternion.Euler(x, y, z);
        return tf;
    }
}