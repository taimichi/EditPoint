using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineManager : MonoBehaviour
{
    [SerializeField, Header("タイムラインの長さ　秒単位")] private int timelineLength = 15;
    [SerializeField, Header("動画の長さ(タイムバーが止まる位置)　秒単位")] private int timebarLimit = 10;

    [SerializeField] private RectTransform Content;

    private Vector3 startPos;   //変更前の座標
    private Vector3 pos;        //変更後の座標
    private Vector2 startSize;  //変更前のサイズ
    private Vector2 size;       //変更後のサイズ

    /// <summary>
    /// 動画の長さ(タイムバーが止まる秒数)を返す
    /// </summary>
    public int ReturnTimebarLimit() => timebarLimit;

    private void Awake()
    {
        TimelineData TLData = TimelineData.TimelineEntity;

        //変更前の座標とサイズを取得
        startPos = Content.localPosition;
        startSize = Content.sizeDelta;

        //変更後の座標を計算　x以外は変更しない
        pos = startPos;
        pos.x = TLData.oneResize * timelineLength;
        //変更後のサイズを計算　width以外は変更しない
        //基準が0.5秒になっているので×2をしている
        size = startSize;
        size.x = (TLData.oneTickWidht * 2) * timelineLength;

        //座標、サイズを変更
        Content.localPosition = pos;
        Content.sizeDelta = size;
    }
}
