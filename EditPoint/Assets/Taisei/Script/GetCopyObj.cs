using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCopyObj : MonoBehaviour
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

    public GameObject ReturnAttachClip()
    {
        return attachClip;
    }
}
