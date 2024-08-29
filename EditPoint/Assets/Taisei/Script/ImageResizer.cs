using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageResizer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform targetImage;  //�N���b�v��RectTransform
    private Vector2 initialSizeDelta;
    private Vector2 initialMousePosition;

    private Vector3 center;
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
    private float f_onetick;

    private Vector3 v3_mousePos;
    private Vector3 v3_offset;
    private float f_dotWidth = 0f;
    private float f_dotHeight = 0f;
    private float f_oneWidth;
    private float f_oneHeight;

    private int i_resizeCount = 0;

    private RectTransform rect_outLeft;    //�^�C�����C���̍��[
    private RectTransform rect_outRight;   //�^�C�����C���̉E�[

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
        f_onetick = timelineData.f_oneTickWidht;

        //�N���b�v�ړ��p
        f_oneWidth = timelineData.f_oneTickWidht;
        f_oneHeight = timelineData.f_oneTickHeight;

        //�^�C�����C���̒[��RectTransform�擾
        rect_outLeft = GameObject.Find("LeftOutLine").GetComponent<RectTransform>();
        rect_outRight = GameObject.Find("RightOutLine").GetComponent<RectTransform>();
    }
    private void Start()
    {
        center = targetImage.TransformPoint(targetImage.rect.center);
    }

    private void Update()
    {
        center = targetImage.TransformPoint(targetImage.rect.center);
    }

    public void OnBeginDrag(PointerEventData eventData)
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

    public void OnDrag(PointerEventData eventData)
    {
        if (mode != CLIP_MODE.resize)
        {
            if (targetImage == null)
            {
                mode = CLIP_MODE.normal;
                return;
            }

            //�N���b�v�ړ�����
            v3_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v3_mousePos.z = 0;

            v3_offset = targetImage.transform.position - center;

            //�h�b�g�ړ��p
            CalculationHeight();
            CalculationWidth();

            targetImage.transform.position = v3_mousePos + v3_offset;

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

        if(IsOverlapping(targetImage,rect_outLeft) || IsOverlapping(targetImage, rect_outRight))
        {
            if(i_resizeCount == 0)
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

        center = targetImage.TransformPoint(targetImage.rect.center);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(mode != CLIP_MODE.normal)
        {
            //���[�ƃN���b�v���d�Ȃ��Ă�ꍇ
            if (IsOverlapping(targetImage, rect_outLeft))
            {
                //�N���b�v�ړ��ɂ��ꍇ
                if(mode == CLIP_MODE.move)
                {

                }
                //�T�C�Y�ύX�ɂ��ꍇ
                else if(mode == CLIP_MODE.resize)
                {
                    ReCalculationSize();
                }
            }
            //�E�[�ƃN���b�v���d�Ȃ��Ă�ꍇ
            else if (IsOverlapping(targetImage, rect_outRight))
            {
                //�N���b�v�ړ��ɂ��ꍇ
                if (mode == CLIP_MODE.move)
                {

                }
                //�T�C�Y�ύX�ɂ��ꍇ
                else if (mode == CLIP_MODE.resize)
                {
                    ReCalculationSize();
                }
            }
            i_resizeCount = 0;
            mode = CLIP_MODE.normal;
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
    /// �T�C�Y���h�b�g�ړ����邽�߂̌v�Z
    /// </summary>
    private void CalculationSize()
    {
        f_dotMove = Mathf.Round(f_newSize / f_onetick);
        f_newSize = f_dotMove * f_onetick;
    }

    private void ReCalculationSize()
    {
        f_dotMove -= i_resizeCount;
        f_newSize = f_dotMove * f_onetick;
        targetImage.sizeDelta = new Vector2(f_newSize, targetImage.sizeDelta.y);
    }


    /// <summary>
    /// X���W�̌v�Z�p
    /// </summary>
    private void CalculationWidth()
    {

    }

    /// <summary>
    /// Y���W�̌v�Z�p
    /// </summary>
    private void CalculationHeight()
    {

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

}
