using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

//クリップのメイン
//クリップとオブジェクトの紐づけや、紐づけたオブジェクトの表示非表示など
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

    private float startTime = 0f;       //クリップの開始時間
    private float maxTime = 0f;         //クリップの長さ

    private MoveGroundManager MGManager;
    private MoveGround move;

    private GetConnectClip getConnectClip;  //このクリップに紐づいているオブジェクトに、このクリップを渡す用

    private CheckOverlap checkOverlap = new CheckOverlap();     //ほかのクリップやタイムバーが重なったかどうかを判定するスクリプト

    [SerializeField] private Materials materialsData;       //クリップを選択した時、紐づいているオブジェクトにアウトラインをつける用

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

                                for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
                                {
                                    if (clickedObject.name != ConnectObj[connectObjNum].name)
                                    {
                                        Debug.Log("新しいオブジェクト追加");
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
                                    Debug.Log("新しいオブジェクト追加");
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
            manualTime = ((float)Math.Truncate(dis / TimelineData.TimelineEntity.oneTickWidth * 10) / 10) / 2;

            //タイムバーを手動で動かしてる時
            if (TimeData.TimeEntity.isDragMode)
            {
                for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
                {
                    //動く床オブジェクトの時
                    if (ConnectObj[connectObjNum].TryGetComponent<MoveGround>(out move))
                    {
                        //クリップの開始秒数を渡す
                        //
                        move.GetClipTime_Manual(startTime + manualTime);
                    }
                }
            }
            //タイムバーが自動で動いているとき
            else
            {
                for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
                {
                    //動く床のオブジェクトの時
                    if (ConnectObj[connectObjNum].TryGetComponent<MoveGround>(out move))
                    {
                        //クリップの開始秒数を渡す
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
        for(int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
        {
            ConnectObj.Remove(ConnectObj[connectObjNum]);
        }
    }

    /// <summary>
    /// 紐づけたオブジェクトを取得
    /// </summary>
    public List<GameObject> ReturnConnectObj() => ConnectObj;

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
        getConnectClip = _outGetObj.GetComponent<GetConnectClip>();
        getConnectClip.GetAttachClip(this.gameObject);
        checkClip = _outGetObj.GetComponent<CheckClipConnect>();
        checkClip.ConnectClip();
        checkClip.GetClipPlay(this.gameObject);
    }

    /// <summary>
    /// クリップを再生しているかどうか
    /// </summary>
    private void ClipPlayNow()
    {
        if (checkOverlap.IsOverlap(rect_Clip, rect_timeBar))
        {
            //タイムバーと接触しているとき
            for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
            {
                //非表示状態だったら
                if (!ConnectObj[connectObjNum].activeSelf)
                {
                    ConnectObj[connectObjNum].SetActive(true);
                }
            }
        }
        else
        {
            //タイムバーと接触していないとき
            for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
            {
                //動く床だったとき
                if (ConnectObj[connectObjNum].name.Contains("MoveGround"))
                {
                    //プレイヤーがMoveGroundの子オブジェクトになってる時
                    if (ConnectObj[connectObjNum].transform.Find("Player") != null)
                    {
                        //子オブジェクトを解除する
                        ConnectObj[connectObjNum].transform.Find("Player").gameObject.transform.parent = null;
                    }

                    //非表示にする前に初期位置に戻す
                    ConnectObj[connectObjNum].GetComponent<MoveGround>().SetStartPos();
                }
                //表示状態だったら
                if (ConnectObj[connectObjNum].activeSelf)
                {
                    ConnectObj[connectObjNum].SetActive(false);

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
    /// 紐づけられたオブジェクトのImageのマテリアルを変更する
    /// </summary>
    /// <param name="_materialNum">変更したいマテリアルの番号
    /// 0=選択されていない時, 1=選択中の時</param>
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
    /// クリップの最大時間を計算
    /// </summary>
    public void CalculationMaxTime()
    {
        maxTime = (rect_Clip.rect.width / (TimelineData.TimelineEntity.oneTickWidth * 2)) + startTime;
    }

    /// <summary>
    /// 特定のオブジェクトをリストから削除
    /// </summary>
    public void ConnectObjRemove(GameObject obj)
    {
        //引数のオブジェクトをリストから検索
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
    /// クリップに関連付けてあるオブジェクトを消す
    /// </summary>
    public void ClipObjDestroy()
    {
        for(int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
        {
            if (ConnectObj[connectObjNum].name.Contains("MoveGround"))
            {
                //プレイヤーがMoveGroundの子オブジェクトになってる時
                if (ConnectObj[connectObjNum].transform.Find("Player") != null)
                {
                    //子オブジェクトを解除する
                    ConnectObj[connectObjNum].transform.Find("Player").gameObject.transform.parent = null;
                }
                MGManager.DeleteMoveGrounds(ConnectObj[connectObjNum]);
            }
            Destroy(ConnectObj[connectObjNum]);
            Debug.Log("test");
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
    public float ReturnMaxTime() => maxTime;
}
