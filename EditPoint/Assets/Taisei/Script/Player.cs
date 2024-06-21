using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Layer2"))
        {
            Debug.Log("“–‚½‚Á‚Ä‚é");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("ŠO‚ê‚½");
    }
}
