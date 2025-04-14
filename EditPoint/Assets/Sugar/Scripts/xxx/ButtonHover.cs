using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;    // UI

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ボタンUIにマウスカーソルが触れているか
    bool onUI = false;

    // UIを動かすクラス
    ClassUIAnim moveUI;

    // サイズ変更
    float sclX=1, sclY = 1;
    // 変更速度
    float sclSpd = 0.02f;
    // 目標
    float sclGoalMax = 1.3f;
    float sclGoalMin = 1;
    [SerializeField] RectTransform myRct;

    // Switch処理
    enum numUI
    {
        sclUP,
        sclDown,
        end
    }

    numUI num;
    void Start()
    {
        // インスタンス生成
        moveUI = new ClassUIAnim();

        num = numUI.sclUP;
    }

    void Update()
    {
        if (onUI)
        {
            SCL();
            myRct.localScale = new Vector2(sclX, sclY);
        }
        else
        {
            myRct.localScale = new Vector2(sclGoalMin, sclGoalMin);
        }
    }
    void SCL()
    {
        switch(num)
        {
            case numUI.sclUP: // スケールアップ
                if (sclX <= sclGoalMax || sclY <= sclGoalMax)
                {
                    sclX += sclSpd;
                    sclY += sclSpd;
                }
                else
                {
                    sclX = sclGoalMax;
                    sclY = sclGoalMax;

                    num = numUI.sclDown;
                }
                break;
            case numUI.sclDown: // スケールダウン
                if (sclX >= sclGoalMin || sclY >= sclGoalMin)
                {
                    sclX -= sclSpd;
                    sclY -= sclSpd;
                }
                else
                {
                    sclX = sclGoalMin;
                    sclY = sclGoalMin;

                    num = numUI.sclUP;
                }
                break;
        }
    }
    #region Method_IPointer
    // UI上にカーソルが触れているか
    public void OnPointerEnter(PointerEventData eventData)
    {
        onUI = true;
    }
    // 離れた場合
    public void OnPointerExit(PointerEventData eventData)
    {
        onUI = false;
    }
    #endregion

    /// <summary>
    /// 外部から強制的に動きを終了する
    /// </summary>
    public void ResetButton()
    {
        onUI = false;
    }
}
