using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClipPlay : MonoBehaviour
{
    private RectTransform rect_timeBar; //�^�C���o�[��RectTransform
    [SerializeField] private RectTransform rect_Clip;   //�N���b�v��RectTransform
    [SerializeField] private Text clipName; //�N���b�v�̖��O��\������e�L�X�g

    [SerializeField] private List<GameObject> ConnectObj = new List<GameObject>();  //�N���b�v�ɕR�Â����Ă���I�u�W�F�N�g    

    private GameObject ClipManager; //�e�I�u�W�F�N�g
    private ClipGenerator clipGenerator;    //�N���b�v�����p�X�N���v�g

    private bool isGetObjMode = false;


    [SerializeField] private ClipSpeed clipSpeed;   //�N���b�v�̍Đ����x�Ɋւ���X�N���v�g
    private float speed = 0f;       //�N���b�v�̍Đ����x
    private List<MoveGround> moveGround = new List<MoveGround>(); //�������̃X�N���v�g
    private CheckClipConnect checkClip; //�I�u�W�F�N�g���N���b�v�ɕR�Â����Ă��邩�m�F����p�̃X�N���v�g

    private AddTextManager addTextManager;

    private float manualTime = 0; //�^�C���o�[���蓮�œ��������Ƃ��̎���

    private MoveGround move;

    private GetCopyObj gpo;

    private float startTime = 0f;
    private float maxTime = 0f;

    private MoveGroundManager MGManager;

    private CheckOverlap checkOverlap = new CheckOverlap();

    private void Awake()
    {
        startTime = 0f;
        manualTime = 0f;
    }

    void Start()
    {
        //�^�C���o�[��RectTransform���擾
        rect_timeBar = GameObject.Find("Timebar").GetComponent<RectTransform>();
        ClipManager = GameObject.Find("ClipManager");
        clipGenerator = ClipManager.GetComponent<ClipGenerator>();
        addTextManager = ClipManager.GetComponent<AddTextManager>();

        MGManager = GameObject.Find("GameManager").GetComponent<MoveGroundManager>();

        //���������N���b�v�̏ꍇ
        if (ConnectObj.Count == 0)
        {
            clipName.text = "����ۂ̃N���b�v" + clipGenerator.ReturnCount();
        }
        else
        {
            for(int i = 0; i < ConnectObj.Count; i++)
            {
                if(ConnectObj[i].GetComponent<MoveGround>() == true)
                {
                    moveGround.Add(ConnectObj[i].GetComponent<MoveGround>());   
                }

                if(ConnectObj[i].GetComponent<CheckClipConnect>() == true)
                {
                    checkClip = ConnectObj[i].GetComponent<CheckClipConnect>();
                    checkClip.ConnectClip();
                }

                if (ConnectObj[i].GetComponent<GetCopyObj>() == true)
                {
                    gpo = ConnectObj[i].GetComponent<GetCopyObj>();
                    gpo.GetAttachClip(this.gameObject);
                }
            }
        }

        CalculationMaxTime();
    }

    private void Update()
    {
        CalculationMaxTime();
        speed = clipSpeed.ReturnPlaySpeed();

        //�N���b�v�ɕR�Â��I�u�W�F�N�g���Ȃ��Ƃ����Ȃ��Ƃ�
        if (ConnectObj.Count == 0)
        {
            clipName.text = "����ۂ̃N���b�v";
        }

        //�I�u�W�F�N�g�擾
        if (isGetObjMode)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0)) // ���N���b�N
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                    Debug.Log(hit.collider.gameObject.name);

                    if (hit.collider != null)
                    {
                        if(hit.collider.tag != "Marcker")
                        {
                            if(hit.collider.tag == "CreateBlock")
                            {
                                GameObject clickedObject = hit.collider.gameObject;
                                Debug.Log(clickedObject);

                                for (int i = 0; i < ConnectObj.Count; i++)
                                {
                                    if (clickedObject.name != ConnectObj[i].name)
                                    {
                                        Debug.Log("�V�����I�u�W�F�N�g�ǉ�");
                                        ConnectObj.Add(clickedObject);
                                        checkClip = clickedObject.GetComponent<CheckClipConnect>();
                                        checkClip.ConnectClip();
                                        if (clickedObject.GetComponent<MoveGround>() == true)
                                        {
                                            moveGround.Add(clickedObject.GetComponent<MoveGround>());
                                        }
                                        addTextManager.AddObj();
                                    }
                                }
                                if (ConnectObj.Count == 0)
                                {
                                    Debug.Log("�V�����I�u�W�F�N�g�ǉ�");
                                    ConnectObj.Add(clickedObject);
                                    checkClip = clickedObject.GetComponent<CheckClipConnect>();
                                    checkClip.ConnectClip();
                                    if (clickedObject.GetComponent<MoveGround>() == true)
                                    {
                                        moveGround.Add(clickedObject.GetComponent<MoveGround>());
                                    }
                                    addTextManager.AddObj();
                                }
                            }
                        }
                    }
                    //�N���b�v�̖��O��ύX
                    clipName.text = "���g�̂���N���b�v";
                }
            }
        }

        //�Đ����x�𔽉f
        if (moveGround.Count != 0)
        {
            for(int i = 0; i < moveGround.Count; i++)
            {
                moveGround[i].ChangePlaySpeed(speed);
            }
        }

        //�N���b�v�ƃ^�C���o�[���G��Ă�Ƃ��̂�
        if (checkOverlap.IsOverlap(rect_Clip, rect_timeBar))
        {
            //�N���b�v�̌o�ߎ���
            Vector3 leftEdge = rect_Clip.localPosition + new Vector3(-rect_Clip.rect.width * rect_Clip.pivot.x, 0, 0);
            float dis = rect_timeBar.localPosition.x - leftEdge.x;
            manualTime = ((float)Math.Truncate(dis / TimelineData.TimelineEntity.f_oneTickWidht * 10) / 10) / 2;

            //�^�C���o�[���蓮�œ������Ă鎞
            if (TimeData.TimeEntity.isDragMode)
            {
                for (int i = 0; i < ConnectObj.Count; i++)
                {
                    if (ConnectObj[i].GetComponent<MoveGround>())
                    {
                        move = ConnectObj[i].GetComponent<MoveGround>();
                        move.GetClipTime_Manual(startTime + manualTime);
                    }
                }
            }
            else
            {
                for (int i = 0; i < ConnectObj.Count; i++)
                {
                    if (ConnectObj[i].GetComponent<MoveGround>())
                    {
                        move = ConnectObj[i].GetComponent<MoveGround>();
                        move.GetClipTime_Auto(startTime);
                    }
                }
            }
        }

        ClipPlayNow();
    }

    /// <summary>
    /// �I�u�W�F�N�g�Ƃ̕R�Â�������
    /// </summary>
    public void DestroyConnectObj()
    {
        for(int i = 0; i < ConnectObj.Count; i++)
        {
            ConnectObj.Remove(ConnectObj[i]);
        }
    }

    /// <summary>
    /// �R�Â����I�u�W�F�N�g���擾
    /// </summary>
    public List<GameObject> ReturnConnectObj()
    {
        return ConnectObj;
    }

    /// <summary>
    /// �O������̃Q�[���I�u�W�F�N�g���擾
    /// </summary>
    /// <param name="_outGetObj">�O������̃I�u�W�F�N�g</param>
    public void OutGetObj(GameObject _outGetObj)
    {
        ConnectObj.Add(_outGetObj);
        if (this.gameObject.tag == "CreateClip")
        {
            clipName.text = "���g�̂���N���b�v";
        }
        gpo = _outGetObj.GetComponent<GetCopyObj>();
        gpo.GetAttachClip(this.gameObject);
        checkClip = _outGetObj.GetComponent<CheckClipConnect>();
        checkClip.ConnectClip();
    }

    /// <summary>
    /// �N���b�v���Đ����Ă��邩�ǂ���
    /// </summary>
    private void ClipPlayNow()
    {
        if (checkOverlap.IsOverlap(rect_Clip, rect_timeBar))
        {
            //�^�C���o�[�ƐڐG���Ă���Ƃ�
            for (int i = 0; i < ConnectObj.Count; i++)
            {
                //��\����Ԃ�������
                if (!ConnectObj[i].activeSelf)
                {
                    ConnectObj[i].SetActive(true);
                }
            }
        }
        else
        {
            //�^�C���o�[�ƐڐG���Ă��Ȃ��Ƃ�
            for (int i = 0; i < ConnectObj.Count; i++)
            {
                //�������������Ƃ�
                if (ConnectObj[i].name.Contains("MoveGround"))
                {
                    //�v���C���[��MoveGround�̎q�I�u�W�F�N�g�ɂȂ��Ă鎞
                    if (ConnectObj[i].transform.Find("Player") != null)
                    {
                        //�q�I�u�W�F�N�g����������
                        ConnectObj[i].transform.Find("Player").gameObject.transform.parent = null;
                    }

                    //��\���ɂ���O�ɏ����ʒu�ɖ߂�
                    ConnectObj[i].GetComponent<MoveGround>().SetStartPos();
                }
                //�\����Ԃ�������
                if (ConnectObj[i].activeSelf)
                {
                    ConnectObj[i].SetActive(false);

                    // �N���b�v�؂ꂽ�Ƃ��̃G�f�B�^�[��\��
                    GameObject objectEditor = GameObject.Find("ObjectEditor");
                    if (objectEditor != null)
                    {
                        objectEditor.SetActive(false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// �N���b�v�̍ő厞�Ԃ��v�Z
    /// </summary>
    public void CalculationMaxTime()
    {
        maxTime = (rect_Clip.rect.width / (TimelineData.TimelineEntity.f_oneTickWidht * 2)) + startTime;
    }


    /// <summary>
    /// �N���b�v�Ɋ֘A�t���Ă���I�u�W�F�N�g������
    /// </summary>
    public void ClipObjDestroy()
    {
        for(int i = 0; i < ConnectObj.Count; i++)
        {
            if (ConnectObj[i].name.Contains("MoveGround"))
            {
                //�v���C���[��MoveGround�̎q�I�u�W�F�N�g�ɂȂ��Ă鎞
                if (ConnectObj[i].transform.Find("Player") != null)
                {
                    //�q�I�u�W�F�N�g����������
                    ConnectObj[i].transform.Find("Player").gameObject.transform.parent = null;
                }
                MGManager.DeleteMoveGrounds(ConnectObj[i]);
            }
        Destroy(ConnectObj[i]);
        }
    }

    /// <summary>
    /// �J�n���Ԃ��X�V
    /// </summary>
    public void UpdateStartTime(float _newStartTime)
    {
        startTime = _newStartTime;
    }

    /// <summary>
    /// �N���b�v�̍ő厞�Ԃ�Ԃ�
    /// </summary>
    /// <returns>�N���b�v�̍ő厞��</returns>
    public float ReturnMaxTime()
    {
        return maxTime;
    }
}
