using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockCreater : MonoBehaviour
{
    bool isDrag;

    Vector2 mousePosition;

    [SerializeField]
    GameObject blockPrefab;

    [SerializeField]
    GameObject markerPrefab;

    GameObject marker;
    BlockMarker bm;

    Vector2 startPosition;
    Vector2 endPosition;

    private int blockCounter = 1;

    private string createName = "CreateBlock";

    private ClipGenerator clipGenerator;

    private PlaySound playSound;

    [SerializeField] private GameObject createText;
    private bool isCheck = false;

    private FunctionLookManager functionLook;

    private Vector2 min_markerSize = new Vector2(0.1f, 0.1f);

    private void Start()
    {
        // nullチェックしてくれ、テスト環境で動かん…
        // すまんかった... by作者
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

        if(GameObject.Find("GameManager") != null)
        {
            functionLook = GameObject.FindWithTag("GameManager").GetComponent<FunctionLookManager>();
        }
        else
        {
            Debug.Log("ゲームマネージャーなし");
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

        if (ModeData.ModeEntity.mode == ModeData.Mode.create)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                    if (hit2d == true && hit2d.collider.tag == "UnCreateArea")
                    {
                        return;
                    }
                    else
                    {
                        marker.SetActive(true);
                        startPosition = mousePosition;
                        bm.isActive = true;
                        isDrag = true;

                    }
                }

                if (Input.GetMouseButtonUp(0) && marker.activeSelf)
                {
                    endPosition = mousePosition;

                    if (!bm.isHitGround)
                    {
                        if (ModeData.ModeEntity.mode == ModeData.Mode.create)
                        {
                            Vector3 PlPos = GameObject.Find("Player").transform.position;

                            float dis = Vector3.Distance(PlPos, mousePosition);

                            //マーカーがプレイヤーと触れてないとき
                            //またはクリックした位置がプレイヤーから半径1.5より離れてる時
                            if (!bm.ReturnHitPL() && dis > 1.5f)
                            {
                                CreateBlock();
                            }
                            //マーカーがプレイヤーと触れてる時
                            else 
                            {
                                playSound.PlaySE(PlaySound.SE_TYPE.develop);
                            }
                        }
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
            isCheck = false;
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
        //マーカーのサイズが特定のサイズ以下のとき
        if(Mathf.Abs(marker.transform.localScale.x) <= min_markerSize.x 
            || Mathf.Abs(marker.transform.localScale.y) <= min_markerSize.y)
        {
            //サイズ設定
            created.transform.localScale = new Vector3(1f, 1f, marker.transform.localScale.z);
        }
        //通常
        else
        {
            //サイズ設定
            created.transform.localScale = new Vector3(
                Mathf.Abs(marker.transform.localScale.x), 
                Mathf.Abs(marker.transform.localScale.y), 
                marker.transform.localScale.z
                );
        }
        created.transform.position = marker.transform.position;
        created.GetComponent<Collider2D>().enabled = false;
        created.GetComponent<Collider2D>().enabled = true;

        created.name = createName + blockCounter;
        blockCounter++;

        clipGenerator.ClipGene(created, isCheck);
        isCheck = true;
    }

    public void CreateSetActive()
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            return;
        }

        //ロックされていないとき
        if ((functionLook.FunctionLook & LookFlags.BlockGenerate) == 0)
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
                isCheck = false;
            }
        }
        else
        {
            playSound.PlaySE(PlaySound.SE_TYPE.cancell);
        }

    }

    public int ReturnBlockCount()
    {
        return blockCounter;
    }

    public string ReturnName()
    {
        return createName;
    }
}
