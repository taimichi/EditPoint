using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Pixeye.Unity;

public class ClipOperation : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Foldout("Start"), SerializeField, Header("�N���b�v�̒���(�b)")]
    private float startLength = 5f;

    [SerializeField] private RectTransform targetImage;  //�N���b�v�摜��RectTransform
    private Vector2 initSizeDelta;
    private Vector2 initMousePos;

    private Vector2 resizeOffset;
    private Vector2 moveOffset;

    private bool isResizeRight;  // �E�������T�C�Y�����ǂ����̃t���O

    private Vector2 size;
    private Vector2 deltaPivot;
    private Vector3 deltaPos;

    private Vector2 startSize;

    [SerializeField, Header("�N���b�v�̍ŏ��T�C�Y")] private float minWidth = 350;
    [SerializeField, Header("�N���b�v�̍ő�T�C�Y")] private float maxWidth = 1400;
    private float newWidth;

    [SerializeField, Header("�T�C�Y�ύX���󂯕t����͈�(���E����)")] private float edgeRange = 10f;

    private float dotMove = 0;
    private float onetick;            //�T�C�Y�ύX���A1��ɃT�C�Y�ύX�����

    private Vector2 mousePos;        //�}�E�X�̍��W
    private float dotPosX = 0f;
    private float dotPosY = 0f;
    private float newPosY;          //�V����Y���W
    private float oneWidth;           //�ړ����A���Ɉړ�����ʁ@��
    private float oneHeight;          //�ړ����A���Ɉړ�����ʁ@�c

    private int resizeCount = 0;

    private RectTransform rect_UpLeft;    //�^�C�����C���̍���
    private RectTransform rect_DownRight;   //�^�C�����C���̉E��

    private GameObject[] Clips;
    private RectTransform[] ClipsRect;

    private Vector3 startPos;   //�����ʒu

    private CheckOverlap checkOverlap = new CheckOverlap();

    private FunctionLookManager functionLook;

    /// <summary>
    /// �N���b�v����̎��
    /// </summary>
    private enum CLIP_MODE
    {
        /// <summary>
        /// �������Ă��Ȃ��A�m�[�}��
        /// </summary>
        normal,
        /// <summary>
        /// �T�C�Y�ύX����
        /// </summary>
        resize,
        /// <summary>
        /// �ړ�����
        /// </summary>
        move,
    }
    //���݂̃N���b�v����̏��
    private CLIP_MODE mode = CLIP_MODE.normal;

    private PlaySound playSound;

    //�N���b�v���^�C�����C���̊O�ɏo����
    private bool isOut = false;

    [SerializeField] private bool isLook = false;

    private void Awake()
    {
        //���T�C�Y�p
        onetick = TimelineData.TimelineEntity.oneResize;

        //�N���b�v�ړ��p
        oneWidth = TimelineData.TimelineEntity.oneTickWidth;
        oneHeight = TimelineData.TimelineEntity.oneTickHeight;

        //�^�C�����C���̒[��RectTransform�擾
        rect_UpLeft = GameObject.Find("UpLeftOutLine").GetComponent<RectTransform>();
        rect_DownRight = GameObject.Find("DownRightOutLine").GetComponent<RectTransform>();

        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        functionLook = GameObject.FindWithTag("GameManager").GetComponent<FunctionLookManager>();

        //�����̒���
        targetImage.sizeDelta = new Vector2(
            startLength * onetick * 2, targetImage.sizeDelta.y
            );

        //�N���b�v�̈ʒu�𒲐�
        CalculationWidth(targetImage.localPosition.x);
        CalculationHeight(targetImage.localPosition.y);
        CheckWidth();
        CheckHeight();
        targetImage.localPosition = new Vector3(newWidth, newPosY, 0);
        startSize = targetImage.sizeDelta;
        targetImage.sizeDelta = new Vector2(startSize.x, startSize.y);

        //�q�I�u�W�F�N�g�̏��Ԃ�ύX
        int childNum = targetImage.parent.transform.childCount;
        transform.SetSiblingIndex(childNum - 2);
    }
    private void Start()
    {
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
                    newPosY = ClipsRect[i].localPosition.y - oneHeight;
                    CheckHeight();

                    //�N���b�v����ԉ��ŏd�Ȃ����ꍇ
                    if (ClipsRect[i].localPosition.y <= rect_DownRight.localPosition.y)
                    {
                        Debug.Log("��ԉ�");
                        newPosY = 0 * oneHeight - 15f;

                        //�d�Ȃ����N���b�v�̉E�[�̍��W���擾
                        float rightEdge = ClipsRect[i].anchoredPosition.x + (ClipsRect[i].rect.width * (1 - ClipsRect[i].pivot.x));

                        newWidth = rightEdge + oneWidth;

                        CalculationWidth(newWidth);
                        CalculationHeight(newPosY);

                        targetImage.localPosition = new Vector3(newWidth, newPosY, 0);
                        for(int j = 0; j < 5 /*�^�C�����C���̃��C���[��*/ ; j++)
                        {
                            if (checkOverlap.IsOverlap(targetImage, ClipsRect[j]))
                            {
                                newPosY -= oneHeight;
                                targetImage.localPosition = new Vector3(newWidth, newPosY, 0);
                            }
                        }
                    }
                    targetImage.localPosition = new Vector3(newWidth, newPosY, 0);
                }
            }
            //�^�O�ύX
            this.gameObject.tag = "SetClip";
        }
        startPos = this.transform.localPosition;
        isOut = false;
    }

    private void Update()
    {
        GetClipRect();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow || isLook)
        {
            return;
        }

        if ((functionLook.FunctionLook & LookFlags.ClipAccess) == 0)
        {
            initSizeDelta = targetImage.sizeDelta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetImage,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localMousePos
            );

            // �}�E�X�ʒu���摜�̉E�[�����[�����`�F�b�N
            if (Mathf.Abs(localMousePos.x - (-targetImage.rect.width * targetImage.pivot.x)) <= edgeRange)
            {
                // ���[
                SetPivot(targetImage, new Vector2(1, 0.5f));
                isResizeRight = false;
                mode = CLIP_MODE.resize;
            }
            else if (Mathf.Abs(localMousePos.x - (targetImage.rect.width * (1 - targetImage.pivot.x))) <= edgeRange)
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
                moveOffset.x = targetImage.position.x - localMousePos.x;
            }

            if (mode == CLIP_MODE.resize)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)targetImage.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out initMousePos
                );
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow || isLook)
        {
            return;
        }

        if ((functionLook.FunctionLook & LookFlags.ClipAccess) == 0)
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
                    out mousePos
                );

                //�h�b�g�ړ��p
                CalculationWidth(mousePos.x + moveOffset.x);
                CalculationHeight(mousePos.y);

                //�^�C�����C���͈̔͊O�ɏo����
                CheckWidth();
                CheckHeight();

                targetImage.localPosition = new Vector3(newWidth, newPosY, 0);
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)targetImage.parent,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 currentMousePos
            );

            resizeOffset = currentMousePos - initMousePos;

            if (isResizeRight)
            {
                newWidth = initSizeDelta.x + resizeOffset.x;
            }
            else
            {
                newWidth = initSizeDelta.x - resizeOffset.x;
            }

            CalculationSize();

            //�N���b�v�̒����ύX�̍ۂɍő�E�ŏ��T�C�Y�𒴂��Ȃ��悤�ɂ���
            newWidth = Mathf.Clamp(newWidth, minWidth, maxWidth);

            //�^�C�����C���̍��[�A�E�[�𒴂���Ƃ�
            if (targetImage.position.x > rect_UpLeft.position.x 
                || targetImage.position.x < rect_DownRight.position.x)
            {
                if (resizeCount == 0)
                {
                    resizeCount = 1;
                }

                //���T�C�Y�O���傫���ꍇ
                if (targetImage.sizeDelta.x > newWidth)
                {
                    resizeCount--;
                }
                //���T�C�Y�O���������ꍇ
                else if (targetImage.sizeDelta.x < newWidth)
                {
                    resizeCount++;
                }
            }

            targetImage.sizeDelta = new Vector2(newWidth, targetImage.sizeDelta.y);        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow || isLook)
        {
            return;
        }

        if ((functionLook.FunctionLook & LookFlags.ClipAccess) == 0)
        {
            //�s�{�b�g�������̂��̂�
            SetPivot(targetImage, new Vector2(0, 0.5f));
            //�d�Ȃ����ꍇ
            GetClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                ClipsRect[i].localPosition = new Vector3(
                    ClipsRect[i].localPosition.x, ClipsRect[i].localPosition.y, ClipsRect[i].localPosition.z);

                if (checkOverlap.IsOverlap(targetImage, ClipsRect[i]))
                {
                    //�����I�u�W�F�N�g����Ȃ��Ƃ�
                    if (targetImage.name != Clips[i].name)
                    {
                        Debug.Log("�d�Ȃ���");
                        targetImage.localPosition = startPos;
                    }
                }
            }

            //�N���b�v���^�C�����C���̊O�ɏo����
            if (isOut)
            {
                targetImage.localPosition = startPos;
                isOut = false;
            }
            
            startPos = this.transform.localPosition;

            if (mode != CLIP_MODE.normal)
            {
                playSound.PlaySE(PlaySound.SE_TYPE.objMove);
                //�^�C�����C���̍��[�ƃN���b�v���d�Ȃ��Ă�ꍇ
                if (targetImage.localPosition.x < rect_UpLeft.localPosition.x)
                {
                    Debug.Log("���d�Ȃ���");
                    //�T�C�Y�ύX�ɂ��ꍇ
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                //�^�C�����C���̉E�[�ƃN���b�v���d�Ȃ��Ă�ꍇ
                else if (targetImage.localPosition.x + targetImage.sizeDelta.x > rect_DownRight.localPosition.x)
                {
                    Debug.Log("�E�d�Ȃ���");
                    //�T�C�Y�ύX�ɂ��ꍇ
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                resizeCount = 0;
                mode = CLIP_MODE.normal;
            }
        }

    }

    private void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        size = rectTransform.rect.size;
        deltaPivot = rectTransform.pivot - pivot;
        deltaPos = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);

        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPos * rectTransform.localScale.x;
    }

    /// <summary>
    /// �T�C�Y�ύX���邽�߂̌v�Z
    /// </summary>
    private void CalculationSize()
    {
        dotMove = (float)Math.Round(newWidth / onetick);
        newWidth = dotMove * onetick;
    }

    /// <summary>
    /// �N���b�v����ʊO�ɍs�����ۂ̏���
    /// </summary>
    private void ReCalculationSize()
    {
        dotMove -= resizeCount;
        newWidth = dotMove * onetick;
        targetImage.sizeDelta = new Vector2(newWidth, targetImage.sizeDelta.y);
    }


    /// <summary>
    /// X���W�̌v�Z�p
    /// </summary>
    private void CalculationWidth(float posX)
    {
        dotPosX = posX - ((float)Math.Round(posX / oneWidth) * oneWidth);
        if (dotPosX < oneWidth / 2)
        {
            newWidth = (float)Math.Round(posX / oneWidth) * oneWidth + 30f;
        }
        else
        {
            newWidth = ((float)Math.Round(posX / oneWidth) + 1) * oneWidth + 30f;
        }
        //�v�Z�ɂ��� 30 �̓^�C�����C���̘g�̕�
    }

    /// <summary>
    /// Y���W�̌v�Z�p
    /// </summary>
    private void CalculationHeight(float posY)
    {
        dotPosY = posY - ((float)Math.Round(posY / oneHeight) * oneHeight);
        if (dotPosY < oneHeight / 2)
        {
            newPosY = (float)Math.Round(posY / oneHeight) * oneHeight - 15f;
        }
        else
        {
            newPosY = ((float)Math.Round(posY / oneHeight) + 1) * oneHeight - 15f;
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
            newWidth = rect_UpLeft.localPosition.x;
            isOut = true;
        }
        //�E��
        else if (targetImage.localPosition.x + targetImage.sizeDelta.x > rect_DownRight.localPosition.x)
        {
            newWidth = rect_DownRight.localPosition.x - targetImage.sizeDelta.x;
            isOut = true;
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
            newPosY = rect_UpLeft.localPosition.y;
            isOut = true;
        }
        //��
        else if (targetImage.localPosition.y < rect_DownRight.localPosition.y)
        {
            newPosY = rect_DownRight.localPosition.y;
            isOut = true;
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
}
