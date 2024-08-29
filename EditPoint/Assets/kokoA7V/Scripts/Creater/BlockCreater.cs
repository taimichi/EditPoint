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

    public enum LayerType
    {
        Layer1,
        Layer2,
        Layer3
    }

    [SerializeField]
    State nowState = State.none;

    [SerializeField]
    LayerType nowLayer = LayerType.Layer1;

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
                    CreateBlock(nowLayer);
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

    private void CreateBlock(LayerType _nowLayer)
    {
        GameObject created = Instantiate(blockPrefab);
        created.transform.localScale = marker.transform.localScale;
        created.transform.position = marker.transform.position;
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
