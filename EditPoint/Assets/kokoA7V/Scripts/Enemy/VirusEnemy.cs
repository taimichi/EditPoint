using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VirusEnemy : MonoBehaviour
{
    [SerializeField]
    GameObject NoSignalCanvas;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(NoSignalCanvas, Vector2.zero, Quaternion.identity);
        }
    }
}
