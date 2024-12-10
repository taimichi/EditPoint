using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //カットした時の保存用構造体
    private struct saveInfo
    {
        public bool isSave;
        public int savePathNum;
        public Vector3 savePos;
        public float saveTime;
    }
    private saveInfo info;

    private void Start()
    {
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
                //再生ボタンを1回も押してなかったら
                if (!isStart)
                {
                    this.transform.position = startPos;
                    isStart = true;

                    //カットをしてあるかどうか
                    if (!isCut)
                    {
                        int i = 0;
                        AutoClipTime = _autoClipTime;
                        //スタート時間を動く床の移動時間から順に引いていく
                        while (AutoClipTime >= pathTime[i])
                        {
                            nowPath++;
                            AutoClipTime -= pathTime[i];
                            i++;
                            if (i >= pathTime.Count - 1)
                            {
                                nowPath = 0;
                                i = 0;
                            }
                        }

                        AutoClipTime = pathTime[i] - AutoClipTime;
                    }
                }

                // 移動用
                Vector3 movePos = this.transform.position;

                Vector3 dist = path[nowPath + 1] - path[nowPath];
                if (nowPath + 1 == path.Count - 1)
                {
                    dist = path[nowPath - 1] - path[nowPath];
                }
                Vector3 moveSpeed = dist / pathTime[nowPath];


                //カットしたときに一回だけ
                if (_autoClipTime > 0 && !isCut)
                {
                    //開始時間を変更
                    //autoClipは上の処理で必ずpathTime[nowPath]以下の値になってるため
                    //引いて残りの時間を求める
                    info.saveTime = pathTime[nowPath] - (pathTime[nowPath] - AutoClipTime);
                    timer = info.saveTime;
                    info.savePathNum = nowPath;

                    //動く床の開始位置を変更
                    info.savePos = path[nowPath] + (moveSpeed * (pathTime[nowPath] - AutoClipTime) * speed * playSpeed);
                    this.transform.position = info.savePos;
                    movePos = info.savePos;

                    isCut = true;
                    info.isSave = true;
                }
                movePos += moveSpeed * Time.deltaTime * speed * playSpeed;
                this.transform.position = movePos;

                // 時間管理
                timer -= Time.deltaTime * speed * playSpeed;

                if (timer <= 0)
                {
                    nowPath++;
                    if (nowPath == path.Count - 1)
                    {
                        nowPath = 0;
                    }
                    //Debug.Log(nowPath + " 到着");
                    timer = pathTime[nowPath];
                }
            }
        }
        //手動
        else
        {
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

            // 移動用
            Vector3 dist = path[nowPath + 1] - path[nowPath];
            if (nowPath + 1 == path.Count - 1)
            {
                dist = path[nowPath - 1] - path[nowPath];
            }

            Vector3 moveSpeed = dist / pathTime[nowPath];
            Vector3 movePos = moveSpeed * ManualClipTime * speed * playSpeed;

            this.transform.position = startPos + movePos;
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
    /// タイムバーリセットがされたかどうか
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
    }

    public void SetStartPos()
    {
        this.transform.position = startPos;
    }
}
