using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GuideSpriteData")]
public class GuideSpriteData : ScriptableObject
{
    /// <summary>
    /// チュートリアル用スプライトデータ
    /// </summary>
    public Sprite[] GuideSprites;
}
