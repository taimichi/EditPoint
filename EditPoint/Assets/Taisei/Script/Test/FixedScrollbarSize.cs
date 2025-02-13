using UnityEngine;
using UnityEngine.UI;

public class FixedScrollbarSize : MonoBehaviour
{
    public Scrollbar scrollbar;
    [Range(0f, 1f)] public float fixedSize = 0.2f; // �Œ�T�C�Y

    void Start()
    {
        if (scrollbar != null)
        {
            scrollbar.size = fixedSize; // �����T�C�Y�ݒ�
        }
    }

    void Update()
    {
        if (scrollbar != null && scrollbar.size != fixedSize)
        {
            scrollbar.size = fixedSize; // �T�C�Y����ɌŒ�
        }
    }
}
