using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    public void UnLock()
    {
        Debug.Log("Ç†ÇÒÇÎÇ¡Ç≠ÅI");
        Destroy(this.gameObject);
    }
}
