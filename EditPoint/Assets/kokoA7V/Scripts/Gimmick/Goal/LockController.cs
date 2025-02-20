using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    [SerializeField] private GameObject KeyM;
    public void UnLock()
    {
        Debug.Log("Ç†ÇÒÇÎÇ¡Ç≠ÅI");
        Destroy(KeyM);
        Destroy(this.gameObject);
    }
}
