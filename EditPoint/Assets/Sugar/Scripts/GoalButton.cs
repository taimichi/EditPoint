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
    }

    // 目標確認を閉じる
    public void EndGoalButton()
    {
        goalObj.SetActive(false);
    }
}
