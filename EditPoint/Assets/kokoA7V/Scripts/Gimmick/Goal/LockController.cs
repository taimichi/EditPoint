using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    //[SerializeField] private GameObject KeyM;
    public void UnLock()
    {
        Debug.Log("���������I");
        //Destroy(KeyM);
        this.gameObject.SetActive(false);
    }

    public void LockReset()
    {
        this.gameObject.SetActive(true);
    }

}
