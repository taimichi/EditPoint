using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaffCredit : MonoBehaviour
{
    [SerializeField] private RectTransform rectCredit;  //クレジットのRectTransform
    private GameObject CreditPanel;                     //クレジットの背景
    [SerializeField] private RectTransform rectStart;   //開始位置
    [SerializeField] private RectTransform rectEnd;     //終了位置

    private float moveSpeed = 2.5f;                     //クレジットの動く速さ

    private float timer = 0f;                           //最後止まっている時間の計測用
    private const float LIMIT_TIME = 3.0f;              //最後止まる時間

    #region fade
    [SerializeField] private Image fadeImage;
    private Color startColor;
    private bool isFade = false;
    private float alpha = 0;
    private float fadeSpeed = 0.02f;
    #endregion

    void Start()
    {
        //初期位置を設定
        rectCredit.localPosition = new Vector3(
                                                rectCredit.localPosition.x,
                                                rectStart.localPosition.y,
                                                rectCredit.localPosition.z);

        //背景取得
        CreditPanel = this.transform.GetChild(0).gameObject;

        //初期のカラー値を設定
        startColor = fadeImage.color;
    }

    void Update()
    {
        //クレジットパネルが表示状態になった時
        if (CreditPanel.activeSelf)
        {
            //クレジットの最後が最終地点に行ったら
            if(rectCredit.localPosition.y >= rectEnd.localPosition.y)
            {
                if(timer >= LIMIT_TIME)
                {
                    StartFade();
                    return;
                }
                timer += Time.deltaTime;
            }
            //そうじゃないとき
            else
            {
                //クレジットを下から上へ
                rectCredit.localPosition += Vector3.up * moveSpeed;
            }
        }
        else
        {
            //初期位置へ
            rectCredit.localPosition = new Vector3(
                                                    rectCredit.localPosition.x,
                                                    rectStart.localPosition.y,
                                                    rectCredit.localPosition.z);
        }
    }

    /// <summary>
    /// フェード処理
    /// </summary>
    private void StartFade()
    {
        if (!isFade)
        {
            //アルファ値を変更
            alpha += fadeSpeed;
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            if (alpha >= 1f)
            {
                isFade = true;
            }
        }
        else
        {
            CreditPanel.SetActive(false);
            //スタート地点に戻る
            rectCredit.localPosition = new Vector3(
                                    rectCredit.localPosition.x,
                                    rectStart.localPosition.y,
                                    rectCredit.localPosition.z);
            //初期状態に戻す
            timer = 0f;
            fadeImage.color = startColor;
            alpha = 0;
            isFade = false;

        }
    }

    //スキップボタンを押したとき
    public void OnSkip()
    {
        //背景を消す
        CreditPanel.SetActive(false);

        //初期位置に戻す
        rectCredit.localPosition = new Vector3(
                                                rectCredit.localPosition.x,
                                                rectStart.localPosition.y,
                                                rectCredit.localPosition.z);
    }
}
