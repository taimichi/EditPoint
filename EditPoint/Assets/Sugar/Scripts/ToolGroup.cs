using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGroup : MonoBehaviour
{
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

    // アニメーション状態
    int state = 3;
   
    // クラス
    ClassUIAnim UAnim;

    bool click = false;
    void Start()
    {
        // インスタンス生成
        UAnim = new ClassUIAnim();
    }

    private void Update()
    {
        if (click)
        {
            UICONTROLL();
        }
        Debug.Log(state);
    }

    void UICONTROLL()
    {
        switch (state)
        {
            case 1: // グループ非表示
                if (TargetRct.anchoredPosition.y <= startPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, -spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = startPos;
                    state=10; //　処理の終了
                }
                break;
            case 3: // グループ表示
                if (TargetRct.anchoredPosition.y >= TargetPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    state=10; //　処理の終了
                }
                break;
            case 10:
                click = false;
                state = TargetRct.anchoredPosition == startPos ? 3 : 1;
                break;
        }

    }

    public void BUTTONCHECK()
    {
        click = true;
    }
}
