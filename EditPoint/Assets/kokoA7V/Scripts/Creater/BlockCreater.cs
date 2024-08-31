using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockCreater : MonoBehaviour
{
    public enum State
    {
        none,
        Create,
        MoveCreate
    }

    [SerializeField]
    State nowState = State.none;

    bool isDrag;

    Vector2 mousePosition;

    [SerializeField]
    GameObject blockPrefab;

    [SerializeField]
    GameObject moveBlockPrefab;

    [SerializeField]
    GameObject markerPrefab;

    GameObject marker;
    BlockMarker bm;

    Vector2 startPosition;
    Vector2 endPosition;

    private int blockCounter = 1;


    private string createName = "CreateBlock";

    private void Start()
    {
        marker = Instantiate(markerPrefab);
        bm = marker.GetComponent<BlockMarker>();
        bm.isActive = false;

        marker.SetActive(false);
    }

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (nowState == State.Create || nowState == State.MoveCreate)
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
                        if (nowState == State.Create)
                        {
                            CreateBlock();
                        }
                        else if (nowState == State.MoveCreate)
                        {
                            CreateMoveBlock();
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

        if (Input.GetMouseButtonDown(1) && !isDrag)
        {
            nowState = State.none;
            marker.SetActive(false);
        }
    }

    private void CreateBlock()
    {
        GameObject created = Instantiate(blockPrefab);
        created.transform.localScale = marker.transform.localScale;
        created.transform.position = marker.transform.position;
        created.GetComponent<Collider2D>().enabled = false;
        created.GetComponent<Collider2D>().enabled = true;

        created.name = createName + blockCounter;
        blockCounter++;
    }

    private void CreateMoveBlock()
    {
        GameObject created = Instantiate(moveBlockPrefab);
        created.transform.localScale = marker.transform.localScale;
        created.transform.position = marker.transform.position;

        MoveGround mg = created.GetComponent<MoveGround>();
        mg.path.Add(marker.transform.position);
        mg.path.Add(new Vector3(marker.transform.position.x, marker.transform.position.y + 2, marker.transform.position.z));
        mg.pathTime.Add(1);
        mg.pathTime.Add(1);

        created.name = createName + blockCounter;
        blockCounter++;
    }

    public void CreateSetActive()
    {
        nowState = nowState == State.none ? State.Create : State.none;
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
