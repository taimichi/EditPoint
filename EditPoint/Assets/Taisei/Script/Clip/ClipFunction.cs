using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClipFunction : MonoBehaviour
{
    [SerializeField] private RectTransform Timebar; //�^�C���o�[��RectTransform

    [SerializeField] private GetClip GetClip;   //�I�������N���b�v���擾����X�N���v�g

    private GameObject Clip;

    private RectTransform grandParentRect;

    private float old_maxTime = 0f; //�J�b�g�O�̃N���b�v�̍ő厞��
    private float new_maxTime = 0f; //�J�b�g������̃N���b�v�̍ő厞��

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
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        Clip = GetClip.ReturnGetClip();
        Clip.transform.GetChild(0).gameObject.SetActive(false);
        RectTransform clipRect = Clip.GetComponent<RectTransform>();
        ClipPlay clipPlay = Clip.GetComponent<ClipPlay>();

        //�J�b�g�@�\���g���̂̓N���b�v�ƃ^�C���o�[���d�Ȃ��Ă鎞�̂�
        if(IsOverlapping(clipRect, Timebar))
        {
            old_maxTime = clipPlay.ReturnMaxTime();

            grandParentRect = clipRect.parent.parent.GetComponent<RectTransform>();

            //�I�������N���b�v�̍��[�̍��W
            Vector3 leftEdge = grandParentRect.InverseTransformPoint(clipRect.position) 
                + new Vector3(clipRect.rect.width * clipRect.pivot.x, 0, 0);
            //���[����̒���
            float dis = Timebar.localPosition.x - leftEdge.x;

            //�T�C�Y�𒲐�
            dis = ((float)Math.Round(dis / TimelineData.TimelineEntity.f_oneResize)) * TimelineData.TimelineEntity.f_oneResize;
            
            //�^�C���o�[����E�[�܂ł̒���
            float newDis = clipRect.rect.width - dis;

            //�J�b�g������̍����̃N���b�v
            clipRect.sizeDelta = new Vector2(dis, clipRect.rect.height);

            //�E�[�擾
            float rightEdge = clipRect.anchoredPosition.x + (clipRect.rect.width * (1 - clipRect.pivot.x));

            //�J�b�g�������̉E���p
            GameObject newClip = Instantiate(Clip, clipRect.localPosition, Quaternion.identity, this.gameObject.transform);
            newClip.name = Clip.name + "(CutClip)";
            RectTransform newClipRect = newClip.GetComponent<RectTransform>();
            //�J�b�g�����N���b�v�̒����𒲐�
            newClipRect.sizeDelta = new Vector2(newDis, newClipRect.rect.height);
            //�J�b�g�����N���b�v�̈ʒu�𒲐�
            newClipRect.localPosition = new Vector2(rightEdge + 0.1f, clipRect.localPosition.y);

            //��U�Е��̃N���b�v�̃I�u�W�F�N�g�Ƃ̕R�Â�������
            ClipPlay newClipPlay = newClip.GetComponent<ClipPlay>();
            newClipPlay.DestroyConnectObj();

            //�N���b�v�ƕR�Â���ꂽ�I�u�W�F�N�g���擾
            List<GameObject> newConnectObj = clipPlay.ReturnConnectObj();

            //�N���b�v�̒����Ƒ����̏����l��ݒ�
            //�N���b�v(��)
            ClipSpeed clipSpeed = Clip.GetComponent<ClipSpeed>();
            clipSpeed.GetStartWidth(dis);   // ����
            clipSpeed.UpdateSpeed(1f);      // ����
            //�N���b�v(�E)
            ClipSpeed newClipSpeed = newClip.GetComponent<ClipSpeed>();
            newClipSpeed.GetStartWidth(newDis); //����
            newClipSpeed.UpdateSpeed(1f);       //����

            newClipPlay.CalculationMaxTime();
            new_maxTime = newClipPlay.ReturnMaxTime();
            newClipPlay.UpdateStartTime(old_maxTime - new_maxTime);

            //�N���b�v�ɕR�Â���ꂽ�I�u�W�F�N�g�𕡐����āA�V�����N���b�v�ƕR�Â�
            for (int i = 0; i < newConnectObj.Count; i++)
            {
                GameObject obj = Instantiate(newConnectObj[i]);
                //��������������
                if(TryGetComponent<MoveGround>(out var test))
                {
                    test.GetClipTime_Auto(old_maxTime - new_maxTime);
                }
                newClipPlay.OutGetObj(obj);
            }
        }
    }

}
