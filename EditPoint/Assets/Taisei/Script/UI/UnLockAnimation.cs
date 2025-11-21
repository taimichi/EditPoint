using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLockAnimation : MonoBehaviour
{
    //ロックマークのアニメーター
    [SerializeField] private Animator animator;
    private int worldNum = 0;       //ワールド番号
    private int stageNum = 0;       //ステージ番号

    private NewStageData sd;        //ステージデータ

    //アニメーション可能かどうか
    //fales=まだ / true=可能
    private bool isAnim = false;

    //番号をゲットしたかどうか
    //false=まだ / true=ゲットした
    private bool isGet = false;

    //処理を1度だけ行うための判断用
    private bool isFirst = false;

    //親オブジェクト
    private GameObject LockPanel;

    private enum LOCK_STATE
    {
        none,   //なにも登録されてない
        stage,  //ステージ
        world,  //ワールド
    }
    private LOCK_STATE thisLockState = LOCK_STATE.none;

    private void Start()
    {
        //親オブジェクト取得
        LockPanel = this.transform.parent.gameObject;
    }

    void Update()
    {
        //番号がゲットされてる時
        if (isGet && !isFirst)
        {
            if (this.gameObject.activeSelf)
            {
                StageSearch();
                isFirst = true;
            }
        }

        //アニメーション可能なとき
        if (isAnim)
        {
            StartCoroutine(UnLockAnim());
        }
    }

    /// <summary>
    /// このロックマークが担当しているステージ情報を入手する
    /// </summary>
    private void StageSearch()
    {
        sd = NewStageData.StageEntity;
        
        //担当ステージ情報を検索
        for(int i = 0; i < sd.stageData.Length; i++)
        {
            //ワールド番号が0じゃないとき
            if(worldNum != 0)
            {
                //ステージ番号が0じゃないとき
                if(stageNum != 0)
                {
                    //登録されてるワールド番号とステージ番号が同じとき
                    if(sd.stageData[i].worldNum == worldNum && sd.stageData[i].stageNum == stageNum)
                    {
                        CheckLockState(i);
                        thisLockState = LOCK_STATE.stage;
                        break;
                    }
                }
                //ステージ番号が0の時
                else
                {
                    //登録されてるワールド番号と同じでステージ番号が1の時
                    if(sd.stageData[i].worldNum == worldNum && sd.stageData[i].stageNum == 1)
                    {
                        CheckLockState(i);
                        thisLockState = LOCK_STATE.world;
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 該当ステージデータのロック状態を調べる
    /// </summary>
    /// <param name="_num">ステージデータの要素番号</param>
    private void CheckLockState(int _num)
    {
        switch (sd.stageData[_num].stagelock)
        {
            case NewStageData.StageLock.Lock:
                LockPanel.SetActive(true);
                break;

            case NewStageData.StageLock.FirstUnLock:
                isAnim = true;
                sd.stageData[_num].stagelock = NewStageData.StageLock.Open;
                break;

            case NewStageData.StageLock.Open:
                LockPanel.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// 開錠用のコルーチン
    /// </summary>
    private IEnumerator UnLockAnim()
    {
        //0.5秒待つ(調整用) 
        yield return new WaitForSeconds(0.5f);

        //開錠アニメーション開始
        animator.Play("UnLock");
        yield return null;

        //アニメーションの長さを取得
        var state = animator.GetCurrentAnimatorStateInfo(0).length;

        //アニメーション終了まで待機
        yield return new WaitForSeconds(state);
        
        //このロックマークが担当しているのがワールドだったとき
        if(thisLockState == LOCK_STATE.world)
        {
            //会話セット用スクリプトを取得
            AllTexts texts = GameObject.Find("TalkCanvas").GetComponent<AllTexts>();

            //会話発生
            switch (worldNum)
            {
                //ワールド１クリア
                case 2:
                    //一度もこの会話が発生していないとき
                    if((GameData.GameEntity.talkFrags & GameData.CLEARTALK_FRAG.stage1) == 0)
                    {
                        //会話発生
                        texts.SetAllTexts(AllTexts.TEXT_MESSAGE.clear_stage1);
                        //１回以上会話発生した判定に
                        GameData.GameEntity.talkFrags |= GameData.CLEARTALK_FRAG.stage1;
                    }
                    break;

                //ワールド２クリア
                case 3:
                    if ((GameData.GameEntity.talkFrags & GameData.CLEARTALK_FRAG.stage2) == 0)
                    {
                        //会話発生
                        texts.SetAllTexts(AllTexts.TEXT_MESSAGE.clear_stage2);
                        //１回以上会話発生した判定に
                        GameData.GameEntity.talkFrags |= GameData.CLEARTALK_FRAG.stage2;
                    }
                    break;

                //ワールド３クリア
                case 4:
                    if ((GameData.GameEntity.talkFrags & GameData.CLEARTALK_FRAG.stage3) == 0)
                    {
                        //会話発生
                        texts.SetAllTexts(AllTexts.TEXT_MESSAGE.clear_stage3);
                        //１回以上会話発生した判定に
                        GameData.GameEntity.talkFrags |= GameData.CLEARTALK_FRAG.stage3;
                    }
                    break;

                //全ステージクリアはGameManagerスクリプトで
            }
        }


        //非表示状態に
        LockPanel.SetActive(false);
    }
    

    /// <summary>
    /// ワールド番号とステージ番号を外部から渡してもらう関数
    /// </summary>
    /// <param name="_world">ワールド番号</param>
    /// <param name="_stage">ステージ番号</param>
    public void GetNum(int _world, int _stage)
    {
        worldNum = _world;
        stageNum = _stage;

        isGet = true;
    }
}
