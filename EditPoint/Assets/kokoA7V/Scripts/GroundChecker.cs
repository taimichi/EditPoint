using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    // �p�u���b�N�ϐ�
    public LayerMask LayerMask; // �`�F�b�N�p�̃��C���[
    // �ϐ�
    BoxCollider2D col;          // �{�b�N�X�R���C�_�[2D
    bool isGround;              // �n�ʃ`�F�b�N�p�̕ϐ�

    const float RayLength = 0.1f;
    
    // ������
    public void InitCol()
    {
        col = GetComponent<BoxCollider2D>();
    }

    // �v���C���[�̒��S�ʒu�i�����j���擾
    public Vector3 GetCenterPos()
    {
        Vector3 pos = transform.position;
        // �{�b�N�X�R���C�_�[�̃I�t�Z�b�g���璆�S���v�Z
        pos.x += col.offset.x;
        pos.y += col.offset.y;
        return pos;
    }

    // �v���C���[�̑������W���擾
    public Vector3 GetFootPos()
    {
        Vector3 pos = GetCenterPos();
        pos.y -= col.size.y / 2;
        return pos;
    }

    // �n�ʂɐڂ��Ă��邩�`�F�b�N
    public void CheckGround()
    {

        isGround = false;   // ��U�󒆔���ɂ��Ă���

        // �f�o�b�O�p�ɐ����o��
        Vector3 foot = GetFootPos();    // �n�_
        Vector3 len = -Vector3.up * RayLength; // ����
        float width = col.size.x / 2;   // �����蔻��̕�

        // ���[�A�����A�E�[�̏��Ƀ`�F�b�N���Ă���
        foot.x += -width;               // �����ʒu�����ɂ��炷

        for (int no = 0; no < 3; ++no)
        {
            // �����蔻��̌��ʗp�̕ϐ�
            RaycastHit2D result;

            // ���C���΂��āA�w�肵�����C���[�ɂԂ��邩�`�F�b�N
            result = Physics2D.Linecast(foot, foot + len, LayerMask);

            // �f�o�b�O�\���p
            Debug.DrawLine(foot, foot + len);

            if (result.collider.gameObject.TryGetComponent<TypeAttr>(out var typeAttr))
            {
                Debug.Log(typeAttr.isGround);
            }

            // �R���C�_�[�ƐڐG�������`�F�b�N
            if (result.collider != null)
            {
                isGround = true;        // �n�ʂƐڐG����
                Debug.Log("�n�ʂƐڐG");
                return;                 // �I��
            }
            foot.x += width;
        }
        Debug.Log("��");
    }
    // �n�ʂɐڂ��Ă���ϐ����擾
    public bool IsGround() { return isGround; }
}
