using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipFunction : MonoBehaviour
{
    private enum MODE_TYPE
    {
        normal,
        cut,
        delete
    }
    private MODE_TYPE mode = MODE_TYPE.normal;

    [SerializeField] private RectTransform Timebar;
    private int cutCount = 0;

    [SerializeField] private TimelineData timelineData;
    [SerializeField] private GetClip GetClip;

    private GameObject Clip;
    private GameObject CutedClip;

    private RectTransform grandParentRect;

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// �N���b�v�ƃ^�C���o�[���d�Ȃ��Ă��邩���`�F�b�N
    /// </summary>
    /// <param name="rect1">�N���b�v��RectTransform</param>
    /// <param name="rect2">�^�C���o�[��RectTransform</param>
    /// <returns>�d�Ȃ��Ă���=true �d�Ȃ��Ă��Ȃ�=false</returns>
    private bool IsOverlapping(RectTransform rect1, RectTransform rect2)
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
    /// <param name="rt">�擾����RectTransform</param>
    /// <returns>���[���h���W�ł�RectTransform</returns>
    private Rect GetWorldRect(RectTransform rt)
    {
        //�l���̃��[���h���W������z��
        Vector3[] corners = new Vector3[4];
        //RectTransform�̎l���̃��[���h���W���擾
        rt.GetWorldCorners(corners);

        return new Rect(corners[0], corners[2] - corners[0]);
    }


    /// <summary>
    /// �J�b�g�@�\�@�{�^���ŌĂяo��
    /// </summary>
    public void OnCut()
    {
        Clip = GetClip.ReturnGetClip();
        RectTransform clipRect = Clip.GetComponent<RectTransform>();

        //�J�b�g�@�\���g���̂̓N���b�v�ƃ^�C���o�[���d�Ȃ��Ă鎞�̂�
        if(IsOverlapping(clipRect, Timebar))
        {
            mode = MODE_TYPE.cut;
            cutCount++;

            grandParentRect = clipRect.parent.parent.GetComponent<RectTransform>();

            Vector3 leftEdge = grandParentRect.InverseTransformPoint(clipRect.position) 
                + new Vector3(clipRect.rect.width * clipRect.pivot.x, 0, 0);
            float dis = Timebar.localPosition.x - leftEdge.x;
        }



    }

    /// <summary>
    /// �N���b�v�폜�@�\�@�{�^���ŌĂяo��
    /// </summary>
    public void OnDelete()
    {
        mode = MODE_TYPE.delete;
    }
}
