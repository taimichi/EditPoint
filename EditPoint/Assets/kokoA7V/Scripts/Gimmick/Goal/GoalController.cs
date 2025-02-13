using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    [SerializeField] ClapperStart clStart;
    private PlayerController plController;

    private PlaySound playSound;

    private void Start()
    {
        plController = GameObject.Find("Player").GetComponent<PlayerController>();
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerItemData>(out var playerItemData))
        {
            if (playerItemData.isKey)
            {
                playSound.PlaySE(PlaySound.SE_TYPE.gool);
                plController.PlayerStop();
                // ゴールタイミングを知らせる
                clStart.GoalFlg = 2;
            }
        }
    }
}
