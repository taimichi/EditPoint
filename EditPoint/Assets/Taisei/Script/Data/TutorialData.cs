using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TutorialData")]
public class TutorialData : ScriptableObject
{
    public const string PATH = "TutorialData";
    private static TutorialData _tutorialEntity;
    public static TutorialData TutorialEntity
    {
        get
        {
            if (_tutorialEntity == null)
            {
                _tutorialEntity = Resources.Load<TutorialData>(PATH);
                if (_tutorialEntity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _tutorialEntity;
        }
    }

    /// <summary>
    /// チュートリアルステージをやったかどうか
    /// </summary>
    [System.Flags]
    public enum Tutorial_Frags
    {
        None = 0,
        clip = 1 << 0,
        block = 1 << 1,
        copy = 1 << 2,
        blower = 1 << 3,
        move = 1 << 4,
        cut = 1 << 5,
        card = 1 << 6,
        moveGround = 1 << 7
    }
    //末尾にマックスのやつ作って要素数を取得できるように

    [EnumFlags] public Tutorial_Frags frags;
}
