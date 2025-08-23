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

    [SerializeField] private ClipSpeed clipSpeed;   //�N���b�v�̍Đ����x�Ɋւ���X�N���v�g
    private float speed = 0f;       //�N���b�v�̍Đ����x
    private List<MoveGround> moveGround = new List<MoveGround>(); //�������̃X�N���v�g
    private CheckClipConnect checkClip; //�I�u�W�F�N�g���N���b�v�ɕR�Â����Ă��邩�m�F����p�̃X�N���v�g

    private float manualTime = 0; //�^�C���o�[���蓮�œ��������Ƃ��̎���

    private float startTime = 0f;       //�N���b�v�̊J�n����
    private float maxTime = 0f;         //�N���b�v�̒���

    private MoveGroundManager MGManager;

    private GetConnectClip getConnectClip;  //���̃N���b�v�ɕR�Â��Ă���I�u�W�F�N�g�ɁA���̃N���b�v��n���p

    private CheckOverlap checkOverlap = new CheckOverlap();     //�ق��̃N���b�v��^�C���o�[���d�Ȃ������ǂ����𔻒肷��X�N���v�g

    [SerializeField] private Materials materialsData;       //�N���b�v��I���������A�R�Â��Ă���I�u�W�F�N�g�ɃA�E�g���C��������p

    private void Awake()
    {
        //���Ԃ�������
        startTime = 0f;
        manualTime = 0f;
    }

    void Start()
    {
        //�^�C���o�[��RectTransform���擾
        rect_timeBar = GameObject.Find("Timebar").GetComponent<RectTransform>();
        //�Q�[���I�u�W�F�N�g�̃N���b�v�}�l�[�W���[���擾
        ClipManager = GameObject.Find("ClipManager");
        //�N���b�v�����X�N���v�g���擾
        clipGenerator = ClipManager.GetComponent<ClipGenerator>();

        //�Q�[���}�l�[�W���[�I�u�W�F�N�g���瓮�����̃}�l�[�W���[�X�N���v�g���擾
        MGManager = GameObject.Find("GameManager").GetComponent<MoveGroundManager>();

        //���������N���b�v�̏ꍇ
        if (ConnectObj.Count == 0)
        {
            //�N���b�v�̖��O��ύX
            clipName.text = "����ۂ̃N���b�v" + clipGenerator.ReturnCount();
        }
        //�����炠��N���b�v�̏ꍇ
        else
        {
            //�N���b�v�ɕR�Â��Ă���I�u�W�F�N�g�̐��̕�����
            for(int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
            {
                //�������������Ƃ�
                if(ConnectObj[connectObjNum].TryGetComponent<MoveGround>(out MoveGround moveGroundScript))
                {
                    moveGround.Add(moveGroundScript);   
                }

                //�I�u�W�F�N�g���ŁA�N���b�v�ƕR�Â���
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

        //�N���b�v�̕b�����v�Z
        CalculationMaxTime();
    }

    private void Update()
    {
        //�N���b�v�̕b�����v�Z
        CalculationMaxTime();
        //�N���b�v�̍Đ����x���擾
        speed = clipSpeed.ReturnPlaySpeed();

        //�N���b�v�ɕR�Â��I�u�W�F�N�g���Ȃ��Ƃ����Ȃ��Ƃ�
        if (ConnectObj.Count == 0)
        {
            clipName.text = "����ۂ̃N���b�v";
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
            //�N���b�v�̌o�ߎ��Ԃ��v�Z
            Vector3 leftEdge = rect_Clip.localPosition + new Vector3(-rect_Clip.rect.width * rect_Clip.pivot.x, 0, 0);
            //�N���b�v��ł̃^�C���o�[�̌��݂̈ʒu
            float dis = rect_timeBar.localPosition.x - leftEdge.x;
            //�蓮�Ń^�C���o�[�𓮂������Ƃ��́A�N���b�v�̌��݂̍Đ��ʒu
            manualTime = ((float)Math.Truncate(dis / TimelineData.TimelineEntity.oneTickWidth * 10) / 10) / 2;

            //�^�C���o�[���蓮�œ������Ă鎞
            if (TimeData.TimeEntity.isDragMode)
            {
                //���������擾
                GameObject moveGround = ReturnConnectMoveObj();
                if (moveGround != null)
                {
                    //�蓮�Đ����̎��Ԃ�n��
                    moveGround.GetComponent<MoveGround>().GetClipTime_Manual(startTime + manualTime);
                }
                
            }
            //�^�C���o�[�������œ����Ă���Ƃ�
            else
            {

            }
        }

        //�N���b�v�̍Đ��֘A�̏���
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
        //�O������n���ꂽ�I�u�W�F�N�g�ɂ��̃N���b�v�ƕR�Â���
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
                    //�\����Ԃɂ���
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
                //�}�e���A���ύX
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
                //���X�g����폜
                MGManager.DeleteMoveGrounds(ConnectObj[connectObjNum]);
            }
            //�I�u�W�F�N�g��j��
            Destroy(ConnectObj[connectObjNum]);
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

    /// <summary>
    /// �R�Â��Ă��铮�����I�u�W�F�N�g��Ԃ�
    /// </summary>
    /// <returns>�������̃Q�[���I�u�W�F�N�g(�����Ȃ��ꍇ��null)</returns>
    public GameObject ReturnConnectMoveObj()
    {
        for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
        {
            //�������I�u�W�F�N�g�̎�
            if (ConnectObj[connectObjNum].name.Contains("MoveGround"))
            {
                //�������I�u�W�F�N�g��Ԃ�
                return ConnectObj[connectObjNum];
            }
        }

        //�����Ȃ������ꍇ��null��Ԃ�
        return null;
    }
}
