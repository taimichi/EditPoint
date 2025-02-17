using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHitGround : MonoBehaviour
{
    //プレイヤーと接触しているかどうか
    private bool isHit = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isHit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isHit = false;
        }
    }

    public bool ReturnHit() => isHit;
}
