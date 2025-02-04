using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity; //インスペクターを整理するためのやつ

public class MoveGround : MonoBehaviour
{
    private MoveGroundManager MGManager;

    [Header("一番下に0,0,0を追加(移動させたい座標とは別)")]public List<Vector3> path = new List<Vector3>();

    [Header("一番下に0を追加(移動させたい座標とは別)")] public List<float> pathTime = new List<float>();

    public float speed = 1;

    [SerializeField]
    int nowPath = 0;

    float timer;

    //再生速度
    private float playSpeed = 1f;

    //外部からの取得用
    //自動再生の時のクリップの時間
    private float _autoClipTime = 0f;
    //手動再生の時のクリップの時間
    private float _manualClipTime = 0f;
    //実際に処理で使う用
    //自動再生の時のクリップの時間
    private float AutoClipTime = 0f;
    //手動再生の時のクリップの時間
    private float ManualClipTime = 0f;

    //初期位置
    Vector3 startPos;

    //スタートを押して一回目かどうか
    private bool isStart = false;

    //カットしたかどうか
    private bool isCut = false;

    /// <summary>
    /// 地面と触れたかどうか
    /// </summary>
    private bool isGroundHit = false;

    /// <summary>
    /// 地面に触れた時の処理をしたかどうか
    /// </summary>
    private bool isCheckGroundHit = false;

    /// <summary>
    /// 動く方向が反転したかどうか
    /// </summary>
    private bool isInvert = false;

    //動く床の幻影
    [Foldout("child"), SerializeField] private GameObject child;
    [Foldout("child"), SerializeField] private CheckMoveGround childCheck;
    [Foldout("child"), SerializeField] private SpriteRenderer childSR;
    [Foldout("child"), SerializeField] private Color N_color;   // 通常の時の色
    [Foldout("child"), SerializeField] private Color C_color;   // 床に触れた時

    private float beforeTime = 0f;


    //カットした時の保存用構造体
    private struct saveInfo
    {
        public bool isSave;         // カットしてあるかどうか
        public int savePathNum;     // 保存したnowPath
        public Vector3 savePos;     // 保存した座標
        public float saveTime;      // 保存した時間
    }
    private saveInfo info;

    private void Start()
    {
        childSR.color = N_color;
        MGManager = GameObject.Find("GameManager").GetComponent<MoveGroundManager>();
        MGManager.GetMoveGrounds(this.gameObject);
        info.isSave = false;
        this.transform.position = path[0];
        timer = pathTime[0];
        nowPath = 0;

        //path.Add(Vector3.zero);
        //pathTime.Add(0);

        startPos = this.transform.position;
        isCut = false;
        isInvert = false;
    }

