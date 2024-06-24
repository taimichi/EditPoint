using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private BoxCollider2D plCol;
    [SerializeField] private Rigidbody2D plrig;

    [SerializeField] private CutAndPaste cap;
    [SerializeField] private GameManager gm;

    private bool plCheckTrigger;

    void Start()
    {
        
    }

    void Update()
    {
        if (gm.ReturnEditMode() == false)
        {
            plCol.isTrigger = false;
            plrig.bodyType = RigidbodyType2D.Dynamic;
        }
        else if(gm.ReturnEditMode() == true)
        {
            plCol.isTrigger = true;
            plrig.bodyType = RigidbodyType2D.Kinematic;
        }
        cap.CheckPasteTrigger(plCheckTrigger);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Layer1") ||
           collision.gameObject.layer == LayerMask.NameToLayer("Layer2") ||
           collision.gameObject.layer == LayerMask.NameToLayer("Layer3"))
        {
            plCheckTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Layer1") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Layer2") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Layer3"))
        {
            plCheckTrigger = false;
        }
    }
}
