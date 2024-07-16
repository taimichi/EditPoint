using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class StageSelect : MonoBehaviour
{
    // FadeCanvas
    [SerializeField] 
    Fade fade;          

    // 動かす対象
    [SerializeField] 
    RectTransform[] targetRct;

    // Rect配列に使う番号
    int rctState = 0;

    // 配列の最大値と最小値
    int max;
    int min;

    // Rectが移動終わるまでボタンを機能させない
    bool Click = true;
    
    // 初期の座標値
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

    // アニメーション状態
    int state = 100;

    // クラス
    ClassUIAnim UAnim;

    // Text何ページか分かりやすく
    [SerializeField] Text page;

    int Nowpage_INT=1;
    
    string ALLpage;
    string NOWpage;

    void Start()
    {
        max = targetRct.Length - 1;
        min = 0;

        // ページの最大数
        ALLpage = targetRct.Length.ToString();

        UAnim = new ClassUIAnim();
    }

    // Update is called once per frame
    void Update()
    {
        // 現在のページ
        // rctStateは配列で使うため数字がー１されるので＋１する
        NOWpage = Nowpage_INT.ToString();

        // テキストに反映
        page.text = NOWpage + "/" + ALLpage;

        UICon();
    }

    // Rectを動かす処理
    void UICon()
    {
        switch (state)
        {
            case 0:
                if (targetRct[rctState].anchoredPosition.x >= CenterStartPos.x)
                {
                    targetRct[rctState] = UAnim.anim_PosChange(targetRct[rctState], -spdX, spdY);
                    if (rctState == max) 
                    {
                        targetRct[min] = UAnim.anim_PosChange(targetRct[min], -spdX, spdY);
                    }
                    else
                    {
                        targetRct[rctState+1] = UAnim.anim_PosChange(targetRct[rctState+1], -spdX, spdY);
                    }
                }
                else
                {
                    targetRct[rctState].anchoredPosition = CenterStartPos;
                    Click = true;
                    state++;
                }
                break;
            case 10:
                if (targetRct[rctState].anchoredPosition.x <= CenterStartPos.x)
                {
                    targetRct[rctState] = UAnim.anim_PosChange(targetRct[rctState], spdX, spdY);
                    if (rctState == min)
                    {
                        targetRct[max] = UAnim.anim_PosChange(targetRct[max], spdX, spdY);
                    }
                    else
                    {
                        targetRct[rctState-1] = UAnim.anim_PosChange(targetRct[rctState - 1], spdX, spdY);
                    }
                }
                else
                {
                    targetRct[rctState].anchoredPosition = CenterStartPos;
                    Click = true;
                    state++;
                }
                break;
        }
    }    

    // 左ボタン
    public void LButton()
    {
        // 移動中ならreturn
        if (!Click) { return; }

        // 最大値を越さないように
        if (rctState == min)
        {
            rctState = max;
            Nowpage_INT = targetRct.Length;
        }
        else
        {
            rctState--;
            Nowpage_INT--;
        }

        // アニメーション処理
        state = 0;

        // 動かす物を右初期座標に移動
        targetRct[rctState].anchoredPosition = RightStartPos;

        Click = false;
    }

    // 右ボタン
    public void RButton()
    {
        // 移動中ならreturn
        if (!Click) { return; }

        // 動かす対象Rectの計算
        if (rctState == max)
        {
            rctState = min;
            Nowpage_INT = 1;
        }
        else
        {
            rctState++;
            Nowpage_INT++;
        }

        // アニメーション処理
        state = 10;

        // 動かす物を左初期座標に移動
        targetRct[rctState].anchoredPosition = LeftStartPos;

        Click = false;
    }

    // ここからステージボタン

    // eventSystem型の変数を宣言　インスペクターにEventSystemをアタッチして取得しておく
    [SerializeField] private EventSystem eventSystem;

    //GameObject型の変数を宣言　ボタンオブジェクトを入れる箱
    private GameObject button_ob;

    //GameObject型の変数を宣言　テキストオブジェクトを入れる箱
    private GameObject NameText_ob;

    //Text型の変数を宣言　テキストコンポーネントを入れる箱
    private Text name_text;

    public void StageButton(string Scenename)
    {
        button_ob = eventSystem.currentSelectedGameObject;

        //ボタンの子のテキストオブジェクトを名前指定で取得 この場合Text100と名前が付いているテキストオブジェクトを探す
        NameText_ob = button_ob.transform.Find("Text").gameObject;

        //テキストオブジェクトのテキストを取得
        name_text = NameText_ob.GetComponent<Text>();

        if (name_text.text == "Rock") { return; }
        // フェード
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene(Scenename);
        });
    }
}
