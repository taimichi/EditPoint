using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GetClip : MonoBehaviour
{
    private GameObject Clip;

    private GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    private GameObject BlinkImageObj;

    private bool isTagHit;

    void Start()
    {
        if(raycaster == null)
        {
            raycaster = GameObject.Find("TimeLineCanvas").GetComponent<GraphicRaycaster>();
        }

        if(eventSystem == null)
        {
            eventSystem = EventSystem.current;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Clip != null)
            {
                BlinkImageObj.SetActive(false);
                Clip = null;
            }

            // マウス位置に基づくレイキャスト
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
           
            raycaster.Raycast(pointerData, results);

            // ヒットしたUIオブジェクトを表示
            foreach (RaycastResult result in results)
            {
                isTagHit = new List<string> { "CreateClip", "SetClip", "Timebar"}.Contains(result.gameObject.tag);

                if (isTagHit)
                {
                    if (result.gameObject.tag != "Timebar")
                    {
                        if (Clip != null && Clip != result.gameObject)
                        {
                            BlinkImageObj.SetActive(false);
                        }
                        BlinkImageObj = result.gameObject.transform.GetChild(0).gameObject;
                        Clip = result.gameObject;
                        BlinkImageObj.SetActive(true);
                    }
                }
            }            
        }

    }

    /// <summary>
    /// 取得したクリップを返す
    /// </summary>
    /// <returns>マウスで選択したクリップ</returns>
    public GameObject ReturnGetClip()
    {
        return Clip;
    }
}
