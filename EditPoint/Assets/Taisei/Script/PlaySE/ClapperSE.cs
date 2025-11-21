using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapperSE : MonoBehaviour
{
    [SerializeField] private PlaySound playSound;

    void Start()
    {
        if (playSound == null)
        {
            playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        }
    }

    void Update()
    {
        
    }

    public void PlaySE()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.katinko);
    }
}
