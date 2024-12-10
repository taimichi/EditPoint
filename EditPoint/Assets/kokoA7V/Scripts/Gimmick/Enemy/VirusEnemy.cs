using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VirusEnemy : MonoBehaviour
{
    [SerializeField]
    GameObject NoSignalCanvas;

    private PlaySound playSound;

    private void Start()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !GameData.GameEntity.isClear)
        {
            playSound.StopBGM();
            playSound.PlaySE(PlaySound.SE_TYPE.death);
            Instantiate(NoSignalCanvas, Vector2.zero, Quaternion.identity);
        }
    }
}
