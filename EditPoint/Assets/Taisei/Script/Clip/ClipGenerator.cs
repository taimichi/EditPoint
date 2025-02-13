using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClipGenerator : MonoBehaviour
{
    [SerializeField] private GameObject timeBar;
    [SerializeField] private GameObject ClipPrefab;

    private int isCreateCount = 0;

    private PlaySound playSound;

    private GameObject clip;

    [SerializeField] private bool isLock = false;

    void Start()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
    }


    //�V�����N���b�v�𐶐�
    /// <summary>
    /// �V�����N���b�v�𐶐��@�u���b�N�����Ɠ���
    /// </summary>
    /// <param name="_getObj">�N���b�v�Ɠ����ɐ��������u���b�N</param>
    public void ClipGene(GameObject _getObj, bool _check)
    {
        if (!_check)
        {
            playSound.PlaySE(PlaySound.SE_TYPE.clipGene);
            isCreateCount++;
            Vector3 clipPos = new Vector3(timeBar.transform.position.x, 0, 0);
            clip = Instantiate(ClipPrefab, clipPos, timeBar.transform.rotation, this.transform.parent);
            clip.name = "CreateClip" + isCreateCount;
            clip.tag = "CreateClip";
        }

        ClipPlay clipPlay = clip.GetComponent<ClipPlay>();
        clipPlay.OutGetObj(_getObj);
    }

    /// <summary>
    /// �V�����N���b�v�𐶐��@�{�^���ŌĂяo��
    /// </summary>
    public void ClipGene()
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        if (!isLock)
        {
            playSound.PlaySE(PlaySound.SE_TYPE.clipGene);
            isCreateCount++;
            Vector3 clipPos = new Vector3(timeBar.transform.position.x, 0, 0);
            GameObject clip = Instantiate(ClipPrefab, clipPos, timeBar.transform.rotation, this.transform.parent);
            clip.name = "CreateClip" + isCreateCount;
            clip.tag = "CreateClip";
        }
        else
        {
            playSound.PlaySE(PlaySound.SE_TYPE.cancell);
        }
    }

    public int ReturnCount() => isCreateCount;
}
