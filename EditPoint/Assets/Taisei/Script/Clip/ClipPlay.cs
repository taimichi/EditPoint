using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClipPlay : MonoBehaviour
{
    private RectTransform rect_timeBar;
    [SerializeField] private RectTransform rect_Clip;
    [SerializeField] private Text clipName;

    private float f_timer = 0;
    /// <summary>
    /// �N���b�v���Đ����邩�ǂ���
    /// </summary>
    private bool b_clipPlay = false;

    [SerializeField] private List<GameObject> ConnectObj = new List<GameObject>();

    private GameObject AllClip;
    private ClipGenerator clipGenerator;

    private bool b_getObjMode = false;

    private ObjectMove objectMove;

    [SerializeField] private ClipSpeed clipSpeed;
    private float speed = 0f;
    private List<MoveGround> moveGround = new List<MoveGround>();
    private CheckClipConnect checkClip;

    private AddTextManager addTextManager;

    [SerializeField] private TimelineData timelineData;
    [SerializeField] private TimeData timeData;

    private RectTransform rect_grandParent;
    private float f_manualTime = 0;

    private MoveGround move;

    private GetCopyObj gpo;


    void Start()
    {
        f_manualTime = 0f;
        rect_grandParent = rect_Clip.parent.parent.GetComponent<RectTransform>();

        //�^�C���o�[��RectTransform���擾
        rect_timeBar = GameObject.Find("Timebar").GetComponent<RectTransform>();
        AllClip = GameObject.Find("AllClip");
        clipGenerator = AllClip.GetComponent<ClipGenerator>();
        addTextManager = AllClip.GetComponent<AddTextManager>();
        objectMove = GameObject.Find("GameManager").GetComponent<ObjectMove>();

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
    }

    private void Update()
    {
        speed = clipSpeed.ReturnPlaySpeed();

        //�N���b�v�ɕR�Â��I�u�W�F�N�g���Ȃ��Ƃ����Ȃ��Ƃ�
        if (ConnectObj.Count == 0)
        {
            clipName.text = "����ۂ̃N���b�v";
        }

        //�I�u�W�F�N�g�擾
        if (b_getObjMode)
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
                Debug.Log("���x�ύX");
            }
        }

        //�N���b�v�ƃ^�C���o�[���G��Ă�Ƃ��̂�
        if (CheckOverrap(rect_Clip, rect_timeBar))
        {
            //�N���b�v�̌o�ߎ���
            Vector3 leftEdge = rect_grandParent.InverseTransformPoint(rect_Clip.position) + new Vector3(-rect_Clip.rect.width * rect_Clip.pivot.x, 0, 0);
            float dis = rect_timeBar.localPosition.x - leftEdge.x;
            f_manualTime = (float)Math.Truncate(dis / timelineData.f_oneTickWidht * 10) / 10;

            //�^�C���o�[���蓮�œ������Ă鎞
            if (timeData.b_DragMode)
            {
                for (int i = 0; i < ConnectObj.Count; i++)
                {
                    if (ConnectObj[i].GetComponent<MoveGround>())
                    {
                        move = ConnectObj[i].GetComponent<MoveGround>();
                        move.GetClipTime(f_manualTime);
                    }
                }
            }
        }

        ClipPlayNow();
    }

    private void FixedUpdate()
    {
        ClipPlayNow();

    }


    //�O������̃Q�[���I�u�W�F�N�g���擾
    public void OutGetObj(GameObject _outGetObj)
    {
        ConnectObj.Add(_outGetObj);
        clipName.text = "���g�̂���N���b�v";
        checkClip = _outGetObj.GetComponent<CheckClipConnect>();
        checkClip.ConnectClip();
    }

    /// <summary>
    /// �N���b�v���Đ����Ă��邩�ǂ���
    /// </summary>
    private void ClipPlayNow()
    {
        if (CheckOverrap(rect_Clip, rect_timeBar))
        {
            //�^�C���o�[�ƐڐG���Ă���Ƃ�
            for (int i = 0; i < ConnectObj.Count; i++)
            {
                ConnectObj[i].SetActive(true);
            }
            f_timer += Time.deltaTime;
        }
        else
        {
            //�^�C���o�[�ƐڐG���Ă��Ȃ��Ƃ�
            for (int i = 0; i < ConnectObj.Count; i++)
            {
                ConnectObj[i].SetActive(false);

            }
        }
    }


    /// <summary>
    /// �N���b�v�Ɋ֘A�t���Ă���I�u�W�F�N�g������
    /// </summary>
    public void ClipObjDestroy()
    {
        for(int i = 0; i < ConnectObj.Count; i++)
        {
            Destroy(ConnectObj[i]);
        }
    }

    /// <summary>
    /// �N���b�v�ƃ^�C���o�[���d�Ȃ��Ă��邩���`�F�b�N
    /// </summary>
    /// <param name="clipRect">�N���b�v��RectTransform</param>
    /// <param name="timeberRect">�^�C���o�[��RectTransform</param>
    /// <returns>�d�Ȃ��Ă���=true �d�Ȃ��Ă��Ȃ�=false</returns>
    private bool CheckOverrap(RectTransform clipRect, RectTransform timeberRect)
    {
        // RectTransform�̋��E�����[���h���W�Ŏ擾
        Rect rect1World = GetWorldRect(clipRect);
        Rect rect2World = GetWorldRect(timeberRect);

        // ���E���d�Ȃ��Ă��邩�ǂ������`�F�b�N
        return rect1World.Overlaps(rect2World);
    }
    
    /// <summary>
    /// ���[���h���W�ł̋��E���擾
    /// </summary>
    /// <param name="rt">�擾����RectTransform</param>
    /// <returns>���[���h���W�ł�RectTransform</returns>
    private Rect GetWorldRect(RectTransform rt)
    {
        //�l���̃��[���h���W������z��
        Vector3[] corners = new Vector3[4];
        //RectTransform�̎l���̃��[���h���W���擾
        rt.GetWorldCorners(corners);

        return new Rect(corners[0], corners[2] - corners[0]);
    }
}