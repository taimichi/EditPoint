using UnityEngine;

/// <summary>
/// 機能ロックフラグ(ビットフラグ形式)
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
    [Header("機能ロックをするかしないか")]
    [EnumFlags] [SerializeField] private LookFlags lookFlags = LookFlags.None;

    /// <summary>
    /// 機能ロック
    /// </summary>
    public LookFlags FunctionLook
    {
        get { return this.lookFlags; }              //取得用
        private set { this.lookFlags = value; }     //値入力用
    }
}
