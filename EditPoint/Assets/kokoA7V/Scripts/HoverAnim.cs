using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAnim : MonoBehaviour
{
    [SerializeField]
    float setPosY = 0;

    [SerializeField]
    float timer = 0;

    [SerializeField]
    float hoverPow = 1;

    [SerializeField]
    float speed = 1;

    private void Update()
    {
        timer += Time.deltaTime * speed;

        setPosY = Mathf.Cos(timer) * hoverPow;

        Vector2 pos = transform.localPosition;
        pos.y = setPosY + hoverPow * 2;
        transform.localPosition = pos;
    }
}
