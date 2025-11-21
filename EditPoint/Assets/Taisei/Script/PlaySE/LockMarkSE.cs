using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMarkSE : MonoBehaviour
{
    private PlaySound playSound;

    void Start()
    {
        if (playSound == null)
        {
            playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        }
    }

    public void UnLockSE()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.unLock);
    }
}
