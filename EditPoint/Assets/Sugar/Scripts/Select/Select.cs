using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Select : MonoBehaviour
{
    #region field
    [SerializeField] Fade fade;          // FadeCanvas
    // ステージ選択のポジション
    [SerializeField] RectTransform[] rPos;
    // ステージサムネ
    [SerializeField] Image[] img;
    // ステージ情報
    [SerializeField] StageDataBase SDB;
    // ステージの説明テキスト
    [SerializeField] Text text;
    // ステージ名のテキスト
    [SerializeField] Text stageName;
    // 再生マークの表示切り替え用
    [SerializeField] GameObject[] Obj;
    // UIを動かすクラス
    ClassUIAnim UAnim;

    [SerializeField]ClapperStart clapper;

    // 基準座標
    // constは一個辺りの差の値
    float posX = 500;
    const int addX = 62;

    float posY = 0;
    const int addY = 350;

    Vector2[] vec2=new Vector2[5];

    // ポジション移動の速度設定
    float spdX = (float)addX/3.0f;
    float spdY = (float)addY/3.0f;

    // マウスホイール
    float wh;

    // 配列に使う
    int num=0;
    int numSDB=2;
    int copyNum;
    int copyNumSDB;
    int upnum;
    int downnum;
    int max;
    int maxSDB;
    int min = 0;

    bool startMove = false;

    // UIを動かす用
    int moveNum = 0;

    enum MoveNum
    {
        Stay=0,
        UPWheel=1,
        DOWNWheel=2,
    }

    #endregion

    void Start()
    {
        // インスタンス生成
        UAnim = new ClassUIAnim();

        // 配列の最大数を求める
        max = rPos.Length-1;
        maxSDB = SDB.STAGE_DATA.Count-1;
        copyNum = num;
        PosDataSet();
        StartSpriteSet();
        text.text = SDB.STAGE_DATA[numSDB].infomation_Stage;
        stageName.text = SDB.STAGE_DATA[numSDB].StageName;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(maxSDB);
        // マウスホイール
        // 左右クリックに変更
        Wheel();
        MoveUI();
    }

    #region Method
    // 入力制御
    void Wheel()
    {
        if (startMove) { return; }
        wh = Input.GetAxis("Mouse ScrollWheel");

        // ↑入力
        if (wh>0)
        {
            if (num != max)
            {
                num++;
            }
            else
            {
                num = min;
            }
            if (numSDB != maxSDB)
            {
                numSDB++;
            }
            else
            {
                numSDB = min;
            }
            // UIを動かすタイミング
            startMove = true;
            copyNum = num;
            copyNumSDB = numSDB;
            moveNum = (int)MoveNum.UPWheel;
        }
        // ↓入力
        else if (wh<0)
        {
            if (num != min)
            {
                num--;
            }
            else
            {
                num = max;
            }
            if (numSDB != min)
            {
                numSDB--;
            }
            else
            {
                numSDB = maxSDB;
            }
            // UIを動かすタイミング
            startMove = true;
            copyNum = num;
            copyNumSDB = numSDB;
            moveNum = (int)MoveNum.DOWNWheel;

        }

        if(Input.GetMouseButtonDown(0))
        {
            // 何も入ってないときは実行しない
            if (SDB.STAGE_DATA[numSDB].StageSceneName == "") { return; }
            clapper.SceneName = SDB.STAGE_DATA[numSDB].StageSceneName;
            startMove = true;
        }
    }

    /// <summary>
    /// 選んでるステージ
    /// </summary>
    void NowSelect()
    {
        for (int i = 0; i < Obj.Length; i++)
        {
            // 一度全て非表示に
            Obj[i].SetActive(false);

            if(rPos[i].anchoredPosition==vec2[2])
            {
                Obj[i].SetActive(true);
            }
        }
        text.text = SDB.STAGE_DATA[numSDB].infomation_Stage;
        stageName.text = SDB.STAGE_DATA[numSDB].StageName;
    }

    void SpriteSet()
    {
        for (int i = 0; i < img.Length; i++)
        {
            if (rPos[i].anchoredPosition == vec2[0])
            {
                if (numSDB + 2 ==maxSDB+1) { img[i].sprite = SDB.STAGE_DATA[min].StageImage; }
                else if (numSDB + 2 ==maxSDB+2) { img[i].sprite = SDB.STAGE_DATA[min + 1].StageImage; }
                else { img[i].sprite = SDB.STAGE_DATA[numSDB + 2].StageImage; }
            }
            if (rPos[i].anchoredPosition == vec2[4])
            {
                if (numSDB - 2 ==-1) { img[i].sprite = SDB.STAGE_DATA[maxSDB].StageImage; }
                else if (numSDB-2  ==-2) { img[i].sprite = SDB.STAGE_DATA[maxSDB - 1].StageImage; }
                else { img[i].sprite = SDB.STAGE_DATA[numSDB - 2].StageImage; }
            }
        }
    }

    /// <summary>
    /// ポジションのデータ
    /// </summary>
    void PosDataSet()
    {
        vec2[0] =new Vector2(posX+(addX*2),posY+(addY*2));
        vec2[1] =new Vector2(posX+(addX*1),posY+(addY*1));
        vec2[2] =new Vector2(posX,posY);
        vec2[3] =new Vector2(posX-(addX*1),posY-(addY*1));
        vec2[4] =new Vector2(posX-(addX*2),posY-(addY*2));
    }

    void StartSpriteSet()
    {

        img[0].sprite = SDB.STAGE_DATA[4].StageImage;
        img[1].sprite = SDB.STAGE_DATA[3].StageImage;
        img[2].sprite = SDB.STAGE_DATA[2].StageImage;
        img[3].sprite = SDB.STAGE_DATA[1].StageImage;
        img[4].sprite = SDB.STAGE_DATA[0].StageImage;
    }

    void MoveUI()
    {
        switch (moveNum)
        {
            case (int)MoveNum.UPWheel:
                if (rPos[0].anchoredPosition.y >= vec2[num].y)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        rPos[i] = UAnim.anim_PosChange(rPos[i], -spdX, -spdY);
                    }
                }
                else if(rPos[0].anchoredPosition.y==vec2[4].y)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        rPos[i] = UAnim.anim_PosChange(rPos[i], -spdX, -spdY);
                    }
                    rPos[0].anchoredPosition = vec2[copyNum];
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        rPos[i].anchoredPosition = vec2[copyNum];
                        copyNum++;
                        if(copyNum>max)
                        {
                            copyNum = min;
                        }
                    }
                    SpriteSet();
                    moveNum = (int)MoveNum.Stay;
                    NowSelect();
                    startMove = false;

                }
                break;
            case (int)MoveNum.DOWNWheel:
                if (rPos[0].anchoredPosition.y <= vec2[num].y)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        rPos[i] = UAnim.anim_PosChange(rPos[i], spdX, spdY);
                    }
                }
                else if(rPos[0].anchoredPosition.y == vec2[0].y)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        rPos[i] = UAnim.anim_PosChange(rPos[i], spdX, spdY);
                    }
                    rPos[0].anchoredPosition = vec2[copyNum];
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        rPos[i].anchoredPosition = vec2[copyNum];
                        copyNum++;
                        if (copyNum >max)
                        {
                            copyNum = min;
                        }
                    }
                    SpriteSet();
                    moveNum = (int)MoveNum.Stay;
                    NowSelect();
                    startMove = false;
                }
                break;
        }
        
        //Debug.Log(numSDB);
    }
    #endregion
}
