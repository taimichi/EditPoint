using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckClipConnect : MonoBehaviour
{
    private bool b_connect = false;

    private void Start()
    {
        this.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!b_connect && GameData.GameEntity.b_playNow)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void ConnectClip()
    {
        b_connect = true;
    }
}
