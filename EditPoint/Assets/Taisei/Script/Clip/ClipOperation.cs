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

    private bool b_ResizeRight;  // �E�������T�C�Y�����ǂ����̃t���O

    private Vector2 v2_size;
    private Vector2 v2_deltaPivot;
    private Vector3 v2_deltaPos;

    [SerializeField, Header("�N���b�v�̍ŏ��T�C�Y")] private float f_minSize = 350;
    [SerializeField, Header("�N���b�v�̍ő�T�C�Y")] private float f_maxSize = 1400;
    private float f_newSize;

    [SerializeField, Header("���E�[�͈̔�")] private float f_edgeRange = 10f;

    private float f_dotMove = 0;
    private float f_onetick;            //�T�C�Y�ύX���A1��ɃT�C�Y�ύX�����

    private Vector2 v2_mousePos;        //�}�E�X�̍��W
    private Vector2 v2_newPos;          //�V�������W
    private float f_dotWidth = 0f;
    private float f_newWidth;           //�V�������̈ړ��ʒu
    private float f_dotHeight = 0f;
    private float f_newHeight;          //�V�����^�e�̈ړ��ʒu
    private float f_oneWidth;           //�ړ����A���Ɉړ�����ʁ@��
    private float f_oneHeight;          //�ړ����A���Ɉړ�����ʁ@�c

    private int i_resizeCount = 0;

    private RectTransform rect_outLeft;    //�^�C�����C���̍��[
    private RectTransform rect_outRight;   //�^�C�����C���̉E�[

    private GameObject[] Clips;
    private RectTransform[] ClipsRect;

    private Vector3 v3_beforePos;

    [SerializeField] private bool b_Lock = false;

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
        rect_outLeft = GameObject.Find("LeftOutLine").GetComponent<RectTransform>();
        rect_outRight = GameObject.Find("RightOutLine").GetComponent<RectTransform>();

        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        CalculationWidth(targetImage.localPosition.x);
        CalculationHeight(targetImage.localPosition.y);
        CheckWidth();
        CheckHeight();
        v2_newPos = new Vector2(f_newWidth, f_newHeight);
        targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);
    }
    private void Start()
    {
        if (this.gameObject.tag == "CreateClip")
        {
            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                //���̃N���b�v�Əd�Ȃ����ꍇ
                if (CheckOverrap(targetImage, ClipsRect[i]))
                {
                    Debug.Log("�d�Ȃ���");
                    //�d�Ȃ����N���b�v�̉��Ɉړ�
                    f_newHeight = ClipsRect[i].localPosition.y - f_oneHeight;
                    CheckHeight();

                    //�N���b�v����ԉ��ŏd�Ȃ����ꍇ
                    if (ClipsRect[i].localPosition.y <= TimelineData.TimelineEntity.f_timelineEndDown)
                    {
                        f_newHeight = 0 * f_oneHeight - 15f;

                        //�d�Ȃ����N���b�v�̉E�[�̍��W���擾
                        float rightEdge = ClipsRect[i].anchoredPosition.x + (ClipsRect[i].rect.width * (1 - ClipsRect[i].pivot.x));

                        f_newWidth = rightEdge + f_oneWidth;

                        CalculationWidth(f_newWidth);
                        CalculationHeight(f_newHeight);

                        v2_newPos = new Vector2(f_newWidth, f_newHeight);
                        targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);
                        for(int j = 0; j < 5; j++)
                        {
                            if (CheckOverrap(targetImage, ClipsRect[j]))
                            {
                                f_newHeight -= f_oneHeight;
                                v2_newPos = new Vector2(f_newWidth, f_newHeight);
                                targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);
                            }
                        }
                    }
                    v2_newPos = new Vector2(f_newWidth, f_newHeight);
                    targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);
                }
            }

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
        if (GameData.GameEntity.b_playNow)
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
                b_ResizeRight = false;
                mode = CLIP_MODE.resize;
            }
            else if (Mathf.Abs(localMousePos.x - (targetImage.rect.width * (1 - targetImage.pivot.x))) <= f_edgeRange)
            {
                // �E�[
                SetPivot(targetImage, new Vector2(0, 0.5f));
                b_ResizeRight = true;
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
        if (GameData.GameEntity.b_playNow)
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
                v2_newPos = new Vector2(f_newWidth, f_newHeight);

                targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);


                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)targetImage.parent,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 currentMousePos
            );

            v2_resizeOffset = currentMousePos - v2_initMousePos;

            if (b_ResizeRight)
            {
                f_newSize = v2_initSizeDelta.x + v2_resizeOffset.x;
            }
            else
            {
                f_newSize = v2_initSizeDelta.x - v2_resizeOffset.x;
            }

            CalculationSize();

            f_newSize = Mathf.Clamp(f_newSize, f_minSize, f_maxSize);

            if (CheckOverrap(targetImage, rect_outLeft) || CheckOverrap(targetImage, rect_outRight))
            {
                if (i_resizeCount == 0)
                {
                    i_resizeCount = 1;
                }

                //���T�C�Y�O���傫���ꍇ
                if (targetImage.sizeDelta.x > f_newSize)
                {
                    i_resizeCount--;

                    Debug.Log("��");
                }
                //���T�C�Y�O���������ꍇ
                else if (targetImage.sizeDelta.x < f_newSize)
                {
                    Debug.Log("��");

                    i_resizeCount++;
                }
            }

            targetImage.sizeDelta = new Vector2(f_newSize, targetImage.sizeDelta.y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.b_playNow)
        {
            return;
        }

        if (!b_Lock)
        {
            //�d�Ȃ����ꍇ
            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                if (CheckOverrap(targetImage, ClipsRect[i]))
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
                //���[�ƃN���b�v���d�Ȃ��Ă�ꍇ
                if (CheckOverrap(targetImage, rect_outLeft))
                {
                    //�T�C�Y�ύX�ɂ��ꍇ
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                //�E�[�ƃN���b�v���d�Ȃ��Ă�ꍇ
                else if (CheckOverrap(targetImage, rect_outRight))
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
    /// �N���b�v����ʊO�ɍs���Ă��܂����ꍇ�p
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
            f_newWidth = (float)Math.Round(posX / f_oneWidth) * f_oneWidth - 30f;
        }
        else
        {
            f_newWidth = ((float)Math.Round(posX / f_oneWidth) + 1) * f_oneWidth - 30f;
        }
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
    }

    /// <summary>
    /// �N���b�v���^�C�����C���̍��E�͈̔͊O�ɏo����
    /// </summary>
    private void CheckWidth()
    {
        if (f_newWidth < TimelineData.TimelineEntity.f_timelineEndLeft)
        {
            f_newWidth = TimelineData.TimelineEntity.f_timelineEndLeft;
        }
        else if (f_newWidth > TimelineData.TimelineEntity.f_timelineEndRight)
        {
            f_newWidth = TimelineData.TimelineEntity.f_timelineEndRight;
        }
    }

    /// <summary>
    /// �N���b�v���^�C�����C���̏㉺�͈̔͊O�ɏo����
    /// </summary>
    private void CheckHeight()
    {
        if (f_newHeight > TimelineData.TimelineEntity.f_timelineEndUp)
        {
            f_newHeight = TimelineData.TimelineEntity.f_timelineEndUp;
        }
        else if (f_newHeight < TimelineData.TimelineEntity.f_timelineEndDown)
        {
            f_newHeight = TimelineData.TimelineEntity.f_timelineEndDown;
        }
    }


    /// <summary>
    /// �N���b�v�ƒ[���d�Ȃ��Ă��邩���`�F�b�N
    /// </summary>
    /// <param name="clipRect">�N���b�v��RectTransform</param>
    /// <param name="edgeRect">�[��RectTransform</param>
    /// <returns>�d�Ȃ��Ă���=true �d�Ȃ��Ă��Ȃ�=false</returns>
    private bool CheckOverrap(RectTransform clipRect, RectTransform edgeRect)
    {
        // RectTransform�̋��E�����[���h���W�Ŏ擾
        Rect rect1World = GetWorldRect(clipRect);
        Rect rect2World = GetWorldRect(edgeRect);

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


    private void GetClipRect()
    {
        Clips = GameObject.FindGameObjectsWithTag("SetClip");
        ClipsRect = new RectTransform[Clips.Length];
        for(int i = 0; i < Clips.Length; i++)
        {
            ClipsRect[i] = Clips[i].GetComponent<RectTransform>();
        }
    }
}
