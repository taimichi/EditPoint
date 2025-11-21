using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool isDragging = false;
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private Canvas canvas; // UIの座標系を取得するためのCanvas
    private NewStageData stageDataScript;
    private bool isLock = true; //ロックされてるかどうか false=ロックされてる / true=ロックされてない

    public RectTransform targetImage; // 目標地点のImageのRectTransform
    public float snapDistance = 50f; // スナップする距離の閾値

    // ダブルクリックか判定する
    int clickCnt = 0;
    // 触れたかどうかの判定
    bool IsHit = false;

    // ここに表示する予定のキャンバスオブジェクトを渡す
    [SerializeField] LoadingProgressBar load;
    // ロードを表示する
    [SerializeField] GameObject LoadObj;
    // 表示するステージパネル
    [SerializeField] GameObject StagePanel;

    //ワールド番号
    [SerializeField] private int worldNum = 1;
    [SerializeField] private GameObject LockPanel;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        // 親のCanvasを取得
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvasが見つかりません！このスクリプトはCanvas内のUIオブジェクトでのみ機能します。");
        }
        Vector2 checkPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);  // チェックしたい位置
        CheckUIObjectAtPosition(checkPosition, canvas);

    }

    private void Start()
    {
        stageDataScript = NewStageData.StageEntity;
        //該当ワールドを検索
        for(int i = 0; i < stageDataScript.stageData.Length; i++)
        {
            //設定したワールド番号がステージデータのワールド番号と同じとき
            if(worldNum == stageDataScript.stageData[i].worldNum)
            {
                //ステージ番号が1の時(1は各ワールドの始めのステージ)
                if(stageDataScript.stageData[i].stageNum == 1)
                {
                    //ステージ1がロックされてたら
                    if (stageDataScript.stageData[i].stagelock == NewStageData.StageLock.Lock)
                    {
                        //ロック状態にする
                        isLock = false;
                        break;
                    }
                    else
                    {
                        //ロック状態を解除
                        isLock = true;
                    }
                }
            }
        }

        UnLockAnimation unLock = LockPanel.transform.GetComponentInChildren<UnLockAnimation>();
        unLock.GetNum(worldNum, 0);
    }

    // UIのインターフェース
    #region interface
    // UI上にカーソルが触れているか
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHit = true;
    }
    // 離れた場合
    public void OnPointerExit(PointerEventData eventData)
    {
        // 離れた場合はリセット
        clickCnt = 0;
        IsHit = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLock)
        {
            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLock)
        {
            if (isDragging && canvas != null)
            {
                // UIの座標変換を考慮して位置を更新
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 localPoint
                );

                rectTransform.anchoredPosition = localPoint;
            }
        }

    }
    #endregion

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLock)
        {
            isDragging = false;

            // 目標のImageとの距離を計算
            float distance = Vector2.Distance(rectTransform.anchoredPosition, targetImage.anchoredPosition);

            // しきい値内なら目標Imageの位置にスナップ
            if (distance < snapDistance)
            {
                rectTransform.anchoredPosition = targetImage.anchoredPosition;
                LoadObj.SetActive(true);
                load.SetObj = StagePanel;
            }
            else
            {
                rectTransform.anchoredPosition = originalPosition; // 元の位置に戻る
            }
        }

    }

    // 指定された座標が特定のRectTransform内にあるかチェック
    public static void CheckUIObjectAtPosition(Vector2 position, Canvas canvas, float tolerance = 0.1f)
    {
        // Canvas内のすべてのUIオブジェクトを取得
        RectTransform[] rectTransforms = canvas.GetComponentsInChildren<RectTransform>();

        foreach (var rectTransform in rectTransforms)
        {
            // RectTransformがスクリーン座標系に基づいているかを確認
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                position,
                canvas.worldCamera,
                out localPosition
            );

            // そのUIオブジェクトのRect内に指定位置が含まれているかチェック
            if (rectTransform.rect.Contains(localPosition))
            {
                // 位置が含まれていればオブジェクト名を出力
                Debug.Log("指定位置にオブジェクトがあります: " + rectTransform.gameObject.name);
            }
        }
    }

    // ダブルクリック処理のみ記載
    // ドラッグする予定の位置に移動すること
    private void FixedUpdate()
    {
        if (isLock)
        {
            // 触れてないなら処理なし
            if (!IsHit) { return; }

            if (Input.GetMouseButtonDown(0))
            {
                clickCnt++;
            }

            // 少なくとも二回押された時にダブルクリックとして扱う
            if (clickCnt >= 2)
            {
                rectTransform.anchoredPosition = targetImage.anchoredPosition;
                LoadObj.SetActive(true);
                load.SetObj = StagePanel;
            }
        }

    }


    /// <summary>
    /// 元の位置に戻す
    /// </summary>
    public void OriginalPos()
    {
        rectTransform.anchoredPosition = originalPosition; // 元の位置に戻る
    }
}
