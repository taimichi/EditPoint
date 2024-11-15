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

    [SerializeField] private List<GameObject> ConnectObj = new List<GameObject>();

    private GameObject AllClip;
    private ClipGenerator clipGenerator;

    private bool b_getObjMode = false;


    [SerializeField] private ClipSpeed clipSpeed;
    private float speed = 0f;
    private List<MoveGround> moveGround = new List<MoveGround>();
    private CheckClipConnect checkClip;

    private AddTextManager addTextManager;

    private RectTransform rect_grandParent;
    private float f_manualTime = 0;

    private MoveGround move;

    private GetCopyObj gpo;

    private float startTime = 0f;
    private float maxTime = 0f;

    private void Awake()
    {
        startTime = 0f;
        f_manualTime = 0f;
    }

    void Start()
    {
        rect_grandParent = rect_Clip.parent.parent.GetComponent<RectTransform>();

        //タイムバーのRectTransformを取得
        rect_timeBar = GameObject.Find("Timebar").GetComponent<RectTransform>();
        AllClip = GameObject.Find("AllClip");
        clipGenerator = AllClip.GetComponent<ClipGenerator>();
        addTextManager = AllClip.GetComponent<AddTextManager>();

        //生成したクリップの場合
        if (ConnectObj.Count == 0)
        {
            clipName.text = "空っぽのクリップ" + clipGenerator.ReturnCount();
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

        //クリップに紐づくオブジェクトがないときがないとき
        if (ConnectObj.Count == 0)
        {
            clipName.text = "空っぽのクリップ";
        }

        //オブジェクト取得
        if (b_getObjMode)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0)) // 左クリック
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
                                        Debug.Log("新しいオブジェクト追加");
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
                                    Debug.Log("新しいオブジェクト追加");
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
                    //クリップの名前を変更
                    clipName.text = "中身のあるクリップ";
                }
            }
        }

        //再生速度を反映
        if (moveGround.Count != 0)
        {
            for(int i = 0; i < moveGround.Count; i++)
            {
                moveGround[i].ChangePlaySpeed(speed);
            }
        }

        //クリップとタイムバーが触れてるときのみ
        if (CheckOverrap(rect_Clip, rect_timeBar))
        {
            //クリップの経過時間
            Vector3 leftEdge = rect_grandParent.InverseTransformPoint(rect_Clip.position) + new Vector3(-rect_Clip.rect.width * rect_Clip.pivot.x, 0, 0);
            float dis = rect_timeBar.localPosition.x - leftEdge.x;
            f_manualTime = (float)Math.Truncate(dis / TimelineData.TimelineEntity.f_oneTickWidht * 10) / 10;

            //タイムバーを手動で動かしてる時
            if (TimeData.TimeEntity.b_DragMode)
            {
                for (int i = 0; i < ConnectObj.Count; i++)
                {
                    if (ConnectObj[i].GetComponent<MoveGround>())
                    {
                        move = ConnectObj[i].GetComponent<MoveGround>();
                        move.GetClipTime_Manual(f_manualTime);
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
    /// オブジェクトとの紐づけを解除
    /// </summary>
    public void DestroyConnectObj()
    {
        for(int i = 0; i < ConnectObj.Count; i++)
        {
            ConnectObj.Remove(ConnectObj[i]);
        }
    }

    /// <summary>
    /// 紐づけたオブジェクトを取得
    /// </summary>
    public List<GameObject> ReturnConnectObj()
    {
        return ConnectObj;
    }

    /// <summary>
    /// 外部からのゲームオブジェクトを取得
    /// </summary>
    /// <param name="_outGetObj">外部からのオブジェクト</param>
    public void OutGetObj(GameObject _outGetObj)
    {
        ConnectObj.Add(_outGetObj);
        if (this.gameObject.tag == "CreateClip")
        {
            clipName.text = "中身のあるクリップ";
        }
        gpo = _outGetObj.GetComponent<GetCopyObj>();
        gpo.GetAttachClip(this.gameObject);
        checkClip = _outGetObj.GetComponent<CheckClipConnect>();
        checkClip.ConnectClip();
    }

    /// <summary>
    /// クリップを再生しているかどうか
    /// </summary>
    private void ClipPlayNow()
    {
        if (CheckOverrap(rect_Clip, rect_timeBar))
        {
            //タイムバーと接触しているとき
            for (int i = 0; i < ConnectObj.Count; i++)
            {
                //非表示状態だったら
                if (!ConnectObj[i].activeSelf)
                {
                    ConnectObj[i].SetActive(true);
                }
            }
        }
        else
        {
            //タイムバーと接触していないとき
            for (int i = 0; i < ConnectObj.Count; i++)
            {
                if (ConnectObj[i].name.Contains("MoveGround"))
                {
                    //プレイヤーがMoveGroundの子オブジェクトになってる時
                    if (ConnectObj[i].transform.Find("Player") != null)
                    {
                        //子オブジェクトを解除する
                        ConnectObj[i].transform.Find("Player").gameObject.transform.parent = null;
                    }
                }
                //表示状態だったら
                if (ConnectObj[i].activeSelf)
                {
                    ConnectObj[i].SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// クリップの最大時間を計算
    /// </summary>
    public void CalculationMaxTime()
    {
        maxTime = rect_Clip.rect.width / (TimelineData.TimelineEntity.f_oneTickWidht * 2);
    }


    /// <summary>
    /// クリップに関連付けてあるオブジェクトを消す
    /// </summary>
    public void ClipObjDestroy()
    {
        for(int i = 0; i < ConnectObj.Count; i++)
        {
            if (ConnectObj[i].name.Contains("MoveGround"))
            {
                //プレイヤーがMoveGroundの子オブジェクトになってる時
                if (ConnectObj[i].transform.Find("Player") != null)
                {
                    //子オブジェクトを解除する
                    ConnectObj[i].transform.Find("Player").gameObject.transform.parent = null;
                }
            }
        Destroy(ConnectObj[i]);
        }
    }

    /// <summary>
    /// 開始時間を更新
    /// </summary>
    public void UpdateStartTime(float _newStartTime)
    {
        startTime = _newStartTime;
    }

    /// <summary>
    /// クリップの最大時間を返す
    /// </summary>
    /// <returns>クリップの最大時間</returns>
    public float ReturnMaxTime()
    {
        return maxTime;
    }

    /// <summary>
    /// クリップとタイムバーが重なっているかをチェック
    /// </summary>
    /// <param name="clipRect">クリップのRectTransform</param>
    /// <param name="timeberRect">タイムバーのRectTransform</param>
    /// <returns>重なっている=true 重なっていない=false</returns>
    private bool CheckOverrap(RectTransform clipRect, RectTransform timeberRect)
    {
        // RectTransformの境界をワールド座標で取得
        Rect rect1World = GetWorldRect(clipRect);
        Rect rect2World = GetWorldRect(timeberRect);

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
