using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMarker : MonoBehaviour
{
    public bool isActive;
    public bool isHitGround;

    //プレイヤーが接触しているか
    private bool isHitPlayer = false;

    SpriteRenderer sr;

    BoxCollider2D bc;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isActive)
        {
            sr.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            sr.color = new Color(1, 1, 1, 0);
            isHitGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<GroundAttr>(out var typeAttr))
        {
            isHitGround = true;
        }

        if (collision.tag == "Player")
        {
            Debug.Log("プレイヤーと接触");
            isHitPlayer = true;
        }

        if (collision.tag == "UnCreateArea")
        {
            Debug.Log("接地不可エリアと接触");
            // プレイヤー接触時の接地不可を流用、ほんとはよくない…
            isHitPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<GroundAttr>(out var typeAttr))
        {
            isHitGround = false;
        }

        if (collision.tag == "Player")
        {
            Debug.Log("Playerと接触解除");
            isHitPlayer = false;
        }

        if (collision.tag == "UnCreateArea")
        {
            // プレイヤー接触時の接地不可を流用、ほんとはよくない…
            isHitPlayer = false;
        }
    }

    public bool ReturnHitPL()
    {
        return isHitPlayer;
    }
}
