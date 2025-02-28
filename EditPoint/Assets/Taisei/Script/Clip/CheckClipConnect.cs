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
    /// クリップと紐づける
    /// </summary>
    public void ConnectClip()
    {
        isConnect = true;
    }

    /// <summary>
    /// 紐づいているクリップを取得
    /// </summary>
    public void GetClipPlay(GameObject obj)
    {
        clipPlay = obj.GetComponent<ClipPlay>();
    }

    /// <summary>
    /// リストからこのオブジェクトを消すよう頼む
    /// </summary>
    public void ListRemove()
    {
        clipPlay.ConnectObjRemove(this.gameObject);
    }

}
