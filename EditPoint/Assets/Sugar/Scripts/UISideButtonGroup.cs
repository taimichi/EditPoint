using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISideButtonGroup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    int state = 0;

    private void Start()
    {
        UAnim = new ClassUIAnim();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        state = 1;
        Debug.Log("触れた");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        state = 3;
        Debug.Log("離れた");
    }

    void Update()
    {
        anim();
    }

    void anim()
    {
        switch (state)
        {
            case 0: //初期化
                Setup();
                break;

            case 1:
                if (TargetRct.anchoredPosition.x >= TargetPos.x)
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
                if (TargetRct.anchoredPosition.x <= startPos.x)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, -spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = startPos;
                    state++;
                }
                break;

        }
    }
    void Setup()
    {
        TargetRct = UAnim.anim_Start(TargetRct, startPos);
    }
}
