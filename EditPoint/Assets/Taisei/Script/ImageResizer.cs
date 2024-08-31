using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ImageResizer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform targetImage;  //�N���b�v�摜��RectTransform
    private Vector2 initialSizeDelta;
    private Vector2 initialMousePosition;

    private Vector2 offset;

    private bool isResizingRight;  // �E�������T�C�Y�����ǂ����̃t���O

    private Vector2 size;
    private Vector2 deltaPivot;
    private Vector3 deltaPosition;

    [SerializeField, Header("�N���b�v�̍ŏ��T�C�Y")] private float f_minSize = 350;
    [SerializeField, Header("�N���b�v�̍ő�T�C�Y")] private float f_maxSize = 1400;
    private float f_newSize;

    [SerializeField, Header("���E�[�͈̔�")] private float f_edgeRange = 10f;

    private float f_dotMove = 0;
    [SerializeField] private TimelineData timelineData;
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

    private void Awake()
    {
        //���T�C�Y�p
        f_onetick = timelineData.f_oneResize;

        //�N���b�v�ړ��p
        f_oneWidth = timelineData.f_oneTickWidht;
        f_oneHeight = timelineData.f_oneTickHeight;

        //�^�C�����C���̒[��RectTransform�擾
        rect_outLeft = GameObject.Find("LeftOutLine").GetComponent<RectTransform>();
        rect_outRight = GameObject.Find("RightOutLine").GetComponent<RectTransform>();

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
            f_newWidth += f_oneWidth;
            CheckWidth();
            v2_newPos = new Vector2(f_newWidth, f_newHeight);
            targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);

            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                //���̃N���b�v�Əd�Ȃ����ꍇ
                if (IsOverlapping(targetImage, ClipsRect[i]))
                {
                    //�d�Ȃ����N���b�v�̉��Ɉړ�
                    f_newHeight = ClipsRect[i].localPosition.y - f_oneHeight;
                    CheckHeight();

                    //�N���b�v����ԉ��ŏd�Ȃ����ꍇ
                    if (ClipsRect[i].localPosition.y <= timelineData.f_timelineEndDown)
                    {
                        f_newHeight = 0 * f_oneHeight - 15f;

                        //�d�Ȃ����N���b�v�̉E�[�̍��W���擾
                        Vector2 overRapSize = ClipsRect[i].rect.size;
                        float pivotX = ClipsRect[i].pivot.x;
                        float rightEdgeX = (0.5f - pivotX) * overRapSize.x;
                        Vector3 rightLocalPos = new Vector3(rightEdgeX, 0, 0);


                        f_newWidth = rightLocalPos.x + f_oneWidth * (i - 1);
                        CalculationWidth(f_newWidth);
                        CalculationHeight(f_newHeight);

                    }
                    v2_newPos = new Vector2(f_newWidth, f_newHeight);
                    targetImage.localPosition = new Vector3(v2_newPos.x, v2_newPos.y, 0);
                }
            }

            this.gameObject.tag = "SetClip";
            v3_beforePos = this.transform.localPosition;

        }

    }

    private void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!b_Lock)
        {
            initialSizeDelta = targetImage.sizeDelta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetImage,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localMousePosition
            );

            // �}�E�X�ʒu���摜�̉E�[�����[�����`�F�b�N
            if (Mathf.Abs(localMousePosition.x - (-targetImage.rect.width * targetImage.pivot.x)) <= f_edgeRange)
            {
                // ���[
                SetPivot(targetImage, new Vector2(1, 0.5f));
                isResizingRight = false;
                mode = CLIP_MODE.resize;
            }
            else if (Mathf.Abs(localMousePosition.x - (targetImage.rect.width * (1 - targetImage.pivot.x))) <= f_edgeRange)
            {
                // �E�[
                SetPivot(targetImage, new Vector2(0, 0.5f));
                isResizingRight = true;
                mode = CLIP_MODE.resize;
            }
            else
            {
                // �[�ȊO�̏ꍇ�̓��T�C�Y�𖳌����A�N���b�v�ړ����[�h�ɂ���
                mode = CLIP_MODE.move;
                SetPivot(targetImage, new Vector2(0, 0.5f));
            }

            if (mode == CLIP_MODE.resize)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)targetImage.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out initialMousePosition
                );
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!b_Lock)
        {
            if (mode != CLIP_MODE.resize)
            {
                if (targetImage == null)
                {
                    mode = CLIP_MODE.normal;
                    return;
                }

                ////�N���b�v�ړ�����
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)targetImage.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out v2_mousePos
                );


                //�h�b�g�ړ��p
                CalculationWidth(v2_mousePos.x);
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
                out Vector2 currentMousePosition
            );

            offset = currentMousePosition - initialMousePosition;

            if (isResizingRight)
            {
                f_newSize = initialSizeDelta.x + offset.x;
            }
            else
            {
                f_newSize = initialSizeDelta.x - offset.x;
            }

            CalculationSize();

            f_newSize = Mathf.Clamp(f_newSize, f_minSize, f_maxSize);

            if (IsOverlapping(targetImage, rect_outLeft) || IsOverlapping(targetImage, rect_outRight))
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
        if (!b_Lock)
        {
            //�d�Ȃ����ꍇ
            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                if (IsOverlapping(targetImage, ClipsRect[i]))
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
                //���[�ƃN���b�v���d�Ȃ��Ă�ꍇ
                if (IsOverlapping(targetImage, rect_outLeft))
                {
                    //�T�C�Y�ύX�ɂ��ꍇ
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                //�E�[�ƃN���b�v���d�Ȃ��Ă�ꍇ
                else if (IsOverlapping(targetImage, rect_outRight))
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
        size = rectTransform.rect.size;
        deltaPivot = rectTransform.pivot - pivot;
        deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);

        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition * rectTransform.localScale.x;
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
        if (f_newWidth < timelineData.f_timelineEndLeft)
        {
            f_newWidth = timelineData.f_timelineEndLeft;
        }
        else if (f_newWidth > timelineData.f_timelineEndRight)
        {
            f_newWidth = timelineData.f_timelineEndRight;
        }
    }

    /// <summary>
    /// �N���b�v���^�C�����C���̏㉺�͈̔͊O�ɏo����
    /// </summary>
    private void CheckHeight()
    {
        if (f_newHeight > timelineData.f_timelineEndUp)
        {
            f_newHeight = timelineData.f_timelineEndUp;
        }
        else if (f_newHeight < timelineData.f_timelineEndDown)
        {
            f_newHeight = timelineData.f_timelineEndDown;
        }
    }


    /// <summary>
    /// �N���b�v�ƒ[���d�Ȃ��Ă��邩���`�F�b�N
    /// </summary>
    /// <param name="rect1">�N���b�v��RectTransform</param>
    /// <param name="rect2">�[��RectTransform</param>
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
