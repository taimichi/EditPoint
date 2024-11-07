using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float speed = 2.0f;        // �㉺�̈ړ����x
    public float height = 0.1f;       // �ړ��̍����͈�

    private Vector3 startPos;

    void Start()
    {
        // �����ʒu��ۑ�
        startPos = transform.position;
    }

    void Update()
    {
        // �I�u�W�F�N�g��Y���W��ύX���ď㉺�ɓ�����
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}