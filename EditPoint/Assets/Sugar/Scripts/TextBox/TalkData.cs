using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData
{
    //  ��b���͂����ŃZ�b�g
    private string[] talk= {
        "�c�B",
        "����{��}��{����}�E��{�Ђ�}�J���ꂽ{��}�C{�͂�}�z�I�H���������āI�I�I\n#{��}��{�߂�}�ʂ�{�܂�}�O��{����}�N�����܂��ˁI�H",
        "����{�Ƃ�}����{��}�҂��Ă܂����I",
        "�ǂ����A�͂��߂܂��āI\n#����{��}��{����}�E�̃A�V�X�^���g���{�b�g�A�G�f�B�ł��I",
        "���Ȃ��ɂ�{����}�����炱��{��}��{����}�E��{����}�����Ă���{��}�s{����}��{����}����\n#{����}�����Ă��炢�܂��A{����}�����Ă��炢�܂��A{����}�����Ă��炢�܂��I",
        "������{��}���܂�΂�����������{�ǂ�}��{��}��t�@�C����{����}�I��ł��������I\n#����{����}��ł��킽������������ƃA�V�X�g���܂���I",
        "{�ւ�}��{�Ƃ�}�����Ă邶��Ȃ��ł����I",
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
