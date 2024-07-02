using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CutAndPaste cap;

    private bool plCheckTrigger;

    void Start()
    {
        //�Q�[���}�l�[�W���[�̃X�N���v�g���擾
        GameObject GM = GameObject.Find("EditControll");
        cap = GM.GetComponent<CutAndPaste>();
    }

    void Update()
    {
        
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
