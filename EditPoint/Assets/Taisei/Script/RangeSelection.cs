using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelection : MonoBehaviour
{
    //初期のステータス
    [SerializeField, Header("範囲選択オブジェクトの初期スケール")] private Vector3 v3_initScale;
    //現在のスケール
    Vector3 v3_nowScale;

    [SerializeField] private Renderer targetRenderer;
    //範囲選択図形の左上と右下
    private Vector3 v3_newTopLeft;
    private Vector3 v3_newBottomRight;

    //マウスで取得する左上の座標
    private Vector3 v3_StartMousePos;
    private Vector3 v3_StartScrWldPos;

    //マウスで取得する右下の座標
    private Vector3 v3_nowMousePos;
    private Vector3 v3_nowScrWldPos;

    //新しい幅と高さ
    float f_newWidth;
    float f_newHeight;

    //新しいスケール
    private Vector3 v3_newScale;

    private bool b_selectMode = false;      //範囲選択できるかどうか
    private bool b_checkSelect = false;     //範囲選択中かどうか

    //範囲選択したオブジェクトを格納する
    private List<GameObject> L_SelectedObj = new List<GameObject>();

    [SerializeField] private Material[] materials = new Material[2];


    void Start()
    {

    }

    void Update()
    {
        if (!b_checkSelect)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (Input.GetMouseButtonDown(0))
            {
                if (hit2d == true)
                {
                    
                }
                else
                {
                    for(int i = 0; i < L_SelectedObj.Count; i++)
                    {
                        L_SelectedObj[i].GetComponent<SpriteRenderer>().material = materials[0];
                    }
                    L_SelectedObj = new List<GameObject>();
                    b_checkSelect = true;
                    b_selectMode = true;
                    v3_StartMousePos = Input.mousePosition;
                    v3_StartMousePos.z = 10;
                    v3_StartScrWldPos = Camera.main.ScreenToWorldPoint(v3_StartMousePos);
                    v3_newTopLeft = v3_StartScrWldPos;
                }
            }

        }
        else
        {

            if (Input.GetMouseButton(0) && b_selectMode)
            {
                v3_nowMousePos = Input.mousePosition;
                v3_nowMousePos.z = 10;
                v3_nowScrWldPos = Camera.main.ScreenToWorldPoint(v3_nowMousePos);
                v3_nowScrWldPos.x += 0.01f;
                v3_nowScrWldPos.y += 0.01f;
                v3_newBottomRight = v3_nowScrWldPos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                b_selectMode = false;
                b_checkSelect = false;
                targetRenderer.transform.localScale = v3_initScale;
                for(int i = 0; i < L_SelectedObj.Count; i++)
                {
                    L_SelectedObj[i].GetComponent<SpriteRenderer>().material = materials[1];
                }
            }

            if (b_selectMode)
            {
                // 新しい幅と高さを計算
                f_newWidth = v3_newBottomRight.x - v3_newTopLeft.x;
                f_newHeight = v3_newTopLeft.y - v3_newBottomRight.y;

                // 現在のスケールを取得
                v3_nowScale = targetRenderer.transform.localScale;
                v3_nowScale.z = 1;

                // 新しいスケールを計算
                v3_newScale.x = f_newWidth / targetRenderer.bounds.size.x;
                v3_newScale.y = f_newHeight / targetRenderer.bounds.size.y;
                v3_newScale.z = 1;

                // スケールを変更
                targetRenderer.transform.localScale = new Vector3(
                    v3_nowScale.x * v3_newScale.x,
                    v3_nowScale.y * v3_newScale.y,
                    v3_nowScale.z * v3_newScale.z
                );

                // 新しい中心位置を計算
                Vector3 newPosition = new Vector3((v3_newTopLeft.x + v3_newBottomRight.x) / 2, (v3_newTopLeft.y + v3_newBottomRight.y) / 2, (v3_newTopLeft.z + v3_newBottomRight.z) / 2);
                targetRenderer.transform.position = newPosition;

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!L_SelectedObj.Contains(collision.gameObject))
        {
            L_SelectedObj.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (L_SelectedObj.Contains(collision.gameObject))
        {
            L_SelectedObj.Remove(collision.gameObject);
        }
    }

}
