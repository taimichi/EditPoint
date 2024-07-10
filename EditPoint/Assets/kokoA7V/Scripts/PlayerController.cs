using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GroundChecker gc;
    Rigidbody2D rb;

    [SerializeField]
    Animator anim;

    MoveController mc;

    [Range(-1, 1), SerializeField]
    int inputLR = 0;

    [SerializeField]
    bool manual = true;

    void Start()
    {
        gc = GetComponent<GroundChecker>();
        rb = GetComponent<Rigidbody2D>();

        //anim = GetComponent<Animator>();

        mc = new MoveController(rb);

        gc.InitCol();

    }

    void Update()
    {
        mc.MoveLR(inputLR);

        if (manual)
        {
            ManualInput();
        }
        else
        {
            AutoInput();
            if (inputLR == 0)
            {
                inputLR = (int)Mathf.Sign(transform.localScale.x);
            }
        }

        gc.CheckGround();

        AnimPlay();
    }

    void AnimPlay()
    {
        Vector3 scale = this.transform.localScale;
        if (inputLR != 0)
        {
            scale.x = Mathf.Abs(scale.x) * inputLR;
        }
        this.transform.localScale = scale;

        if (inputLR != 0)
        {
            anim.Play("Move_Base");
        }
        else
        {
            anim.Play("Idle_Base");
        }
    }

    void ManualInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            //mc.inputLR = 1;
            inputLR = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //mc.inputLR = -1;
            inputLR = -1;
        }
        else
        {
            //mc.inputLR = 0;
            inputLR = 0;
        }
    }

    void AutoInput()
    {
        bool isHit = false;

        float RayLength = 0.5f;
        Vector3 center = gc.GetCenterPos();    // 始点
        Vector3 len = Vector3.right * RayLength; // 長さ

        // 当たり判定の結果用の変数
        RaycastHit2D result;

        // レイを飛ばして、指定したレイヤーにぶつかるかチェック
        result = Physics2D.Linecast(center, center + len, gc.LayerMask);

        // デバッグ表示用
        Debug.DrawLine(center, center + len);

        // コライダーと接触したかチェック
        if (result.collider != null)
        {
            isHit = true;
            //Debug.Log("ぶつかった");
        }
        else
        {
            //Debug.Log("すすむ");
        }

        // 向き切り替え
        if (isHit)
        {
            inputLR *= -1;
            isHit = false;
        }
    }
}
