using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController
{
    Rigidbody2D rb;

    const float moveSpeed = 2.5f;

    public MoveController(Rigidbody2D _rb)
    {
        rb = _rb;
    }

    // update“à‚É“ü‚ê‚ÄinputLR‚Ì’l‚ð•Ï‚¦‚é‚±‚Æ‚Å‰¡ˆÚ“®‰Â”\
    public void MoveLR(int input)
    {
        Vector3 moveVelocity = rb.velocity;

        // ‰¡ˆÚ“®
        moveVelocity.x = input * moveSpeed;

        // ŽÎ‚ßˆÚ“®Žž‚Ì•‚‚«ã‚ª‚è–hŽ~
        if(input == 0)
        {
            if (moveVelocity.y > 0)
            {
                moveVelocity.y = 0;
            }
        }

        rb.velocity = moveVelocity;
    }
}
