using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{

    [SerializeField] private ScrollRect scrollRect; 
    [SerializeField] private RectTransform content; 
    [SerializeField] private RectTransform target;
    [SerializeField] private float scrollAmount_auto = 0.022789f;    //移動量(0～1)
    private float scrollAmount_manual = 0.005f;

    [SerializeField] private RectTransform viewport;

    private void Start()
    {
        scrollAmount_auto = Screen.width / content.rect.width;
    }

    void Update()
    {

        // ターゲットのUIオブジェクトの座標を取得
        Vector3[] targetCorners = new Vector3[4];
        target.GetWorldCorners(targetCorners);

        // ScrollViewのViewportの座標を取得
        Vector3[] viewportCorners = new Vector3[4];
        scrollRect.viewport.GetWorldCorners(viewportCorners);

        if (!TimeData.TimeEntity.b_DragMode)
        {
            // 右端の判定
            if (targetCorners[2].x > viewportCorners[2].x)
            {
                ScrollToPositionRight(scrollAmount_auto); // 右にスクロール
            }
        }
        else
        {
            // 右端の判定
            if (targetCorners[2].x > viewportCorners[2].x)
            {
                ScrollToPositionRight(scrollAmount_manual); // 右にスクロール
            }
            else if (targetCorners[1].x < viewportCorners[1].x)
            {
                ScrollToPositionLeft(scrollAmount_manual); //左にスクロール
            }

        }

    }

    // 右にスクロール
    private void ScrollToPositionRight(float normalizedPosition)
    {
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + normalizedPosition);
    }

    //左にスクロール
    private void ScrollToPositionLeft(float normalizedPosition)
    {
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition - normalizedPosition);
    }

}
