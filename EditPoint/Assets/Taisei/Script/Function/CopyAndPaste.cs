using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CopyAndPaste : MonoBehaviour
{
    [SerializeField] private Text copyModeText;

    //�R�s�[���̃I�u�W�F�N�g
    private GameObject CopyObj;

    [SerializeField, Header("�y�[�X�g�ł����")] private int pasteNum = 1000;
    [SerializeField, Header("�R�s�[�ł����")] private int copyNum = 1000;

    //�I�u�W�F�N�g�̃y�[�X�g��ړ�����Ƃ���true�ɂ���
    private bool isSetOnOff = false;

    //�y�[�X�g���̃I�u�W�F�N�g
    private GameObject PasteObj;

    //�}�E�X�̍��W�֘A
    private Vector3 mousePos;
    private Vector3 scrWldPos;

    //�I�������I�u�W�F�N�g
    private GameObject ClickObj;

    //�������������ϐ�
    private bool isNoHit;
    private bool isSpecificTag;

    private PlaySound playSound;

    private ClipPlay clipPlay;
    private GetCopyObj gco;

    private FunctionLookManager functionLook;

    void Start()
    {
        functionLook = GameObject.FindWithTag("GameManager").GetComponent<FunctionLookManager>();
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        CopyObj = null;
        PasteObj = null;
        copyModeText.enabled = false;
    }

    void Update()
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow)
        {
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            return;
        }

        //�R�s�[���[�h�̎�
        if (ModeData.ModeEntity.mode == ModeData.Mode.copy)
        {
            Copy();
            return;
        }

        if(isSetOnOff && ModeData.ModeEntity.mode == ModeData.Mode.paste)
        {
            mousePos = Input.mousePosition;
            mousePos.z = 10;

            scrWldPos = Camera.main.ScreenToWorldPoint(mousePos);

                PasteObj.transform.position = scrWldPos;

            //UI�̏ザ��Ȃ�������
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //�N���b�N������J�[�\���̈ʒu�ɃI�u�W�F�N�g��u��
                if (Input.GetMouseButtonDown(0))
                {
                    playSound.PlaySE(PlaySound.SE_TYPE.paste);
                    if (CopyObj.name.Contains("Blower"))
                    {
                        PasteObj.transform.GetComponent<Collider2D>().isTrigger = false;
                    }
                    else
                    {
                        PasteObj.GetComponent<Collider2D>().isTrigger = false;
                    }

                    if(CopyObj.GetComponent<GetCopyObj>() == true)
                    {
                        gco = CopyObj.GetComponent<GetCopyObj>();
                        clipPlay = gco.ReturnAttachClip().GetComponent<ClipPlay>();
                        clipPlay.OutGetObj(PasteObj);
                    }

                    Paste();
                }
            }

            //�E�N���b�N�Ńy�[�X�g���[�h����
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("�y�[�X�g���[�h����");
                playSound.PlaySE(PlaySound.SE_TYPE.cancell);
                    Destroy(PasteObj);
                    PasteObj = null;
                ModeData.ModeEntity.mode = ModeData.Mode.normal;
                isSetOnOff = false;
                copyModeText.enabled = false;
            }
        }

        //���������o�Ă��Ȃ������̂łƂ肠���������
        if(ModeData.ModeEntity.mode == ModeData.Mode.copy || ModeData.ModeEntity.mode == ModeData.Mode.paste)
        {

        }
        else
        {
            copyModeText.enabled = false;
        }
    }

    public bool ReturnSetOnOff()
    {
        return isSetOnOff;
    }

    private void GetObj()
    {
        //�N���b�N�����Ƃ��ɑI�������I�u�W�F�N�g�̃��C���[�ɕύX
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            isNoHit = (hit2d == false);
            if (!isNoHit)
            {
                isSpecificTag = (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Gimmick"));
            }

            if (isNoHit || !isSpecificTag)
            {
                ClickObj = null;
                return;
            }


            ClickObj = hit2d.collider.gameObject;
            if (ClickObj.transform.parent != null && ClickObj.transform.parent.gameObject.name.Contains("Blower"))
            {
                ClickObj = hit2d.collider.transform.gameObject;
            }
            //�R�s�[���[�h�̎��̂�
            if (ModeData.ModeEntity.mode == ModeData.Mode.copy)
            {
                //�N���b�N�����I�u�W�F�N�g���I���\�I�u�W�F�N�g��������
                if (ClickObj != null)
                {
                    playSound.PlaySE(PlaySound.SE_TYPE.copy);
                        if (copyNum > 0)
                        {
                            CopyObj = ClickObj;
                            PasteObj = null;
                        }
                    Paste();
                    ModeData.ModeEntity.mode = ModeData.Mode.paste;
                }
            }
        }
    }

    private void Copy()
    {
        GetObj();

        //�R�s�[���[�h������
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("�R�s�[���[�h����");
            playSound.PlaySE(PlaySound.SE_TYPE.cancell);
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            ClickObj = null;
            CopyObj = null;
            copyModeText.enabled = false;
        }
    }

    //�y�[�X�g
    private void Paste()
    {
        copyModeText.text = "���݃y�[�X�g���[�h�ł�";
        if (pasteNum > 0)
        {
            PasteObj = Instantiate(CopyObj);
            if(CopyObj.name.Contains("Blower"))
            {
                PasteObj.transform.GetComponent<Collider2D>().isTrigger = true;
            }
            else
            {
                PasteObj.GetComponent<Collider2D>().isTrigger = true;
            }
            pasteNum--;
        }
        isSetOnOff = true;
    }

    //�R�s�[�{�^������������
    public void OnCopy()
    {
        //�Đ����̎��͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow)
        {
            return;
        }

        if ((functionLook.FunctionLook & LookFlags.CopyPaste) == 0)
        {
            if (ModeData.ModeEntity.mode == ModeData.Mode.copy || ModeData.ModeEntity.mode == ModeData.Mode.paste)
            {
                ModeData.ModeEntity.mode = ModeData.Mode.normal;
                if (PasteObj != null)
                {
                    Destroy(PasteObj);
                }
                ClickObj = null;
                CopyObj = null;
                PasteObj = null;
                isSetOnOff = false;
                copyModeText.enabled = false;
            }
            else
            {
                copyNum--;
                ModeData.ModeEntity.mode = ModeData.Mode.copy;
                copyModeText.enabled = true;
                copyModeText.text = "���݃R�s�[���[�h�ł�";

            }
        }
        else
        {
            playSound.PlaySE(PlaySound.SE_TYPE.cancell);
        }
    }

}
