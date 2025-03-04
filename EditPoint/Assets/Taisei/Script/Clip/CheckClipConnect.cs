using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckClipConnect : MonoBehaviour
{
    private bool isConnect = false;
    private ClipPlay clipPlay;

    private void Start()
    {
        this.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isConnect && GameData.GameEntity.isPlayNow)
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �N���b�v�ƕR�Â���
    /// </summary>
    public void ConnectClip()
    {
        isConnect = true;
    }

    /// <summary>
    /// �R�Â��Ă���N���b�v���擾
    /// </summary>
    public void GetClipPlay(GameObject obj)
    {
        clipPlay = obj.GetComponent<ClipPlay>();
    }

    /// <summary>
    /// ���X�g���炱�̃I�u�W�F�N�g�������悤����
    /// </summary>
    public void ListRemove()
    {
        clipPlay.ConnectObjRemove(this.gameObject);
    }

}
