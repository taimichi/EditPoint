using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowerWind : MonoBehaviour
{
    Blower blower;

    private void Start()
    {
        blower = this.transform.parent.gameObject.GetComponent<Blower>();
    }

    private void Update()
    {
        Vector2 scale = transform.localScale;
        scale.y = blower.length;
        transform.localScale = scale;

        Vector2 pos = transform.localPosition;
        pos.y = blower.length / 2;
        transform.localPosition = pos;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<GeneralMoveController>(out var mc))
        {
            if (blower.dir == 0)
            {
                mc.Flic(new Vector2(0, blower.power));
            }
            else if (blower.dir == 2)
            {
                mc.Flic(new Vector2(0, -blower.power));
            }
            else if (blower.dir == 1)
            {
                mc.Flic(new Vector2(-blower.power, 0));
            }
            else if (blower.dir == 3)
            {
                mc.Flic(new Vector2(blower.power, 0));
            }
        }
    }
}