    private void Update()
    {
        //タイムバーが限界に行ったら、移行の処理はしない
        if (GameData.GameEntity.isLimitTime)
        {
            return;
        }

        //自動
        if (!TimeData.TimeEntity.b_DragMode)
        {
            //再生中のみ
            if (GameData.GameEntity.isPlayNow)
            {
                child.SetActive(false);
                //再生ボタンを1回も押してなかったら
                if (!isStart)
                {
                    this.transform.position = startPos;
                    isStart = true;
                }

                // 移動用
                Vector3 movePos = this.transform.position;

                Vector3 dist = path[nowPath + 1] - path[nowPath];
                if (nowPath + 1 == path.Count - 1)
                {
                    dist = path[nowPath - 1] - path[nowPath];
                }
                Vector3 moveSpeed = dist / pathTime[nowPath];

                //地面と触れた時
                if (isGroundHit)
                {
                    Debug.Log("地面と接触中" + nowPath);

                    isCheckGroundHit = true;
                    isInvert = true;
                }
                //触れていないとき
                else
                {
                    movePos += moveSpeed * Time.deltaTime * speed * playSpeed;
                }
                this.transform.position = movePos;  //座標更新

                // 時間管理
                timer -= Time.deltaTime * speed * playSpeed;

                //pathの座標外に出た時の処理
                Vector3 nowPos = this.transform.position;
                nowPos.x = Mathf.Floor(nowPos.x * 10) / 10;
                nowPos.y = Mathf.Floor(nowPos.y * 10) / 10;

                if (isInvert)
                {
                    int i = nowPath;
                    if(nowPath == 0)
                    {
                        i = path.Count - 1;
                    }
                    if (nowPos == path[i - 1])
                    {
                        nowPath++;
                        if (nowPath == path.Count - 1)
                        {
                            nowPath = 0;
                        }
                        timer = pathTime[nowPath];
                        isGroundHit = false;
                        isInvert = false;
                    }
                }

                if (timer <= 0)
                {
                    nowPath++;
                    if (nowPath == path.Count - 1)
                    {
                        nowPath = 0;
                    }
                    //Debug.Log(nowPath + " 到着");
                    timer = pathTime[nowPath];
                    isGroundHit = false;
                }

                if (isCheckGroundHit)
                {
                    nowPath++;
                    if (nowPath == path.Count - 1)
                    {
                        nowPath = 0;
                    }
                    timer = pathTime[nowPath];
                    isGroundHit = false;
                    isCheckGroundHit = false;
                }

            }
        }
        //手動
        else
        {
            child.SetActive(true);
            isStart = false;

            //拡張性がないので、そのうち直す必要あり…
            //↓手動で動かすときの処理　２点間なら動く(ブロックの動く座標が3か所になったら上手くいくかわからない)
            ManualClipTime = _manualClipTime;
            int i = 0;
            //現在の時間を求める
            while (ManualClipTime >= pathTime[i])
            {
                ManualClipTime -= pathTime[i];
                i++;
                if (i >= pathTime.Count - 1)
                {
                    i = 0;
                }
            }
            if (i == pathTime.Count - 2)
            {
                ManualClipTime = pathTime[i] - ManualClipTime;
            }
            //↑仮処理ここまで

            Vector3 dist = path[nowPath + 1] - path[nowPath];
            if (nowPath + 1 == path.Count - 1)
            {
                dist = path[nowPath - 1] - path[nowPath];
            }

            Vector3 moveSpeed = dist / pathTime[nowPath];

            Vector3 movePos = moveSpeed * ManualClipTime * speed * playSpeed;
            child.transform.position = startPos + movePos;

            //幻影の色変化処理
            if (childCheck.ReturnIsTrigger() && beforeTime == 0)
            {
                beforeTime = _manualClipTime;
                childSR.color = C_color;
            }
            else if (beforeTime > _manualClipTime && !childCheck.ReturnIsTrigger())
            {
                childSR.color = N_color;
                beforeTime = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Playerを子オブジェクト化
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = this.transform;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Playerが離れたら子オブジェクト解除
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }

    /// <summary>
    /// 初期位置更新
    /// </summary>
    private void StartPosUpdate()
    {
        //カット機能を使った際の処理
        if (_autoClipTime > 0 && !isCut)
        {
            int i = 0;
            AutoClipTime = _autoClipTime;
            //スタート時間を動く床の移動時間から順に引いていく
            while (AutoClipTime >= pathTime[i])
            {
                AutoClipTime -= pathTime[i];
                i++;
                if (i >= pathTime.Count - 1)
                {
                    i = 0;
                }
            }
            AutoClipTime = pathTime[i] - AutoClipTime;

            Vector3 dist = path[i + 1] - path[i];
            if (i + 1 == path.Count - 1)
            {
                dist = path[i - 1] - path[i];
            }
            Vector3 moveSpeed = dist / pathTime[i];

            //開始時間を変更
            //autoClipは上の処理で必ずpathTime[nowPath]以下の値になってるため
            //引いて残りの時間を求める
            info.saveTime = pathTime[i] - (pathTime[i] - AutoClipTime);
            timer = info.saveTime;
            info.savePathNum = nowPath;

            //動く床の開始位置を変更
            info.savePos = path[i] + (moveSpeed * (pathTime[i] - AutoClipTime) * speed * playSpeed);
            //this.transform.position = info.savePos;
            //startPos = this.transform.position;

            isCut = true;
            info.isSave = true;
        }

    }

    /// <summary>
    /// タイムバーリセット(初期位置に戻る)がされたかどうか
    /// </summary>
    public void CheckReset()
    {
        if (GameData.GameEntity.isTimebarReset)
        {
            //カットされたクリップの時
            if(info.isSave)
            {
                this.transform.position = info.savePos;
                startPos = info.savePos;
                nowPath = info.savePathNum;
                timer = info.saveTime;
            }
            else
            {
                this.transform.position = path[0];
                timer = pathTime[0];
                nowPath = 0;
            }
            isInvert = false;
        }
    }

    /// <summary>
    /// クリップの長さをもとに再生速度を変更
    /// </summary>
    /// <param name="_playSpeed">新しい再生速度</param>
    public void ChangePlaySpeed(float _playSpeed)
    {
        playSpeed = _playSpeed;
    }

    /// <summary>
    /// クリップから手動時の現在のクリップの秒数を取得
    /// </summary>
    /// <param name="_clipTime">クリップの現在の秒数(手動時)</param>
    public void GetClipTime_Manual(float _clipTime)
    {
        _manualClipTime = _clipTime;
    }

    /// <summary>
    /// クリップから自動時の現在にクリップの秒数を取得
    /// </summary>
    /// <param name="_clipTime">クリップの現在の秒数(自動時)</param>
    public void GetClipTime_Auto(float _clipTime)
    {
        _autoClipTime = _clipTime;

        StartPosUpdate();
    }

    /// <summary>
    /// 現在の位置から初期位置に戻す
    /// </summary>
    public void SetStartPos()
    {
        this.transform.position = startPos;
    }

    public void SetTrigger(bool trigger)
    {
        isGroundHit = trigger;
    }
}
