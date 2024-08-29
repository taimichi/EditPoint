using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipPlay : MonoBehaviour
{
    private RectTransform rect_timeBar;
    [SerializeField] private RectTransform rect_Clip;

    private float f_timer = 0;
    /// <summary>
    /// �N���b�v���Đ����邩�ǂ���
    /// </summary>
    private bool b_clipPlay = false;

    void Start()
    {
        //�^�C���o�[��RectTransform���擾
        rect_timeBar = GameObject.Find("Timebar").GetComponent<RectTransform>();
    }

    void Update()
    {
        if (IsOverlapping(rect_Clip, rect_timeBar))
        {
            Debug.Log("UI�I�u�W�F�N�g���ڐG���Ă��܂�");
            b_clipPlay = true;
        }
        else
        {
            Debug.Log("UI�I�u�W�F�N�g���ڐG���Ă��܂���");
            b_clipPlay = false;
        }

        //�N���b�v�Đ����̏���
        if (b_clipPlay)
        {
            f_timer += Time.deltaTime;

        }
        //�N���b�v�Đ����ĂȂ��Ƃ��̏���
        else
        {
            f_timer = 0f;
        }
    }

    public float ReturnClipTime()
    {
        return f_timer;
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
}
