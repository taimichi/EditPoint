using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickGenerator : MonoBehaviour
{
    [SerializeField] private RectTransform content; // �^�C�����C���̃x�[�X
    [SerializeField] private GameObject tickPrefab; // �ڐ���̃v���n�u
    [SerializeField, Header("�^�C�����C���̑S�̎���")] private float duration = 60f; // �^�C�����C���̑S�̎��ԁi��: 60�b�j
    [SerializeField, Header("�ڐ��̊Ԋu")] private float tickInterval = 1f; // �ڐ���̊Ԋu�i��: 1�b���Ɓj

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
