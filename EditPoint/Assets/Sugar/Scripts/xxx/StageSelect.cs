using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelect : MonoBehaviour
{
    // Rectを動かす時の判断用
    enum RectMove
    {
        rightFeed, // 右送り処理
        leftFeed,  // 左送り処理
        etc        // 終了処理
    }

// 変数
#region variable
    // インスペクター側で設定するもの
#region Inspector
    // FadeCanvas
    // Textで今何ページか分かりやすくする
    [SerializeField] 
    Text page;

    [SerializeField]
    Fade fade;

    // 動かす対象
    [SerializeField]
    RectTransform[] targetRct;

    // 初期の座標値
    // 中央、左、右
    [SerializeField]
    Vector2 CenterStartPos = new Vector2(0, 0);
    [SerializeField]
    Vector2 LeftStartPos = new Vector2(0, 0);
    [SerializeField]
    Vector2 RightStartPos = new Vector2(0, 0);

    // 目標の座標
    [SerializeField]
    Vector2 CenterTargetPos = new Vector2(0, 0);
    [SerializeField]
    Vector2 LeftTargetPos = new Vector2(0, 0);
    [SerializeField]
    Vector2 RighttargetPos = new Vector2(0, 0);

    // 座標変更時の速度
    [SerializeField]
    float spdX = 0, spdY = 0;

    // eventSystem型の変数を宣言　インスペクターにEventSystemをアタッチして取得しておく
    [SerializeField] private EventSystem eventSystem;
#endregion

    // Rect配列に使う番号
    int rctNum = 0;

    // 配列の最大値と最小値
    int max;
    int min;
    // ページの計算に使う最大と最小の値
    int pMax;
    int pMin;

    // 現在のページ数カウント用
    int pageCount = 1;

    // Rectが移動終わるまでボタンを機能させない
    bool isClick = true;
  
    // 全体ページと現在ページの文字
    string allPage;
    string nowPage;

    /// <summary>
    /// enum変数
    /// </summary>
    RectMove rMove;

    // UIを動かすクラス
    /// <summary>
    /// サイズ変更やポジション移動等の処理のメソッドがあるクラス
    /// </summary>
    ClassUIAnim UAnim;

    // ボタンオブジェクトを入れる箱
    private GameObject button_ob;

    // テキストオブジェクトを入れる箱
    private GameObject NameText_ob;

    // テキストコンポーネントを入れる箱
    private Text name_text;

#endregion
// メソッド
#region Method
    void Start()
    {
        // ページの計算用
        pMax = targetRct.Length;
        pMin = targetRct.Length / targetRct.Length;

        // 配列に使いたいので要素数から値を-1
        max = targetRct.Length - 1;
        min = 0;

        // ページの最大数
        allPage = targetRct.Length.ToString();

        // インスタンス生成
        UAnim = new ClassUIAnim();
    }

    void Update()
    {
        // 現在のページカウントををstringに変換
        nowPage = pageCount.ToString();

        // テキストに反映
        page.text = nowPage + "/" + allPage;

        // Rect処理
        UICon();
    }

    /// <summary>
    /// Rectを動かす処理
    /// </summary>
    void UICon()
    {
        switch (rMove)
        {
            case RectMove.rightFeed: // 右送り

                // 動かす対象のRectと目標座標（センター）を比較
                if (targetRct[rctNum].anchoredPosition.x >= CenterStartPos.x)
                {
                    // 動かす対象のRectを目標座標（センター）に近づける
                    UAnim.anim_PosChange(targetRct[rctNum], -spdX, spdY);

                    // センターにすでにあるUIをカメラの外へ
                    // センターに動かす予定のRct配列の次の要素を動かす
                    // 最大は超えないように
                    if (rctNum == min) 
                    {
                        UAnim.anim_PosChange(targetRct[max], -spdX, spdY);
                    }
                    else
                    {
                        UAnim.anim_PosChange(targetRct[rctNum-1], -spdX, spdY);
                    }
                }
                // 動かし終わったら指定の場所にずれがないようにセット
                else
                {
                    targetRct[rctNum].anchoredPosition = CenterStartPos;

                    // 移動が終わったので次のボタン入力を受け付ける
                    isClick = true;

                    // 処理終了
                    rMove = RectMove.etc;
                }
                break;
            case RectMove.leftFeed: // 左送り

                // 動かす対象のRectと目標座標（センター）を比較
                if (targetRct[rctNum].anchoredPosition.x <= CenterStartPos.x)
                {
                    // 動かす対象のRectを目標座標（センター）に近づける
                    UAnim.anim_PosChange(targetRct[rctNum], spdX, spdY);

                    // センターにすでにあるUIをカメラの外へ
                    // センターに動かす予定のRct配列の一個前の要素を動かす
                    // 最大は超えないように
                    if (rctNum == max)
                    {
                        UAnim.anim_PosChange(targetRct[min], spdX, spdY);
                    }
                    else
                    {
                        UAnim.anim_PosChange(targetRct[rctNum + 1], spdX, spdY);
                    }
                }
                else
                {
                    targetRct[rctNum].anchoredPosition = CenterStartPos;

                    // 移動が終わったので次のボタン入力を受け付ける
                    isClick = true;

                    // 処理終了
                    rMove = RectMove.etc;
                }
                break;
        }
    }

    /// <summary>
    /// 左矢印ボタンのメソッド
    /// </summary>
    public void LButton()
    {
        // 移動中ならreturn
        if (!isClick) { return; }
        // 最大値を越さないように
        if (rctNum == min)
        {
            rctNum = max;
        }
        else
        {
            rctNum--;
        }

        // 配列の要素数を超えないようにする
        if (pageCount == pMin)
        {
            // 最小値である1を求める
            // minだと配列用にしてあるので0になってしまうため
            pageCount = pMax;
        }
        else
        {
            pageCount--;
        }
        
        // 動かす物を右初期座標に移動
        targetRct[rctNum].anchoredPosition = LeftStartPos;

        // アニメーション処理
        rMove = RectMove.leftFeed;

        // 移動状態
        isClick = false;
    }
    
    /// <summary>
    /// 右矢印ボタンのメソッド
    /// </summary>
    public void RButton()
    {
        // 移動中ならreturn
        if (!isClick) { return; }


        // 動かす対象Rectの計算
        if (rctNum == max)
        {
            rctNum = min;
        }
        else
        {
            rctNum++;

        }

        // 配列の要素数を超えないようにする
        if (pageCount==pMax)
        {
            // 最小値である1を求める
            // minだと配列用にしてあるので0になってしまうため
            pageCount = pMin;
        }
        else
        {
            pageCount++;
        }

        // 動かす物を左初期座標に移動
        targetRct[rctNum].anchoredPosition = RightStartPos;

        // アニメーション処理
        rMove = RectMove.rightFeed;

        // 移動状態
        isClick = false;
    }

    /// <summary>
    /// ステージに遷移するボタンのメソッド
    /// </summary>
    /// <param name="Scenename"> 遷移先のステージ名を入れる</param>
    public void StageButton(string Scenename)
    {
        button_ob = eventSystem.currentSelectedGameObject;

        //ボタンの子のテキストオブジェクトを名前指定で取得 この場合Text100と名前が付いているテキストオブジェクトを探す
        NameText_ob = button_ob.transform.Find("Text").gameObject;

        //テキストオブジェクトのテキストを取得
        name_text = NameText_ob.GetComponent<Text>();

        // もしステージ名がRockならシーン遷移させない
        if (name_text.text == TypeName.Rock) { return; }

        // フェード
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene(Scenename);
        });
    }
#endregion
}

// StageButtonで使う
/// <summary>
/// 特定のステージ名に対してシーン遷移しないように制限する
/// </summary>
public static class TypeName
{
    public static string Rock = "Rock";
}
