using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetConnectClip : MonoBehaviour
{
    private GameObject attachClip;

    /// <summary>
    /// ���̃I�u�W�F�N�g�ƕR�Â����Ă���N���b�v���擾
    /// </summary>
    /// <param name="_clip">�R�Â����Ă���N���b�v</param>
    public void GetAttachClip(GameObject _clip)
    {
        attachClip = _clip;
    }

    /// <summary>
    /// �R�Â���ꂽ�N���b�v��Ԃ�
    /// </summary>
    /// <returns>���̃X�N���v�g�����Ă���I�u�W�F�N�g�ƕR�Â��Ă���N���b�v</returns>
    public GameObject ReturnAttachClip() => attachClip;
}
