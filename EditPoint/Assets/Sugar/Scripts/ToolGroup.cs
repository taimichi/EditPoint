using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGroup : MonoBehaviour
{
    enum SetUI
    {
        display,
        hide,
        CHECK
    }

    // 動かすターゲットUI
    [SerializeField]
    RectTransform TargetRct;

    // 初期の座標値
    [SerializeField]
    Vector2 startPos = new Vector2(0, 0);

    // 目標の座標
    [SerializeField]
    Vector2 TargetPos = new Vector2(0, 0);

    // 座標変更時の速度
    [SerializeField]
    float spdX = 0, spdY = 0;

    // 対象のRectの位置から表示するのか非表示にするのかチェックから
    SetUI set =SetUI.CHECK ;
   
    // クラス
    ClassUIAnim UAnim;

    bool isClick = false;
    void Start()
    {
        // インスタンス生成
        UAnim = new ClassUIAnim();

        set = TargetRct.anchoredPosition == startPos ? SetUI.hide : SetUI.display;
    }

    private void Update()
    {
        if (isClick)
        {
            UICONTROLL();
        }
    }

    void UICONTROLL()
    {
        switch (set)
        {
            case SetUI.display: // グループ表示
                if (TargetRct.anchoredPosition.y >= startPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, -spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = startPos;
                    set=SetUI.CHECK; //　処理の終了
                }
                break;
            case SetUI.hide: // グループ非表示
                if (TargetRct.anchoredPosition.y <= TargetPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    set = SetUI.CHECK; //　処理の終了
                }
                break;
            case SetUI.CHECK:
                isClick = false;

                // 次にボタンが押されたときに表示するのか
                // 表示/非表示にするのかチェックしてセット
                set = TargetRct.anchoredPosition == startPos ? SetUI.hide : SetUI.display;
                break;
        }

    }

    /// <summary>
    /// ボタン入力チェック
    /// </summary>
    public void BUTTONCHECK()
    {
        isClick = true;
    }
}
