using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// このスクリプトではクリアキャンバスの
// タイミングを指定する
public class ClearManager : MonoBehaviour
{
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

    // 座標変更時の速度
    [SerializeField]
    float spdX = 0, spdY = 0;

    // フェード速度
    [SerializeField]
    float Fadespd=0.1f;

    int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        // インスタンス生成
        UAnim = new ClassUIAnim();
    }

    // Update is called once per frame
    void Update()
    {
        UICONTROLL();
    }
    void UICONTROLL()
    {
        switch (state)
        {
            case 0:
                // 初期座標へ
                BtnGroup.anchoredPosition = startPos;
                state++;
                break;
            case 1:
                if (ClearPanel.color.a < 0.5f)
                    ClearPanel = UAnim.anim_Fade_I(ClearPanel, Fadespd);
                else
                    state++;
                break;
            case 2:
                if (BtnGroup.anchoredPosition.x >= TargetPos.x)
                    BtnGroup = UAnim.anim_PosChange(BtnGroup, spdX, spdY);
                else
                {
                    BtnGroup.anchoredPosition = TargetPos;
                    Destroy(panel);
                    state++;
                }
                break;
        }

    }
}
