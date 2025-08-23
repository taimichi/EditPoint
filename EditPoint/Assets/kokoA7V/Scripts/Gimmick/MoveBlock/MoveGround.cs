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
    //�蓮�Đ��̎��̃N���b�v�̎���
    private float _manualClipTime = 0f;
    //���ۂɏ����Ŏg���p
    //�蓮�Đ��̎��̃N���b�v�̎���
    private float ManualClipTime = 0f;

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

    private bool isStart = false;

    //�������̌��e
    [Foldout("Child"), SerializeField] private GameObject child;            //�e�p�I�u�W�F�N�g
    [Foldout("Child"), SerializeField] private CheckMoveGround childCheck;  //����p�X�N���v�g
    [Foldout("Child"), SerializeField] private SpriteRenderer childSR;      //�e�p�I�u�W�F�N�g�̃X�v���C�g�����_���[
    [Foldout("Child"), SerializeField] private Color N_color;               // �ʏ�̎��̐F
    [Foldout("Child"), SerializeField] private Color C_color;               // ���ɐG�ꂽ��

    private float beforeTime = 0f;

    private struct moveGroundState
    {
        public Vector3 startPos;    //�J�n�ʒu
        public float startTime;     //�J�n����
        public int startPath;       //�J�n����nowPath
        public Vector3 child_startPos;
    }
    private moveGroundState normal;


    private void Awake()
    {
        //�ʒu���ŏ��̈ʒu�ɐݒ�
        this.transform.position = path[0];
        //�ړ����Ԃ�ݒ�
        timer = pathTime[0];
        nowPath = 0;

        //�J�n���̏�Ԃ�ۑ�
        normal.startPos = this.transform.position;
        normal.startPath = nowPath;
        normal.startTime = timer;

        normal.child_startPos = normal.startPos;
    }

    private void Start()
    {
        //�q�I�u�W�F�N�g�̃J���[��ʏ펞�̐F��
        childSR.color = N_color;
        //�������̃}�l�[�W���[�X�N���v�g���擾
        MGManager = GameObject.Find("GameManager").GetComponent<MoveGroundManager>();
        MGManager.GetMoveGrounds(this.gameObject);

        isInvert = false;
        isStart = false;
    }

    private void Update()
    {
        //�^�C���o�[�����E�ɍs������A�ڍs�̏����͂��Ȃ�
        if (GameData.GameEntity.isLimitTime)
        {
            return;
        }

        //����
        if (!TimeData.TimeEntity.isDragMode)
        {
            //�Đ����̂�
            if (GameData.GameEntity.isPlayNow)
            {
                //�e�p�I�u�W�F�N�g���\����
                child.SetActive(false);
                //���[�v�n�߂̂P��ڂ̂�
                if (!isStart)
                {
                    isStart = true;
                    this.transform.position = normal.startPos;
                }

                // �ړ��p
                Vector3 movePos = this.transform.position;

                //�������v�Z
                Vector3 dist = path[nowPath + 1] - path[nowPath];
                if (nowPath + 1 == path.Count - 1)
                {
                    dist = path[nowPath - 1] - path[nowPath];
                }
                Vector3 moveSpeed = dist / pathTime[nowPath];

                //�n�ʂƐG�ꂽ��
                if (isGroundHit)
                {
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

                //���̈ړ����]����
                if (isInvert)
                {
                    int i = nowPath;
                    if (nowPath == 0)
                    {
                        i = path.Count - 1;
                    }
                    if (nowPos == path[i - 1])
                    {
                        //���̍��W�ʒu�̔ԍ���
                        nowPath++;
                        //���W�ʒu�̔ԍ������W�ʒu�z��̍ő吔-1�ɒB������
                        if (nowPath == path.Count - 1)
                        {
                            nowPath = 0;
                        }
                        //���ԍX�V
                        timer = pathTime[nowPath];
                        isGroundHit = false;
                        isInvert = false;
                    }
                }

                //���Ԃ�0�b�ȉ��ɂȂ�����
                if (timer <= 0)
                {
                    //���̍��W�ʒu�̔ԍ���
                    nowPath++;
                    //���W�ʒu�̔ԍ������W�ʒu�z��̍ő吔-1�ɒB������
                    if (nowPath == path.Count - 1)
                    {
                        nowPath = 0;
                    }
                    //���ԍX�V
                    timer = pathTime[nowPath];
                    isGroundHit = false;
                }

                //���������ʂ̒n�ʂɏՓ˂�����
                if (isCheckGroundHit)
                {
                    //���̍��W�ʒu�̔ԍ���
                    nowPath++;
                    //���W�ʒu�̔ԍ������W�ʒu�z��̍ő吔-1�ɒB������
                    if (nowPath == path.Count - 1)
                    {
                        nowPath = 0;
                    }
                    //���ԍX�V
                    timer = pathTime[nowPath];
                    isGroundHit = false;
                    isCheckGroundHit = false;
                }

            }
        }
        //�蓮
        else
        {
            isStart = false;
            child.SetActive(true);
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

            Vector3 dist = path[i + 1] - path[i];
            if (i + 1 == path.Count - 1)
            {
                dist = path[i - 1] - path[i];
            }
            Vector3 moveSpeed = dist / pathTime[i];
            Vector3 movePos = moveSpeed * ManualClipTime * speed * playSpeed;
            if (i == path.Count - 2)
            {
                movePos *= -1;
            }
            child.transform.position = normal.child_startPos + movePos;

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

    //�Փˎ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player���q�I�u�W�F�N�g��
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = this.transform;
        }

    }

    //�Փˌ�
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Player�����ꂽ��q�I�u�W�F�N�g����
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }


    /// <summary>
    /// �^�C���o�[���Z�b�g(�����ʒu�ɖ߂�)�����ꂽ���ǂ���
    /// </summary>
    public void CheckReset()
    {
        if (GameData.GameEntity.isTimebarReset)
        {
            this.transform.position = normal.startPos;
            timer = normal.startTime;
            nowPath = normal.startPath;

            isInvert = false;
            isStart = false;
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
    /// ���݂̈ʒu���珉���ʒu�ɖ߂�
    /// </summary>
    public void SetStartPos()
    {
        this.transform.position = normal.startPos;
    }

    public void SetTrigger(bool trigger)
    {
        isGroundHit = trigger;
    }

    /// <summary>
    /// �R�Â���ꂽ�N���b�v���J�b�g�������ꂽ�Ƃ�
    /// </summary>
    public void Cuted(float newStartTime)
    {
        int i = 0;
        float time = newStartTime;
        while (time >= pathTime[i])
        {
            time -= pathTime[i];
            i++;
            if (i >= pathTime.Count - 1)
            {
                i = 0;
            }
        }
        if (i == pathTime.Count - 2)
        {
            time = pathTime[i] - time;
        }

        Vector3 dist = path[i + 1] - path[i];
        if (i + 1 == path.Count - 1)
        {
            dist = path[i - 1] - path[i];
        }
        Vector3 moveSpeed = dist / pathTime[i];
        Vector3 movePos = moveSpeed * time * speed * playSpeed;
        if (i == path.Count - 2)
        {
            movePos *= -1;
        }
        //�J�n���W�ύX
        normal.startPos += movePos;
        this.transform.position = normal.startPos;
        //�J�n���ԕύX
        time = pathTime[i] - time;
        normal.startTime = pathTime[i] - (pathTime[i] - time);
        timer = normal.startTime;
        //�J�nnowPath�ύX
        normal.startPath = i;
        nowPath = normal.startPath;
    }
}
