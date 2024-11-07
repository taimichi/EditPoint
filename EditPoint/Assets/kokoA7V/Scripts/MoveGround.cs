using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    public List<Vector3> path = new List<Vector3>();

    public List<float> pathTime = new List<float>();

    public float speed = 1;

    [SerializeField]
    int nowPath = 0;

    float timer;

    private float playSpeed = 1f;
    private float manualClipTime = 0f;
    private float f_test = 0f;
    Vector3 startPos;

    //�X�^�[�g�������Ĉ��ڂ��ǂ���
    private bool b_start = false;

    private void Start()
    {
        this.transform.position = path[0];
        timer = pathTime[0];

        path.Add(Vector3.zero);
        pathTime.Add(0);

        startPos = this.transform.position;
    }

    private void Update()
    {
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
                }

                // �ړ��p
                Vector3 movePos = this.transform.position;

                Vector3 dist = path[nowPath + 1] - path[nowPath];
                if (nowPath + 1 == path.Count - 1)
                {
                    dist = path[0] - path[nowPath];
                }
                Vector3 moveSpeed = dist / pathTime[nowPath];
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
            while (f_test > 1)
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

            if (i == 1)
            {
                f_test = 1 - f_test;
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

        CheckReset();
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
    private void CheckReset()
    {
        if (GameData.GameEntity.b_timebarReset)
        {
            this.transform.position = path[0];
            timer = pathTime[0];
            nowPath = 0;
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
    /// <param name="_ClipTime">�N���b�v�̌��݂̕b��(�蓮��)</param>
    public void GetClipTime(float _ClipTime)
    {
        manualClipTime = _ClipTime;
    }
}
