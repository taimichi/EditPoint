using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClipFunction : MonoBehaviour
{
    [SerializeField] private RectTransform Timebar; //タイムバーのRectTransform

    [SerializeField] private GetClip GetClip;   //選択したクリップを取得するスクリプト

    private GameObject Clip;

    private float old_maxTime = 0f; //カット前のクリップの最大時間
    private float new_maxTime = 0f; //カットした後のクリップの最大時間

    private CheckOverlap checkOverlap = new CheckOverlap();

    private FunctionLookManager functionLook;

    private PlaySound playSound;

    private void Awake()
    {
        functionLook = GameObject.FindWithTag("GameManager").GetComponent<FunctionLookManager>();
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();


        Button cutButton = GameObject.Find("Cut").GetComponent<Button>();
        cutButton.onClick.AddListener(OnCut);
    }

    /// <summary>
    /// カット機能　ボタンで呼び出す
    /// </summary>
    public void OnCut()
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        //カット機能がロックされていないとき
        if((functionLook.FunctionLook & LookFlags.Cut) == 0)
        {
            //クリックしたクリップを取得
            Clip = GetClip.ReturnGetClip();
            //クリップの枠を非表示
            Clip.transform.GetChild(0).gameObject.SetActive(false);
            RectTransform clipRect = Clip.GetComponent<RectTransform>();
            ClipPlay clipPlay = Clip.GetComponent<ClipPlay>();

            //カット機能を使うのはクリップとタイムバーが重なってる時のみ
            if (checkOverlap.IsOverlap(clipRect, Timebar))
            {
                old_maxTime = clipPlay.ReturnMaxTime();

                //選択したクリップの左端の座標
                Vector3 leftEdge = clipRect.localPosition
                    + new Vector3(clipRect.rect.width * clipRect.pivot.x, 0, 0);
                //左端からの長さ
                float dis = Timebar.localPosition.x - leftEdge.x;

                //サイズを調整
                dis = ((float)Math.Round(dis / TimelineData.TimelineEntity.oneResize)) * TimelineData.TimelineEntity.oneResize;

                //タイムバーから右端までの長さ
                float newDis = clipRect.rect.width - dis;

                //カットした後の左側のクリップ
                clipRect.sizeDelta = new Vector2(dis, clipRect.rect.height);

                //右端取得
                float rightEdge = clipRect.localPosition.x + (clipRect.rect.width * (1 - clipRect.pivot.x));

                //カットした時の右側用
                GameObject newClip = Instantiate(Clip, clipRect.localPosition, Quaternion.identity, this.transform.parent);
                newClip.name = Clip.name + "(CutClip)";
                RectTransform newClipRect = newClip.GetComponent<RectTransform>();
                //カットしたクリップの長さを調整
                newClipRect.sizeDelta = new Vector2(newDis, newClipRect.rect.height);
                //カットしたクリップの位置を調整
                newClipRect.localPosition = new Vector2(rightEdge, clipRect.localPosition.y);

                //一旦片方のクリップのオブジェクトとの紐づけを解除(2重の紐づけを解消するため)
                ClipPlay newClipPlay = newClip.GetComponent<ClipPlay>();
                newClipPlay.DestroyConnectObj();

                //クリップと紐づけられたオブジェクトを取得
                List<GameObject> newConnectObj = clipPlay.ReturnConnectObj();

                //クリップの長さと速さの初期値を設定
                //クリップ(左)
                ClipSpeed clipSpeed = Clip.GetComponent<ClipSpeed>();
                clipSpeed.GetStartWidth(dis);   // 長さ
                clipSpeed.UpdateSpeed(1f);      // 速さ
                //クリップ(右)
                ClipSpeed newClipSpeed = newClip.GetComponent<ClipSpeed>();
                newClipSpeed.GetStartWidth(newDis); //長さ
                newClipSpeed.UpdateSpeed(1f);       //速さ

                newClipPlay.CalculationMaxTime();
                new_maxTime = newClipPlay.ReturnMaxTime();
                newClipPlay.UpdateStartTime(old_maxTime - new_maxTime);

                //クリップに紐づけられたオブジェクトを複製して、新しいクリップと紐づけ
                for (int i = 0; i < newConnectObj.Count; i++)
                {
                    GameObject obj = Instantiate(newConnectObj[i]);
                    //動く床だった時
                    if (TryGetComponent<MoveGround>(out var test))
                    {
                        test.GetClipTime_Auto(old_maxTime - new_maxTime);
                    }
                    newClipPlay.OutGetObj(obj);
                }

                playSound.PlaySE(PlaySound.SE_TYPE.cut);
            }
        }
    }

}
