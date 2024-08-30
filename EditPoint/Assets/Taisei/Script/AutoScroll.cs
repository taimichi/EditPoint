using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect; 
    [SerializeField] private RectTransform content; 
    [SerializeField] private RectTransform target;
    [SerializeField] private float scrollAmount = 0.022789f;    //移動量(0～1)

    void Update()
    {
        // ターゲットのUIオブジェクトの座標を取得
        Vector3[] targetCorners = new Vector3[4];
        target.GetWorldCorners(targetCorners);

        // ScrollViewのViewportの座標を取得
        Vector3[] viewportCorners = new Vector3[4];
        scrollRect.viewport.GetWorldCorners(viewportCorners);

        // 右端の判定
        if (targetCorners[2].x > viewportCorners[2].x)
        {
            ScrollToPosition(scrollAmount); // 右にスクロール
        }
    }

    // スクロール位置を設定するメソッド
    private void ScrollToPosition(float normalizedPosition)
    {
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + normalizedPosition);
    }

}
