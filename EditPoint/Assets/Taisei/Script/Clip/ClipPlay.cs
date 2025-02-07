using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClipPlay : MonoBehaviour
{
    private RectTransform rect_timeBar; //タイムバーのRectTransform
    [SerializeField] private RectTransform rect_Clip;   //クリップのRectTransform
    [SerializeField] private Text clipName; //クリップの名前を表示するテキスト

    [SerializeField] private List<GameObject> ConnectObj = new List<GameObject>();  //クリップに紐づけられているオブジェクト    

    private GameObject ClipManager; //親オブジェクト
    private ClipGenerator clipGenerator;    //クリップ生成用スクリプト

    private bool isGetObjMode = false;


    [SerializeField] private ClipSpeed clipSpeed;   //クリップの再生速度に関するスクリプト
    private float speed = 0f;       //クリップの再生速度
    private List<MoveGround> moveGround = new List<MoveGround>(); //動く床のスクリプト
    private CheckClipConnect checkClip; //オブジェクトがクリップに紐づけられているか確認する用のスクリプト

    private AddTextManager addTextManager;

    private float manualTime = 0; //タイムバーを手動で動かしたときの時間

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
        //タイムバーのRectTransformを取得
        rect_timeBar = GameObject.Find("Timebar").GetComponent<RectTransform>();
        ClipManager = GameObject.Find("ClipManager");
        clipGenerator = ClipManager.GetComponent<ClipGenerator>();
        addTextManager = ClipManager.GetComponent<AddTextManager>();

        MGManager = GameObject.Find("GameManager").GetComponent<MoveGroundManager>();

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
        if (isGetObjMode)
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
        if (checkOverlap.IsOverlap(rect_Clip, rect_timeBar))
        {
            //クリップの経過時間
            Vector3 leftEdge = rect_Clip.localPosition + new Vector3(-rect_Clip.rect.width * rect_Clip.pivot.x, 0, 0);
            float dis = rect_timeBar.localPosition.x - leftEdge.x;
            manualTime = ((float)Math.Truncate(dis / TimelineData.TimelineEntity.f_oneTickWidht * 10) / 10) / 2;

            //タイムバーを手動で動かしてる時
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
        if (checkOverlap.IsOverlap(rect_Clip, rect_timeBar))
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
                //動く床だったとき
                if (ConnectObj[i].name.Contains("MoveGround"))
                {
                    //プレイヤーがMoveGroundの子オブジェクトになってる時
                    if (ConnectObj[i].transform.Find("Player") != null)
                    {
                        //子オブジェクトを解除する
                        ConnectObj[i].transform.Find("Player").gameObject.transform.parent = null;
                    }

                    //非表示にする前に初期位置に戻す
                    ConnectObj[i].GetComponent<MoveGround>().SetStartPos();
                }
                //表示状態だったら
                if (ConnectObj[i].activeSelf)
                {
                    ConnectObj[i].SetActive(false);

                    // クリップ切れたときのエディター非表示
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
    /// クリップの最大時間を計算
    /// </summary>
    public void CalculationMaxTime()
    {
        maxTime = (rect_Clip.rect.width / (TimelineData.TimelineEntity.f_oneTickWidht * 2)) + startTime;
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
                MGManager.DeleteMoveGrounds(ConnectObj[i]);
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
}
