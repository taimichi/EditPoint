using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class ClassUIAnim
{
    #region UIMoveMethod
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
    public Transform T_anim_rotation(Transform tf,float x,float y,float z)
    {
        //tf.localEulerAngles += new Vector3(x, y, z);
        tf.rotation = Quaternion.Euler(x, y, z);
        return tf;
    }

    public RectTransform R_anim_rotation(RectTransform tf, float x, float y, float z)
    {
        //tf.localEulerAngles += new Vector3(x, y, z);
        tf.rotation = Quaternion.Euler(x, y, z);
        return tf;
    }
    #endregion

    #region DOTweenMethod
    public enum RotateMode
    {
        //
        // 概要:
        //     Fastest way that never rotates beyond 360°
        Fast = 0,
        //
        // 概要:
        //     Fastest way that rotates beyond 360°
        FastBeyond360 = 1,
        //
        // 概要:
        //     Adds the given rotation to the transform using world axis and an advanced precision
        //     mode (like when using transform.Rotate(Space.World)).
        //     In this mode the end value is is always considered relative
        WorldAxisAdd = 2,
        //
        // 概要:
        //     Adds the given rotation to the transform's local axis (like when rotating an
        //     object with the "local" switch enabled in Unity's editor or using transform.Rotate(Space.Self)).
        //     In this mode the end value is is always considered relative
        LocalAxisAdd = 3
    }

    public void POSITION(RectTransform rct)
    {
        rct.DOMove(new Vector3(5f, 0f, 0f), 3f);
    }

    public void ROTATION_Z(RectTransform rct,float rotSpd,float timer)
    {
        rct.DORotate(Vector3.forward * rotSpd,timer);
    }
    #endregion
}