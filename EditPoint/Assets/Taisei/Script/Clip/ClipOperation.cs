using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Pixeye.Unity;
using System.Collections;
using System.Collections.Generic;

//クリップの位置やサイズ、合成などの操作関連
public class ClipOperation : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Foldout("Start"), SerializeField, Header("クリップの長さ(秒)")]
    private float startLength = 5f;

    [SerializeField] private RectTransform targetImage;  //クリップ画像のRectTransform
    private Vector2 biginSizeDelta;             //変更前のクリップの画像サイズ
    private Vector2 beginMouse_LocalPos;        //クリックしたときのローカル座標

    private Vector2 resizeOffset;               //変更前と変更後のサイズ差
    private Vector2 moveOffset;                 //クリップの中心座標からマウス座標の差

    private bool isResizeRight;  // 右側をリサイズ中かどうかのフラグ

    //位置を動かさず、Pivotのみを変更するため用の変数
    private Vector2 size;
    private Vector2 deltaPivot;
    private Vector3 deltaPos;

    private Vector2 startSize;  //初期サイズ

    [SerializeField, Header("クリップの最小サイズ")] private float minWidth = 350;
    [SerializeField, Header("クリップの最大サイズ")] private float maxWidth = 1400;
    private float newWidth;         //新しいクリップの長さ

    [SerializeField, Header("サイズ変更を受け付ける範囲(左右共通)")] private float edgeRange = 10f;

    private float dotMove = 0;
    private float onetick;            //サイズ変更時、1回にサイズ変更する量

    private Vector2 NowMouse_LocalPos;        //マウスの座標

    private float dotPosX = 0f;     //ドット単位移動用　X座標
    private float newPosX;          //新しいX座標
    private float dotPosY = 0f;     //ドット単位移動用　Y座標
    private float newPosY;          //新しいY座標

    private float oneWidth;           //移動時、一回に移動する量　横
    private float oneHeight;          //移動時、一回に移動する量　縦

    private int resizeCount = 0;        //元のサイズからなんマス分変更されたか

    private RectTransform rect_UpLeft;    //タイムラインの左上
    private RectTransform rect_DownRight;   //タイムラインの右下

    private GameObject[] Clips;
    private RectTransform[] ClipsRect;

    private Vector3 startPos;   //初期位置

    //クリップが重なっているか計算する用
    private CheckOverlap checkOverlap = new CheckOverlap();

    //機能ロックスクリプト
    private FunctionLookManager functionLook;

    /// <summary>
    /// クリップ操作の種類
    /// </summary>
    private enum CLIP_MODE
    {
        /// <summary>
        /// 何もしていない、ノーマル
        /// </summary>
        normal,
        /// <summary>
        /// サイズ変更操作
        /// </summary>
        resize,
        /// <summary>
        /// 移動操作
        /// </summary>
        move,
    }
    //現在のクリップ操作の状態
    private CLIP_MODE mode = CLIP_MODE.normal;

    private PlaySound playSound;

    //クリップがタイムラインの外に出たか
    private bool isOut = false;

    [SerializeField, Header("干渉可能かどうか false=可能 / true=不可")] private bool isLook = false;

    #region ClipSprite
    [Foldout("Sprite"), SerializeField] private Sprite ActiveClipSprite;    //操作可能時のクリップのスプライト
    [Foldout("Sprite"), SerializeField] private Sprite NoActiveClipSprite;  //操作不可能事のクリップのスプライト
    #endregion

    private Image ClipImage;    //クリップのImage

    private SelectYesNo select_ClipCombine;

    private void Awake()
    {
        //リサイズ用
        onetick = TimelineData.TimelineEntity.oneResize;
        //初期の長さ
        targetImage.sizeDelta = new Vector2(
            startLength * onetick * 2, targetImage.sizeDelta.y
            );
    }

    private void Start()
    {
        //リサイズ用
        onetick = TimelineData.TimelineEntity.oneResize;

        //クリップ移動用
        oneWidth = TimelineData.TimelineEntity.oneTickWidth;
        oneHeight = TimelineData.TimelineEntity.oneTickHeight;

        //タイムラインの端のRectTransform取得
        rect_UpLeft = GameObject.Find("UpLeftOutLine").GetComponent<RectTransform>();
        rect_DownRight = GameObject.Find("DownRightOutLine").GetComponent<RectTransform>();

        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

        functionLook = GameObject.FindWithTag("GameManager").GetComponent<FunctionLookManager>();

        select_ClipCombine = GameObject.Find("Selects").GetComponent<SelectYesNo>();
        select_ClipCombine.SelectPanelActive(false);

        //クリップの位置を調整
        CalculationWidth(targetImage.localPosition.x);
        CalculationHeight(targetImage.localPosition.y);
        CheckWidth();
        CheckHeight();
        //位置を設定
        targetImage.localPosition = new Vector3(newPosX, newPosY, 0);
        //開始サイズを取得
        startSize = targetImage.sizeDelta;
        //サイズを設定
        targetImage.sizeDelta = new Vector2(startSize.x, startSize.y);

        //子オブジェクトの順番を変更
        int childNum = targetImage.parent.transform.childCount;
        transform.SetSiblingIndex(childNum - 3);

        //クリップの画像を取得
        ClipImage = this.gameObject.GetComponent<Image>();

        //クリップの画像を変更
        if (!isLook)
        {
            //可動クリップの画像に変更
            ClipImage.sprite = ActiveClipSprite;
        }
        else
        {
            //非可動クリップの画像に変更
            ClipImage.sprite = NoActiveClipSprite;
        }

        //作成したばっかのクリップの時
        if (this.gameObject.tag == "CreateClip")
        {
            //全クリップを取得
            GetAllClipRect();
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                //他のクリップと重なった場合
                if (checkOverlap.IsOverlap(targetImage, ClipsRect[i]))
                {
                    //重なったクリップの下の段に移動
                    newPosY = ClipsRect[i].localPosition.y - oneHeight;
                    CheckHeight();

                    //クリップが一番下で重なった場合
                    if (ClipsRect[i].localPosition.y <= rect_DownRight.localPosition.y)
                    {
                        Debug.Log("一番下");
                        newPosY = 0 * oneHeight - 15f;

                        //重なったクリップの右端の座標を取得
                        float rightEdge = ClipsRect[i].anchoredPosition.x + (ClipsRect[i].rect.width * (1 - ClipsRect[i].pivot.x));
                        //新たなx座標を取得
                        newPosX = rightEdge + oneWidth;

                        //再度計算し、座標を調整
                        CalculationWidth(newPosX);
                        CalculationHeight(newPosY);

                        //座標更新
                        targetImage.localPosition = new Vector3(newPosX, newPosY, 0);

                        for(int j = 0; j < 5 /*タイムラインのレイヤー数*/ ; j++)
                        {
                            //まだ他のクリップと重なった場合
                            if (checkOverlap.IsOverlap(targetImage, ClipsRect[j]))
                            {
                                //新たなy座標を設定
                                newPosY -= oneHeight;
                                //座標更新
                                targetImage.localPosition = new Vector3(newPosX, newPosY, 0);
                            }
                        }
                    }
                    //座標更新
                    targetImage.localPosition = new Vector3(newPosX, newPosY, 0);
                }
            }
            //タグ変更
            this.gameObject.tag = "SetClip";
        }
        //開始時の座標を取得
        startPos = this.transform.localPosition;
        isOut = false;
    }

    private void Update()
    {
        GetAllClipRect();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow || isLook)
        {
            return;
        }

        if ((functionLook.FunctionLook & LookFlags.ClipAccess) == 0)
        {
            startPos = this.transform.localPosition;

            //変更前のクリップのサイズを取得
            biginSizeDelta = targetImage.sizeDelta;
            //マウスカーソルの座標を取得し、ローカル座標に直す
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetImage,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localMousePos
            );

            // マウス位置が画像の右端か左端かをチェック
            if (Mathf.Abs(localMousePos.x - (-targetImage.rect.width * targetImage.pivot.x)) <= edgeRange)
            {
                // 左端
                //クリップのピボットを変更
                SetPivot(targetImage, new Vector2(1, 0.5f));
                //左側変更フラグに
                isResizeRight = false;
                mode = CLIP_MODE.resize;
            }
            else if (Mathf.Abs(localMousePos.x - (targetImage.rect.width * (1 - targetImage.pivot.x))) <= edgeRange)
            {
                // 右端
                //クリップのピボットを変更
                SetPivot(targetImage, new Vector2(0, 0.5f));
                //右側変更フラグに
                isResizeRight = true;
                mode = CLIP_MODE.resize;
            }
            else
            {
                // 端以外の場合はリサイズを無効化、クリップ移動モードにする
                mode = CLIP_MODE.move;
                //ピボットを通常の状態に
                SetPivot(targetImage, new Vector2(0, 0.5f));
                //マウスカーソルとクリップの中心位置の距離を取得
                moveOffset.x = targetImage.position.x - localMousePos.x;
            }

            //クリップサイズ変更モードの時
            if (mode == CLIP_MODE.resize)
            {
                //マウスカーソルの座標を取得し、ローカル座標に直す
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
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow || isLook)
        {
            return;
        }

        //クリップ操作が禁止されていないとき
        if ((functionLook.FunctionLook & LookFlags.ClipAccess) == 0)
        {
            //スクリーン座標をRectTransform上のローカル座標に変換
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (
                    (RectTransform)targetImage.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out NowMouse_LocalPos
                );

            //クリップ移動状態の時
            if (mode == CLIP_MODE.move)
            {
                if (targetImage == null)
                {
                    mode = CLIP_MODE.normal;
                    return;
                }

                //ドット移動用
                CalculationWidth(NowMouse_LocalPos.x + moveOffset.x);
                CalculationHeight(NowMouse_LocalPos.y);

                //タイムラインの範囲外に出た時
                CheckWidth();
                CheckHeight();

                //位置更新
                targetImage.localPosition = new Vector3(newPosX, newPosY, 0);

                return;
            }

            //クリップサイズ変更の時
            if(mode == CLIP_MODE.resize)
            {
                //変更前と変更後のマウスの移動量を計算
                resizeOffset = NowMouse_LocalPos - beginMouse_LocalPos;

                //クリップの右端のとき
                if (isResizeRight)
                {
                    newWidth = biginSizeDelta.x + resizeOffset.x;
                }
                //クリップの左端のとき
                else
                {
                    newWidth = biginSizeDelta.x - resizeOffset.x;
                }

                //変更するサイズ調整
                CalculationSize();

                //クリップの長さ変更の際に最大・最小サイズを超えないようにする
                newWidth = Mathf.Clamp(newWidth, minWidth, maxWidth);

                //タイムラインの左端、右端を超えるとき
                if (targetImage.position.x > rect_UpLeft.position.x
                    || targetImage.position.x < rect_DownRight.position.x)
                {
                    if (resizeCount == 0)
                    {
                        resizeCount = 1;
                    }

                    //リサイズ前が大きい場合
                    if (targetImage.sizeDelta.x > newWidth)
                    {
                        resizeCount--;
                    }
                    //リサイズ前が小さい場合
                    else if (targetImage.sizeDelta.x < newWidth)
                    {
                        resizeCount++;
                    }
                }

                //クリップのサイズを変更
                targetImage.sizeDelta = new Vector2(newWidth, targetImage.sizeDelta.y);
            }

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow || isLook)
        {
            return;
        }

        if ((functionLook.FunctionLook & LookFlags.ClipAccess) == 0)
        {
            //クリップがタイムラインの外に出た時
            if (isOut)
            {
                targetImage.localPosition = startPos;
                isOut = false;
            }

            //ピボットを初期のものに
            SetPivot(targetImage, new Vector2(0, 0.5f));
            GetAllClipRect();
            //重なったかどうか
            for (int i = 0; i < ClipsRect.Length; i++)
            {
                //重なった時
                if (checkOverlap.IsOverlap(targetImage, ClipsRect[i]))
                {
                    //同じオブジェクトじゃないとき
                    if (targetImage.name != Clips[i].name)
                    {
                        Debug.Log("重なった");

                        //重なったクリップが干渉不能なクリップじゃないとき
                        ClipOperation overRapClip = Clips[i].GetComponent<ClipOperation>();
                        if (!overRapClip.CheckIsLook())
                        {
                            //クリップを合成するかどうか
                            StartCoroutine(CheckClipCombine(Clips[i]));
                        }
                        else
                        {
                            //元の位置に戻す
                            targetImage.localPosition = startPos;
                        }
                        break;
                    }
                }
            }
            
            if (mode != CLIP_MODE.normal)
            {
                playSound.PlaySE(PlaySound.SE_TYPE.objMove);
                //クリップがタイムラインの左端を超えてる時
                if (targetImage.localPosition.x < rect_UpLeft.localPosition.x)
                {
                    Debug.Log("左重なった");
                    //サイズ変更による場合
                    if (mode == CLIP_MODE.resize)
                    {
                        ReCalculationSize();
                    }
                }
                //クリップがタイムラインの右端を超えてる時
                else if (targetImage.localPosition.x + targetImage.sizeDelta.x > rect_DownRight.localPosition.x)
                {
                    Debug.Log("右重なった");
                    //サイズ変更による場合
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
    /// クリップを合成するかしないかを決める
    /// 引数には重なったクリップを入れる
    /// </summary>
    IEnumerator CheckClipCombine(GameObject _clip)
    {
        select_ClipCombine.SelectPanelActive(true);
        //選択がされるまでストップ
        yield return new WaitUntil(() => select_ClipCombine.ReturnOnClick() == true);

        //選択されたら↓

        //合成するかどうか
        //いいえのとき
        if (!select_ClipCombine.ReturnSelect())
        {
            Debug.Log("元に戻すよ");
            targetImage.localPosition = startPos;
        }
        //はいのとき
        else
        {
            Debug.Log("合成するよ");
            ClipCombine(_clip);
        }

        select_ClipCombine.SelectPanelActive(false);
    }

    /// <summary>
    /// クリップを合成
    /// </summary>
    private void ClipCombine(GameObject _clip)
    {
        //重なったクリップ
        ClipPlay overRapClip = _clip.GetComponent<ClipPlay>();

        //このクリップ
        ClipPlay thisClip = this.gameObject.GetComponent<ClipPlay>();
        //このクリップに紐づいているオブジェクトを取得
        List<GameObject> connectObj = thisClip.ReturnConnectObj();
        
        //重なったクリップに現在持っているクリップに紐づけられたオブジェクトを移す
        for(int i = 0; i < connectObj.Count; i++)
        {
            overRapClip.OutGetObj(connectObj[i]);
        }

        //このクリップを削除
        Destroy(this.gameObject);

    }

    /// <summary>
    /// 引数のRectTransformのPivotのみを変更
    /// (Pivotのみを直接変更すると画像の位置が変わるため)
    /// </summary>
    /// <param name="rectTransform">変更したいPivotのRectTransform</param>
    /// <param name="pivot">変更後のPivotの値</param>
    private void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        //変更したいRectTransformのサイズを取得
        size = rectTransform.rect.size;
        //ピボットの変更前と変更後の差分
        deltaPivot = rectTransform.pivot - pivot;
        //変更前の変更後の座標の差分
        deltaPos = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);

        //新たなピボットを設定
        rectTransform.pivot = pivot;
        //ピボット変更による座標ずれを修正
        rectTransform.localPosition -= deltaPos * rectTransform.localScale.x;
    }

    /// <summary>
    /// サイズ変更するための計算
    /// </summary>
    private void CalculationSize()
    {
        dotMove = (float)Math.Round(newWidth / onetick);
        newWidth = dotMove * onetick;
    }

    /// <summary>
    /// クリップが画面外に行った際の処理
    /// </summary>
    private void ReCalculationSize()
    {
        dotMove -= resizeCount;
        newWidth = dotMove * onetick;
        targetImage.sizeDelta = new Vector2(newWidth, targetImage.sizeDelta.y);
    }


    /// <summary>
    /// X座標の計算用
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
        //計算にある 30 はタイムラインの枠の分
    }

    /// <summary>
    /// Y座標の計算用
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
        //計算にある 15 はタイムラインの枠の分
    }

    /// <summary>
    /// クリップがタイムラインの左右の範囲外に出た時
    /// </summary>
    private void CheckWidth()
    {
        //左側
        if (targetImage.localPosition.x < rect_UpLeft.localPosition.x)
        {
            newPosX = rect_UpLeft.localPosition.x;
            isOut = true;
        }
        //右側
        else if (targetImage.localPosition.x + targetImage.sizeDelta.x > rect_DownRight.localPosition.x)
        {
            newPosX = rect_DownRight.localPosition.x - targetImage.sizeDelta.x;
            isOut = true;
        }
        else
        {
            //Debug.Log("左右超えてない");
        }
    }

    /// <summary>
    /// クリップがタイムラインの上下の範囲外に出た時
    /// </summary>
    private void CheckHeight()
    {
        //上
        if (targetImage.localPosition.y > rect_UpLeft.localPosition.y)
        {
            newPosY = rect_UpLeft.localPosition.y;
            isOut = true;
        }
        //下
        else if (targetImage.localPosition.y < rect_DownRight.localPosition.y)
        {
            newPosY = rect_DownRight.localPosition.y;
            isOut = true;
        }
        else
        {
            //Debug.Log("上下超えてない");
        }
    }

    /// <summary>
    /// クリップがロックされているかどうか
    /// </summary>
    /// <returns>false=ロックされていない / true=ロックされている</returns>
    public bool CheckIsLook() => isLook;

    /// <summary>
    /// 全クリップを取得
    /// </summary>
    private void GetAllClipRect()
    {
        //クリップをGameObject型で取得
        Clips = GameObject.FindGameObjectsWithTag("SetClip");

        //クリップのRectTransformを取得
        ClipsRect = new RectTransform[Clips.Length];
        for(int i = 0; i < Clips.Length; i++)
        {
            ClipsRect[i] = Clips[i].GetComponent<RectTransform>();
        }
    }
}
