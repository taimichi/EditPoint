using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickGenerator : MonoBehaviour
{
    [SerializeField] private RectTransform content; // タイムラインのベース
    [SerializeField] private GameObject tickPrefab; // 目盛りのプレハブ
    [SerializeField, Header("タイムラインの全体時間")] private float duration = 60f; // タイムラインの全体時間（例: 60秒）
    [SerializeField, Header("目盛の間隔")] private float tickInterval = 1f; // 目盛りの間隔（例: 1秒ごと）

    void Start()
    {
        GenerateTicks();
    }

    private void GenerateTicks()
    {
        float contentWidth = content.rect.width;
        float timePerPixel = duration / contentWidth;
        int numberOfTicks = Mathf.CeilToInt(duration / tickInterval);

        for (int i = 0; i <= numberOfTicks; i++)
        {
            float time = i * tickInterval;
            float xPos = time / timePerPixel;

            GameObject newTick = Instantiate(tickPrefab, content);
            newTick.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0);
            newTick.GetComponentInChildren<Text>().text = FormatTime(time);
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        float fraction = (time * 100) % 100;

        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, fraction);
    }
}
