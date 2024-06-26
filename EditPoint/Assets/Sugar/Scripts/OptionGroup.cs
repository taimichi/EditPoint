using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionGroup : MonoBehaviour,IPointerExitHandler
{
    // 値を渡す
    [SerializeField]
    int state = 0;

    // stateを渡す
    [SerializeField]
    UISideButtonGroup[] SetState;

    public void OptionGroupButton()
    {
        // 既に目標にいるなら動作しない
        if (state == 1) { return; }
        state = 1;
        State();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // UIが動いてないなら動作なし
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
