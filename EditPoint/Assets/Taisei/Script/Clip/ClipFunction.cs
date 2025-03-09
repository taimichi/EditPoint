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

    private float old_maxTime = 0f; //�J�b�g�O�̃N���b�v�̍ő厞��
    private float new_maxTime = 0f; //�J�b�g������̃N���b�v�̍ő厞��

    private CheckOverlap checkOverlap = new CheckOverlap();

    private FunctionLookManager functionLook;

    private PlaySound playSound;

    private void Awake()
    {
        functionLook = GameObject.FindWithTag("GameManager").GetComponent<FunctionLookManager>();
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();


        Button cutButton = GameObject.Find("Cut").GetComponent<Button>();
        cutButton.onClick.AddListener(OnCut);
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

        //�J�b�g�@�\�����b�N����Ă��Ȃ��Ƃ�
        if((functionLook.FunctionLook & LookFlags.Cut) == 0)
        {
            //�N���b�N�����N���b�v���擾
            Clip = GetClip.ReturnGetClip();
            //�N���b�v�̘g���\��
            Clip.transform.GetChild(0).gameObject.SetActive(false);
            RectTransform clipRect = Clip.GetComponent<RectTransform>();
            ClipPlay clipPlay = Clip.GetComponent<ClipPlay>();

            //�J�b�g�@�\���g���̂̓N���b�v�ƃ^�C���o�[���d�Ȃ��Ă鎞�̂�
            if (checkOverlap.IsOverlap(clipRect, Timebar))
            {
                old_maxTime = clipPlay.ReturnMaxTime();

                //�I�������N���b�v�̍��[�̍��W
                Vector3 leftEdge = clipRect.localPosition
                    + new Vector3(clipRect.rect.width * clipRect.pivot.x, 0, 0);
                //���[����̒���
                float dis = Timebar.localPosition.x - leftEdge.x;

                //�T�C�Y�𒲐�
                dis = ((float)Math.Round(dis / TimelineData.TimelineEntity.oneResize)) * TimelineData.TimelineEntity.oneResize;

                //�^�C���o�[����E�[�܂ł̒���
                float newDis = clipRect.rect.width - dis;

                //�J�b�g������̍����̃N���b�v
                clipRect.sizeDelta = new Vector2(dis, clipRect.rect.height);

                //�E�[�擾
                float rightEdge = clipRect.localPosition.x + (clipRect.rect.width * (1 - clipRect.pivot.x));

                //�J�b�g�������̉E���p
                GameObject newClip = Instantiate(Clip, clipRect.localPosition, Quaternion.identity, this.transform.parent);
                newClip.name = Clip.name + "(CutClip)";
                RectTransform newClipRect = newClip.GetComponent<RectTransform>();
                //�J�b�g�����N���b�v�̒����𒲐�
                newClipRect.sizeDelta = new Vector2(newDis, newClipRect.rect.height);
                //�J�b�g�����N���b�v�̈ʒu�𒲐�
                newClipRect.localPosition = new Vector2(rightEdge, clipRect.localPosition.y);

                //��U�Е��̃N���b�v�̃I�u�W�F�N�g�Ƃ̕R�Â�������(2�d�̕R�Â����������邽��)
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
                    if (TryGetComponent<MoveGround>(out var test))
                    {
                        test.GetClipTime_Auto(old_maxTime - new_maxTime);
                    }
                    newClipPlay.OutGetObj(obj);
                }

                playSound.PlaySE(PlaySound.SE_TYPE.cut);
            }
        }
    }

}
