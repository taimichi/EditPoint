using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// このスクリプトではクリアキャンバスの
// タイミングを指定する
public class ClearManager : MonoBehaviour
{
    #region field
    // クラス
    ClassUIAnim UAnim;

    // フェードをかける
    [SerializeField]
    Image ClearPanel;

    [SerializeField]
    RectTransform BtnGroup;

    // 初期の座標値
    [SerializeField]
    Vector2 startPos = new Vector2(0, 0);

    // 目標の座標
    [SerializeField]
    Vector2 TargetPos = new Vector2(0, 0);

    [SerializeField]
    GameObject panel;

    float sizeA=0;

    // 座標変更時の速度
    [SerializeField]
    float spdX = 0, spdY = 0;

    // フェード速度
    [SerializeField]
    float Fadespd=0.1f;

    // 処理の小分けするため
    int num = 0;
    #endregion
    void Start()
    {
        // インスタンス生成
        UAnim = new ClassUIAnim();
    }

    void Update()
    {
        UICONTROLL();
    }
    void UICONTROLL()
    {
        switch (num)
        {
            case 0:
                // 初期座標へ
                BtnGroup.anchoredPosition = startPos;
                num++;
                break;
            case 1:
                if (ClearPanel.color.a < 0.9f)
                {
                    ClearPanel = UAnim.anim_Fade_I(ClearPanel, Fadespd);
                    sizeA += 200;
                    ClearPanel.rectTransform.sizeDelta = new Vector2(sizeA,sizeA);
                }
                else
                    num++;
                break;
            case 2:
                if (BtnGroup.anchoredPosition.y <= TargetPos.y)
                    BtnGroup = UAnim.anim_PosChange(BtnGroup, spdX, spdY);
                else
                {
                    BtnGroup.anchoredPosition = TargetPos;
                    Destroy(panel);
                    num++;
                }
                break;
        }

    }
}
