using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMarker : MonoBehaviour
{
    public bool isActive;
    public bool isHitGround;

    //�v���C���[���ڐG���Ă��邩
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
            Debug.Log("�v���C���[�ƐڐG");
            isHitPlayer = true;
        }

        if (collision.tag == "UnCreateArea")
        {
            Debug.Log("�ڒn�s�G���A�ƐڐG");
            // �v���C���[�ڐG���̐ڒn�s�𗬗p�A�ق�Ƃ͂悭�Ȃ��c
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
            Debug.Log("Player�ƐڐG����");
            isHitPlayer = false;
        }

        if (collision.tag == "UnCreateArea")
        {
            // �v���C���[�ڐG���̐ڒn�s�𗬗p�A�ق�Ƃ͂悭�Ȃ��c
            isHitPlayer = false;
        }
    }

    public bool ReturnHitPL()
    {
        return isHitPlayer;
    }
}
