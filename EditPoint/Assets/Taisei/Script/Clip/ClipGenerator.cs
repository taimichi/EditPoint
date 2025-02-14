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

    private FunctionLookManager functionLook;

    void Start()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        functionLook = GameObject.FindWithTag("GameManager").GetComponent<FunctionLookManager>();
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
    /// 新しいクリップを生成　ボタンで呼び出す
    /// </summary>
    public void ClipGene()
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        if ((functionLook.FunctionLook & LookFlags.ClipGenerate) == 0)
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
