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
            PointerEventData pointData = new PointerEventData(eventSystem);
            pointData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
           
            raycaster.Raycast(pointData, results);

            foreach (RaycastResult result in results)
            {
                isTagHit = new List<string> { "CreateClip", "SetClip", "Timebar"}.Contains(result.gameObject.tag);

                if(isTagHit)
                {
                    //タイムバーに触れた時はforeachから即抜け出す
                    if (result.gameObject.tag == "Timebar")
                    {
                        break;
                    }
                    //クリップだったときの処理
                    else
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

        //クリップ削除
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (Clip != null)
            {
                ClipPlay clipPlay = Clip.GetComponent<ClipPlay>();
                clipPlay.ClipObjDestroy();
                Destroy(Clip);
                Clip = null;
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
