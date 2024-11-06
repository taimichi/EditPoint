using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class TimeBar : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private float f_speed = 278f;
    private RectTransform barPos;
    private Vector2 v2_nowPos;
    [SerializeField, Header("����(�b)")] private float limit = 60f;
    private Vector2 v2_startPos;

    private float f_limitPos = 0f;
    private float f_nowTime = 0;
    private bool b_dragMode = false;

    private PlayerController plController;

    private void Awake()
    {
        TimeData.TimeEntity.b_DragMode = b_dragMode;
        TimeData.TimeEntity.f_nowTime = f_nowTime;
    }

    void Start()
    {
        barPos = this.gameObject.GetComponent<RectTransform>();
        v2_nowPos = barPos.localPosition;
        v2_startPos = barPos.localPosition;
        f_limitPos = v2_startPos.x + (limit * 2 * TimelineData.TimelineEntity.f_oneTickWidht);
        plController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //�Đ������ǂ���
        if (GameData.GameEntity.b_playNow)
        {
            if (barPos.localPosition.x < f_limitPos)
            {
                barPos.localPosition = v2_nowPos;
                v2_nowPos.x += f_speed * Time.deltaTime;
            }
            else
            {
                //���Ԃ̌��E���̏���
                plController.PlayerStop();
            }
        }
        else
        {
            float f_distance = this.transform.localPosition.x - v2_startPos.x;
            f_nowTime = (float)Math.Truncate(f_distance / f_speed * 10) / 10;
            TimeData.TimeEntity.f_nowTime = f_nowTime;

        }

    }

    //�h���b�O�J�n����Ƃ�
    public void OnBeginDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.b_playNow)
        {
            return;
        }

        b_dragMode = true;
        TimeData.TimeEntity.b_DragMode = b_dragMode;
    }

    //�h���b�O��
    public void OnDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.b_playNow)
        {
            return;
        }

        //�}�E�X���W�擾
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)barPos.parent,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 v2_mousePos
        );

        v2_mousePos.y = v2_startPos.y;
        if (v2_mousePos.x <= v2_startPos.x)
        {
            v2_mousePos.x = v2_startPos.x;
        }
        else if(v2_mousePos.x>= f_limitPos)
        {
            v2_mousePos.x = f_limitPos;
        }
        barPos.localPosition = v2_mousePos;
        v2_nowPos = barPos.localPosition;
    }

    //�h���b�O�I����
    public void OnEndDrag(PointerEventData eventData)
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.b_playNow)
        {
            return;
        }

        b_dragMode = false;
        TimeData.TimeEntity.b_DragMode = b_dragMode;
    }

    /// <summary>
    /// �{�^�����������Ƃ��^�C���o�[�̈ʒu��0�b�̏ꏊ�ɖ߂�
    /// </summary>
    public void OnReStart()
    {
        barPos.localPosition = v2_startPos;
        v2_nowPos = v2_startPos;

    }

    /// <summary>
    /// �^�C���o�[���h���b�O�����ǂ����̔����Ԃ�
    /// </summary>
    /// <returns>true=�h���b�O���@false=�h���b�O���ĂȂ�</returns>
    public bool ReturenDragMode()
    {
        return b_dragMode;
    }
}
