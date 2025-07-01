using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Pixeye.Unity;
using System.Collections;
using System.Collections.Generic;

//�N���b�v�̈ʒu��T�C�Y�A�����Ȃǂ̑���֘A
public class ClipOperation : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Foldout("Start"), SerializeField, Header("�N���b�v�̒���(�b)")]
    private float startLength = 5f;

    [SerializeField] private RectTransform targetImage;  //�N���b�v�摜��RectTransform
    private Vector2 biginSizeDelta;             //�ύX�O�̃N���b�v�̉摜�T�C�Y
    private Vector2 beginMouse_LocalPos;        //�N���b�N�����Ƃ��̃��[�J�����W

    private Vector2 resizeOffset;               //�ύX�O�ƕύX��̃T�C�Y��
    private Vector2 moveOffset;                 //�N���b�v�̒��S���W����}�E�X���W�̍�

    private bool isResizeRight;  // �E�������T�C�Y�����ǂ����̃t���O

    //�ʒu�𓮂������APivot�݂̂�ύX���邽�ߗp�̕ϐ�
    private Vector2 size;
    private Vector2 deltaPivot;
    private Vector3 deltaPos;

    private Vector2 startSize;  //�����T�C�Y

    [SerializeField, Header("�N���b�v�̍ŏ��T�C�Y")] private float minWidth = 350;
    [SerializeField, Header("�N���b�v�̍ő�T�C�Y")] private float maxWidth = 1400;
    private float newWidth;         //�V�����N���b�v�̒���

    [SerializeField, Header("�T�C�Y�ύX���󂯕t����͈�(���E����)")] private float edgeRange = 10f;

    private float dotMove = 0;
    private float onetick;            //�T�C�Y�ύX���A1��ɃT�C�Y�ύX�����

    private Vector2 NowMouse_LocalPos;        //�}�E�X�̍��W

    private float dotPosX = 0f;     //�h�b�g�P�ʈړ��p�@X���W
    private float newPosX;          //�V����X���W
    private float dotPosY = 0f;     //�h�b�g�P�ʈړ��p�@Y���W
    private float newPosY;          //�V����Y���W

    private float oneWidth;           //�ړ����A���Ɉړ�����ʁ@��
    private float oneHeight;          //�ړ����A���Ɉړ�����ʁ@�c

    private int resizeCount = 0;        //���̃T�C�Y����Ȃ�}�X���ύX���ꂽ��

    private RectTransform rect_UpLeft;    //�^�C�����C���̍���
    private RectTransform rect_DownRight;   //�^�C�����C���̉E��

    private GameObject[] Clips;
    private RectTransform[] ClipsRect;

    private Vector3 startPos;   //�����ʒu

    //�N���b�v���d�Ȃ��Ă��邩�v�Z����p
    private CheckOverlap checkOverlap = new CheckOverlap();

    //�@�\���b�N�X�N���v�g
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

    [SerializeField, Header("���\���ǂ��� false=�\ / true=�s��")] private bool isLook = false;

    #region ClipSprite
    [Foldout("Sprite"), SerializeField] private Sprite ActiveClipSprite;    //����\���̃N���b�v�̃X�v���C�g
    [Foldout("Sprite"), SerializeField] private Sprite NoActiveClipSprite;  //����s�\���̃N���b�v�̃X�v���C�g
    #endregion

    private Image ClipImage;    //�N���b�v��Image

    private SelectYesNo select_ClipCombine;

    private void Awake()
    {
        //���T�C�Y�p
        onetick = TimelineData.TimelineEntity.oneResize;
        //�����̒���
        targetImage.sizeDelta = new Vector2(
            startLength * onetick * 2, targetImage.sizeDelta.y
            );
    }

    private void Start()
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

        select_ClipCombine = GameObject.Find("Selects").GetComponent<SelectYesNo>();
        select_ClipCombine.SelectPanelActive(false);

        //�N���b�v�̈ʒu�𒲐�
        CalculationWidth(targetImage.localPosition.x);
        CalculationHeight(targetImage.localPosition.y);
        CheckWidth();
        CheckHeight();
        //�ʒu��ݒ�
        targetImage.localPosition = new Vector3(newPosX, newPosY, 0);
        //�J�n�T�C�Y���擾
        startSize = targetImage.sizeDelta;
        //�T�C�Y��ݒ�
        targetImage.sizeDelta = new Vector2(startSize.x, startSize.y);

        //�q�I�u�W�F�N�g�̏��Ԃ�ύX
        int childNum = targetImage.parent.transform.childCount;
        transform.SetSiblingIndex(childNum - 3);

        //�N���b�v�̉摜���擾
        ClipImage = this.gameObject.GetComponent<Image>();

        //�N���b�v�̉摜��ύX
        if (!isLook)
        {
            //���N���b�v�̉摜�ɕύX
            ClipImage.sprite = ActiveClipSprite;
        }
        else
        {
            //����N���b�v�̉摜�ɕύX
            ClipImage.sprite = NoActiveClipSprite;
        }

        //�쐬�����΂����̃N���b�v�̎�
        if (this.gameObject.tag == "CreateClip")
        {
            //�S�N���b�v���擾
            GetAllClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                //���̃N���b�v�Əd�Ȃ����ꍇ
                if (checkOverlap.IsOverlap(targetImage, ClipsRect[i]))
                {
                    //�d�Ȃ����N���b�v�̉��̒i�Ɉړ�
                    newPosY = ClipsRect[i].localPosition.y - oneHeight;
                    CheckHeight();

                    //�N���b�v����ԉ��ŏd�Ȃ����ꍇ
                    if (ClipsRect[i].localPosition.y <= rect_DownRight.localPosition.y)
                    {
                        Debug.Log("��ԉ�");
                        newPosY = 0 * oneHeight - 15f;

                        //�d�Ȃ����N���b�v�̉E�[�̍��W���擾
                        float rightEdge = ClipsRect[i].anchoredPosition.x + (ClipsRect[i].rect.width * (1 - ClipsRect[i].pivot.x));
                        //�V����x���W���擾
                        newPosX = rightEdge + oneWidth;

                        //�ēx�v�Z���A���W�𒲐�
                        CalculationWidth(newPosX);
                        CalculationHeight(newPosY);

                        //���W�X�V
                        targetImage.localPosition = new Vector3(newPosX, newPosY, 0);

                        for(int j = 0; j < 5 /*�^�C�����C���̃��C���[��*/ ; j++)
                        {
                            //�܂����̃N���b�v�Əd�Ȃ����ꍇ
                            if (checkOverlap.IsOverlap(targetImage, ClipsRect[j]))
                            {
                                //�V����y���W��ݒ�
                                newPosY -= oneHeight;
                                //���W�X�V
                                targetImage.localPosition = new Vector3(newPosX, newPosY, 0);
                            }
                        }
                    }
                    //���W�X�V
                    targetImage.localPosition = new Vector3(newPosX, newPosY, 0);
                }
            }
            //�^�O�ύX
            this.gameObject.tag = "SetClip";
        }
        //�J�n���̍��W���擾
        startPos = this.transform.localPosition;
        isOut = false;
    }

    private void Update()
    {
        GetAllClipRect();
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
            startPos = this.transform.localPosition;

            //�ύX�O�̃N���b�v�̃T�C�Y���擾
            biginSizeDelta = targetImage.sizeDelta;
            //�}�E�X�J�[�\���̍��W���擾���A���[�J�����W�ɒ���
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
                //�N���b�v�̃s�{�b�g��ύX
                SetPivot(targetImage, new Vector2(1, 0.5f));
                //�����ύX�t���O��
                isResizeRight = false;
                mode = CLIP_MODE.resize;
            }
            else if (Mathf.Abs(localMousePos.x - (targetImage.rect.width * (1 - targetImage.pivot.x))) <= edgeRange)
            {
                // �E�[
                //�N���b�v�̃s�{�b�g��ύX
                SetPivot(targetImage, new Vector2(0, 0.5f));
                //�E���ύX�t���O��
                isResizeRight = true;
                mode = CLIP_MODE.resize;
            }
            else
            {
                // �[�ȊO�̏ꍇ�̓��T�C�Y�𖳌����A�N���b�v�ړ����[�h�ɂ���
                mode = CLIP_MODE.move;
                //�s�{�b�g��ʏ�̏�Ԃ�
                SetPivot(targetImage, new Vector2(0, 0.5f));
                //�}�E�X�J�[�\���ƃN���b�v�̒��S�ʒu�̋������擾
                moveOffset.x = targetImage.position.x - localMousePos.x;
            }

            //�N���b�v�T�C�Y�ύX���[�h�̎�
            if (mode == CLIP_MODE.resize)
            {
                //�}�E�X�J�[�\���̍��W���擾���A���[�J�����W�ɒ���
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)targetImage.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out beginMouse_LocalPos
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

        //�N���b�v���삪�֎~����Ă��Ȃ��Ƃ�
        if ((functionLook.FunctionLook & LookFlags.ClipAccess) == 0)
        {
            //�X�N���[�����W��RectTransform��̃��[�J�����W�ɕϊ�
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (
                    (RectTransform)targetImage.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out NowMouse_LocalPos
                );

            //�N���b�v�ړ���Ԃ̎�
            if (mode == CLIP_MODE.move)
            {
                if (targetImage == null)
                {
                    mode = CLIP_MODE.normal;
                    return;
                }

                //�h�b�g�ړ��p
                CalculationWidth(NowMouse_LocalPos.x + moveOffset.x);
                CalculationHeight(NowMouse_LocalPos.y);

                //�^�C�����C���͈̔͊O�ɏo����
                CheckWidth();
                CheckHeight();

                //�ʒu�X�V
                targetImage.localPosition = new Vector3(newPosX, newPosY, 0);

                return;
            }

            //�N���b�v�T�C�Y�ύX�̎�
            if(mode == CLIP_MODE.resize)
            {
                //�ύX�O�ƕύX��̃}�E�X�̈ړ��ʂ��v�Z
                resizeOffset = NowMouse_LocalPos - beginMouse_LocalPos;

                //�N���b�v�̉E�[�̂Ƃ�
                if (isResizeRight)
                {
                    newWidth = biginSizeDelta.x + resizeOffset.x;
                }
                //�N���b�v�̍��[�̂Ƃ�
                else
                {
                    newWidth = biginSizeDelta.x - resizeOffset.x;
                }

                //�ύX����T�C�Y����
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

                //�N���b�v�̃T�C�Y��ύX
                targetImage.sizeDelta = new Vector2(newWidth, targetImage.sizeDelta.y);
            }

        }
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
            //�N���b�v���^�C�����C���̊O�ɏo����
            if (isOut)
            {
                targetImage.localPosition = startPos;
                isOut = false;
            }

            //�s�{�b�g�������̂��̂�
            SetPivot(targetImage, new Vector2(0, 0.5f));
            GetAllClipRect();
            //�d�Ȃ������ǂ���
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                //�d�Ȃ�����
                if (checkOverlap.IsOverlap(targetImage, ClipsRect[i]))
                {
                    //�����I�u�W�F�N�g����Ȃ��Ƃ�
                    if (targetImage.name != Clips[i].name)
                    {
                        Debug.Log("�d�Ȃ���");

                        //�d�Ȃ����N���b�v�����s�\�ȃN���b�v����Ȃ��Ƃ�
                        ClipOperation overRapClip = Clips[i].GetComponent<ClipOperation>();
                        if (!overRapClip.CheckIsLook())
                        {
                            //�N���b�v���������邩�ǂ���
                            StartCoroutine(CheckClipCombine(Clips[i]));
                        }
                        else
                        {
                            //���̈ʒu�ɖ߂�
                            targetImage.localPosition = startPos;
                        }
                        break;
                    }
                }
            }
            
            if (mode != CLIP_MODE.normal)
            {
                playSound.PlaySE(PlaySound.SE_TYPE.objMove);
                //�N���b�v���^�C�����C���̍��[�𒴂��Ă鎞
                if (targetImage.localPosition.x < rect_UpLeft.localPosition.x)
                {
                    Debug.Log("���d�Ȃ���");
                    //�T�C�Y�ύX�ɂ��ꍇ
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                //�N���b�v���^�C�����C���̉E�[�𒴂��Ă鎞
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

    /// <summary>
    /// �N���b�v���������邩���Ȃ��������߂�
    /// �����ɂ͏d�Ȃ����N���b�v������
    /// </summary>
    IEnumerator CheckClipCombine(GameObject _clip)
    {
        select_ClipCombine.SelectPanelActive(true);
        //�I���������܂ŃX�g�b�v
        yield return new WaitUntil(() => select_ClipCombine.ReturnOnClick() == true);

        //�I�����ꂽ�火

        //�������邩�ǂ���
        //�������̂Ƃ�
        if (!select_ClipCombine.ReturnSelect())
        {
            Debug.Log("���ɖ߂���");
            targetImage.localPosition = startPos;
        }
        //�͂��̂Ƃ�
        else
        {
            Debug.Log("���������");
            ClipCombine(_clip);
        }

        select_ClipCombine.SelectPanelActive(false);
    }

    /// <summary>
    /// �N���b�v������
    /// </summary>
    private void ClipCombine(GameObject _clip)
    {
        //�d�Ȃ����N���b�v
        ClipPlay overRapClip = _clip.GetComponent<ClipPlay>();

        //���̃N���b�v
        ClipPlay thisClip = this.gameObject.GetComponent<ClipPlay>();
        //���̃N���b�v�ɕR�Â��Ă���I�u�W�F�N�g���擾
        List<GameObject> connectObj = thisClip.ReturnConnectObj();
        
        //�d�Ȃ����N���b�v�Ɍ��ݎ����Ă���N���b�v�ɕR�Â���ꂽ�I�u�W�F�N�g���ڂ�
        for(int i = 0; i < connectObj.Count; i++)
        {
            overRapClip.OutGetObj(connectObj[i]);
        }

        //���̃N���b�v���폜
        Destroy(this.gameObject);

    }

    /// <summary>
    /// ������RectTransform��Pivot�݂̂�ύX
    /// (Pivot�݂̂𒼐ڕύX����Ɖ摜�̈ʒu���ς�邽��)
    /// </summary>
    /// <param name="rectTransform">�ύX������Pivot��RectTransform</param>
    /// <param name="pivot">�ύX���Pivot�̒l</param>
    private void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        //�ύX������RectTransform�̃T�C�Y���擾
        size = rectTransform.rect.size;
        //�s�{�b�g�̕ύX�O�ƕύX��̍���
        deltaPivot = rectTransform.pivot - pivot;
        //�ύX�O�̕ύX��̍��W�̍���
        deltaPos = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);

        //�V���ȃs�{�b�g��ݒ�
        rectTransform.pivot = pivot;
        //�s�{�b�g�ύX�ɂ����W������C��
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
            newPosX = (float)Math.Round(posX / oneWidth) * oneWidth + 30f;
        }
        else
        {
            newPosX = ((float)Math.Round(posX / oneWidth) + 1) * oneWidth + 30f;
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
            newPosX = rect_UpLeft.localPosition.x;
            isOut = true;
        }
        //�E��
        else if (targetImage.localPosition.x + targetImage.sizeDelta.x > rect_DownRight.localPosition.x)
        {
            newPosX = rect_DownRight.localPosition.x - targetImage.sizeDelta.x;
            isOut = true;
        }
        else
        {
            //Debug.Log("���E�����ĂȂ�");
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
        else
        {
            //Debug.Log("�㉺�����ĂȂ�");
        }
    }

    /// <summary>
    /// �N���b�v�����b�N����Ă��邩�ǂ���
    /// </summary>
    /// <returns>false=���b�N����Ă��Ȃ� / true=���b�N����Ă���</returns>
    public bool CheckIsLook() => isLook;

    /// <summary>
    /// �S�N���b�v���擾
    /// </summary>
    private void GetAllClipRect()
    {
        //�N���b�v��GameObject�^�Ŏ擾
        Clips = GameObject.FindGameObjectsWithTag("SetClip");

        //�N���b�v��RectTransform���擾
        ClipsRect = new RectTransform[Clips.Length];
        for(int i = 0; i < Clips.Length; i++)
        {
            ClipsRect[i] = Clips[i].GetComponent<RectTransform>();
        }
    }
}
