using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    private ClipGenerator clipGenerator;

    private bool b_getObjMode = false;

    [SerializeField] private Image buttonImage;

    private ObjectMove objectMove;

    void Start()
    {
        //�^�C���o�[��RectTransform���擾
        rect_timeBar = GameObject.Find("Timebar").GetComponent<RectTransform>();
        clipGenerator = GameObject.Find("AllClip").GetComponent<ClipGenerator>();
        objectMove = GameObject.Find("GameManager").GetComponent<ObjectMove>();

        //���������N���b�v�̏ꍇ
        if (correspondenceObj.Count == 0)
        {
            clipName.text = "���������N���b�v" + clipGenerator.ReturnCount();
        }
    }

    private void Update()
    {
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
                            GameObject clickedObject = hit.collider.gameObject;
                            Debug.Log(clickedObject);

                            for (int i = 0; i < correspondenceObj.Count; i++)
                            {
                                if (clickedObject.name != correspondenceObj[i].name)
                                {
                                    Debug.Log("�V�����I�u�W�F�N�g�ǉ�");
                                    correspondenceObj.Add(clickedObject);
                                }
                            }
                            if (correspondenceObj.Count == 0)
                            {
                                Debug.Log("�V�����I�u�W�F�N�g�ǉ�");
                                correspondenceObj.Add(clickedObject);
                            }
                        }
                    }
                }
            }
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
            f_timer = 0f;
        }

    }

    //�I�u�W�F�N�g�擾���[�h�t���O��ύX
    public void OnGetObj()
    {
        b_getObjMode = b_getObjMode == false ? true : false;
        objectMove.ObjSetMode(b_getObjMode);
        buttonImage.color = b_getObjMode == false ? Color.white : Color.red;
    }


    public float ReturnClipTime()
    {
        return f_timer;
    }

    public bool ReturnTriggerTimeber()
    {
        return IsOverlapping(rect_Clip, rect_timeBar);
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