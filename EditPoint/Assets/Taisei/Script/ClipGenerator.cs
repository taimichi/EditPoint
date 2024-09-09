using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClipGenerator : MonoBehaviour
{
    [SerializeField] private GameObject timeBar;
    [SerializeField] private GameObject ClipPrefab;

    private int i_createCount = 0;


    void Start()
    {
    }

    void Update()
    {

    }

    //�V�����N���b�v�𐶐�
    /// <summary>
    /// �V�����N���b�v�𐶐��@�u���b�N�����Ɠ���
    /// </summary>
    /// <param name="_getObj">�N���b�v�Ɠ����ɐ��������u���b�N</param>
    public void ClipGene(GameObject _getObj)
    {
        i_createCount++;
        Vector3 clipPos = new Vector3(timeBar.transform.position.x, 0, 0);
        GameObject clip = Instantiate(ClipPrefab, clipPos, timeBar.transform.rotation, this.gameObject.transform);
        clip.name = "CreateClip" + i_createCount;
        clip.tag = "CreateClip";

        ClipPlay clipPlay = clip.GetComponent<ClipPlay>();
        clipPlay.OutGetObj(_getObj);
    }

    /// <summary>
    /// �V�����N���b�v�𐶐��@�{�^���ŌĂяo��
    /// </summary>
    public void ClipGene()
    {
        i_createCount++;
        Vector3 clipPos = new Vector3(timeBar.transform.position.x, 0, 0);
        GameObject clip = Instantiate(ClipPrefab, clipPos, timeBar.transform.rotation, this.gameObject.transform);
        clip.name = "CreateClip" + i_createCount;
        clip.tag = "CreateClip";
    }


    public int ReturnCount()
    {
        return i_createCount;
    }
}
