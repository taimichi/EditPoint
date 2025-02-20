using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEffect : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject Trail;
    [SerializeField] private GameObject TapEffectPrefab;
    private Vector3 pos;

    void Update()
    {
        pos = _camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 1;
        Trail.transform.position = pos;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Instantiate(TapEffectPrefab, pos, Quaternion.identity);
        }
    }
}
