using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    public void UnLock()
    {
        Debug.Log("���������I");
        Destroy(this.gameObject);
    }
}
