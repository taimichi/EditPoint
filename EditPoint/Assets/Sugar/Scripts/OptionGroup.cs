using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionGroup : MonoBehaviour,IPointerExitHandler
{
    // ’l‚ğ“n‚·
    [SerializeField]
    int state = 0;

    // state‚ğ“n‚·
    [SerializeField]
    UISideButtonGroup[] SetState;

    public void OptionGroupButton()
    {
        // Šù‚É–Ú•W‚É‚¢‚é‚È‚ç“®ì‚µ‚È‚¢
        if (state == 1) { return; }
        state = 1;
        State();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // UI‚ª“®‚¢‚Ä‚È‚¢‚È‚ç“®ì‚È‚µ
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
