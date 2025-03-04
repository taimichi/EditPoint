using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowerWind : MonoBehaviour
{
    [SerializeField]
    GameObject blowerBody;

    [SerializeField]
    float power = 5;

    [SerializeField]
    float length = 5;

    private void Update()
    {
        Vector2 scale = transform.localScale;
        scale.y = length;
        transform.localScale = scale;

        Vector2 pos = transform.localPosition;
        pos.y = length / 2;
        transform.localPosition = pos;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameData.GameEntity.isPlayNow && ModeData.ModeEntity.mode != ModeData.Mode.moveANDdirect)
        {
            if (collision.gameObject.TryGetComponent<GeneralMoveController>(out var mc))
            {
                float rot = (blowerBody.transform.localEulerAngles.z + 90) * Mathf.Deg2Rad;

                mc.Flic(new Vector2(Mathf.Cos(rot) * power, Mathf.Sin(rot) * power));
                //Debug.Log(new Vector2(Mathf.Cos(rot) * power, Mathf.Sin(rot) * power));
                //Debug.Log(rot);

                //if (blower.dir == 0)
                //{
                //    mc.Flic(new Vector2(0, blower.power));
                //}
                //else if (blower.dir == 2)
                //{
                //    mc.Flic(new Vector2(0, -blower.power));
                //}
                //else if (blower.dir == 1)
                //{
                //    mc.Flic(new Vector2(-blower.power, 0));
                //}
                //else if (blower.dir == 3)
                //{
                //    mc.Flic(new Vector2(blower.power, 0));
                //}
            }
        }
    }
}
