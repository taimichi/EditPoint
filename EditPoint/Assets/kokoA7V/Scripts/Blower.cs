using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour
{
    public enum Dir
    {
        up,
        right,
        down,
        left
    }

    //public Dir dir = Dir.up;
    [Range(0, 3)]
    public int dir = 0;

    public float power = 5;

    public float length = 5;

    private void Update()
    {
        Vector3 rot = transform.localEulerAngles;

        rot.z = 90 * dir;

        transform.localEulerAngles = rot;
    }
}
