using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData
{
    //  ��b���͂����ŃZ�b�g
    private string[] talk= {
        "�c�B",
        "���̐��E���J���ꂽ�C�z�I�H���������āI�I�I��ʂ̑O�ɒN�����܂��ˁI�H",
        "���̎���҂��Ă܂����I",
        "�ǂ����A�͂��߂܂��āI���̐��E�̃A�V�X�^���g���{�b�g�A�u�G�f�B�v�Ɛ\���܂��I",
    };
    // ��b�L�����̖��O
    private string[] name =
    {
        "�G�f�B",
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
