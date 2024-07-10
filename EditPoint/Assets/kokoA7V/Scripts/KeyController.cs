using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerItemData>(out var playerItemData))
        {
            playerItemData.isKey = true;
            Destroy(this.gameObject);
        }
    }
}
