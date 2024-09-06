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
    /// クリップを再生するかどうか
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

    void Start()
    {
        //タイムバーのRectTransformを取得
        rect_timeBar = GameObject.Find("Timebar").GetComponent<RectTransform>();
        AllClip = GameObject.Find("AllClip");
        clipGenerator = AllClip.GetComponent<ClipGenerator>();
        addTextManager = AllClip.GetComponent<AddTextManager>();
        objectMove = GameObject.Find("GameManager").GetComponent<ObjectMove>();

        //生成したクリップの場合
        if (correspondenceObj.Count == 0)
        {
            clipName.text = "生成したクリップ" + clipGenerator.ReturnCount();
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

        //オブジェクト取得
        if (b_getObjMode)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0)) // 左クリック
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
                                        Debug.Log("新しいオブジェクト追加");
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
                                    Debug.Log("新しいオブジェクト追加");
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
                }
            }
        }

        //再生速度を反映
        if (moveGround.Count != 0)
        {
            for(int i = 0; i < moveGround.Count; i++)
            {
                moveGround[i].ChangePlaySpeed(speed);
                Debug.Log("速度変更");
            }
        }

    }

    private void FixedUpdate()
    {

        if (IsOverlapping(rect_Clip, rect_timeBar))
        {
            Debug.Log("UIオブジェクトが接触しています");
            b_clipPlay = true;
        }
        else
        {
            Debug.Log("UIオブジェクトが接触していません");
            b_clipPlay = false;
        }


        //クリップ再生中の処理
        if (b_clipPlay)
        {
            for(int i = 0; i < correspondenceObj.Count; i++)
            {
                correspondenceObj[i].SetActive(true);
            }
            f_timer += Time.deltaTime;

        }
        //クリップ再生してないときの処理
        else
        {
            for (int i = 0; i < correspondenceObj.Count; i++)
            {
                correspondenceObj[i].SetActive(false);
            }
        }

    }

    //オブジェクト取得モードフラグを変更
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

    /// <summary>
    /// クリップとタイムバーが重なっているかをチェック
    /// </summary>
    /// <param name="rect1">クリップのRectTransform</param>
    /// <param name="rect2">タイムバーのRectTransform</param>
    /// <returns>重なっている=true 重なっていない=false</returns>
    private bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        // RectTransformの境界をワールド座標で取得
        Rect rect1World = GetWorldRect(rect1);
        Rect rect2World = GetWorldRect(rect2);

        // 境界が重なっているかどうかをチェック
        return rect1World.Overlaps(rect2World);
    }
    
    /// <summary>
    /// ワールド座標での境界を取得
    /// </summary>
    /// <param name="rt">取得するRectTransform</param>
    /// <returns>ワールド座標でのRectTransform</returns>
    private Rect GetWorldRect(RectTransform rt)
    {
        //四隅のワールド座標を入れる配列
        Vector3[] corners = new Vector3[4];
        //RectTransformの四隅のワールド座標を取得
        rt.GetWorldCorners(corners);

        return new Rect(corners[0], corners[2] - corners[0]);
    }
}
