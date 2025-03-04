using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private LayerMask lm;


    //�I�u�W�F�N�g���ړ�������ۂ̃}�E�X���W
    private Vector3 scrWldPos;

    //�N���b�N�����ʒu�ƃI�u�W�F�N�g�̍��W�Ƃ̍�
    private Vector3 mousePos;
    private Vector3 offset;

    private Vector3 nowPos;

    //�P�̈ړ�
    private GameObject Obj;
    //�I�u�W�F�N�g�ړ�����
    private bool isObjMove = false;

    //�I�����ɃA�E�g���C��������
    private GameObject ClickObj;

    //�������ȗ�������ϐ�
    private bool isNoHit;
    private bool isSpecificTag = false;

    private PlaySound playSound;

    private CheckHitGround checkHG;

    // ObjectScaleEditor�ǉ�
    // �A�^�b�`���邱��
    [SerializeField]
    GameObject ObjectScaleEditor;

    //�����ɂ��炷��
    [SerializeField, Range(0.1f,0.3f)] private float inLine = 0.1f;
    //�������邩�ǂ���
    private bool isMove = false;

    //�@�\���g�p�\���ǂ���
    [SerializeField] private bool isLook = false;


    void Start()
    {
        if (GameObject.Find("AudioCanvas") != null)
        {
            playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        }
        else
        {
            Debug.Log("audio�Ȃ�");
        }
    }


    void Update()
    {
        //�Đ����͕ҏW�@�\�����b�N
        if (GameData.GameEntity.isPlayNow || isLook)
        {
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            return;
        }

        if (!isLook)
        {
            if (Input.GetMouseButtonDown(0) &&
                (ModeData.ModeEntity.mode == ModeData.Mode.normal || ModeData.ModeEntity.mode == ModeData.Mode.moveANDdirect))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, Mathf.Infinity, lm);

                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                //�n���h���������ꍇ
                //if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Handle"))
                //{
                //    Debug.Log("�n���h���_���[��");
                //    return;
                //}

                //�I�u�W�F�N�g�����݂��邩�ǂ���
                isNoHit = (hit2d == false);
                //�I�u�W�F�N�g������Ƃ�
                if (!isNoHit)
                {
                    //����̃^�O�̎��Ƀt���O�𗧂Ă�
                    isSpecificTag = new List<string> { "Player", "UnTouch", "Marcker", "MoveGround" }.Contains(hit2d.collider.tag);
                }

                //UI�⓮���������Ȃ��I�u�W�F�N�g�������炾�����牽�����Ȃ�
                //����
                if (isNoHit || isSpecificTag)
                {
                    if (ClickObj != null)
                    {
                        ObjectScaleEditor.SetActive(false);
                    }
                    ClickObj = null;
                    isMove = false;

                    return;
                }

                ClickObj = hit2d.collider.gameObject;
                if (ClickObj.transform.parent != null && ClickObj.transform.parent.gameObject.name.Contains("Blower"))
                {
                    ClickObj = hit2d.collider.transform.parent.gameObject;

                }
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset = ClickObj.transform.position - mousePos;
                playSound.PlaySE(PlaySound.SE_TYPE.select);

                //�I�u�W�F�N�g���[�u�ƃT�C�Y�E�p�x�ύX���d�����č쓮���Ă��܂����߁A
                //�N���G�C�g�u���b�N�̎l���̍��W�����A�}�E�X�̈ʒu������炩������ɏ������炵�����W�Ȃ�
                //�擾�ł���悤�ɂ���
                //��
                SpriteRenderer sr = ClickObj.GetComponent<SpriteRenderer>();
                Bounds bound = sr.bounds;

                //�}�E�X�̈ʒu���I�u�W�F�N�g�𓮂����Ă����͈͓����ǂ���
                if (mousePos.x >= bound.min.x + inLine && mousePos.x <= bound.max.x - inLine
                    && mousePos.y >= bound.min.y + inLine && mousePos.y <= bound.max.y - inLine)
                {
                    isMove = true;
                    Debug.Log("�������[");
                }

                if (hit2d)
                {
                    Obj = ClickObj;

                    if (Obj.name.Contains("Blower"))
                    {
                        Obj.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = true;
                    }
                    else
                    {
                        Obj.GetComponent<Collider2D>().isTrigger = true;
                    }

                    nowPos = Obj.transform.position;
                    isObjMove = true;
                    ModeData.ModeEntity.mode = ModeData.Mode.moveANDdirect;

                    // �G�f�B�^�[�ǉ�
                    ObjectScaleEditor.SetActive(true);
                    //ObjectScaleEditor.GetComponent<ObjectScaleEditor>().GetObjTransform(Obj);
                }


            }

            if (isObjMove && isMove && ModeData.ModeEntity.mode == ModeData.Mode.moveANDdirect)
            {
                if (Input.GetMouseButton(0))
                {
                    scrWldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    scrWldPos.z = 10;
                    //Obj.transform.position = scrWldPos + offset;


                    //ObjectScaleEditor.GetComponent<ObjectScaleEditor>().GetObjTransform(Obj);

                }
                else if (Input.GetMouseButtonUp(0))
                {
                    checkHG = Obj.GetComponent<CheckHitGround>();
                    if (checkHG.ReturnHit())
                    {
                        Obj.transform.position = nowPos;
                    }

                    playSound.PlaySE(PlaySound.SE_TYPE.objMove);
                    isObjMove = false;
                    if (Obj.name.Contains("Blower"))
                    {
                        Obj.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = false;
                    }
                    else
                    {
                        Obj.GetComponent<Collider2D>().isTrigger = false;
                    }
                    isMove = false;
                    Obj = null;
                    ModeData.ModeEntity.mode = ModeData.Mode.normal;
                }
            }
        }

        //delete�L�[�őI�����Ă�I�u�W�F�N�g������
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (ClickObj != null)
            {
                Destroy(ClickObj);
                ClickObj = null;
                isMove = false;
                ObjectScaleEditor.SetActive(false);
            }
        }
    }

    public GameObject ReturnClickObj() => ClickObj;
}
