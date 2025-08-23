using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class TimeBar : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private float speed = 140f;
    private RectTransform barPos;
    private Vector2 nowPos;
    private float limit = 60f;
    private Vector2 startPos;

    private float limitPosX = 0f;
    private float nowTime = 0;
    private bool isDragMode = false;

    private PlayerController plController;
    [SerializeField] private TimelineManager TLManager;

    private GameObject TimeLineCanvas;
    private Camera UICamera;
    private GraphicRaycaster raycaster;

    private QuickGuideMenu quickGuide;

    private void Awake()
    {
        TimeData.TimeEntity.isDragMode = isDragMode;
        TimeData.TimeEntity.nowTime = nowTime;
        barPos = this.gameObject.GetComponent<RectTransform>();
        startPos = barPos.localPosition;

        limit = TLManager.ReturnTimebarLimit();

        limitPosX = startPos.x + (limit * 2 * TimelineData.TimelineEntity.oneTickWidth);

        transform.SetAsLastSibling();
    }

    void Start()
    {
        TimeData.TimeEntity.isDragMode = isDragMode;
        TimeData.TimeEntity.nowTime = nowTime;
        barPos = this.gameObject.GetComponent<RectTransform>();
        startPos = barPos.localPosition;

        limit = TLManager.ReturnTimebarLimit();

        limitPosX = startPos.x + (limit * 2 * TimelineData.TimelineEntity.oneTickWidth);

        transform.SetAsLastSibling();


        nowPos = barPos.localPosition;


        plController = GameObject.Find("Player").GetComponent<PlayerController>();

        TimeLineCanvas = this.transform.root.gameObject;    //�^�C�����C���L�����o�X(��ԏ�̐e�I�u�W�F�N�g)�擾

        UICamera = TimeLineCanvas.GetComponent<Canvas>().worldCamera;   //�^�C�����C���L�����o�X�ɐݒ肳��Ă�J�������擾
        raycaster = TimeLineCanvas.GetComponent<GraphicRaycaster>();

        quickGuide = GameObject.Find("GuideCanvas").GetComponent<QuickGuideMenu>();
    }

    private void Update()
    {
        //�Đ���
        if (GameData.GameEntity.isPlayNow)
        {
            //���ԓ��̎����X�e�[�W���N���A����ĂȂ��Ƃ�
            if (barPos.localPosition.x < limitPosX && !GameData.GameEntity.isClear)
            {
                barPos.localPosition = nowPos;
                nowPos.x += speed * Time.deltaTime;
            }
            //�Ō�܂ōĐ����������X�e�[�W���N���A���ꂽ�Ƃ�
            else
            {
                //���Ԃ̌��E���̏���
                plController.PlayerStop();
                GameData.GameEntity.isLimitTime = true;
            }
        }
        //�Đ����ĂȂ��Ƃ�
        else
        {
            //�^�C���o�[�̊J�n�ʒu���猻�݂̈ʒu�̋��������߂�
            float f_distance = this.transform.localPosition.x - startPos.x;
            //�^�C���o�[�̂���ʒu�̎��Ԃ����߂�
            nowTime = (float)Math.Truncate(f_distance / speed * 10) / 10;
            //���݂̎��Ԃ�
            TimeData.TimeEntity.nowTime = nowTime;

        }

    }

    //�h���b�O�J�n����Ƃ�
    public void OnBeginDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        isDragMode = true;
        TimeData.TimeEntity.isDragMode = isDragMode;
    }

    //�h���b�O��
    public void OnDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        //�}�E�X���W�擾
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)barPos.parent,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 mouse_LocalPos
        );

        mouse_LocalPos.y = startPos.y;
        if (mouse_LocalPos.x <= startPos.x)
        {
            mouse_LocalPos.x = startPos.x;
        }
        else if(mouse_LocalPos.x >= limitPosX)
        {
            mouse_LocalPos.x = limitPosX;
        }
        barPos.localPosition = mouse_LocalPos;
        nowPos = barPos.localPosition;
    }

    //�h���b�O�I����
    public void OnEndDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        isDragMode = false;
        TimeData.TimeEntity.isDragMode = isDragMode;
    }

    /// <summary>
    /// �{�^�����������Ƃ��^�C���o�[�̈ʒu��0�b�̏ꏊ�ɖ߂�
    /// </summary>
    public void OnReStart()
    {
        barPos.localPosition = startPos;
        nowPos = startPos;
        GameData.GameEntity.isLimitTime = false;
    }

    /// <summary>
    /// �^�C���o�[���h���b�O�����ǂ����̔����Ԃ�
    /// </summary>
    /// <returns>true=�h���b�O���@false=�h���b�O���ĂȂ�</returns>
    public bool ReturnDragMode() => isDragMode;

    /// <summary>
    /// �^�C���o�[�̌��E�l��Ԃ�
    /// </summary>
    /// <returns>�^�C���o�[�̌��EX���W(float�^)</returns>
    public float ReturnLimitPos() => limitPosX;
}
