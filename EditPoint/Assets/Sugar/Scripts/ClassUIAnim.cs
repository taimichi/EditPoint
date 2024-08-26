using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  //DOTween���g���Ƃ��͂���using������

public class ClassUIAnim
{
    #region UIMoveMethod
    /// <summary>
    /// UI���������W�l�ɃZ�b�g����
    /// </summary>
    /// <param name="rct">������UI</param>
    /// <param name="strPos">�������W</param>
    /// <returns>�Z�b�g����rct��Ԃ�</returns>
    public RectTransform anim_Start(RectTransform rct,Vector2 strPos)
    {
        rct.anchoredPosition = strPos;
        return rct;
    }

    /// <summary>
    /// rct���ړ��������
    /// </summary>
    /// <param name="rct">������UI</param>
    /// <param name="spdX">X�������x</param>
    /// <param name="spdY">Y�������x</param>
    /// <returns>�ړ�����rct��Ԃ�</returns>
    public RectTransform anim_PosChange(RectTransform rct,float spdX,float spdY)
    {
        Debug.Log("PosChange");
        rct.anchoredPosition +=new Vector2( spdX,spdY);
        return rct;
    }

    /// <summary>
    /// Height��Width���g�����T�C�Y�ύX
    /// </summary>
    /// <param name="rct">�Ώۂ�UI</param>
    /// <param name="sclX">�g��T�C�Y���xX</param>
    /// <param name="sclY">�g��T�C�Y���xY</param>
    /// <returns>�T�C�Y�ύX����rct��ԋp</returns>
    public RectTransform anim_SclChange(RectTransform rct,float sclX,float sclY)
    {
        rct.sizeDelta += new Vector2(sclX, sclY);
        return rct;
    }

    /// <summary>
    /// �C���[�W���t�F�[�h������
    /// </summary>
    /// <param name="Col">�Ώۂ�UI</param>
    /// <param name="fadeSpd">�t�F�[�h���x</param>
    /// <returns>�t�F�[�h�������Col��Ԃ���</returns>
    public Image anim_Fade_I(Image Col,float fadeSpd)
    {
        Col.color +=new Color(0,0,0,fadeSpd);
        return Col;
    }
    /// <summary>
    /// �e�L�X�g���t�F�[�h������
    /// </summary>
    /// <param name="Col">�Ώۂ�UI</param>
    /// <param name="fadeSpd">�t�F�[�h���x</param>
    /// <returns>�t�F�[�h�������Col��Ԃ���</returns>
    public Text anim_Fade_T(Text Col, float fadeSpd)
    {
        Col.color += new Color(0, 0, 0, fadeSpd);
        return Col;
    }
    /// <summary>
    /// ��]�����iTransform�j
    /// </summary>
    /// <param name="tf">�Ώۂ�Obj</param>
    /// <param name="x">���xX</param>
    /// <param name="y">���xY</param>
    /// <param name="z">���xZ</param>
    /// <returns>��]����tf��Ԃ�</returns>
    public Transform T_anim_rotation(Transform tf,float x,float y,float z)
    {
        //tf.localEulerAngles += new Vector3(x, y, z);
        tf.rotation = Quaternion.Euler(x, y, z);
        return tf;
    }
    /// <summary>
    /// ��]�����iRectTransform�j
    /// </summary>
    /// <param name="tf">�Ώۂ�UI</param>
    /// <param name="x">���xX</param>
    /// <param name="y">���xY</param>
    /// <param name="z">���xZ</param>
    /// <returns>��]����tf��Ԃ�</returns>
    public RectTransform R_anim_rotation(RectTransform tf, float x, float y, float z)
    {
        //tf.localEulerAngles += new Vector3(x, y, z);
        tf.rotation = Quaternion.Euler(x, y, z);
        return tf;
    }
    #endregion

    // �g�����킩��Ȃ��̂ŕK�v�ɂȂ莟��쐬���Ă����܂�
    #region DOTweenMethod
    public enum RotateMode
    {
        //
        // �T�v:
        //     Fastest way that never rotates beyond 360��
        Fast = 0,
        //
        // �T�v:
        //     Fastest way that rotates beyond 360��
        FastBeyond360 = 1,
        //
        // �T�v:
        //     Adds the given rotation to the transform using world axis and an advanced precision
        //     mode (like when using transform.Rotate(Space.World)).
        //     In this mode the end value is is always considered relative
        WorldAxisAdd = 2,
        //
        // �T�v:
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