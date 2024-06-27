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
        UAnim = new ClassUIAnim();
    }

    public int Statereceive
    {
        set
        {
            state = value;
        }
    }
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    state = 1;
    //    Debug.Log("触れた");
    //}

    //public void OptionGroupButton()
    //{
    //    // 既に目標にいるなら動作しない
    //    if (TargetRct.anchoredPosition == TargetPos) { return; }

    //    state = 1;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    // UIが動いてないなら動作なし
    //    if (TargetRct.anchoredPosition==startPos) { return; }
    //    state = 3;
    //    Debug.Log("離れた");
    //}

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
    void Setup()
    {
        TargetRct = UAnim.anim_Start(TargetRct, startPos);
    }
}
