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

    [SerializeField] private ClipSpeed clipSpeed;   //クリップの再生速度に関するスクリプト
    private float speed = 0f;       //クリップの再生速度
    private List<MoveGround> moveGround = new List<MoveGround>(); //動く床のスクリプト
    private CheckClipConnect checkClip; //オブジェクトがクリップに紐づけられているか確認する用のスクリプト

    private float manualTime = 0; //タイムバーを手動で動かしたときの時間

    private float startTime = 0f;       //クリップの開始時間
    private float maxTime = 0f;         //クリップの長さ

    private MoveGroundManager MGManager;

    private GetConnectClip getConnectClip;  //このクリップに紐づいているオブジェクトに、このクリップを渡す用

    private CheckOverlap checkOverlap = new CheckOverlap();     //ほかのクリップやタイムバーが重なったかどうかを判定するスクリプト

    [SerializeField] private Materials materialsData;       //クリップを選択した時、紐づいているオブジェクトにアウトラインをつける用

    private void Awake()
    {
        //時間を初期化
        startTime = 0f;
        manualTime = 0f;
    }

    void Start()
    {
        //タイムバーのRectTransformを取得
        rect_timeBar = GameObject.Find("Timebar").GetComponent<RectTransform>();
        //ゲームオブジェクトのクリップマネージャーを取得
        ClipManager = GameObject.Find("ClipManager");
        //クリップ生成スクリプトを取得
        clipGenerator = ClipManager.GetComponent<ClipGenerator>();

        //ゲームマネージャーオブジェクトから動く床のマネージャースクリプトを取得
        MGManager = GameObject.Find("GameManager").GetComponent<MoveGroundManager>();

        //生成したクリップの場合
        if (ConnectObj.Count == 0)
        {
            //クリップの名前を変更
            clipName.text = "空っぽのクリップ" + clipGenerator.ReturnCount();
        }
        //元からあるクリップの場合
        else
        {
            //クリップに紐づいているオブジェクトの数の分だけ
            for(int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
            {
                //動く床だったとき
                if(ConnectObj[connectObjNum].TryGetComponent<MoveGround>(out MoveGround moveGroundScript))
                {
                    moveGround.Add(moveGroundScript);   
                }

                //オブジェクト側で、クリップと紐づける
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

        //クリップの秒数を計算
        CalculationMaxTime();
    }

    private void Update()
    {
        //クリップの秒数を計算
        CalculationMaxTime();
        //クリップの再生速度を取得
        speed = clipSpeed.ReturnPlaySpeed();

        //クリップに紐づくオブジェクトがないときがないとき
        if (ConnectObj.Count == 0)
        {
            clipName.text = "空っぽのクリップ";
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
            //クリップの経過時間を計算
            Vector3 leftEdge = rect_Clip.localPosition + new Vector3(-rect_Clip.rect.width * rect_Clip.pivot.x, 0, 0);
            //クリップ上でのタイムバーの現在の位置
            float dis = rect_timeBar.localPosition.x - leftEdge.x;
            //手動でタイムバーを動かしたときの、クリップの現在の再生位置
            manualTime = ((float)Math.Truncate(dis / TimelineData.TimelineEntity.oneTickWidth * 10) / 10) / 2;

            //タイムバーを手動で動かしてる時
            if (TimeData.TimeEntity.isDragMode)
            {
                //動く床を取得
                GameObject moveGround = ReturnConnectMoveObj();
                if (moveGround != null)
                {
                    //手動再生時の時間を渡す
                    moveGround.GetComponent<MoveGround>().GetClipTime_Manual(startTime + manualTime);
                }
                
            }
            //タイムバーが自動で動いているとき
            else
            {

            }
        }

        //クリップの再生関連の処理
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
        //外部から渡されたオブジェクトにこのクリップと紐づける
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
                    //表示状態にする
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
                //マテリアル変更
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
                //リストから削除
                MGManager.DeleteMoveGrounds(ConnectObj[connectObjNum]);
            }
            //オブジェクトを破壊
            Destroy(ConnectObj[connectObjNum]);
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

    /// <summary>
    /// 紐づいている動く床オブジェクトを返す
    /// </summary>
    /// <returns>動く床のゲームオブジェクト(何もない場合はnull)</returns>
    public GameObject ReturnConnectMoveObj()
    {
        for (int connectObjNum = 0; connectObjNum < ConnectObj.Count; connectObjNum++)
        {
            //動く床オブジェクトの時
            if (ConnectObj[connectObjNum].name.Contains("MoveGround"))
            {
                //動く床オブジェクトを返す
                return ConnectObj[connectObjNum];
            }
        }

        //何もなかった場合はnullを返す
        return null;
    }
}
