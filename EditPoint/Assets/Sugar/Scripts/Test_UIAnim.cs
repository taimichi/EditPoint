using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Test_UIAnim : MonoBehaviour
{
    // 動かすターゲットUI
    [SerializeField]
    RectTransform TargetRct;
    [SerializeField]
    Transform tf;

    // 初期の座標値
    [SerializeField]
    Vector2 startPos = new Vector2(0, 0);
    // 目標の座標
    [SerializeField]
    Vector2 TargetPos = new Vector2(0, 0);
    [SerializeField]
    Vector3 startRot = new Vector3(0,0,0);
    // 回転値
    [SerializeField]
    float rotX, rotY, rotZ = 0;



    // 座標変更時の速度
    [SerializeField]
    float spdX=0, spdY=0;

    // UIの幅と高さ
    [SerializeField]
    float Width=1000, Height=400;
    // スケールのサイズ変更時の速度
    [SerializeField]
    float sclX = 0, sclY = 0;

    // フェードの実装
    [SerializeField]
    Image ImgCol;
    [SerializeField]
    Text TextCol;
    // フェード速度
    [SerializeField]
    float FadeSpd = 0.05f;
    // アニメーションの状態
    [SerializeField] // デバッグ用に触れるようにする
    int state = 0;

    // 表示時間
    [SerializeField]
    float _time = 5f;
    float Settime;
    // クラス
    ClassUIAnim UAnim;
    void Start()
    {
        // インスタンス生成
        UAnim = new ClassUIAnim();
        Settime = _time;
    }

    void Update()
    {
        // アニメーション実行
        anim();
    }

    void anim()
    {
// case 0-2までがテキストボックスに使う予定のアニメーション
        switch (state)
        {
            case 0: // 初期化
                Setup();
                break;
            case 1: // 移動
                if (TargetRct.anchoredPosition.y >= TargetPos.y)
                {　
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    state++;
                }
                break;
            case 2: // 拡大
                if (TargetRct.sizeDelta.x <= Width)
                {
                    TargetRct = UAnim.anim_SclChange(TargetRct, sclX, sclY);
                }
                else
                {
                    TargetRct.sizeDelta = new Vector2(Width,Height);
                   
                    state++;
                }
                break;

            case 3: // 何も実行しない。動作を分けてるだけです
                break;

// case4-6まで画面上部にUIがぴょこんと出てくる
            case 4: // 初期化
                Setup();
                break;
            case 5: // 上にUIボタンが表示される
                if (TargetRct.anchoredPosition.y >= TargetPos.y)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    state++;
                }
                break;
                
            case 6: // 何も実行しない。動作を分けてるだけです
                break;

// case7-9までイメージとテキストの二つを使ったフェードで最初に動く
            case 7:
                Setup();
                break;
            case 8:
                if (TargetRct.anchoredPosition.x >= TargetPos.x)
                {
                    TargetRct = UAnim.anim_PosChange(TargetRct, spdX, spdY);
                }
                else
                {
                    TargetRct.anchoredPosition = TargetPos;
                    state++;
                }
                break;
            case 9:
                TimeCount();
                break;
            case 10: // フェードアウト開始
                if (TextCol.color.a >= 0.0f)
                {
                    TextCol = UAnim.anim_Fade_T(TextCol, -FadeSpd);
                    ImgCol = UAnim.anim_Fade_I(ImgCol, -FadeSpd);
                }
                else
                {
                    state++;
                }
                break;

            case 11:
                break;

// case11-13までシンプルなフェード
            case 12: // フェードイン開始
                if (TextCol.color.a <= 1.0f)
                {
                    TextCol = UAnim.anim_Fade_T(TextCol, FadeSpd);
                    ImgCol = UAnim.anim_Fade_I(ImgCol, FadeSpd);
                }
                else 
                {
                    state++;
                }
                break;
            case 13: // 表示してるよ
                TimeCount();
                break;
            case 14:
                if (TextCol.color.a >= 0.0f)
                {
                    TextCol = UAnim.anim_Fade_T(TextCol, -FadeSpd);
                    ImgCol = UAnim.anim_Fade_I(ImgCol, -FadeSpd);
                }
                else
                {
                    state++;
                }
                break;

            case 15:
                break;

            case 16:
                // 回転初期値
                tf.rotation = Quaternion.Euler(startRot.x, startRot.y, startRot.z);
                state++;
                break;
            case 17:
                if (startRot.z <= 0)
                {
                    startRot.z += rotZ;
                    UAnim.T_anim_rotation(tf, startRot.x, startRot.y, startRot.z);
                }
                else
                {
                    state++;
                }
                break;

            default:
                break;
        }
    }
    void Setup()
    {
        TargetRct = UAnim.anim_Start(TargetRct, startPos);
        state++;
    }
    void TimeCount()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            state++;
            _time = Settime;
        }
    }
}
