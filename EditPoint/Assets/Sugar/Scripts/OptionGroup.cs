using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionGroup : MonoBehaviour,IPointerExitHandler
{
    // �l��n��
    [SerializeField]
    int state = 0;

    // state��n��
    [SerializeField]
    UISideButtonGroup[] SetState;

    public void OptionGroupButton()
    {
        // ���ɖڕW�ɂ���Ȃ瓮�삵�Ȃ�
        if (state == 1) { return; }
        state = 1;
        State();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // UI�������ĂȂ��Ȃ瓮��Ȃ�
        if (state == 3) { return; }

        state = 3;
        State();
    }

    void State()
    {
        SetState[0].Statereceive = state;
        SetState[1].Statereceive = state;
        SetState[2].Statereceive = state;
    }
}
