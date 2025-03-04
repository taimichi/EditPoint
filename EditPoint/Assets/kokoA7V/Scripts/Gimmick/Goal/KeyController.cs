using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public List<LockController> lockList = new List<LockController>();
    [SerializeField] private GameObject GetEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerItemData>(out var playerItemData))
        {
            playerItemData.isKey = true;
            Instantiate(GetEffect, this.transform.position, Quaternion.identity);
            for (int i = 0; i < lockList.Count; i++)
            {
                lockList[i].UnLock();
            }
            Destroy(this.gameObject);
        }
    }
}
