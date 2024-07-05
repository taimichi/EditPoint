using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelection : MonoBehaviour
{
    //初期のステータス
    [SerializeField, Header("範囲選択オブジェクトの初期スケール")] private Vector3 v3_initScale;
    //現在のスケール
    Vector3 currentScale;

    [SerializeField] private Renderer targetRenderer;
    private Vector3 v3_newTopLeft;
    private Vector3 v3_newBottomRight;

    private Vector3 v3_StartMousePos;
    private Vector3 v3_StartScrWldPos;

    private Vector3 v3_nowMousePos;
    private Vector3 v3_nowScrWldPos;

    float f_newWidth;
    float f_newHeight;

    private Vector3 v3_newScale;

    private bool b_selectMode = false;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            b_selectMode = true;
            v3_StartMousePos = Input.mousePosition;
            v3_StartMousePos.z = 10;
            v3_StartScrWldPos = Camera.main.ScreenToWorldPoint(v3_StartMousePos);
            v3_newTopLeft = v3_StartScrWldPos;
        }

        if (Input.GetMouseButton(0) && b_selectMode)
        {
            v3_nowMousePos = Input.mousePosition;
            v3_nowMousePos.z = 10;
            v3_nowScrWldPos = Camera.main.ScreenToWorldPoint(v3_nowMousePos);
            v3_nowScrWldPos.x += 0.01f;
            v3_nowScrWldPos.y += 0.01f;
            v3_newBottomRight = v3_nowScrWldPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            b_selectMode = false;
            targetRenderer.transform.localScale = v3_initScale;
        }

        if (b_selectMode)
        {
            // 新しい幅と高さを計算
            f_newWidth = v3_newBottomRight.x - v3_newTopLeft.x;
            f_newHeight = v3_newTopLeft.y - v3_newBottomRight.y;

            // 現在のスケールを取得
            currentScale = targetRenderer.transform.localScale;
            currentScale.z = 1;

            // 新しいスケールを計算
            v3_newScale.x = f_newWidth / targetRenderer.bounds.size.x;
            v3_newScale.y = f_newHeight / targetRenderer.bounds.size.y;
            v3_newScale.z = 1;

            // スケールを変更
            targetRenderer.transform.localScale = new Vector3(
                currentScale.x * v3_newScale.x,
                currentScale.y * v3_newScale.y,
                currentScale.z * v3_newScale.z
            );

            // 新しい中心位置を計算
            Vector3 newPosition = new Vector3((v3_newTopLeft.x + v3_newBottomRight.x) / 2, (v3_newTopLeft.y + v3_newBottomRight.y) / 2, (v3_newTopLeft.z + v3_newBottomRight.z) / 2);
            targetRenderer.transform.position = newPosition;

        }
    }
}
