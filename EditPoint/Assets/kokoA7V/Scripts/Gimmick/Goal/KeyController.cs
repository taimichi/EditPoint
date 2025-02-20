using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public List<LockController> lockList = new List<LockController>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerItemData>(out var playerItemData))
        {
            playerItemData.isKey = true;
            for (int i = 0; i < lockList.Count; i++)
            {
                lockList[i].UnLock();
            }
            Destroy(this.gameObject);
        }
    }
}
