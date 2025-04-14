using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetConnectClip : MonoBehaviour
{
    private GameObject attachClip;

    /// <summary>
    /// このオブジェクトと紐づけられているクリップを取得
    /// </summary>
    /// <param name="_clip">紐づけられているクリップ</param>
    public void GetAttachClip(GameObject _clip)
    {
        attachClip = _clip;
    }

    /// <summary>
    /// 紐づけられたクリップを返す
    /// </summary>
    /// <returns>このスクリプトがついているオブジェクトと紐づいているクリップ</returns>
    public GameObject ReturnAttachClip() => attachClip;
}
