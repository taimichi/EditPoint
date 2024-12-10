using UnityEngine;
using UnityEngine.UI;

public class FixedScrollbarSize : MonoBehaviour
{
    public Scrollbar scrollbar;
    [Range(0f, 1f)] public float fixedSize = 0.2f; // 固定サイズ

    void Start()
    {
        if (scrollbar != null)
        {
            scrollbar.size = fixedSize; // 初期サイズ設定
        }
    }

    void Update()
    {
        if (scrollbar != null && scrollbar.size != fixedSize)
        {
            scrollbar.size = fixedSize; // サイズを常に固定
        }
    }
}
