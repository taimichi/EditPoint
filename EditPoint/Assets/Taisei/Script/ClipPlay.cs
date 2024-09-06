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

    [SerializeField] private List<GameObject> correspondenceObj = new List<GameObject>();

    private GameObject AllClip;
    private ClipGenerator clipGenerator;

    private bool b_getObjMode = false;

    [SerializeField] private Image buttonImage;

    private ObjectMove objectMove;

    [SerializeField] private ClipSpeed clipSpeed;
    private float speed = 0f;
    private List<MoveGround> moveGround = new List<MoveGround>();
    private CheckClipConnect checkClip;

    private AddTextManager addTextManager;

    [SerializeField] private TimelineData timelineData;

    private RectTransform rect_grandParent;
    private float f_manualTime = 0;

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
        if (correspondenceObj.Count == 0)
        {
            clipName.text = "����ۂ̃N���b�v" + clipGenerator.ReturnCount();
        }
        else
        {
            for(int i = 0; i < correspondenceObj.Count; i++)
            {
                if(correspondenceObj[i].GetComponent<MoveGround>() == true)
                {
                    moveGround.Add(correspondenceObj[i].GetComponent<MoveGround>());   
                }
            }
        }
    }

    private void Update()
    {
        speed = clipSpeed.ReturnPlaySpeed();

        //�I�u�W�F�N�g�擾
        if (b_getObjMode)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0)) // ���N���b�N
                {
                    Physics2D.Simulate(Time.fixedDeltaTime);
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

                                for (int i = 0; i < correspondenceObj.Count; i++)
                                {
                                    if (clickedObject.name != correspondenceObj[i].name)
                                    {
                                        Debug.Log("�V�����I�u�W�F�N�g�ǉ�");
                                        correspondenceObj.Add(clickedObject);
                                        checkClip = clickedObject.GetComponent<CheckClipConnect>();
                                        checkClip.ConnectClip();
                                        if (clickedObject.GetComponent<MoveGround>() == true)
                                        {
                                            moveGround.Add(clickedObject.GetComponent<MoveGround>());
                                        }
                                        addTextManager.AddObj();
                                    }
                                }
                                if (correspondenceObj.Count == 0)
                                {
                                    Debug.Log("�V�����I�u�W�F�N�g�ǉ�");
                                    correspondenceObj.Add(clickedObject);
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
        if (IsOverlapping(rect_Clip, rect_timeBar))
        {
            //�N���b�v�̌o�ߎ���
            Vector3 leftEdge = rect_grandParent.InverseTransformPoint(rect_Clip.position) + new Vector3(-rect_Clip.rect.width * rect_Clip.pivot.x, 0, 0);
            float dis = rect_timeBar.localPosition.x - leftEdge.x;
            f_manualTime = (float)Math.Truncate(dis / timelineData.f_oneTickWidht * 10) / 10;
            Debug.Log("�^�C���o�[�蓮����" + f_manualTime + "�b");
        }
    }

    private void FixedUpdate()
    {

        if (IsOverlapping(rect_Clip, rect_timeBar))
        {
            Debug.Log("UI�I�u�W�F�N�g���ڐG���Ă��܂�");
            b_clipPlay = true;
        }
        else
        {
            Debug.Log("UI�I�u�W�F�N�g���ڐG���Ă��܂���");
            b_clipPlay = false;
        }


        //�N���b�v�Đ����̏���
        if (b_clipPlay)
        {
            for(int i = 0; i < correspondenceObj.Count; i++)
            {
                correspondenceObj[i].SetActive(true);
            }
            f_timer += Time.deltaTime;

        }
        //�N���b�v�Đ����ĂȂ��Ƃ��̏���
        else
        {
            for (int i = 0; i < correspondenceObj.Count; i++)
            {
                correspondenceObj[i].SetActive(false);
            }
        }

    }

    //�I�u�W�F�N�g�擾���[�h�t���O��ύX
    public void OnGetObj()
    {
        b_getObjMode = b_getObjMode == false ? true : false;
        objectMove.ObjSetMode(b_getObjMode);
        buttonImage.color = b_getObjMode == false ? Color.white : Color.red;
    }

    /// <summary>
    /// �^�C���o�[�������œ������Ă鎞�A�N���b�v�̌��ݎ��Ԃ�Ԃ�
    /// </summary>
    /// <returns>f_timer(����)</returns>
    public float ReturnClipTime()
    {
        return f_timer;
    }

    /// <summary>
    /// �^�C���o�[���蓮�œ������Ă鎞�A�N���b�v�̌��ݎ��Ԃ�Ԃ�
    /// </summary>
    /// <returns>f_manualTime(�蓮)</returns>
    public float ReturnManualTime()
    {
        return f_manualTime;
    }

    /// <summary>
    /// �N���b�v�ƃ^�C���o�[���d�Ȃ��Ă��邩���`�F�b�N
    /// </summary>
    /// <param name="rect1">�N���b�v��RectTransform</param>
    /// <param name="rect2">�^�C���o�[��RectTransform</param>
    /// <returns>�d�Ȃ��Ă���=true �d�Ȃ��Ă��Ȃ�=false</returns>
    private bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        // RectTransform�̋��E�����[���h���W�Ŏ擾
        Rect rect1World = GetWorldRect(rect1);
        Rect rect2World = GetWorldRect(rect2);

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
