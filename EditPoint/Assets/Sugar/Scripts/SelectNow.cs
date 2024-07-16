using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;    // UI
public class SelectNow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /*----Vector----*/
    Vector3 startRot = new Vector3(0, 0, 0);

    /*----int変数----*/
    int I_state = 0;  // Switch文で使う

    /*----float----*/
    const float F_rotSpd = 2.0f;
    float F_timer ;
    const float F_settimer = 2.0f;

    /*----string----*/

    /*----bool----*/
    bool B_onUI = false;

    /*----その他変数（コンポーネントとかスクリプト）----*/
    [SerializeField]
    RectTransform rct;  // 動かす対象のUI

    ClassUIAnim UAnim;  // UIのアニメーションクラス

    [SerializeField]
    GameObject Obj; // ツール機能の紹介するオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        // インスタンス生成
        UAnim = new ClassUIAnim();

        // タイマーセット
        F_timer = F_settimer;
    }

    // Update is called once per frame
    void Update()
    { 
        SelectButton();
        UIName();
    }

    void UIName()
    {
        if (!B_onUI) { return; }
        if (Obj.activeSelf == true) { return; }

        F_timer -= Time.deltaTime;
        if(F_timer<=0)
        {
            Obj.SetActive(true);
            F_timer = F_settimer;
        }
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
        B_onUI = true;
    }
    // 離れた場合
    public void OnPointerExit(PointerEventData eventData)
    {
        I_state = 0;
        F_timer = F_settimer;
        if (Obj.activeSelf == true) { Obj.SetActive(false); }
        B_onUI = false;
    }
}
