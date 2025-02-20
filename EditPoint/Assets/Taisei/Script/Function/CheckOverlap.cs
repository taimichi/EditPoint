using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI���m���d�Ȃ��Ă��邩�`�F�b�N���邽�ߗp
/// </summary>
public class CheckOverlap
{
    // �����@Rect�N���X�Ɋւ���
    // Rect�N���X��Box�����N���X
    // Rect(����X�A����Y�A�����A�c��)�Ƃ��Ďg��
    // ����āARect�̍��W(0,0)�͍���̍��W�ƂȂ�

    /// <summary>
    /// UI���m���d�Ȃ��Ă��邩�`�F�b�N
    /// </summary>
    /// <returns>�d�Ȃ��Ă���=true �d�Ȃ��Ă��Ȃ�=false</returns>
    public bool IsOverlap(RectTransform rect1, RectTransform rect2)
    {
        // RectTransform�̋��E�����[���h���W�Ŏ擾
        Rect rect1World = GetWorldRect(rect1);
        Rect rect2World = GetWorldRect(rect2);

        // ���E���d�Ȃ��Ă��邩�ǂ������`�F�b�N
        return rect1World.Overlaps(rect2World);
    }

    /// <summary>
    /// ���[���h���W�ł̋��E���擾
    /// </summary>
    /// <returns>���[���h���W�ł�RectTransform</returns>
    public Rect GetWorldRect(RectTransform rt)
    {
        //�l���̃��[���h���W������z��
        Vector3[] corners = new Vector3[4];
        //RectTransform�̎l���̃��[���h���W���擾
        rt.GetWorldCorners(corners);

        return new Rect(corners[0], corners[2] - corners[0]);
    }

}
