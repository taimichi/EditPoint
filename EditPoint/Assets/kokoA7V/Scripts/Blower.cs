using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour
{
    public enum Dir
    {
        up,
        down,
        left,
        right
    }

    public Dir dir = Dir.up;

    public float power = 5;

    public float length = 5;

    private void Update()
    {
        Vector3 rot = transform.localEulerAngles;

        if (dir == Dir.up)
        {
            rot.z = 0;
        }
        else if (dir == Dir.down)
        {
            rot.z = 180;
        }
        else if (dir == Dir.left)
        {
            rot.z = 90;
        }
        else if (dir == Dir.right)
        {
            rot.z = 270;
        }

        transform.localEulerAngles = rot;
    }
}
