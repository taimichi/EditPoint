using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitUnCreateArea : MonoBehaviour
{
    private bool isHitArea = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "UnCreateArea")
        {
            isHitArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "UnCreateArea")
        {
            isHitArea = false;
        }
    }

    public bool ReturnHit() => isHitArea;
}
