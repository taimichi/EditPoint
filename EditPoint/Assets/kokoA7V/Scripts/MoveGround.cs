using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity; //�C���X�y�N�^�[�𐮗����邽�߂̂��

public class MoveGround : MonoBehaviour
{
    private MoveGroundManager MGManager;

    [Header("��ԉ���0,0,0��ǉ�(�ړ������������W�Ƃ͕�)")]public List<Vector3> path = new List<Vector3>();

    [Header("��ԉ���0��ǉ�(�ړ������������W�Ƃ͕�)")] public List<float> pathTime = new List<float>();

    public float speed = 1;

    [SerializeField]
    int nowPath = 0;

    float timer;

    //�Đ����x
    private float playSpeed = 1f;

    //�O������̎擾�p
    //�����Đ��̎��̃N���b�v�̎���
    private float _autoClipTime = 0f;
    //�蓮�Đ��̎��̃N���b�v�̎���
    private float _manualClipTime = 0f;
    //���ۂɏ����Ŏg���p
    //�����Đ��̎��̃N���b�v�̎���
    private float AutoClipTime = 0f;
    //�蓮�Đ��̎��̃N���b�v�̎���
    private float ManualClipTime = 0f;

    //�����ʒu
    Vector3 startPos;

    //�X�^�[�g�������Ĉ��ڂ��ǂ���
    private bool isStart = false;

    //�J�b�g�������ǂ���
    private bool isCut = false;

    /// <summary>
    /// �n�ʂƐG�ꂽ���ǂ���
    /// </summary>
    private bool isGroundHit = false;

    /// <summary>
    /// �n�ʂɐG�ꂽ���̏������������ǂ���
    /// </summary>
    private bool isCheckGroundHit = false;

    /// <summary>
    /// �������������]�������ǂ���
    /// </summary>
    private bool isInvert = false;

    //�������̌��e
    [Foldout("child"), SerializeField] private GameObject child;
    [Foldout("child"), SerializeField] private CheckMoveGround childCheck;
    [Foldout("child"), SerializeField] private SpriteRenderer childSR;
    [Foldout("child"), SerializeField] private Color N_color;   // �ʏ�̎��̐F
    [Foldout("child"), SerializeField] private Color C_color;   // ���ɐG�ꂽ��

    private float beforeTime = 0f;


    //�J�b�g�������̕ۑ��p�\����
    private struct saveInfo
    {
        public bool isSave;         // �J�b�g���Ă��邩�ǂ���
        public int savePathNum;     // �ۑ�����nowPath
        public Vector3 savePos;     // �ۑ��������W
        public float saveTime;      // �ۑ���������
    }
    private saveInfo info;

    private void Start()
    {
        childSR.color = N_color;
        MGManager = GameObject.Find("GameManager").GetComponent<MoveGroundManager>();
        MGManager.GetMoveGrounds(this.gameObject);
        info.isSave = false;
        this.transform.position = path[0];
        timer = pathTime[0];
        nowPath = 0;

        //path.Add(Vector3.zero);
        //pathTime.Add(0);

        startPos = this.transform.position;
        isCut = false;
        isInvert = false;
    }

