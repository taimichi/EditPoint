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
    [SerializeField, Header("時間(秒)")] private float limit = 60f;
    private Vector2 v2_startPos;

    private float f_limitPos = 0f;
    private float f_nowTime = 0;
    private bool b_dragMode = false;

    private PlayerController plController;

    private void Awake()
    {
        TimeData.TimeEntity.b_DragMode = b_dragMode;
        TimeData.TimeEntity.f_nowTime = f_nowTime;

        f_limitPos = v2_startPos.x + (limit * 2 * TimelineData.TimelineEntity.f_oneTickWidht);

        transform.SetAsLastSibling();
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
        //再生中かどうか
        if (GameData.GameEntity.isPlayNow)
        {
            if (barPos.localPosition.x < f_limitPos - 1)
            {
                barPos.localPosition = v2_nowPos;
                v2_nowPos.x += f_speed * Time.deltaTime;
            }
            else
            {
                //時間の限界時の処理
                plController.PlayerStop();
                GameData.GameEntity.isLimitTime = true;
            }
        }
        else
        {
            float f_distance = this.transform.localPosition.x - v2_startPos.x;
            f_nowTime = (float)Math.Truncate(f_distance / f_speed * 10) / 10;
            TimeData.TimeEntity.f_nowTime = f_nowTime;

        }

    }

    //ドラッグ開始するとき
    public void OnBeginDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        b_dragMode = true;
        TimeData.TimeEntity.b_DragMode = b_dragMode;
    }

    //ドラッグ中
    public void OnDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        //マウス座標取得
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
        else if(v2_mousePos.x >= f_limitPos)
        {
            v2_mousePos.x = f_limitPos;
        }
        barPos.localPosition = v2_mousePos;
        v2_nowPos = barPos.localPosition;
    }

    //ドラッグ終了時
    public void OnEndDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        b_dragMode = false;
        TimeData.TimeEntity.b_DragMode = b_dragMode;
    }

    /// <summary>
    /// ボタンを押したときタイムバーの位置を0秒の場所に戻す
    /// </summary>
    public void OnReStart()
    {
        barPos.localPosition = v2_startPos;
        v2_nowPos = v2_startPos;
        GameData.GameEntity.isLimitTime = false;
    }

    /// <summary>
    /// タイムバーがドラッグ中かどうかの判定を返す
    /// </summary>
    /// <returns>true=ドラッグ中　false=ドラッグしてない</returns>
    public bool ReturnDragMode()
    {
        return b_dragMode;
    }

    /// <summary>
    /// タイムバーの限界値を返す
    /// </summary>
    /// <returns>タイムバーの限界X座標(float型)</returns>
    public float ReturnLimitPos()
    {
        return f_limitPos;
    }
}
