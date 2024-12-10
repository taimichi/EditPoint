using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHitGround : MonoBehaviour
{
    //ƒvƒŒƒCƒ„[‚ÆÚG‚µ‚Ä‚¢‚é‚©‚Ç‚¤‚©
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

    public bool ReturnHit()
    {
        return isHit;
    }
}
