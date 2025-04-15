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
                //�������Ȃ��N���b�v����Ȃ��Ƃ�
                if (!clipOp.CheckIsLook())
                {
                    deleteScript.ButtonActive(false, Clip);
                }
            }
        }

        //�N���b�v�폜
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            ClipDestroy();
        }
    }

    /// <summary>
    /// �N���b�v�擾�̊֐�
    /// </summary>
    private void ClipGet()
    {
        PointerEventData pointData = new PointerEventData(eventSystem);
        pointData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointData, results);

        foreach (RaycastResult result in results)
        {
            //�N���b�v���^�C���o�[���ǂ���
            isTagHit = new List<string> { "CreateClip", "SetClip", "Timebar" }.Contains(result.gameObject.tag);

            //�N���b�v���^�C���o�[�̎��̂�
            if (isTagHit)
            {
                //�^�C���o�[�ɐG�ꂽ����foreach���瑦�����o��
                if (result.gameObject.tag == "Timebar")
                {
                    Debug.Log("test");
                    break;
                }
                //�N���b�v�������Ƃ��̏���
                else
                {
                    //���b�N���ꂽ�N���b�v�̂Ƃ�
                    if (result.gameObject.TryGetComponent<ClipOperation>(out var operation))
                    {
                        if (operation.CheckIsLook())
                        {
                            //��I����Ԃɂ���
                            if(result.gameObject.TryGetComponent<ClipPlay>(out var clipPlay))
                            {
                                clipPlay.ConnectObjMaterialChange(0);
                            }
                            break;
                        }
                    }

                    //�ʂ̃N���b�v���I������Ă����ꍇ�A��������x��������
                    if (Clip != null && Clip != result.gameObject)
                    {
                        //��I����Ԃɂ���
                        if(Clip.TryGetComponent<ClipPlay>(out var clipPlay))
                        {
                            clipPlay.ConnectObjMaterialChange(0);
                        }
                        BlinkImageObj.SetActive(false);
                    }

                    //��ڂ̎q�I�u�W�F�N�g(�擾�������ɏo��g)���擾
                    BlinkImageObj = result.gameObject.transform.GetChild(0).gameObject;
                    Clip = result.gameObject;
                    BlinkImageObj.SetActive(true);
                    if (result.gameObject.TryGetComponent<ClipPlay>(out var clipPlayScript))
                    {
                        clipPlayScript.ConnectObjMaterialChange(1);
                    }

                    break;
                }
            }
            else
            {
                if (Clip != null)
                {
                    //��I����Ԃɂ���
                    if (Clip.TryGetComponent<ClipPlay>(out var clipPlay))
                    {
                        clipPlay.ConnectObjMaterialChange(0);
                    }
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
    /// �擾�����N���b�v��Ԃ�
    /// </summary>
    /// <returns>�}�E�X�őI�������N���b�v</returns>
    public GameObject ReturnGetClip() => Clip;
}
