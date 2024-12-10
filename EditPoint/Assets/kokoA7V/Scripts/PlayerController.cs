using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //GroundChecker gc;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField]
    Animator anim;

    // MoveController mc;
    GeneralMoveController mc;

    public float moveSpeed = 2.5f;

    [Range(-1, 1), SerializeField]
    int inputLR = 0;

    //[SerializeField]
    //bool manual = true;

    private bool b_firstButton = false;

    private Vector2 playerStartPos;

    private PlaySound playSound;
    private bool b_deathed = false;

    // 反転処理
    [SerializeField]
    LayerMask returnLayerMask;

    void Start()
    {
        // Nullチェックしてくれbykoko20240926
        if (GameObject.Find("AudioCanvas") != null)
        {
            playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        }
        //gc = GetComponent<GroundChecker>();
        //rb = GetComponent<Rigidbody2D>();

        //anim = GetComponent<Animator>();

        //mc = new MoveController(rb);
        mc = GetComponent<GeneralMoveController>();

        //gc.InitCol();

        b_firstButton = false;

        playerStartPos = this.gameObject.transform.position;
    }

    void Update()
    {
        if (GameData.GameEntity.isClear)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            return;
        }

        //mc.MoveLR(inputLR);
        mc.Run(new Vector2(inputLR * moveSpeed, 0));
        mc.MoveUpdate();
        mc.Friction(0.98f);

        //if (manual)
        //{
        //    ManualInput();
        //}
        //else
        //{
        //    AutoInput();
        //}

        AutoInput();

        //gc.CheckGround();

        AnimPlay();

        //落下によるゲームオーバー
        if (this.transform.position.y <= -15)
        {
            if (!b_deathed)
            {
                b_deathed = true;
                Debug.Log("ｱﾜﾜﾜﾜ!!!");
            }
        }

        //落下した時の処理
        if (b_deathed)
        {
            StartCoroutine(WaitFade());

        }
    }

    /// <summary>
    /// 落ちた時のフェードの処理(コルーチン)
    /// </summary>
    IEnumerator WaitFade()
    {
        //演出がまだ
        //newスーパーマリオ2の土管入って移動するときのようなイメージ
        FallUI fallUI = GameObject.Find("GameManager").GetComponent<FallUI>();
        fallUI.FadeStart();

        yield return new WaitForSeconds(0.3f);
        //遅らせたい処理
        OnPlayerReset();
        TimeBar timeBar = GameObject.Find("Timebar").GetComponent<TimeBar>();
        timeBar.OnReStart();
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.OnReset();

        b_deathed = false;

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

        float rayLength = 0.3f;
        float rayWidth = 0.25f;
        //Vector3 center = gc.GetCenterPos();    // 始点
        Vector3 center = this.transform.position;    // 始点
        Vector3 len = Vector3.right * rayLength * inputLR; // 長さ

        // 当たり判定の結果用の変数
        RaycastHit2D result;

        center.y += rayWidth;

        for (int i = 0; i < 3; i++)
        {
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(center, Vector2.right, len.x, returnLayerMask))
            {
                if (hit.collider.gameObject.TryGetComponent<GroundAttr>(out var typeAttr))
                {
                    Debug.Log("あたったやつ:" + hit.collider);
                    if (typeAttr.isGround)
                    {
                        isHit = true;
                        Debug.Log("はんてーん");
                    }
                }
            }

                // レイを飛ばして、指定したレイヤーにぶつかるかチェック
                result = Physics2D.Linecast(center, center + len, returnLayerMask);
            // デバッグ表示用
            Debug.DrawLine(center, center + len);

            //if (result.collider != null)
            //{
            //    //Debug.Log("あたったやつ:" + result.collider);
            //    if (result.collider.gameObject.TryGetComponent<GroundAttr>(out var typeAttr))
            //    {
            //        if (typeAttr.isGround)
            //        {
            //            isHit = true;
            //            Debug.Log("はんてーん");
            //        }
            //    }
            //}
            center.y -= rayWidth;
        }

        // 向き切り替え
        if (isHit)
        {
            inputLR *= -1;
            isHit = false;
        }
    }

    // 被ダメージ
    public void TakeDamage(int value)
    {
        Debug.Log(value + "だめーじ！");
    }

    public void OnPlayerStart()
    {
        if (!b_firstButton)
        {
            inputLR = 1;
            b_firstButton = true;
        }
    }

    public void OnPlayerReset()
    {
        Vector3 scale = this.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * 1;
        this.transform.localScale = scale;

        mc.ResetMove();
        inputLR = 0;
        b_firstButton = false;
        transform.position = playerStartPos;

        rb.constraints = RigidbodyConstraints2D.FreezePositionX & RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void PlayerStop()
    {
        inputLR = 0;
    }
}
