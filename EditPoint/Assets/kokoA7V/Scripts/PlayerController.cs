using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GroundChecker gc;
    //Rigidbody2D rb;

    //MoveController mc;

    Vector3 scale;

    [SerializeField]
    float moveSpeed = 0.1f;

    int inputLR = 0;
    Vector3 movePos;

    void Start()
    {
        gc = GetComponent<GroundChecker>();
        //rb = GetComponent<Rigidbody2D>();

        //mc = new MoveController(rb);

        gc.InitCol();

        inputLR = 1;
    }

    void Update()
    {
        gc.CheckGround();

        //mc.MoveLR();

        //PlayerInput();

        AutoInput();

        TestMove();
    }

    void TestMove()
    {
        movePos = this.transform.position;
        if (inputLR != 0)
        {
            movePos.x += inputLR * moveSpeed;
        }
        this.transform.position = movePos;

        scale = this.transform.localScale;
        if (inputLR != 0)
        {
            scale.x = Mathf.Abs(scale.x) * inputLR;
        }
        this.transform.localScale = scale;
    }

    void PlayerInput()
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

        float RayLength = 2;
        Vector3 center = gc.GetCenterPos();    // 始点
        Vector3 len = Vector3.right * RayLength * transform.localScale.x; // 長さ

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
            Debug.Log("ぶつかった");
        }
        else
        {
            Debug.Log("すすむ");
        }

        // 向き切り替え
        if (isHit)
        {
            inputLR *= -1;
            isHit = false;
        }
    }
}
