using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class ClassUIAnim
{
    #region UIMoveMethod
    /// <summary>
    /// UIを初期座標値にセットする
    /// </summary>
    /// <param name="rct">動かすUI</param>
    /// <param name="strPos">初期座標</param>
    /// <returns>セットしたrctを返す</returns>
    public RectTransform anim_Start(RectTransform rct,Vector2 strPos)
    {
        rct.anchoredPosition = strPos;
        return rct;
    }

    /// <summary>
    /// rctを移動させるよ
    /// </summary>
    /// <param name="rct">動かすUI</param>
    /// <param name="spdX">X方向速度</param>
    /// <param name="spdY">Y方向速度</param>
    /// <returns>移動したrctを返す</returns>
    public RectTransform anim_PosChange(RectTransform rct,float spdX,float spdY)
    {
        Debug.Log("PosChange");
        rct.anchoredPosition +=new Vector2( spdX,spdY);
        return rct;
    }

    /// <summary>
    /// HeightとWidthを使ったサイズ変更
    /// </summary>
    /// <param name="rct">対象のUI</param>
    /// <param name="sclX">拡大サイズ速度X</param>
    /// <param name="sclY">拡大サイズ速度Y</param>
    /// <returns>サイズ変更したrctを返却</returns>
    public RectTransform anim_SclChange(RectTransform rct,float sclX,float sclY)
    {
        rct.sizeDelta += new Vector2(sclX, sclY);
        return rct;
    }

    /// <summary>
    /// イメージをフェードさせる
    /// </summary>
    /// <param name="Col">対象のUI</param>
    /// <param name="fadeSpd">フェード速度</param>
    /// <returns>フェードした後のColを返すよ</returns>
    public Image anim_Fade_I(Image Col,float fadeSpd)
    {
        Col.color +=new Color(0,0,0,fadeSpd);
        return Col;
    }
    /// <summary>
    /// テキストをフェードさせる
    /// </summary>
    /// <param name="Col">対象のUI</param>
    /// <param name="fadeSpd">フェード速度</param>
    /// <returns>フェードした後のColを返すよ</returns>
    public Text anim_Fade_T(Text Col, float fadeSpd)
    {
        Col.color += new Color(0, 0, 0, fadeSpd);
        return Col;
    }
    /// <summary>
    /// 回転処理（Transform）
    /// </summary>
    /// <param name="tf">対象のObj</param>
    /// <param name="x">速度X</param>
    /// <param name="y">速度Y</param>
    /// <param name="z">速度Z</param>
    /// <returns>回転したtfを返す</returns>
    public Transform T_anim_rotation(Transform tf,float x,float y,float z)
    {
        //tf.localEulerAngles += new Vector3(x, y, z);
        tf.rotation = Quaternion.Euler(x, y, z);
        return tf;
    }
    /// <summary>
    /// 回転処理（RectTransform）
    /// </summary>
    /// <param name="tf">対象のUI</param>
    /// <param name="x">速度X</param>
    /// <param name="y">速度Y</param>
    /// <param name="z">速度Z</param>
    /// <returns>回転したtfを返す</returns>
    public RectTransform R_anim_rotation(RectTransform tf, float x, float y, float z)
    {
        //tf.localEulerAngles += new Vector3(x, y, z);
        tf.rotation = Quaternion.Euler(x, y, z);
        return tf;
    }
    #endregion

    // 使うかわからないので必要になり次第作成しておきます
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