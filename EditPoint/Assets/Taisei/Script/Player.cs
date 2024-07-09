using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CutAndPaste cap;
    private ObjectMove objMove;

    private bool plCheckTrigger;

    private Rigidbody2D rb;
    private Vector3 force;

    void Start()
    {
        //ゲームマネージャーのスクリプトを取得
        GameObject GM = GameObject.Find("EditControll");
        cap = GM.GetComponent<CutAndPaste>();
        rb = this.GetComponent<Rigidbody2D>();

        objMove = GM.GetComponent<ObjectMove>();
    }

    void Update()
    {
        force = new Vector3(7.5f, 0, 0);
        rb.AddForce(force);

        objMove.CheckPlTrigger(plCheckTrigger);
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
