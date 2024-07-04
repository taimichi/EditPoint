using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISideButtonGroup : MonoBehaviour//IPointerExitHandler //IPointerEnterHandler 
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

    // クラス
    ClassUIAnim UAnim;

    // アニメーションの状態
    [SerializeField] // デバッグ用に触れるようにする
    int state;

    private void Start()
    {
        // インスタンス生成
        UAnim = new ClassUIAnim();
    }

    // UIを動かすタイミングの受け取り
    public int Statereceive
    {
        set
        {
            state = value;
        }
    }

    void Update()
    {
        UICONTROLL();
    }

    // UIを動かす
    void UICONTROLL()
    {
        switch (state)
        {
            case 0: //初期化
                Setup();
                break;

            case 1:
                if (TargetRct.anchoredPosition.y >= TargetPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    state++; //　処理の終了
                }
                break;

            case 3:
                if (TargetRct.anchoredPosition.y <= startPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, -spdX, -spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = startPos;
                    state++;
                }
                break;

        }
    }

    // 初期化用の関数
    void Setup()
    {
        TargetRct = UAnim.anim_Start(TargetRct, startPos);
    }
}
