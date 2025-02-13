using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualObjectTriggerChecker : MonoBehaviour
{
    [SerializeField]
    List<GameObject> objList = new List<GameObject>();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        objList.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objList.Remove(collision.gameObject);
    }

}
