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

    private PlaySound playSound;
    private bool b_deathed = false;

    // ���]����
    [SerializeField]
    LayerMask returnLayerMask;

    void Start()
    {
        // Null�`�F�b�N���Ă���bykoko20240926
        if (GameObject.Find("AudioCanvas") != null)
        {
            playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        }

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

        // �ړ�
        mc.Run(new Vector2(inputLR * moveSpeed, 0));
        mc.MoveUpdate();
        mc.Friction(0.98f);

        // ���]����
        AutoInput();

        AnimPlay();

        //�����ɂ��Q�[���I�[�o�[
        if (this.transform.position.y <= -15)
        {
            if (!b_deathed)
            {
                b_deathed = true;
                Debug.Log("�����!!!");
            }
        }

        //�����������̏���
        if (b_deathed)
        {
            StartCoroutine(WaitFade());

        }
    }

    /// <summary>
    /// ���������̃t�F�[�h�̏���(�R���[�`��)
    /// </summary>
    IEnumerator WaitFade()
    {
        //���o���܂�
        //new�X�[�p�[�}���I2�̓y�Ǔ����Ĉړ�����Ƃ��̂悤�ȃC���[�W
        FallUI fallUI = GameObject.Find("GameManager").GetComponent<FallUI>();
        fallUI.FadeStart();

        yield return new WaitForSeconds(0.3f);
        //�x�点��������
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

        float rayLength = 0.5f;
        float rayWidth = 0.375f;
        Vector3 center = this.transform.position;    // �n�_
        Vector3 len = Vector3.right * rayLength * inputLR; // ����

        // �J�v�Z���R���C�_�[�̓s����A���C�̈ʒu�����������̂Œ���
        float centerOffset = 0.125f;
        center.y += centerOffset;

        center.y += rayWidth;

        for (int i = 0; i < 3; i++)
        {
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(center, Vector2.right, len.x, returnLayerMask))
            {
                if (hit.collider.gameObject.TryGetComponent<GroundAttr>(out var typeAttr))
                {
                    //Debug.Log("�����������:" + hit.collider);
                    if (typeAttr.isGround)
                    {
                        isHit = true;
                        //Debug.Log("�͂�ā[��");
                    }
                }
            }

            // �f�o�b�O�\���p
            Debug.DrawLine(center, center + len);

            center.y -= rayWidth;
        }

        // �����؂�ւ�
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
