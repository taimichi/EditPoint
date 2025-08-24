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
    //手動再生の時のクリップの時間
    private float _manualClipTime = 0f;
    //実際に処理で使う用
    //手動再生の時のクリップの時間
    private float ManualClipTime = 0f;

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

    private bool isStart = false;

    //動く床の幻影
    [Foldout("Child"), SerializeField] private GameObject child;            //影用オブジェクト
    [Foldout("Child"), SerializeField] private CheckMoveGround childCheck;  //判定用スクリプト
    [Foldout("Child"), SerializeField] private SpriteRenderer childSR;      //影用オブジェクトのスプライトレンダラー
    [Foldout("Child"), SerializeField] private Color N_color;               // 通常の時の色
    [Foldout("Child"), SerializeField] private Color C_color;               // 床に触れた時

    private float beforeTime = 0f;

    private struct moveGroundState
    {
        public Vector3 startPos;    //開始位置
        public float startTime;     //開始時間
        public int startPath;       //開始時のnowPath
        public Vector3 child_startPos;
    }
    private moveGroundState normal;


    private void Awake()
    {
        //位置を最初の位置に設定
        this.transform.position = path[0];
        //移動時間を設定
        timer = pathTime[0];
        nowPath = 0;

        //開始時の状態を保存
        normal.startPos = this.transform.position;
        normal.startPath = nowPath;
        normal.startTime = timer;

        normal.child_startPos = normal.startPos;
    }

    private void Start()
    {
        //子オブジェクトのカラーを通常時の色に
        childSR.color = N_color;
        //動く床のマネージャースクリプトを取得
        MGManager = GameObject.Find("GameManager").GetComponent<MoveGroundManager>();
        MGManager.GetMoveGrounds(this.gameObject);

        isInvert = false;
        isStart = false;
    }

    private void Update()
    {
        //タイムバーが限界に行ったら、移行の処理はしない
        if (GameData.GameEntity.isLimitTime)
        {
            return;
        }

        //自動
        if (!TimeData.TimeEntity.isDragMode)
        {
            //再生中のみ
            if (GameData.GameEntity.isPlayNow)
            {
                //影用オブジェクトを非表示に
                child.SetActive(false);
                //ループ始めの１回目のみ
                if (!isStart)
                {
                    isStart = true;
                    this.transform.position = normal.startPos;
                }

                // 移動用
                Vector3 movePos = this.transform.position;

                //方向を計算
                Vector3 dist = path[nowPath + 1] - path[nowPath];
                if (nowPath + 1 == path.Count - 1)
                {
                    dist = path[nowPath - 1] - path[nowPath];
                }
                Vector3 moveSpeed = dist / pathTime[nowPath];

                //地面と触れた時
                if (isGroundHit)
                {
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

                //床の移動反転処理
                if (isInvert)
                {
                    int i = nowPath;
                    if (nowPath == 0)
                    {
                        i = path.Count - 1;
                    }
                    if (nowPos == path[i - 1])
                    {
                        //次の座標位置の番号へ
                        nowPath++;
                        //座標位置の番号が座標位置配列の最大数-1に達した時
                        if (nowPath == path.Count - 1)
                        {
                            nowPath = 0;
                        }
                        //時間更新
                        timer = pathTime[nowPath];
                        isGroundHit = false;
                        isInvert = false;
                    }
                }

                //時間が0秒以下になったら
                if (timer <= 0)
                {
                    //次の座標位置の番号へ
                    nowPath++;
                    //座標位置の番号が座標位置配列の最大数-1に達した時
                    if (nowPath == path.Count - 1)
                    {
                        nowPath = 0;
                    }
                    //時間更新
                    timer = pathTime[nowPath];
                    isGroundHit = false;
                }

                //動く床が別の地面に衝突した時
                if (isCheckGroundHit)
                {
                    //次の座標位置の番号へ
                    nowPath++;
                    //座標位置の番号が座標位置配列の最大数-1に達した時
                    if (nowPath == path.Count - 1)
                    {
                        nowPath = 0;
                    }
                    //時間更新
                    timer = pathTime[nowPath];
                    isGroundHit = false;
                    isCheckGroundHit = false;
                }

            }
        }
        //手動
        else
        {
            isStart = false;
            child.SetActive(true);
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

            Vector3 dist = path[i + 1] - path[i];
            if (i + 1 == path.Count - 1)
            {
                dist = path[i - 1] - path[i];
            }
            Vector3 moveSpeed = dist / pathTime[i];
            Vector3 movePos = moveSpeed * ManualClipTime * speed * playSpeed;
            if (i == path.Count - 2)
            {
                movePos *= -1;
            }
            child.transform.position = normal.child_startPos + movePos;

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

    //衝突時
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Playerを子オブジェクト化
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = this.transform;
        }

    }

    //衝突後
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Playerが離れたら子オブジェクト解除
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }


    /// <summary>
    /// タイムバーリセット(初期位置に戻る)がされたかどうか
    /// </summary>
    public void CheckReset()
    {
        if (GameData.GameEntity.isTimebarReset)
        {
            this.transform.position = normal.startPos;
            timer = normal.startTime;
            nowPath = normal.startPath;

            isInvert = false;
            isStart = false;
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
    /// 現在の位置から初期位置に戻す
    /// </summary>
    public void SetStartPos()
    {
        this.transform.position = normal.startPos;
    }

    public void SetTrigger(bool trigger)
    {
        isGroundHit = trigger;
    }

    /// <summary>
    /// 紐づけられたクリップがカット処理されたとき
    /// </summary>
    public void Cuted(float newStartTime)
    {
        int i = 0;
        float time = newStartTime;
        while (time >= pathTime[i])
        {
            time -= pathTime[i];
            i++;
            if (i >= pathTime.Count - 1)
            {
                i = 0;
            }
        }
        if (i == pathTime.Count - 2)
        {
            time = pathTime[i] - time;
        }

        Vector3 dist = path[i + 1] - path[i];
        if (i + 1 == path.Count - 1)
        {
            dist = path[i - 1] - path[i];
        }
        Vector3 moveSpeed = dist / pathTime[i];
        Vector3 movePos = moveSpeed * time * speed * playSpeed;
        if (i == path.Count - 2)
        {
            movePos *= -1;
        }
        //開始座標変更
        normal.startPos += movePos;
        this.transform.position = normal.startPos;
        //開始時間変更
        time = pathTime[i] - time;
        normal.startTime = pathTime[i] - (pathTime[i] - time);
        timer = normal.startTime;
        //開始nowPath変更
        normal.startPath = i;
        nowPath = normal.startPath;

    }
}
