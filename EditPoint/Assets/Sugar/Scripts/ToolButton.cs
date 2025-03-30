using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移

public class ToolButton : MonoBehaviour
{
    /*----string変数----*/
    string S_stageName;   // ステージ名取得用変数         

    /*----その他変数（コンポーネントとかスクリプト）----*/
    [SerializeField]
    Fade fade;          // FadeCanvas

    [SerializeField] GameObject clpObj;
    [SerializeField] ClapperStart clapper;
    // メニューウィンドウ
    [SerializeField] GameObject menu;

    [SerializeField] SelectYesNo selectScript;  //リセットするときに即リセットをしないように選択肢を設ける


    private void Start()
    {
     /* fade.cutoutRange = 1;
        // シーンの始まりでフェード
        fade.FadeOut(1.0f);
     */

        // 現在のシーン名を取得(シーンリロードに使う)
        S_stageName = SceneManager.GetActiveScene().name;
    }

    /*----ここから下にボタンの処理の中身を記載----*/

    public void GroupSettings() // 設定ボタン
    {
       
    }
    public void GroupRestart() // シーンリロードボタン（やり直す）
    {
        // フェード
        //clpObj.SetActive(true);
        //clapper.SceneName = S_stageName;
        StartCoroutine(SelectNow());
        //    fade.FadeIn(0.5f, () => {
        //        SceneManager.LoadScene(S_stageName);
        //    });
    }
    public void SelectScene() // タイトルシーンへ（[2024.07/05]今のところタイトルだがステージ選択に戻す可能性あり）
    {
        // フェード
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene("Select");
        });
    }

    //ボタンでメニューを開く
    public void OpenMenu()
    {
        menu.SetActive(true);
    }

    /// <summary>
    /// 選択中の処理
    /// </summary>
    IEnumerator SelectNow()
    {
        selectScript.SelectPanelActive(true);
        yield return new WaitUntil(() => selectScript.ReturnOnClick());

        if (selectScript.ReturnSelect())
        {            
            SceneManager.LoadScene(S_stageName);
        }
        else
        {
            selectScript.SelectPanelActive(false);
        }

    }
}
