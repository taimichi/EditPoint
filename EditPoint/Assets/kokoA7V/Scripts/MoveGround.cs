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
    [SerializeField] private TimeData timeData;
    private float f_test = 0f;

    Vector3 startPos;

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
        if (!timeData.b_DragMode)
        {
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
        //�蓮
        else
        {
            f_test = manualClipTime;
            while (f_test >= 1)
            {
                int i = 0;
                f_test -= pathTime[i];
                i++;
                if (i == path.Count - 1)
                {
                    i = 0;
                }
            }

            // �ړ��p
            Vector3 dist = path[nowPath + 1] - path[nowPath];
            if (nowPath + 1 == path.Count - 1)
            {
                dist = path[0] - path[nowPath];
            }
            Debug.Log(dist);
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
