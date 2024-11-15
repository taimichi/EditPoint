using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    private MoveGroundManager MGManager;

    [Header("��ԉ���0,0,0��ǉ�(�ړ������������W�Ƃ͕�)")]public List<Vector3> path = new List<Vector3>();

    [Header("��ԉ���0��ǉ�(�ړ������������W�Ƃ͕�)")] public List<float> pathTime = new List<float>();

    public float speed = 1;

    [SerializeField]
    int nowPath = 0;

    float timer;

    private float playSpeed = 1f;
    private float autoClipTime = 0f;
    private float manualClipTime = 0f;
    private float f_test = 0f;
    Vector3 startPos;

    //�X�^�[�g�������Ĉ��ڂ��ǂ���
    private bool b_start = false;

    private bool b_first = false;

    private struct saveInfo
    {
        public bool isSave;
        public int savePathNum;
        public Vector3 savePos;
        public float saveTime;
    }
    saveInfo info;

    private void Start()
    {
        MGManager = GameObject.Find("GameManager").GetComponent<MoveGroundManager>();
        MGManager.GetMoveGrounds(this.gameObject);
        info.isSave = false;
        this.transform.position = path[0];
        timer = pathTime[0];

        //path.Add(Vector3.zero);
        //pathTime.Add(0);

        startPos = this.transform.position;
        b_first = false;
    }

    private void Update()
    {
        if (GameData.GameEntity.b_limitTime)
        {
            return;
        }
        //����
        if (!TimeData.TimeEntity.b_DragMode)
        {
            //�Đ����̂�
            if (GameData.GameEntity.b_playNow)
            {
                if (!b_start)
                {
                    this.transform.position = startPos;
                    b_start = true;

                    if (!b_first)
                    {
                        int i = 0;
                        while (autoClipTime >= pathTime[i])
                        {
                            nowPath++;
                            autoClipTime -= pathTime[i];
                            i++;
                            if (i > pathTime.Count - 2)
                            {
                                nowPath = 0;
                                i = 0;
                            }
                        }

                        if (i == pathTime.Count - 2)
                        {
                            autoClipTime = pathTime[i] - autoClipTime;
                        }
                        info.savePathNum = nowPath;
                    }
                }

                // �ړ��p
                Vector3 movePos = this.transform.position;

                Vector3 dist = path[nowPath + 1] - path[nowPath];
                if (nowPath + 1 == path.Count - 1)
                {
                    dist = path[nowPath - 1] - path[nowPath];
                }
                Vector3 moveSpeed = dist / pathTime[nowPath];

                //�J�b�g�����Ƃ�
                if (autoClipTime > 0 && !b_first)
                {
                    info.savePos = this.transform.position + (-moveSpeed * autoClipTime * speed * playSpeed);
                    this.transform.position = info.savePos;
                    movePos = info.savePos;
                    info.saveTime = pathTime[nowPath] - (pathTime[nowPath] - autoClipTime);
                    timer = info.saveTime;
                    Debug.Log("movepos " + info.savePos);

                    b_first = true;
                    info.isSave = true;
                }
                movePos += moveSpeed * Time.deltaTime * speed * playSpeed;

                this.transform.position = movePos;

                // ���ԊǗ�
                timer -= Time.deltaTime * speed * playSpeed;

                if (timer <= 0)
                {
                    nowPath++;
                    if (nowPath == path.Count - 1)
                    {
                        nowPath = 0;
                    }
                    //Debug.Log(nowPath + " ����");
                    timer = pathTime[nowPath];
                }
            }
        }
        //�蓮
        else
        {
            b_start = false;

            //���S������(�u���b�N�̓������W��3�����ɂȂ������肭�������킩��Ȃ�)
            f_test = manualClipTime;
            int i = 0;
            while (f_test > pathTime[i])
            {
                if (i > path.Count - 1)
                {
                    i = 0;
                    f_test -= pathTime[i];
                }
                else
                {
                    f_test -= pathTime[i];
                    i++;
                }

                if (i >= path.Count - 1)
                {
                    i = 0;
                }
            }

            if (i == pathTime.Count - 2)
            {
                f_test = pathTime[i] - f_test;
            }
            //���S�����������܂�

            // �ړ��p
            Vector3 dist = path[nowPath + 1] - path[nowPath];
            if (nowPath + 1 == path.Count - 1)
            {
                dist = path[0] - path[nowPath];
            }
            Vector3 moveSpeed = dist / pathTime[nowPath];
            Vector3 movePos = moveSpeed * f_test * speed * playSpeed;

            this.transform.position = startPos + movePos;
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
    /// �^�C���o�[���Z�b�g�����ꂽ���ǂ���
    /// </summary>
    public void CheckReset()
    {
        if (GameData.GameEntity.b_timebarReset)
        {
            //�Ă΂�Ȃ����(11/15�ؗj)
            Debug.Log(this.gameObject.name+ " " + info.isSave);
            //�J�b�g���ꂽ�N���b�v�̎�
            if(info.isSave)
            {
                Debug.Log("�Ă΂ꂽ");
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
            GameData.GameEntity.b_timebarReset = false;
        }
    }

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
        manualClipTime = _clipTime;
    }

    public void GetClipTime_Auto(float _clipTime)
    {
        autoClipTime = _clipTime;   
    }
}
