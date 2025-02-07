using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClipOperation : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform targetImage;  //�N���b�v�摜��RectTransform
    private Vector2 v2_initSizeDelta;
    private Vector2 v2_initMousePos;

    private Vector2 v2_resizeOffset;
    private Vector2 v2_moveOffset;

    private bool isResizeRight;  // �E�������T�C�Y�����ǂ����̃t���O

    private Vector2 v2_size;
    private Vector2 v2_deltaPivot;
    private Vector3 v2_deltaPos;

    private Vector2 v2_startSize;

    [SerializeField, Header("�N���b�v�̍ŏ��T�C�Y")] private float f_minSize = 350;
    [SerializeField, Header("�N���b�v�̍ő�T�C�Y")] private float f_maxSize = 1400;
    private float f_newSize;

    [SerializeField, Header("�T�C�Y�ύX���󂯕t����͈�(���E����)")] private float f_edgeRange = 10f;

    private float f_dotMove = 0;
    private float f_onetick;            //�T�C�Y�ύX���A1��ɃT�C�Y�ύX�����

    private Vector2 v2_mousePos;        //�}�E�X�̍��W
    private float f_dotWidth = 0f;
    private float f_newWidth;           //�V����X���W
    private float f_dotHeight = 0f;
    private float f_newHeight;          //�V����Y���W
    private float f_oneWidth;           //�ړ����A���Ɉړ�����ʁ@��
    private float f_oneHeight;          //�ړ����A���Ɉړ�����ʁ@�c

    private float timeBarLimitPos = 0f; //�^�C���o�[�̌��EX���W

    private int i_resizeCount = 0;

    private RectTransform rect_UpLeft;    //�^�C�����C���̍���
    private RectTransform rect_DownRight;   //�^�C�����C���̉E��

    private GameObject[] Clips;
    private RectTransform[] ClipsRect;

    private Vector3 v3_beforePos;
    private Vector2 savePos;

    //�N���b�v�̈ړ��A�T�C�Y�ύX�@�\���g�p�\���ǂ���
    [SerializeField] private bool b_Lock = false;

    private CheckOverlap checkOverlap = new CheckOverlap();

    private enum CLIP_MODE
    {
        normal,
        resize,
        move,
    }
    private CLIP_MODE mode = CLIP_MODE.normal;

    private PlaySound playSound;

    private void Awake()
    {
        //���T�C�Y�p
        f_onetick = TimelineData.TimelineEntity.f_oneResize;

        //�N���b�v�ړ��p
        f_oneWidth = TimelineData.TimelineEntity.f_oneTickWidht;
        f_oneHeight = TimelineData.TimelineEntity.f_oneTickHeight;

        //�^�C�����C���̒[��RectTransform�擾
        rect_UpLeft = GameObject.Find("UpLeftOutLine").GetComponent<RectTransform>();
        rect_DownRight = GameObject.Find("DownRightOutLine").GetComponent<RectTransform>();

        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        //�N���b�v�̈ʒu�𒲐�
        CalculationWidth(targetImage.localPosition.x);
        CalculationHeight(targetImage.localPosition.y);
        CheckWidth();
        CheckHeight();
        targetImage.localPosition = new Vector3(f_newWidth, f_newHeight, 0);
        v2_startSize = targetImage.sizeDelta;
        targetImage.sizeDelta = new Vector2(v2_startSize.x, v2_startSize.y);

        int childNum = targetImage.parent.transform.childCount;
        transform.SetSiblingIndex(childNum - 2);
    }
    private void Start()
    {
        //�^�C���o�[�̌��E���W���擾
        timeBarLimitPos = GameObject.Find("Timebar").GetComponent<TimeBar>().ReturnLimitPos();

        //�쐬�����΂����̃N���b�v�̎�
        if (this.gameObject.tag == "CreateClip")
        {
            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                //���̃N���b�v�Əd�Ȃ����ꍇ
                if (checkOverlap.IsOverlap(targetImage, ClipsRect[i]))
                {
                    //�d�Ȃ����N���b�v�̉��Ɉړ�
                    f_newHeight = ClipsRect[i].localPosition.y - f_oneHeight;
                    CheckHeight();

                    //�N���b�v����ԉ��ŏd�Ȃ����ꍇ
                    if (ClipsRect[i].localPosition.y <= rect_DownRight.localPosition.y)
                    {
                        Debug.Log("��ԉ�");
                        f_newHeight = 0 * f_oneHeight - 15f;

                        //�d�Ȃ����N���b�v�̉E�[�̍��W���擾
                        float rightEdge = ClipsRect[i].anchoredPosition.x + (ClipsRect[i].rect.width * (1 - ClipsRect[i].pivot.x));

                        f_newWidth = rightEdge + f_oneWidth;

                        CalculationWidth(f_newWidth);
                        CalculationHeight(f_newHeight);

                        targetImage.localPosition = new Vector3(f_newWidth, f_newHeight, 0);
                        for(int j = 0; j < 5 /*�^�C�����C���̃��C���[��*/ ; j++)
                        {
                            if (checkOverlap.IsOverlap(targetImage, ClipsRect[j]))
                            {
                                f_newHeight -= f_oneHeight;
                                targetImage.localPosition = new Vector3(f_newWidth, f_newHeight, 0);
                            }
                        }
                    }
                    targetImage.localPosition = new Vector3(f_newWidth, f_newHeight, 0);
                }
            }
            //�^�O�ύX
            this.gameObject.tag = "SetClip";
        }
        v3_beforePos = this.transform.localPosition;

    }

    private void Update()
    {
        GetClipRect();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        if (!b_Lock)
        {
            v2_initSizeDelta = targetImage.sizeDelta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetImage,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localMousePos
            );

            // �}�E�X�ʒu���摜�̉E�[�����[�����`�F�b�N
            if (Mathf.Abs(localMousePos.x - (-targetImage.rect.width * targetImage.pivot.x)) <= f_edgeRange)
            {
                // ���[
                SetPivot(targetImage, new Vector2(1, 0.5f));
                isResizeRight = false;
                mode = CLIP_MODE.resize;
            }
            else if (Mathf.Abs(localMousePos.x - (targetImage.rect.width * (1 - targetImage.pivot.x))) <= f_edgeRange)
            {
                // �E�[
                SetPivot(targetImage, new Vector2(0, 0.5f));
                isResizeRight = true;
                mode = CLIP_MODE.resize;
            }
            else
            {
                // �[�ȊO�̏ꍇ�̓��T�C�Y�𖳌����A�N���b�v�ړ����[�h�ɂ���
                mode = CLIP_MODE.move;
                SetPivot(targetImage, new Vector2(0, 0.5f));
                v2_moveOffset.x = targetImage.position.x - localMousePos.x;
            }

            if (mode == CLIP_MODE.resize)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)targetImage.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out v2_initMousePos
                );
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        if (!b_Lock)
        {
            if (mode != CLIP_MODE.resize)
            {
                if (targetImage == null)
                {
                    mode = CLIP_MODE.normal;
                    return;
                }

                //�N���b�v�ړ�����
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)targetImage.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out v2_mousePos
                );

                //�h�b�g�ړ��p
                CalculationWidth(v2_mousePos.x + v2_moveOffset.x);
                CalculationHeight(v2_mousePos.y);

                //�^�C�����C���͈̔͊O�ɏo����
                CheckWidth();
                CheckHeight();

                targetImage.localPosition = new Vector3(f_newWidth, f_newHeight, 0);

                ////�^�C���o�[�̌��E���W�𒴂�����
                //if (CheckLimitPos())
                //{
                //    v2_newPos.x = timeBarLimitPos - targetImage.rect.width - 30;
                //    this.targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);
                //}

                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)targetImage.parent,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 currentMousePos
            );

            v2_resizeOffset = currentMousePos - v2_initMousePos;

            if (isResizeRight)
            {
                f_newSize = v2_initSizeDelta.x + v2_resizeOffset.x;
            }
            else
            {
                f_newSize = v2_initSizeDelta.x - v2_resizeOffset.x;
            }

            CalculationSize();

            //�N���b�v�̒����ύX�̍ۂɍő�E�ŏ��T�C�Y�𒴂��Ȃ��悤�ɂ���
            f_newSize = Mathf.Clamp(f_newSize, f_minSize, f_maxSize);

            //�^�C�����C���̍��[�A�E�[�𒴂���Ƃ�
            if (targetImage.position.x > rect_UpLeft.position.x 
                || targetImage.position.x < rect_DownRight.position.x)
            {
                if (i_resizeCount == 0)
                {
                    i_resizeCount = 1;
                }

                //���T�C�Y�O���傫���ꍇ
                if (targetImage.sizeDelta.x > f_newSize)
                {
                    i_resizeCount--;
                }
                //���T�C�Y�O���������ꍇ
                else if (targetImage.sizeDelta.x < f_newSize)
                {
                    i_resizeCount++;
                }
            }

            targetImage.sizeDelta = new Vector2(f_newSize, targetImage.sizeDelta.y);

            ////�T�C�Y�ύX���Ƀ^�C���o�[�̌��E���W�ɍs�����Ƃ�
            //if (CheckLimitPos())
            //{
            //    targetImage.sizeDelta = savePos;

            //}
            //savePos = targetImage.sizeDelta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        if (!b_Lock)
        {
            //�s�{�b�g�������̂��̂�
            SetPivot(targetImage, new Vector2(0, 0.5f));
            //�d�Ȃ����ꍇ
            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                ClipsRect[i].localPosition = new Vector3(
                    ClipsRect[i].localPosition.x - 0.1f, ClipsRect[i].localPosition.y, ClipsRect[i].localPosition.z);

                if (checkOverlap.IsOverlap(targetImage, ClipsRect[i]))
                {
                    //�����I�u�W�F�N�g����Ȃ��Ƃ�
                    if (targetImage.name != Clips[i].name)
                    {
                        Debug.Log("�d�Ȃ���");
                        targetImage.localPosition = v3_beforePos;
                    }
                }
            }

            v3_beforePos = this.transform.localPosition;

            if (mode != CLIP_MODE.normal)
            {
                playSound.PlaySE(PlaySound.SE_TYPE.objMove);
                //�^�C�����C���̍��[�ƃN���b�v���d�Ȃ��Ă�ꍇ
                if (checkOverlap.IsOverlap(targetImage, rect_UpLeft))
                {
                    //�T�C�Y�ύX�ɂ��ꍇ
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                //�^�C�����C���̉E�[�ƃN���b�v���d�Ȃ��Ă�ꍇ
                else if (checkOverlap.IsOverlap(targetImage, rect_DownRight))
                {
                    //�T�C�Y�ύX�ɂ��ꍇ
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                i_resizeCount = 0;
                mode = CLIP_MODE.normal;
            }
        }

    }

    private void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        v2_size = rectTransform.rect.size;
        v2_deltaPivot = rectTransform.pivot - pivot;
        v2_deltaPos = new Vector3(v2_deltaPivot.x * v2_size.x, v2_deltaPivot.y * v2_size.y);

        rectTransform.pivot = pivot;
        rectTransform.localPosition -= v2_deltaPos * rectTransform.localScale.x;
    }

    /// <summary>
    /// �T�C�Y�ύX���邽�߂̌v�Z
    /// </summary>
    private void CalculationSize()
    {
        f_dotMove = (float)Math.Round(f_newSize / f_onetick);
        f_newSize = f_dotMove * f_onetick;
    }

    /// <summary>
    /// �N���b�v����ʊO�ɍs�����ۂ̏���
    /// </summary>
    private void ReCalculationSize()
    {
        f_dotMove -= i_resizeCount;
        f_newSize = f_dotMove * f_onetick;
        targetImage.sizeDelta = new Vector2(f_newSize, targetImage.sizeDelta.y);
    }


    /// <summary>
    /// X���W�̌v�Z�p
    /// </summary>
    private void CalculationWidth(float posX)
    {
        f_dotWidth = posX - ((float)Math.Round(posX / f_oneWidth) * f_oneWidth);
        if (f_dotWidth < f_oneWidth / 2)
        {
            f_newWidth = (float)Math.Round(posX / f_oneWidth) * f_oneWidth + 30f;
        }
        else
        {
            f_newWidth = ((float)Math.Round(posX / f_oneWidth) + 1) * f_oneWidth + 30f;
        }
        //�v�Z�ɂ��� 30 �̓^�C�����C���̘g�̕�
    }

    /// <summary>
    /// Y���W�̌v�Z�p
    /// </summary>
    private void CalculationHeight(float posY)
    {
        f_dotHeight = posY - ((float)Math.Round(posY / f_oneHeight) * f_oneHeight);
        if (f_dotHeight < f_oneHeight / 2)
        {
            f_newHeight = (float)Math.Round(posY / f_oneHeight) * f_oneHeight - 15f;
        }
        else
        {
            f_newHeight = ((float)Math.Round(posY / f_oneHeight) + 1) * f_oneHeight - 15f;
        }
        //�v�Z�ɂ��� 15 �̓^�C�����C���̘g�̕�
    }

    /// <summary>
    /// �N���b�v���^�C�����C���̍��E�͈̔͊O�ɏo����
    /// </summary>
    private void CheckWidth()
    {
        //����
        if (targetImage.localPosition.x < rect_UpLeft.localPosition.x)
        {
            f_newWidth = rect_UpLeft.localPosition.x;
        }
        //�E��
        else if (targetImage.localPosition.x > rect_DownRight.localPosition.x - targetImage.sizeDelta.x)
        {
            f_newWidth = rect_DownRight.localPosition.x - targetImage.sizeDelta.x;
        }
    }

    /// <summary>
    /// �N���b�v���^�C�����C���̏㉺�͈̔͊O�ɏo����
    /// </summary>
    private void CheckHeight()
    {
        //��
        if (targetImage.localPosition.y > rect_UpLeft.localPosition.y)
        {
            f_newHeight = rect_UpLeft.localPosition.y;
        }
        //��
        else if (targetImage.localPosition.y < rect_DownRight.localPosition.y)
        {
            f_newHeight = rect_DownRight.localPosition.y;
        }
    }

    private void GetClipRect()
    {
        Clips = GameObject.FindGameObjectsWithTag("SetClip");
        ClipsRect = new RectTransform[Clips.Length];
        for(int i = 0; i < Clips.Length; i++)
        {
            ClipsRect[i] = Clips[i].GetComponent<RectTransform>();
        }
    }

    /// <summary>
    /// �^�C���o�[�̌��E���W�𒴂������̔���
    /// </summary>
    /// <returns>false=�����ĂȂ��@true=������</returns>
    private bool CheckLimitPos()
    {
        bool isCheck = false;

        //�^�C���o�[�̌��E���W�𒴂�����
        float rightEdge = targetImage.anchoredPosition.x + (targetImage.rect.width * (1 - targetImage.pivot.x));
        if (rightEdge > timeBarLimitPos)
        {
            isCheck = true;
        }


        return isCheck;
    }
}
