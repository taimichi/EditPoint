using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGroundManager : MonoBehaviour
{
    private List<GameObject> MoveGrounds = new List<GameObject>();
    private List<MoveGround> moveGroundScripts = new List<MoveGround>();

    void Update()
    {
        MoveGroundController();
    }

    /// <summary>
    /// MoveGround�����X�g�ɒǉ�
    /// </summary>
    /// <param name="_getObj">�ǉ�����I�u�W�F�N�g</param>
    public void GetMoveGrounds(GameObject _getObj)
    {
        MoveGrounds.Add(_getObj);
        moveGroundScripts.Add(_getObj.GetComponent<MoveGround>());
    }

    /// <summary>
    /// MoveGround�����X�g����폜
    /// </summary>
    /// <param name="_delObj">�폜����I�u�W�F�N�g</param>
    public void DeleteMoveGrounds(GameObject _delObj)
    {
        //�폜�������I�u�W�F�N�g�����X�g����T��
        for(int i = 0; i < MoveGrounds.Count; i++)
        {
            //�����I�u�W�F�N�g�̎�
            if (MoveGrounds[i] == _delObj)
            {
                //���������v�f�̏ꏊ�ɍŌ���̗v�f���ڂ��A
                //��ԍŌ�̗v�f������
                //�������闝�R:���X�g�ŏ����v�f���擪�ɋ߂��قǁA
                //�R�s�[����������v�f�������Ȃ菈�����d���Ȃ邽��
                //�����������̕��@�͏��Ԃ��֌W�Ȃ��Ƃ��Ɏg����
                MoveGrounds[i] = MoveGrounds[MoveGrounds.Count - 1];
                MoveGrounds.RemoveAt(MoveGrounds.Count - 1);
                moveGroundScripts[i] = moveGroundScripts[moveGroundScripts.Count - 1];
                moveGroundScripts.RemoveAt(moveGroundScripts.Count - 1);
                break;
            }
        }

    }

    /// <summary>
    /// �������ʂ�Ƃ��AMoveGround.cs��CheckReset�֐������s
    /// </summary>
    private void MoveGroundController()
    {
        //������������A���Z�b�g����������
        if (GameData.GameEntity.isTimebarReset && MoveGrounds.Count > 0)
        {
            for(int i = 0; i < MoveGrounds.Count; i++)
            {
                moveGroundScripts[i].CheckReset();
            }
            GameData.GameEntity.isTimebarReset = false;
        }
    }
}
