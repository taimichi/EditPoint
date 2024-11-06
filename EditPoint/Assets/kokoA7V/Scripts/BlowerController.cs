using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowerController : MonoBehaviour
{
    [SerializeField]
    LayerMask lm;

    [SerializeField]
    Blower nowBlower;

    [SerializeField]
    int nowDir = 0;

    private void Update()
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.b_playNow)
        {
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            return;
        }


        if (ModeData.ModeEntity.mode == ModeData.Mode.normal || ModeData.ModeEntity.mode == ModeData.Mode.moveANDdirect)
        {
            if (Input.GetMouseButtonDown(0))
            {
                nowBlower = null;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D rh = Physics2D.Linecast(mousePos, mousePos, lm);
                if (rh.collider != null)
                {
                    if (rh.collider.transform.parent.gameObject.TryGetComponent<Blower>(out var _blower))
                    {
                        nowBlower = _blower;
                        nowDir = (int)nowBlower.dir;
                        ModeData.ModeEntity.mode = ModeData.Mode.moveANDdirect;
                    }
                    else
                    {
                        nowBlower = null;
                        ModeData.ModeEntity.mode = ModeData.Mode.normal;
                    }
                }
            }
        }


        nowDir += (int)Input.mouseScrollDelta.y;
        if (nowDir > 3)
        {
            nowDir = 0;
        }
        else if (nowDir < 0)
        {
            nowDir = 3;
        }

        if (nowBlower != null)
        {
            nowBlower.dir = nowDir;
        }
    }
}
