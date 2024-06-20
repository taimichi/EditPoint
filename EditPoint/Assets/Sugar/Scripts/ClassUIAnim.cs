using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassUIAnim
{
    // ������UI,�������W,�ǂ��܂œ��������̖ڕW,X��Y�̈ʒu
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

    // �C���[�W�̃t�F�[�h
    public Image anim_Fade_I(Image Col,float fadeSpd)
    {
        Col.color +=new Color(0,0,0,fadeSpd);
        return Col;
    }
    // �e�L�X�g�̃t�F�[�h
    public Text anim_Fade_T(Text Col, float fadeSpd)
    {
        Col.color += new Color(0, 0, 0, fadeSpd);
        return Col;
    }
    // ��]
    public Transform anim_rotation(Transform tf,float x,float y,float z)
    {
        //tf.localEulerAngles += new Vector3(x, y, z);
        tf.rotation = Quaternion.Euler(x, y, z);
        return tf;
    }
}