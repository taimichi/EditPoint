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

        TimeLineCanvas = this.transform.root.gameObject;    //タイムラインキャンバス(一番上の親オブジェクト)取得

        UICamera = TimeLineCanvas.GetComponent<Canvas>().worldCamera;   //タイムラインキャンバスに設定されてるカメラを取得
        raycaster = TimeLineCanvas.GetComponent<GraphicRaycaster>();

        quickGuide = GameObject.Find("GuideCanvas").GetComponent<QuickGuideMenu>();
    }

    private void Update()
    {
        //再生中
        if (GameData.GameEntity.isPlayNow)
        {
            //時間内の時かつステージがクリアされてないとき
            if (barPos.localPosition.x < limitPosX && !GameData.GameEntity.isClear)
            {
                barPos.localPosition = nowPos;
                nowPos.x += speed * Time.deltaTime;
            }
            //最後まで再生した時かステージがクリアされたとき
            else
            {
                //時間の限界時の処理
                plController.PlayerStop();
                GameData.GameEntity.isLimitTime = true;
            }
        }
        //再生してないとき
        else
        {
            //タイムバーの開始位置から現在の位置の距離を求める
            float f_distance = this.transform.localPosition.x - startPos.x;
            //タイムバーのある位置の時間を求める
            nowTime = (float)Math.Truncate(f_distance / speed * 10) / 10;
            //現在の時間を
            TimeData.TimeEntity.nowTime = nowTime;

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

        isDragMode = true;
        TimeData.TimeEntity.isDragMode = isDragMode;
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

    //ドラッグ終了時
    public void OnEndDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        isDragMode = false;
        TimeData.TimeEntity.isDragMode = isDragMode;
    }

    /// <summary>
    /// ボタンを押したときタイムバーの位置を0秒の場所に戻す
    /// </summary>
    public void OnReStart()
    {
        barPos.localPosition = startPos;
        nowPos = startPos;
        GameData.GameEntity.isLimitTime = false;
    }

    /// <summary>
    /// タイムバーがドラッグ中かどうかの判定を返す
    /// </summary>
    /// <returns>true=ドラッグ中　false=ドラッグしてない</returns>
    public bool ReturnDragMode() => isDragMode;

    /// <summary>
    /// タイムバーの限界値を返す
    /// </summary>
    /// <returns>タイムバーの限界X座標(float型)</returns>
    public float ReturnLimitPos() => limitPosX;
}
