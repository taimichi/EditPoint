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
                    //�^�C���o�[�ɐG�ꂽ����foreach���瑦�����o��
                    if (result.gameObject.tag == "Timebar")
                    {
                        break;
                    }
                    //�N���b�v�������Ƃ��̏���
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

        //�N���b�v�폜
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
    /// �擾�����N���b�v��Ԃ�
    /// </summary>
    /// <returns>�}�E�X�őI�������N���b�v</returns>
    public GameObject ReturnGetClip()
    {
        return Clip;
    }
}
