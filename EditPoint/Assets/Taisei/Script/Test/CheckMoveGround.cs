using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMoveGround : MonoBehaviour
{
    [SerializeField] private MoveGround move;
    private bool isTrigger = false;

    public bool ReturnIsTrigger() => isTrigger;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (collision.gameObject.tag != "MoveGround")
            {
                isTrigger = true;
                if (move != null)
                {
                    move.SetTrigger(isTrigger);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (collision.gameObject.tag != "MoveGround")
            {
                isTrigger = false;
            }
        }
    }
}
