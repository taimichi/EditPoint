using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEffect : MonoBehaviour
{
    [SerializeField] Camera _camera;                        // ÉJÉÅÉâÇÃç¿ïW
    [SerializeField] private GameObject test;
    private Vector3 pos;

    void Update()
    {

        pos = _camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 1;
        test.transform.position = pos;
    }
}
