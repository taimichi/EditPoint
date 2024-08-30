using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreater : MonoBehaviour
{
    public enum State
    {
        none,
        Create,
    }

    public bool isMoveBlock = false;

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

    private void Start()
    {
        marker = Instantiate(markerPrefab);
        bm = marker.GetComponent<BlockMarker>();
        bm.isActive = false;
    }

    private void Update()
    {

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (nowState == State.Create)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = mousePosition;
                bm.isActive = true;
                isDrag = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                endPosition = mousePosition;

                if (!bm.isHitGround)
                {
                    if (!isMoveBlock)
                    {
                        CreateBlock();
                    }
                    else
                    {
                        CreateMoveBlock();
                    }
                }

                bm.isActive = false;
                isDrag = false;
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

    private void CreateBlock()
    {
        GameObject created = Instantiate(blockPrefab);
        created.transform.localScale = marker.transform.localScale;
        created.transform.position = marker.transform.position;
    }

    private void CreateMoveBlock()
    {
        GameObject created = Instantiate(moveBlockPrefab);
        created.transform.localScale = marker.transform.localScale;
        created.transform.position = marker.transform.position;

        MoveGround mg = created.GetComponent<MoveGround>();
        Vector3 markerPos = marker.transform.position;
        mg.path.Add(markerPos);
        mg.path.Add(new Vector3(markerPos.x, markerPos.y + 2, markerPos.z));
        mg.pathTime.Add(1);
        mg.pathTime.Add(1);

    }

    public void CreateSetActive(bool tf)
    {
        if (tf)
        {
            nowState = State.Create;
        }
        else
        {
            nowState = State.none;
        }
    }
}
