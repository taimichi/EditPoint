using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallUI : MonoBehaviour
{
    [SerializeField] private GameObject FallCanvas;
    [SerializeField] private GameObject FadeInImage;    //フェードイン用画像
    private RectTransform InRect;                       //フェードイン画像のRectTransform
    [SerializeField] private GameObject FadeOutImage;   //フェードアウト用画像
    private RectTransform OutRect;                      //フェードアウト画像のRectTransform

    private Vector2 InStartSize;                        //フェードイン画像の初期サイズ
    private Vector2 OutStartSize;                       //フェードアウト画像の初期サイズ

    private Vector3 InStartPos;                         //フェードイン画像の初期座標
    private Vector3 OutStartPos;                        //フェードアウト画像の初期座標

    private float fadeTimer = 0f;                       //フェードタイマー
    private const float MAX_FADETIME = 0.3f;            //フェードの最大時間

    private const float MAX_SIZE = 2750f;               //最大サイズ(高さ)
    private const float MIN_SIZE = 540f;                //最小サイズ(高さ)

    private float offset = 0;                           //変更するサイズ差
    private float fadeSpeedSize;                        //サイズ変更のスピード

    private const float MAX_POSY = 300f;                //高さの最大値
    private const float MIN_POSY = -800f;               //高さの最小値

    private float dis = 0;                              //変更する距離
    private float fadeSpeedPos;                         //移動スピード

    private bool startFade = false;                     //フェードを開始するかどうか
    /// <summary>
    /// false = 暗転  true = 明転
    /// </summary>
    private bool fade = false;                          //暗転か明転か

    private PlaySound playSound;

    void Start()
    {
        offset = MAX_SIZE - MIN_SIZE;                   //サイズ差を求める
        dis = MAX_POSY - MIN_POSY;                      //距離を求める

        fadeSpeedSize = offset / MAX_FADETIME;          //サイズ変更の速度を求める
        fadeSpeedPos = dis / MAX_FADETIME;              //Y座標の移動速度を求める

        InRect = FadeInImage.GetComponent<RectTransform>();
        OutRect = FadeOutImage.GetComponent<RectTransform>();

        InStartSize = InRect.sizeDelta;                     //フェードイン画像の初期サイズを取得
        InStartPos = FadeInImage.transform.localPosition;   //フェードイン画像の初期座標を取得
        OutStartSize = OutRect.sizeDelta;                   //フェードアウト画像の初期サイズを取得
        OutStartPos = FadeOutImage.transform.localPosition; //フェードアウト画像の初期座標を取得

        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();

    }

    void Update()
    {
        //フェード処理が開始可能かどうか
        if (startFade)
        {
            //暗転処理
            if (!fade)
            {
                Fall_FadeIn();
            }
            //明転処理
            else
            {
                Fall_FadeOut();
            }
        }
    }

    /// <summary>
    /// 暗転
    /// </summary>
    private void Fall_FadeIn()
    {
        InRect.sizeDelta += new Vector2(0, fadeSpeedSize * Time.deltaTime);         //サイズ更新
        InRect.localPosition += new Vector3(0, fadeSpeedPos * Time.deltaTime, 0);   //位置更新
        //フェード時間が最大値を超えたら
        if (fadeTimer >= MAX_FADETIME)
        {
            //画像切り替え　フェードイン画像を非表示、フェードアウト画像を表示
            FadeInImage.SetActive(false);
            FadeOutImage.SetActive(true);
            fade = true;                    //フェードを明転に切り替え
            fadeTimer = 0;                  //タイマーをリセット
        }
        fadeTimer += Time.deltaTime;
    }

    /// <summary>
    /// 明転
    /// </summary>
    private void Fall_FadeOut()
    {
        OutRect.sizeDelta -= new Vector2(0, fadeSpeedSize * Time.deltaTime);        //サイズ更新
        OutRect.localPosition += new Vector3(0, fadeSpeedPos * Time.deltaTime, 0);  //位置更新
        //フェード時間が最大値を超えたら
        if (fadeTimer >= MAX_FADETIME)
        {
            //画像切り替え　フェードアウト画像を非表示、フェードイン画像を表示
            FadeInImage.SetActive(true);
            FadeOutImage.SetActive(false);

            //画像サイズなどをそれぞれ初期サイズに戻す(再度使えるようにするため)
            InRect.sizeDelta = InStartSize;
            FadeInImage.transform.localPosition = InStartPos;
            OutRect.sizeDelta = OutStartSize;
            FadeOutImage.transform.localPosition = OutStartPos;

            fade = false;           //フェードを暗転に切り替え
            startFade = false;      //フェード処理を実行不可能にする
            fadeTimer = 0;          //タイマーをリセット

            //キャンバスを非表示に
            FallCanvas.SetActive(false);
        }
        fadeTimer += Time.deltaTime;
    }

    /// <summary>
    /// フェード開始用の関数
    /// </summary>
    public void FadeStart()
    {
        FallCanvas.SetActive(true);
        startFade = true;
        playSound.PlaySE(PlaySound.SE_TYPE.fall);
    }
}
