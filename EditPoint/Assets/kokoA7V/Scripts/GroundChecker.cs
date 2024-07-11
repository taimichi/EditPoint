using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    // �p�u���b�N�ϐ�
    public LayerMask L_LayerMask; // �`�F�b�N�p�̃��C���[
    // �ϐ�
    CapsuleCollider2D col;          // �{�b�N�X�R���C�_�[2D
    bool isGround;              // �n�ʃ`�F�b�N�p�̕ϐ�

    const float RayLength = 0.1f;

    [SerializeField] private GameObject layer1obj;
    [SerializeField] private GameObject layer2obj;
    [SerializeField] private GameObject layer3obj;

    private int i_1Index;
    private int i_2Index;
    private int i_3Index;

    [SerializeField] private PlayerLayer plLayer;

    private void Start()
    {
        i_1Index = layer1obj.transform.GetSiblingIndex() - 2;
        i_2Index = layer2obj.transform.GetSiblingIndex() - 2;
        i_3Index = layer3obj.transform.GetSiblingIndex() - 2;
    }

    private void Update()
    {
        i_1Index = layer1obj.transform.GetSiblingIndex() - 2;
        i_2Index = layer2obj.transform.GetSiblingIndex() - 2;
        i_3Index = layer3obj.transform.GetSiblingIndex() - 2;

    }

    // ������
    public void InitCol()
    {
        col = GetComponent<CapsuleCollider2D>();
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

            LayerCheck();

            // ���C���΂��āA�w�肵�����C���[�ɂԂ��邩�`�F�b�N
            result = Physics2D.Linecast(foot, foot + len, L_LayerMask);

            // �f�o�b�O�\���p
            Debug.DrawLine(foot, foot + len);

            // �R���C�_�[�ƐڐG�������`�F�b�N
            if (result.collider != null)
            {
                if (result.collider.gameObject.TryGetComponent<GroundAttr>(out var typeAttr))
                {
                    if (typeAttr.isGround)
                    {
                        isGround = true;        // �n�ʂƐڐG����
                        //Debug.Log("�n�ʂƐڐG");
                        return;                 // �I��
                    }
                }

                //isGround = true;        // �n�ʂƐڐG����
                //Debug.Log("�n�ʂƐڐG");
                //return;                 // �I��
            }
            foot.x += width;
        }
        Debug.Log("��");
    }
    // �n�ʂɐڂ��Ă���ϐ����擾
    public bool IsGround() { return isGround; }

    private void LayerCheck()
    {
        switch (plLayer.ReturnPLLayer() - 1)
        {
            //���C���[1
            case 0:
                switch (i_1Index)
                {
                    //�P����
                    case 0:
                        SetMultipleLayerMask(new int[] {8});
                        break;

                    case 1:
                        //�P�ƂQ
                        if(i_2Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 8, 9 });
                        }
                        //�P�ƂR
                        else if (i_3Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 8, 10 });
                        }
                        break;

                    //�P�ƂQ�ƂR
                    case 2:
                        SetMultipleLayerMask(new int[] { 8, 9, 10 });
                        break;
                }
                break;

            //���C���[�Q
            case 1:
                switch (i_2Index)
                {
                    //�Q�̂�
                    case 0:
                        SetMultipleLayerMask(new int[] { 9 });
                        break;

                    case 1:
                        //�P�ƂQ
                        if(i_1Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 8, 9 });
                        }
                        //�Q�ƂR
                        else if(i_3Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 9, 10 });
                        }
                        break;

                    //�P�ƂQ�ƂR
                    case 2:
                        SetMultipleLayerMask(new int[] { 8, 9, 10 });
                        break;
                }
                break;

            //���C���[�R
            case 2:
                switch (i_3Index)
                {
                    //�R�̂�
                    case 0:
                        SetMultipleLayerMask(new int[] { 10 });
                        break;

                    case 1:
                        //�P�ƂR
                        if(i_1Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 8, 10 });
                        }
                        //�Q�ƂR
                        else if (i_2Index == 0)
                        {
                            SetMultipleLayerMask(new int[] { 9, 10 });
                        }
                        break;

                    //�P�ƂQ�ƂR
                    case 2:
                        SetMultipleLayerMask(new int[] { 8, 9, 10 });
                        break;
                }
                break;
        }
    }

    void SetMultipleLayerMask(int[] layers)
    {
        L_LayerMask = 0;
        foreach (int layer in layers)
        {
            L_LayerMask |= (1 << layer);
        }
    }
}
