using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField]
    Animator anim;

    GeneralMoveController mc;

    public float moveSpeed = 2.5f;

    [Range(-1, 1), SerializeField]
    int inputLR = 0;

    private bool b_firstButton = false;

    private Vector2 playerStartPos;

    private bool b_deathed = false;

    // 反転処理
    [SerializeField]
    LayerMask returnLayerMask;

    void Start()
    {
        mc = GetComponent<GeneralMoveController>();

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

        // 移動
        mc.Run(new Vector2(inputLR * moveSpeed, 0));
        mc.MoveUpdate();
        mc.Friction(0.98f);

        if (inputLR == 0)
        {
            rb.gravityScale = 0;
            mc.ResetMove();
        }
        else
        {
            rb.gravityScale = 1;
        }

        // 反転処理
        AutoInput();

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

    void AutoInput()
    {
        bool isHit = false;

        float rayLength = 0.3f;
        float rayWidth = 0.3f;
        Vector3 center = this.transform.position;    // 始点
        Vector3 len = Vector3.right * rayLength * inputLR; // 長さ

        float centerOffset = 0.125f;
        center.y += centerOffset;

        center.y += rayWidth;

        for (int i = 0; i < 3; i++)
        {
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(center, Vector2.right, len.x, returnLayerMask))
            {
                if (hit.collider.gameObject.TryGetComponent<GroundAttr>(out var typeAttr))
                {
                    //Debug.Log("あたったやつ:" + hit.collider);
                    if (typeAttr.isGround)
                    {
                        isHit = true;
                        //Debug.Log("はんてーん");
                    }
                }
            }

            // デバッグ表示用
            Debug.DrawLine(center, center + len);

            center.y -= rayWidth;
        }

        // 向き切り替え
        if (isHit)
        {
            inputLR *= -1;
            isHit = false;
        }
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
        if(this.transform.parent != null)
        {
            this.transform.parent = null;
        }

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
