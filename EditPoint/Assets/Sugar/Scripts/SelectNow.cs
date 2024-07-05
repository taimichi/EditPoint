using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;    // UI
public class SelectNow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /*----Vector----*/
    Vector3 startRot = new Vector3(0, 0, 0);

    /*----int変数----*/
    int I_state = 0;  // Switch文で使う

    /*----float----*/
    const float F_rotSpd = 2;

    /*----その他変数（コンポーネントとかスクリプト）----*/
    [SerializeField]
    RectTransform rct;  // 動かす対象のUI

    ClassUIAnim UAnim;  // UIのアニメーションクラス

    // Start is called before the first frame update
    void Start()
    {
        // インスタンス生成
        UAnim = new ClassUIAnim();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(I_state);
        SelectButton();
    }

    void SelectButton()
    {
        switch (I_state)
        {
            case 0: // ここは初期に戻す状態
                // 選択が外れた場合にここの処理
                rct.rotation = Quaternion.Euler(0, 0, 0);
                startRot.z = 0;
                break;

            // １と２の処理を交互に動作させる
            case 1: // 左回転
                if (startRot.z <= 30)
                {
                    startRot.z += F_rotSpd;
                    rct = UAnim.R_anim_rotation(rct, 0, 0, startRot.z);
                }
                else
                {
                    I_state++;
                }
                break;
            case 2: // 右回転
                if (startRot.z >= -30)
                {
                    startRot.z -= F_rotSpd;
                    rct = UAnim.R_anim_rotation(rct, 0, 0, startRot.z);
                }
                else
                {
                    I_state--;
                }
                break;
        }

    }

    // UI上にカーソルが触れているか
    public void OnPointerEnter(PointerEventData eventData)
    {
        I_state++;
    }
    // 離れた場合
    public void OnPointerExit(PointerEventData eventData)
    {
        I_state = 0;
    }
}
