using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoCreateArea : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    void Start()
    {
        
    }

    void Update()
    {
        //Ä¶’†‚Ì
        if (GameData.GameEntity.isPlayNow)
        {
            sprite.enabled = false;
        }
        //•ÒW’†‚Ì‚Æ‚«
        else
        {
            sprite.enabled = true;
        }
    }
}
