using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GuideSpriteList")]
public class GuideSpriteListData : ScriptableObject
{
    /// <summary>
    /// チュートリアルの種類
    /// </summary>
    public enum GUIDE
    {
        none,
        clip,
        blockGene,
        copy,
        move,
        delete,
        timeline,
        button,
        blower,
        moveGround,
        card,
        cut,
        other
    }

    /// <summary>
    /// チュートリアルの画像データを入れるdictionary
    /// </summary>
    [SerializeField] public SerializedDictionary<GUIDE, GuideSpriteData> GuideSprites;
}
