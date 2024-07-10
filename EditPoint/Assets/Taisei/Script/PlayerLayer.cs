using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayer : MonoBehaviour
{
    [SerializeField, Range(1,3), Header("初期で設定するプレイヤーのレイヤー")] private int i_plLayer = 1;
    private bool b_plIsTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Layer1") ||
           collision.gameObject.layer == LayerMask.NameToLayer("Layer2") ||
           collision.gameObject.layer == LayerMask.NameToLayer("Layer3"))
        {
            b_plIsTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Layer1") ||
           collision.gameObject.layer == LayerMask.NameToLayer("Layer2") ||
           collision.gameObject.layer == LayerMask.NameToLayer("Layer3"))
        {
            b_plIsTrigger = false;
        }
    }

    public int ReturnPLLayer()
    {
        return i_plLayer;
    }

    public bool ReturnPlTrigger()
    {
        return b_plIsTrigger;
    }
}
