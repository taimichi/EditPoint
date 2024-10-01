using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClipGenerator : MonoBehaviour
{
    [SerializeField] private GameObject timeBar;
    [SerializeField] private GameObject ClipPrefab;

    private int i_createCount = 0;

    private PlaySound playSound;

    private GameObject clip;


    void Start()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
    }

    void Update()
    {

    }

    //新しいクリップを生成
    /// <summary>
    /// 新しいクリップを生成　ブロック生成と同時
    /// </summary>
    /// <param name="_getObj">クリップと同時に生成したブロック</param>
    public void ClipGene(GameObject _getObj, bool _check)
    {
        if (!_check)
        {
            playSound.PlaySE(PlaySound.SE_TYPE.clipGene);
            i_createCount++;
            Vector3 clipPos = new Vector3(timeBar.transform.position.x, 0, 0);
            clip = Instantiate(ClipPrefab, clipPos, timeBar.transform.rotation, this.gameObject.transform);
            clip.name = "CreateClip" + i_createCount;
            clip.tag = "CreateClip";
        }

        ClipPlay clipPlay = clip.GetComponent<ClipPlay>();
        clipPlay.OutGetObj(_getObj);
    }

    /// <summary>
    /// 新しいクリップを生成　ボタンで呼び出す
    /// </summary>
    public void ClipGene()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.clipGene);
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
