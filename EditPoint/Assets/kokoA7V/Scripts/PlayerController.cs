using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GroundChecker gc;
    Rigidbody2D rb;

    [SerializeField]
    Animator anim;

    // MoveController mc;
    [SerializeField]
    GeneralMoveController mc;

    public float moveSpeed = 2.5f;

    [Range(-1, 1), SerializeField]
    int inputLR = 0;

    [SerializeField]
    bool manual = true;

    private bool b_firstButton = false;
    [SerializeField] private TimeData timeData;

    private Vector2 playerStartPos;

    private PlaySound playSound;
    [SerializeField] private GameObject NoSignalCanvas;
    private bool b_deathed = false;

    void Start()
    {
        // Null�`�F�b�N���Ă���bykoko20240926
        if (GameObject.Find("AudioCanvas") != null)
        {
            playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        }
        gc = GetComponent<GroundChecker>();
        rb = GetComponent<Rigidbody2D>();

        //anim = GetComponent<Animator>();

        //mc = new MoveController(rb);
        mc = GetComponent<GeneralMoveController>();

        gc.InitCol();

        b_firstButton = false;

        playerStartPos = this.gameObject.transform.position;
    }

    void Update()
    {
        //�����Ń^�C�����C�����蓮�œ��������Ƃ��̃v���C���[�̏�����ǉ�
        //if (timeData.b_DragMode)
        //{
        //    inputLR = 1;
        //    mc.FutureCalculation(timeData.f_nowTime);
        //}

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

        gc.CheckGround();

        AnimPlay();

        //�����ɂ��Q�[���I�[�o�[
        if (this.transform.position.y <= -25)
        {
            if (!b_deathed)
            {
                b_deathed = true;
                playSound.StopBGM();
                playSound.PlaySE(PlaySound.SE_TYPE.death);
                Instantiate(NoSignalCanvas, Vector2.zero, Quaternion.identity);
                Debug.Log("�����!!!");
            }
        }
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
        Vector3 center = gc.GetCenterPos();    // �n�_
        Vector3 len = Vector3.right * rayLength * inputLR; // ����

        // �����蔻��̌��ʗp�̕ϐ�
        RaycastHit2D result;

        center.y += rayWidth;

        for (int i = 0; i < 3; i++)
        {
            // ���C���΂��āA�w�肵�����C���[�ɂԂ��邩�`�F�b�N
            result = Physics2D.Linecast(center, center + len, gc.L_LayerMask);
            // �f�o�b�O�\���p
            Debug.DrawLine(center, center + len);

            if (result.collider != null)
            {
                if (result.collider.gameObject.TryGetComponent<GroundAttr>(out var typeAttr))
                {
                    if (typeAttr.isGround)
                    {
                        isHit = true;
                    }
                }
            }
            center.y -= rayWidth;
        }

        // �����؂�ւ�
        if (isHit)
        {
            inputLR *= -1;
            isHit = false;
        }
    }

    // ��_���[�W
    public void TakeDamage(int value)
    {
        Debug.Log(value + "���߁[���I");
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
        inputLR = 0;
        b_firstButton = false;
        transform.position = playerStartPos;
        mc.ResetMove();
    }

    public void PlayerStop()
    {
        inputLR = 0;
    }
}
