using UnityEngine;

/// <summary>
/// �@�\���b�N�t���O(�r�b�g�t���O�`��)
/// </summary>
[System.Flags]
public enum LookFlags
{
    None          = 0,
    CopyPaste     = 1 << 0,
    ObjectMove    = 1 << 1,
    ClipAccess    = 1 << 2,
    Cut           = 1 << 3,
    ClipGenerate  = 1 << 4,
    BlockGenerate = 1 << 5
}


public class FunctionLookManager : MonoBehaviour
{
    [Header("�@�\���b�N�����邩���Ȃ���")]
    [EnumFlags] [SerializeField] private LookFlags lookFlags = LookFlags.None;

    /// <summary>
    /// �@�\���b�N
    /// </summary>
    public LookFlags FunctionLook
    {
        get { return this.lookFlags; }              //�擾�p
        private set { this.lookFlags = value; }     //�l���͗p
    }
}
