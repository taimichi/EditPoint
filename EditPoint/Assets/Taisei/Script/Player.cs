using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D plCol;
    private Rigidbody2D plrig;

    private CutAndPaste cap;
    private GameManager gm;

    private bool plCheckTrigger;

    void Start()
    {
        //プレイヤーの当たり判定とリジットボディを取得
        GameObject pl = GameObject.Find("Player");
        plCol = pl.GetComponent<BoxCollider2D>();
        plrig = pl.GetComponent<Rigidbody2D>();

        //ゲームマネージャーのスクリプトを取得
        GameObject GM = GameObject.Find("PlayerControll");
        cap = GM.GetComponent<CutAndPaste>();
        gm = GM.GetComponent<GameManager>();

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
