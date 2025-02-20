using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandleType
{
    def,
    rot,
    body
}

public class HandleSign : MonoBehaviour
{
    public Vector2 handleSign;
    public HandleType handleType = HandleType.def;
    public float priority;
}
