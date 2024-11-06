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

    private void FixedUpdate()
    {
        if (!b_connect)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void ConnectClip()
    {
        b_connect = true;
    }
}
