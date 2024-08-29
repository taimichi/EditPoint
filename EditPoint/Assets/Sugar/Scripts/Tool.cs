using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tool : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region field
    /*
      ツールボックスの表示非表示に合わせてサイズ変更
    */
    // Pos
    const float posYDisp = 0;
    const float posYHide = 400;
    // Height
    const float heightDisp = 900;
    const float heightHide = 50;

    // 表示状態でも非表示でも固定
    const float posX = 0;
    const float width = 400;

    // これで表示非表示の判定
    [SerializeField] Toggle toggle;

    // 対象のRect(ツールバー)
    [SerializeField] RectTransform rctTool;
    [SerializeField] RectTransform hitBox;

    // マウス座標に移動する
    [SerializeField] RectTransform rctGroup;

    // その他表示状態を変えるObj
    [SerializeField] GameObject[] obj;

    // Canvas
    [SerializeField] Canvas canvas;

    // マウスのスクリーン座標を取得
    Vector3 mouseScreenPos;

    // UIに触れたか
    private bool isCheck = false;

    // Canvas座標を求めるのに使う
    Vector2 localPoint;
    #endregion


    void Update()
    {
        DispOrHide();

        // マウス座標を求める 
        mouseScreenPos = Input.mousePosition;

        // UIを動かす処理
        if (isCheck)
        {
            if (Input.GetMouseButton(0))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform, mouseScreenPos, canvas.worldCamera, out localPoint);

                rctGroup.anchoredPosition = localPoint+new Vector2(0,-posYHide);
            }
        }
    }

    #region Method
    #region Interface
    // UI上にカーソルが触れているか
    public void OnPointerEnter(PointerEventData eventData)
    {
        isCheck = true;
    }

    // UIから離れた場合
    public void OnPointerExit(PointerEventData eventData)
    {
        isCheck = false;
    }
    #endregion

    /// <summary>
    /// Toggleにチェックが入っているかをチェック
    /// </summary>
    private bool IsOn()
    {
        return toggle.isOn;
    }

    /// <summary>
    /// Rectのサイズ変更を行う処理
    /// </summary>
    private void DispOrHide()
    {
        if(IsOn()) // 表示状態
        {
            rctTool.anchoredPosition = new Vector2(posX, posYDisp);
            rctTool.sizeDelta = new Vector2(width,heightDisp);

            hitBox.anchoredPosition = new Vector2(posX, posYDisp);
            hitBox.sizeDelta = new Vector2(width, heightDisp);

            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].SetActive(true);
            }
        }
        else // 非表示状態
        {
            rctTool.anchoredPosition = new Vector2(posX, posYHide);
            rctTool.sizeDelta = new Vector2(width, heightHide);

            hitBox.anchoredPosition = new Vector2(posX, posYHide);
            hitBox.sizeDelta = new Vector2(width, heightHide);
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].SetActive(false);
            }
        }
    }
    #endregion
}
