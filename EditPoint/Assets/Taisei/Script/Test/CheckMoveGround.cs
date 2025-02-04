using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMoveGround : MonoBehaviour
{
    [SerializeField] private MoveGround move;
    private bool isTrigger = false;
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isTrigger = true;
            move.SetTrigger(isTrigger);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isTrigger = false;
            //move.SetTrigger(isTrigger);
        }
    }
}
