using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockCreater : MonoBehaviour
{
    //public enum State
    //{
    //    none,
    //    Create,
    //    //MoveCreate
    //}

    //[SerializeField]
    //State nowState = State.none;

    bool isDrag;

    Vector2 mousePosition;

    [SerializeField]
    GameObject blockPrefab;

    //[SerializeField]
    //GameObject moveBlockPrefab;

    [SerializeField]
    GameObject markerPrefab;

    GameObject marker;
    BlockMarker bm;

    Vector2 startPosition;
    Vector2 endPosition;

    private int blockCounter = 1;

    private string createName = "CreateBlock";
    //[SerializeField] private Image moveBlockButton;

    private ClipGenerator clipGenerator;

    private PlaySound playSound;

    [SerializeField] private GameObject createText;
    private bool b_check = false;

    [SerializeField] private bool b_Lock = false;

    private void Start()
    {
        // nullチェックしてくれ、テスト環境で動かん…
        // すまんかった... byたいせい
        if (GameObject.Find("AudioCanvas") != null)
        {
            playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        }
        else
        {
            Debug.Log("audioないよ～");
        }

        if (GameObject.Find("ClipManager") != null)
        {
            clipGenerator = GameObject.Find("ClipManager").GetComponent<ClipGenerator>();
        }
        else
        {
            Debug.Log("clipないよ～");
        }

        marker = Instantiate(markerPrefab);
        bm = marker.GetComponent<BlockMarker>();
        bm.isActive = false;

        marker.SetActive(false);

        createText.SetActive(false);
    }

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (ModeData.ModeEntity.mode == ModeData.Mode.create /*|| nowState == State.MoveCreate*/)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    marker.SetActive(true);
                    startPosition = mousePosition;
                    bm.isActive = true;
                    isDrag = true;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    endPosition = mousePosition;

                    if (!bm.isHitGround)
                    {
                        if (ModeData.ModeEntity.mode == ModeData.Mode.create)
                        {
                            //マーカーがプレイヤーと触れてないとき
                            if (!bm.ReturnHitPL())
                            {
                                CreateBlock();
                            }
                            //マーカーがプレイヤーと触れてる時
                            else 
                            {
                                playSound.PlaySE(PlaySound.SE_TYPE.develop);
                            }
                        }
                        //else if (nowState == State.MoveCreate)
                        //{
                        //    CreateMoveBlock();
                        //}

                    }

                    bm.isActive = false;
                    isDrag = false;
                    marker.SetActive(false);
                }

                if (isDrag)
                {
                    Vector3 markerSize = Vector3.zero;
                    markerSize.x = mousePosition.x - startPosition.x;
                    markerSize.y = mousePosition.y - startPosition.y;
                    marker.transform.localScale = markerSize;

                    marker.transform.position = (Vector3)startPosition + (markerSize / 2);
                }

            }
        }

        if (Input.GetMouseButtonDown(1) && !isDrag && ModeData.ModeEntity.mode == ModeData.Mode.create)
        {
            playSound.PlaySE(PlaySound.SE_TYPE.cancell);
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            marker.SetActive(false);

            createText.SetActive(false);
            b_check = false;
        }

        if (ModeData.ModeEntity.mode != ModeData.Mode.create)
        {
            createText.SetActive(false);
        }
    }

    // ブロック生成
    private void CreateBlock()
    {
        playSound.PlaySE(PlaySound.SE_TYPE.blockGene);
        GameObject created = Instantiate(blockPrefab);
        created.transform.localScale = new Vector3 (Mathf.Abs(marker.transform.localScale.x), Mathf.Abs(marker.transform.localScale.y), marker.transform.localScale.z);
        created.transform.position = marker.transform.position;
        created.GetComponent<Collider2D>().enabled = false;
        created.GetComponent<Collider2D>().enabled = true;

        created.name = createName + blockCounter;
        blockCounter++;

        clipGenerator.ClipGene(created, b_check);
        b_check = true;
    }

    //private void CreateMoveBlock()
    //{
    //    GameObject created = Instantiate(moveBlockPrefab);
    //    created.transform.localScale = marker.transform.localScale;
    //    created.transform.position = marker.transform.position;

    //    MoveGround mg = created.GetComponent<MoveGround>();
    //    mg.path.Add(marker.transform.position);
    //    mg.path.Add(new Vector3(marker.transform.position.x, marker.transform.position.y + 2, marker.transform.position.z));
    //    mg.pathTime.Add(1);
    //    mg.pathTime.Add(1);

    //    created.name = createName + blockCounter;
    //    blockCounter++;
    //}

    public void CreateSetActive()
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            return;
        }


        if (!b_Lock)
        {
            if (ModeData.ModeEntity.mode != ModeData.Mode.create)
            {
                ModeData.ModeEntity.mode = ModeData.Mode.create;
            }
            else
            {
                ModeData.ModeEntity.mode = ModeData.Mode.normal;
            }

            if (ModeData.ModeEntity.mode == ModeData.Mode.create)
            {
                createText.SetActive(true);
            }
            else
            {
                createText.SetActive(false);
                b_check = false;
            }
        }
        else
        {
            playSound.PlaySE(PlaySound.SE_TYPE.cancell);
        }

    }

    //public void CreateMoveSetActive()
    //{
    //    if(nowState != State.Create)
    //    {
    //        nowState = nowState == State.none ? State.MoveCreate : State.none;
    //        moveBlockButton.color = nowState == State.none ? Color.white : Color.yellow;
    //    }
    //    else
    //    {
    //        nowState = State.MoveCreate;
    //        blockButton.color = Color.white;
    //        moveBlockButton.color = Color.yellow;

    //    }
    //}

    public int ReturnBlockCount()
    {
        return blockCounter;
    }

    public string ReturnName()
    {
        return createName;
    }
}
