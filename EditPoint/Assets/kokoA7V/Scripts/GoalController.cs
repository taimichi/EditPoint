using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    [SerializeField] private GameObject ClearCanvas;
    private PlayerController plController;

    private void Start()
    {
        plController = GameObject.Find("Player").GetComponent<PlayerController>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerItemData>(out var playerItemData))
        {
            if (playerItemData.isKey)
            {
                plController.PlayerStop();
                ClearCanvas.SetActive(true);
            }
        }
    }
}
