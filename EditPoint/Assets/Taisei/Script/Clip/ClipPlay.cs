using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

//�N���b�v�̃��C��
//�N���b�v�ƃI�u�W�F�N�g�̕R�Â���A�R�Â����I�u�W�F�N�g�̕\����\���Ȃ�
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

    private float startTime = 0f;       //�N���b�v�̊J�n����
    private float maxTime = 0f;         //�N���b�v�̒���

    private MoveGroundManager MGManager;
    private MoveGround move;

    private GetConnectClip getConnectClip;  //���̃N���b�v�ɕR�Â��Ă���I�u�W�F�N�g�ɁA���̃N���b�v��n���p

    private CheckOverlap checkOverlap = new CheckOverlap();     //�ق��̃N���b�v��^�C���o�[���d�Ȃ������ǂ����𔻒肷��X�N���v�g

    [SerializeField] private Materials materialsData;       //�N���b�v��I���������A�R�Â��Ă���I�u�W�F�N�g�ɃA�E�g���C��������p

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
            for(int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
            {
                if(ConnectObj[connectObjNum].TryGetComponent<MoveGround>(out MoveGround moveGroundScript))
                {
                    moveGround.Add(moveGroundScript);   
                }

                if(ConnectObj[connectObjNum].TryGetComponent<CheckClipConnect>(out checkClip))
                {
                    checkClip.ConnectClip();
                    checkClip.GetClipPlay(this.gameObject);
                }

                if (ConnectObj[connectObjNum].TryGetComponent<GetConnectClip>(out getConnectClip))
                {
                    getConnectClip.GetAttachClip(this.gameObject);
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

                                for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
                                {
                                    if (clickedObject.name != ConnectObj[connectObjNum].name)
                                    {
                                        Debug.Log("�V�����I�u�W�F�N�g�ǉ�");
                                        ConnectObj.Add(clickedObject);
                                        checkClip = clickedObject.GetComponent<CheckClipConnect>();
                                        checkClip.ConnectClip();
                                        checkClip.GetClipPlay(this.gameObject);
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
                                    checkClip.GetClipPlay(this.gameObject);
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
            manualTime = ((float)Math.Truncate(dis / TimelineData.TimelineEntity.oneTickWidth * 10) / 10) / 2;

            //�^�C���o�[���蓮�œ������Ă鎞
            if (TimeData.TimeEntity.isDragMode)
            {
                for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
                {
                    //�������I�u�W�F�N�g�̎�
                    if (ConnectObj[connectObjNum].TryGetComponent<MoveGround>(out move))
                    {
                        //�N���b�v�̊J�n�b����n��
                        //
                        move.GetClipTime_Manual(startTime + manualTime);
                    }
                }
            }
            //�^�C���o�[�������œ����Ă���Ƃ�
            else
            {
                for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
                {
                    //�������̃I�u�W�F�N�g�̎�
                    if (ConnectObj[connectObjNum].TryGetComponent<MoveGround>(out move))
                    {
                        //�N���b�v�̊J�n�b����n��
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
        for(int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
        {
            ConnectObj.Remove(ConnectObj[connectObjNum]);
        }
    }

    /// <summary>
    /// �R�Â����I�u�W�F�N�g���擾
    /// </summary>
    public List<GameObject> ReturnConnectObj() => ConnectObj;

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
        getConnectClip = _outGetObj.GetComponent<GetConnectClip>();
        getConnectClip.GetAttachClip(this.gameObject);
        checkClip = _outGetObj.GetComponent<CheckClipConnect>();
        checkClip.ConnectClip();
        checkClip.GetClipPlay(this.gameObject);
    }

    /// <summary>
    /// �N���b�v���Đ����Ă��邩�ǂ���
    /// </summary>
    private void ClipPlayNow()
    {
        if (checkOverlap.IsOverlap(rect_Clip, rect_timeBar))
        {
            //�^�C���o�[�ƐڐG���Ă���Ƃ�
            for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
            {
                //��\����Ԃ�������
                if (!ConnectObj[connectObjNum].activeSelf)
                {
                    ConnectObj[connectObjNum].SetActive(true);
                }
            }
        }
        else
        {
            //�^�C���o�[�ƐڐG���Ă��Ȃ��Ƃ�
            for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
            {
                //�������������Ƃ�
                if (ConnectObj[connectObjNum].name.Contains("MoveGround"))
                {
                    //�v���C���[��MoveGround�̎q�I�u�W�F�N�g�ɂȂ��Ă鎞
                    if (ConnectObj[connectObjNum].transform.Find("Player") != null)
                    {
                        //�q�I�u�W�F�N�g����������
                        ConnectObj[connectObjNum].transform.Find("Player").gameObject.transform.parent = null;
                    }

                    //��\���ɂ���O�ɏ����ʒu�ɖ߂�
                    ConnectObj[connectObjNum].GetComponent<MoveGround>().SetStartPos();
                }
                //�\����Ԃ�������
                if (ConnectObj[connectObjNum].activeSelf)
                {
                    ConnectObj[connectObjNum].SetActive(false);

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
    /// �R�Â���ꂽ�I�u�W�F�N�g��Image�̃}�e���A����ύX����
    /// </summary>
    /// <param name="_materialNum">�ύX�������}�e���A���̔ԍ�
    /// 0=�I������Ă��Ȃ���, 1=�I�𒆂̎�</param>
    public void ConnectObjMaterialChange(int _materialNum)
    {
        for(int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
        {
            if(ConnectObj[connectObjNum].TryGetComponent<SpriteRenderer>(out var ConnectSprite))
            {
                ConnectSprite.material = materialsData.MaterialData[_materialNum];
            }
        }
    }

    /// <summary>
    /// �N���b�v�̍ő厞�Ԃ��v�Z
    /// </summary>
    public void CalculationMaxTime()
    {
        maxTime = (rect_Clip.rect.width / (TimelineData.TimelineEntity.oneTickWidth * 2)) + startTime;
    }

    /// <summary>
    /// ����̃I�u�W�F�N�g�����X�g����폜
    /// </summary>
    public void ConnectObjRemove(GameObject obj)
    {
        //�����̃I�u�W�F�N�g�����X�g���猟��
        for(int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
        {
            if(obj == ConnectObj[connectObjNum])
            {
                ConnectObj.Remove(ConnectObj[connectObjNum]);
                break;
            }
        }
    }


    /// <summary>
    /// �N���b�v�Ɋ֘A�t���Ă���I�u�W�F�N�g������
    /// </summary>
    public void ClipObjDestroy()
    {
        for(int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
        {
            if (ConnectObj[connectObjNum].name.Contains("MoveGround"))
            {
                //�v���C���[��MoveGround�̎q�I�u�W�F�N�g�ɂȂ��Ă鎞
                if (ConnectObj[connectObjNum].transform.Find("Player") != null)
                {
                    //�q�I�u�W�F�N�g����������
                    ConnectObj[connectObjNum].transform.Find("Player").gameObject.transform.parent = null;
                }
                MGManager.DeleteMoveGrounds(ConnectObj[connectObjNum]);
            }
            Destroy(ConnectObj[connectObjNum]);
            Debug.Log("test");
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
    public float ReturnMaxTime() => maxTime;
}
