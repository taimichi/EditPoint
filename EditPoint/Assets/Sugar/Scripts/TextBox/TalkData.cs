using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData
{
    //  ��b���͂����ŃZ�b�g
    private string[] talk= { 
        "�ŏ��̕�������",
        "���̕�������",
    };
    // ��b�L�����̖��O
    private string[] name =
    {
        "AD",
    };

    /// <summary>
    /// ���b�Z�[�W�𑗂�
    /// </summary>
    /// <param name="num">���Ԗڂ̉�b�����邩</param>
    /// <returns>���b�Z�[�W�f�[�^�𑗂�</returns>
    public string SendText(int num)
    {
        return talk[num];
    }
    /// <summary>
    /// ���O�𑗂�
    /// </summary>
    /// <param name="num">�L�����̔ԍ�</param>
    /// <returns>�L�����̖��O�𑗂�</returns>
    public string SendName(int num)
    {
        return name[num];
    }
}
