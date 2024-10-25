using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalButton : MonoBehaviour
{
    [SerializeField] GameObject goalObj;

    // �ڕW�m�F�{�^��
    public void StartGoalButton()
    {
        goalObj.SetActive(true);
    }

    // �ڕW�m�F�����
    public void EndGoalButton()
    {
        goalObj.SetActive(false);
    }
}
