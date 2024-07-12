using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerItemData>(out var playerItemData))
        {
            if (playerItemData.isKey)
            {
                SceneManager.LoadScene("ClearTestScene");
            }
        }
    }
}
