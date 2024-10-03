using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeButtonColor : MonoBehaviour
{
    private GameObject image;
    private Image imageColor;
    private EventSystem eventSystem;
    private bool isFirst = false;

    public void OnChangeColor()
    {
        eventSystem = EventSystem.current;
        if (image != null)
        {
            if(image != eventSystem.currentSelectedGameObject)
            {
                isFirst = false;
                imageColor.color = Color.white;
            }
        }
        image = eventSystem.currentSelectedGameObject;
        imageColor = image.GetComponent<Image>();
        imageColor.color = isFirst == false ? Color.yellow : Color.white;
        isFirst = !isFirst;

    }
}
