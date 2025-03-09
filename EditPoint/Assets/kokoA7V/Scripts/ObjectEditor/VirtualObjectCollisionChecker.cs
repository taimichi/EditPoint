using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualObjectCollisionChecker : MonoBehaviour
{
    // �Ԃ����Ă���I�u�W�F�N�g���X�g
    [SerializeField]
    List<GameObject> collisionList = new List<GameObject>();

    // �ڐG����Atrue�Ȃ��Q�ɂԂ����Ă���
    public bool isCollision = false;

    // �ҏW�Ώ�
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
        // �ړ��悪����
        // �d�l�ύX�ɂ��s�v
        //// collision�Ώۃ��C���[�w��A�v�C��
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")
        //    || collision.gameObject.layer == LayerMask.NameToLayer("Gimmick")
        //    || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        //{
        //    // ���ݕҏW���̃I�u�W�F�N�g�͑ΏۂɂȂ�Ȃ�
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
