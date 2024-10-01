using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapperSE : MonoBehaviour
{
    [SerializeField] private PlaySound playSound;

    // Start is called before the first frame update
    void Start()
    {
        if (playSound == null)
        {
            playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySE()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.katinko);
    }
}
