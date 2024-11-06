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

    [SerializeField, Header("�y�[�X�g�ł����")] private int i_PasteNum = 1000;
    [SerializeField, Header("�R�s�[�ł����")] private int i_CopyNum = 1000;

    //�I�u�W�F�N�g�̃y�[�X�g��ړ�����Ƃ���true�ɂ���
    private bool b_setOnOff = false;

    //�y�[�X�g���̃I�u�W�F�N�g
    private GameObject PasteObj;


    //�}�E�X�̍��W�֘A
    private Vector3 v3_mousePos;
    private Vector3 v3_scrWldPos;

    //�I�������I�u�W�F�N�g
    private GameObject ClickObj;
    /// <summary>
    /// �X�N���v�^�u���I�u�W�F�N�g�ł܂Ƃ߂Ă���}�e���A��
    /// "layerMaterials"�Ƃ������X�g�������Ă���
    /// </summary>
    [SerializeField] private Materials materials;


    //�������������ϐ�
    private bool b_isNoHit;
    private bool b_isSpecificTag;

    private PlaySound playSound;

    [SerializeField] private bool b_Lock = false;

    private ClipPlay clipPlay;
    private GetCopyObj gco;


    void Start()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        CopyObj = null;
        PasteObj = null;
        copyModeText.enabled = false;
    }

    void Update()
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.b_playNow)
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

        if(b_setOnOff && ModeData.ModeEntity.mode == ModeData.Mode.paste)
        {
            v3_mousePos = Input.mousePosition;
            v3_mousePos.z = 10;

            v3_scrWldPos = Camera.main.ScreenToWorldPoint(v3_mousePos);

                PasteObj.transform.position = v3_scrWldPos;

            //UI�̏ザ��Ȃ�������
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //�N���b�N������J�[�\���̈ʒu�ɃI�u�W�F�N�g��u��
                if (Input.GetMouseButtonDown(0))
                {
                    playSound.PlaySE(PlaySound.SE_TYPE.paste);
                    if (CopyObj.name.Contains("Blower"))
                    {
                        PasteObj.transform.GetChild(0).GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
                        PasteObj.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = false;

                    }
                    else
                    {
                        PasteObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
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
                    MaterialReset();
                    Destroy(PasteObj);
                    PasteObj = null;
                ModeData.ModeEntity.mode = ModeData.Mode.normal;
                b_setOnOff = false;
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
        return b_setOnOff;
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

            b_isNoHit = (hit2d == false);
            if (!b_isNoHit)
            {
                b_isSpecificTag = new List<string> { "Player", "UnTouch", "Markar" }.Contains(hit2d.collider.tag);
            }

            if (b_isNoHit || b_isSpecificTag)
            {
                if (ClickObj != null)
                {
                    MaterialReset();
                }
                ClickObj = null;
                return;
            }

            //�V���ȃI�u�W�F�N�g�ɍX�V����O�Ɍ��̃}�e���A���ɖ߂�
            if (ClickObj != null)
            {
                MaterialReset();
            }

            ClickObj = hit2d.collider.gameObject;
            if (ClickObj.transform.parent != null && ClickObj.transform.parent.gameObject.name.Contains("Blower"))
            {
                ClickObj = hit2d.collider.transform.parent.gameObject;
                ClickObj.transform.GetChild(0).GetComponent<SpriteRenderer>().material = materials.layerMaterials[1];
            }
            else
            {
                ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[1];
            }

            //�R�s�[���[�h�̎��̂�
            if (ModeData.ModeEntity.mode == ModeData.Mode.copy)
            {
                //�N���b�N�����I�u�W�F�N�g���I���\�I�u�W�F�N�g��������
                if (ClickObj != null)
                {
                    playSound.PlaySE(PlaySound.SE_TYPE.copy);
                        if (i_CopyNum > 0)
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
            MaterialReset();
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
        if (i_PasteNum > 0)
        {
            PasteObj = Instantiate(CopyObj);
            if(CopyObj.name.Contains("Blower"))
            {
                PasteObj.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = true;
            }
            else
            {
                PasteObj.GetComponent<Collider2D>().isTrigger = true;
            }
            i_PasteNum--;
        }
        b_setOnOff = true;
    }

    //�R�s�[�{�^������������
    public void OnCopy()
    {
        //�Đ����̎��͕ҏW�@�\�����b�N
        if (GameData.GameEntity.b_playNow)
        {
            return;
        }

        if (!b_Lock)
        {
            if (ModeData.ModeEntity.mode == ModeData.Mode.copy || ModeData.ModeEntity.mode == ModeData.Mode.paste)
            {
                ModeData.ModeEntity.mode = ModeData.Mode.normal;
                MaterialReset();
                if (PasteObj != null)
                {
                    Destroy(PasteObj);
                }
                ClickObj = null;
                CopyObj = null;
                PasteObj = null;
                b_setOnOff = false;
                copyModeText.enabled = false;
            }
            else
            {
                i_CopyNum--;
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

    /// <summary>
    /// �}�e���A�����ŏ��̏�Ԃɖ߂�
    /// </summary>
    private void MaterialReset()
    {
        if (ClickObj != null)
        {
            if(ClickObj.name.Contains("Blower"))
            {
                ClickObj.transform.GetChild(0).GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
            }
            else
            {
                ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
            }
        }
    }


}
