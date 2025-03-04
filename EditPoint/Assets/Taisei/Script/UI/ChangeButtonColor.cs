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

    private void Update()
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            if (imageColor != null)
            {
                imageColor.color = Color.white;
            }
            return;
        }

        if (image != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                imageColor.color = Color.white;
                isFirst = false;
            }
        }
    }

    public void OnChangeColor()
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            if(imageColor != null)
            {
                imageColor.color = Color.white;
            }
            return;
        }

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
