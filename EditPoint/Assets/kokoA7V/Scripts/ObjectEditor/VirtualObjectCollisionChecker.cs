using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualObjectCollisionChecker : MonoBehaviour
{
    // ぶつかっているオブジェクトリスト
    [SerializeField]
    List<GameObject> collisionList = new List<GameObject>();

    // 接触判定、trueなら障害にぶつかっている
    public bool isCollision = false;

    // 編集対象
    public GameObject nowEditObject;

    private void Update()
    {
        if (collisionList.Count > 0)
        {
            isCollision = true;
        }
        else
        {
            isCollision = false;
        }

        if (isCollision)
        {
            CollisionDisp();
        }
        else
        {
            UnCollisionDisp();
        }

        collisionList.Remove(nowEditObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 移動先が被ると
        // 仕様変更につき不要
        //// collision対象レイヤー指定、要修正
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")
        //    || collision.gameObject.layer == LayerMask.NameToLayer("Gimmick")
        //    || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        //{
        //    // 現在編集中のオブジェクトは対象にならない
        //    if (collision.gameObject != nowEditObject)
        //    {
        //        collisionList.Add(collision.gameObject);
        //    }
        //}

        if (collision.gameObject.tag == "UnCreateArea")
        {
            collisionList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionList.Remove(collision.gameObject);
    }

    void CollisionDisp()
    {
        GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 128);
        //GetComponent<SpriteRenderer>().color = new Color32(0, 255, 255, 128);
    }

    void UnCollisionDisp()
    {
        GetComponent<SpriteRenderer>().color = new Color32(0, 255, 255, 128);
    }

}
