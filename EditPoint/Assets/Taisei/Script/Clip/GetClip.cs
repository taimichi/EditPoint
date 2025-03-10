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

    private SelectDelete deleteScript;

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

        deleteScript = GameObject.Find("DeleteCanvas").GetComponent<SelectDelete>();
        deleteScript.Get_getClip(this.gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClipGet();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ClipGet();
            if(Clip != null)
            {
                ClipOperation clipOp = Clip.GetComponent<ClipOperation>();
                //動かせないクリップじゃないとき
                if (!clipOp.CheckIsLook())
                {
                    deleteScript.ButtonActive(false, Clip);
                }
            }
        }

        //クリップ削除
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            ClipDestroy();
        }
    }

    /// <summary>
    /// クリップ取得の関数
    /// </summary>
    private void ClipGet()
    {
        PointerEventData pointData = new PointerEventData(eventSystem);
        pointData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointData, results);

        foreach (RaycastResult result in results)
        {
            //クリップかタイムバーかどうか
            isTagHit = new List<string> { "CreateClip", "SetClip", "Timebar" }.Contains(result.gameObject.tag);

            //クリップかタイムバーの時のみ
            if (isTagHit)
            {
                //タイムバーに触れた時はforeachから即抜け出す
                if (result.gameObject.tag == "Timebar")
                {
                    break;
                }
                //クリップだったときの処理
                else
                {
                    //ロックされたクリップのとき
                    if(result.gameObject.TryGetComponent<ClipOperation>(out var operation))
                    {
                        if (operation.CheckIsLook())
                        {
                            return;
                        }
                    }

                    if (Clip != null && Clip != result.gameObject)
                    {
                        BlinkImageObj.SetActive(false);
                    }
                    //一つ目の子オブジェクト(取得した時に出る枠)を取得
                    BlinkImageObj = result.gameObject.transform.GetChild(0).gameObject;
                    Clip = result.gameObject;
                    BlinkImageObj.SetActive(true);
                    break;
                }
            }
            else
            {
                if (Clip != null)
                {
                    BlinkImageObj.SetActive(false);
                    deleteScript.SetActiveButton(false);
                    Clip = null;
                }
            }
        }

    }

    public void ClipDestroy()
    {
        if (Clip != null)
        {
            ClipPlay clipPlay = Clip.GetComponent<ClipPlay>();
            clipPlay.ClipObjDestroy();
            Destroy(Clip);
            Clip = null;
        }
    }

    /// <summary>
    /// 取得したクリップを返す
    /// </summary>
    /// <returns>マウスで選択したクリップ</returns>
    public GameObject ReturnGetClip() => Clip;
}