    private void Update()
    {
        //�^�C���o�[�����E�ɍs������A�ڍs�̏����͂��Ȃ�
        if (GameData.GameEntity.isLimitTime)
        {
            return;
        }

        //����
        if (!TimeData.TimeEntity.b_DragMode)
        {
            //�Đ����̂�
            if (GameData.GameEntity.isPlayNow)
            {
                child.SetActive(false);
                //�Đ��{�^����1��������ĂȂ�������
                if (!isStart)
                {
                    this.transform.position = startPos;
                    isStart = true;
                }

                // �ړ��p
                Vector3 movePos = this.transform.position;

                Vector3 dist = path[nowPath + 1] - path[nowPath];
                if (nowPath + 1 == path.Count - 1)
                {
                    dist = path[nowPath - 1] - path[nowPath];
                }
                Vector3 moveSpeed = dist / pathTime[nowPath];

                //�n�ʂƐG�ꂽ��
                if (isGroundHit)
                {
                    Debug.Log("�n�ʂƐڐG��" + nowPath);

                    isCheckGroundHit = true;
                    isInvert = true;
                }
                //�G��Ă��Ȃ��Ƃ�
                else
                {
                    movePos += moveSpeed * Time.deltaTime * speed * playSpeed;
                }
                this.transform.position = movePos;  //���W�X�V

                // ���ԊǗ�
                timer -= Time.deltaTime * speed * playSpeed;

                //path�̍��W�O�ɏo�����̏���
                Vector3 nowPos = this.transform.position;
                nowPos.x = Mathf.Floor(nowPos.x * 10) / 10;
                nowPos.y = Mathf.Floor(nowPos.y * 10) / 10;

                if (isInvert)
                {
                    int i = nowPath;
                    if(nowPath == 0)
                    {
                        i = path.Count - 1;
                    }
                    if (nowPos == path[i - 1])
                    {
                        nowPath++;
                        if (nowPath == path.Count - 1)
                        {
                            nowPath = 0;
                        }
                        timer = pathTime[nowPath];
                        isGroundHit = false;
                        isInvert = false;
                    }
                }

                if (timer <= 0)
                {
                    nowPath++;
                    if (nowPath == path.Count - 1)
                    {
                        nowPath = 0;
                    }
                    //Debug.Log(nowPath + " ����");
                    timer = pathTime[nowPath];
                    isGroundHit = false;
                }

                if (isCheckGroundHit)
                {
                    nowPath++;
                    if (nowPath == path.Count - 1)
                    {
                        nowPath = 0;
                    }
                    timer = pathTime[nowPath];
                    isGroundHit = false;
                    isCheckGroundHit = false;
                }

            }
        }
        //�蓮
        else
        {
            child.SetActive(true);
            isStart = false;

            //�g�������Ȃ��̂ŁA���̂��������K�v����c
            //���蓮�œ������Ƃ��̏����@�Q�_�ԂȂ瓮��(�u���b�N�̓������W��3�����ɂȂ������肭�������킩��Ȃ�)
            ManualClipTime = _manualClipTime;
            int i = 0;
            //���݂̎��Ԃ����߂�
            while (ManualClipTime >= pathTime[i])
            {
                ManualClipTime -= pathTime[i];
                i++;
                if (i >= pathTime.Count - 1)
                {
                    i = 0;
                }
            }
            if (i == pathTime.Count - 2)
            {
                ManualClipTime = pathTime[i] - ManualClipTime;
            }
            //�������������܂�

            Vector3 dist = path[nowPath + 1] - path[nowPath];
            if (nowPath + 1 == path.Count - 1)
            {
                dist = path[nowPath - 1] - path[nowPath];
            }

            Vector3 moveSpeed = dist / pathTime[nowPath];

            Vector3 movePos = moveSpeed * ManualClipTime * speed * playSpeed;
            child.transform.position = startPos + movePos;

            //���e�̐F�ω�����
            if (childCheck.ReturnIsTrigger() && beforeTime == 0)
            {
                beforeTime = _manualClipTime;
                childSR.color = C_color;
            }
            else if (beforeTime > _manualClipTime && !childCheck.ReturnIsTrigger())
            {
                childSR.color = N_color;
                beforeTime = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player���q�I�u�W�F�N�g��
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = this.transform;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Player�����ꂽ��q�I�u�W�F�N�g����
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }

    /// <summary>
    /// �����ʒu�X�V
    /// </summary>
    private void StartPosUpdate()
    {
        //�J�b�g�@�\���g�����ۂ̏���
        if (_autoClipTime > 0 && !isCut)
        {
            int i = 0;
            AutoClipTime = _autoClipTime;
            //�X�^�[�g���Ԃ𓮂����̈ړ����Ԃ��珇�Ɉ����Ă���
            while (AutoClipTime >= pathTime[i])
            {
                AutoClipTime -= pathTime[i];
                i++;
                if (i >= pathTime.Count - 1)
                {
                    i = 0;
                }
            }
            AutoClipTime = pathTime[i] - AutoClipTime;

            Vector3 dist = path[i + 1] - path[i];
            if (i + 1 == path.Count - 1)
            {
                dist = path[i - 1] - path[i];
            }
            Vector3 moveSpeed = dist / pathTime[i];

            //�J�n���Ԃ�ύX
            //autoClip�͏�̏����ŕK��pathTime[nowPath]�ȉ��̒l�ɂȂ��Ă邽��
            //�����Ďc��̎��Ԃ����߂�
            info.saveTime = pathTime[i] - (pathTime[i] - AutoClipTime);
            timer = info.saveTime;
            info.savePathNum = nowPath;

            //�������̊J�n�ʒu��ύX
            info.savePos = path[i] + (moveSpeed * (pathTime[i] - AutoClipTime) * speed * playSpeed);
            //this.transform.position = info.savePos;
            //startPos = this.transform.position;

            isCut = true;
            info.isSave = true;
        }

    }

    /// <summary>
    /// �^�C���o�[���Z�b�g(�����ʒu�ɖ߂�)�����ꂽ���ǂ���
    /// </summary>
    public void CheckReset()
    {
        if (GameData.GameEntity.isTimebarReset)
        {
            //�J�b�g���ꂽ�N���b�v�̎�
            if(info.isSave)
            {
                this.transform.position = info.savePos;
                startPos = info.savePos;
                nowPath = info.savePathNum;
                timer = info.saveTime;
            }
            else
            {
                this.transform.position = path[0];
                timer = pathTime[0];
                nowPath = 0;
            }
            isInvert = false;
        }
    }

    /// <summary>
    /// �N���b�v�̒��������ƂɍĐ����x��ύX
    /// </summary>
    /// <param name="_playSpeed">�V�����Đ����x</param>
    public void ChangePlaySpeed(float _playSpeed)
    {
        playSpeed = _playSpeed;
    }

    /// <summary>
    /// �N���b�v����蓮���̌��݂̃N���b�v�̕b�����擾
    /// </summary>
    /// <param name="_clipTime">�N���b�v�̌��݂̕b��(�蓮��)</param>
    public void GetClipTime_Manual(float _clipTime)
    {
        _manualClipTime = _clipTime;
    }

    /// <summary>
    /// �N���b�v���玩�����̌��݂ɃN���b�v�̕b�����擾
    /// </summary>
    /// <param name="_clipTime">�N���b�v�̌��݂̕b��(������)</param>
    public void GetClipTime_Auto(float _clipTime)
    {
        _autoClipTime = _clipTime;

        StartPosUpdate();
    }

    /// <summary>
    /// ���݂̈ʒu���珉���ʒu�ɖ߂�
    /// </summary>
    public void SetStartPos()
    {
        this.transform.position = startPos;
    }

    public void SetTrigger(bool trigger)
    {
        isGroundHit = trigger;
    }
}
