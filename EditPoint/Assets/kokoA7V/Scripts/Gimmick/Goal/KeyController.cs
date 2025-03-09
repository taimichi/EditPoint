using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
    public List<LockController> lockList = new List<LockController>();
    [SerializeField] private GameObject GetEffect;
    private PlayerItemData playerItemData;

    private PlaySound playSound;

    private void Start()
    {
        if (GameObject.Find("TimebarReset") != null)
        {
            Button resetButton = GameObject.Find("TimebarReset").GetComponent<Button>();
            resetButton.onClick.AddListener(KeyReset);
        }
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.AddKeyList(this.GetComponent<KeyController>());

        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerItemData>(out playerItemData))
        {
            playSound.PlaySE(PlaySound.SE_TYPE.itemGet);
            playerItemData.isKey = true;
            Instantiate(GetEffect, this.transform.position, Quaternion.identity);
            for (int i = 0; i < lockList.Count; i++)
            {
                lockList[i].UnLock();
            }
            this.gameObject.SetActive(false);
        }
    }

    public void KeyReset()
    {
        this.gameObject.SetActive(true);
        playerItemData.isKey = false;
        for(int i = 0; i < lockList.Count; i++)
        {
            lockList[i].LockReset();
        }
    }
}
