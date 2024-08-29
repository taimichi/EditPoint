using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    [SerializeField] private float f_speed = 278f;
    private RectTransform barPos;
    private Vector2 v2_nowPos;

    void Start()
    {
        barPos = this.gameObject.GetComponent<RectTransform>();
        v2_nowPos = barPos.localPosition;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        barPos.localPosition = v2_nowPos;
        v2_nowPos.x += f_speed * Time.deltaTime;
    }
}
