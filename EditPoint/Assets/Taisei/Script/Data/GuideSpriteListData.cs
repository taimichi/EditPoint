using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GuideSpriteList")]
public class GuideSpriteListData : ScriptableObject
{
    /// <summary>
    /// �`���[�g���A���̎��
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
    /// �`���[�g���A���̉摜�f�[�^������dictionary
    /// </summary>
    [SerializeField] public SerializedDictionary<GUIDE, GuideSpriteData> GuideSprites;
}
