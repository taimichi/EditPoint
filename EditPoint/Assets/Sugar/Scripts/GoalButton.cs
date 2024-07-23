using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalButton : MonoBehaviour
{
    [SerializeField] GameObject goalObj;

    // 目標確認ボタン
    public void StartGoalButton()
    {
        goalObj.SetActive(true);

        // 時間停止
        Time.timeScale = 0;
    }

    // 目標確認を閉じる
    public void EndGoalButton()
    {
        goalObj.SetActive(false);

        // 時間再生
        Time.timeScale = 1;
    }
}
