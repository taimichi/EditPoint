using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;    // UI

public class ButtonSelectImg : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // カーソルが触れたらこのオブジェクトをtrue
    [SerializeField] GameObject img;
    [SerializeField] GameObject fileObj;

    // UI上にカーソルが触れているか
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 表示
        img.SetActive(true);
    }
    // 離れた場合
    public void OnPointerExit(PointerEventData eventData)
    {
        // 非表示
        img.SetActive(false);
    }

    void Update()
    {
        // 選ばれた状態でのみ
        if(img.activeSelf)
        {
            // クリックしたらそのファイルを起動
            if (Input.GetMouseButtonDown(0))
            {
                fileObj.SetActive(true);
                img.SetActive(false);
            }
        }
    }
}
