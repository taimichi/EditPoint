using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckClipConnect : MonoBehaviour
{
    private bool isConnect = false;

    private void Start()
    {
        this.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isConnect && GameData.GameEntity.isPlayNow)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void ConnectClip()
    {
        isConnect = true;
    }
}
