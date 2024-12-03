using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableObject : MonoBehaviour
{
    [SerializeField]
    LayerMask handle_LayerMask;

    // 仮トランスフォーム
    [SerializeField]
    Vector2 objPosition = new Vector2(0, 0);
    [SerializeField]
    float objRotation = 0;
    [SerializeField]
    Vector2 objScale = new Vector2(1, 1);

    [SerializeField]
    GameObject[] handle = new GameObject[9];

    // UR = 0
    // UL = 1
    // DR = 2
    // DL = 3
    // R = 4
    // L = 5
    // U = 6
    // D = 7

    //ハンドルサイズ調整用
    [SerializeField]
    float handleSize = 0.1f;

    // クリックの状態
    Vector2 clickStartPos;
    bool isHandleGrab = false;

    // 取得したハンドル情報
    float nowHandlePriority = 0;
    Vector2 scaleSign;
    bool isHandleRot = false;

    // 仮想オブジェクトデータ
    [SerializeField] GameObject virtualObject;
    Vector2 virtualObjPosition;
    float virtualObjRotation;
    Vector2 virtualObjScale;

    [SerializeField]
    GameObject markerObject;

    private void Start()
    {
        virtualObjPosition = objPosition;
        virtualObjRotation = objRotation;
        virtualObjScale = objScale;
    }

    private void Update()
    {

        // ハンドルポジション
        handle[0].transform.localPosition = new Vector2(objScale.x / 2, objScale.y / 2);
        handle[1].transform.localPosition = new Vector2(-objScale.x / 2, objScale.y / 2);
        handle[2].transform.localPosition = new Vector2(objScale.x / 2, -objScale.y / 2);
        handle[3].transform.localPosition = new Vector2(-objScale.x / 2, -objScale.y / 2);

        handle[4].transform.localPosition = new Vector2(objScale.x / 2, 0);
        handle[5].transform.localPosition = new Vector2(-objScale.x / 2, 0);
        handle[6].transform.localPosition = new Vector2(0, objScale.y / 2);
        handle[7].transform.localPosition = new Vector2(0, -objScale.y / 2);

        handle[8].transform.localPosition = new Vector2(0, objScale.y / 2 + 0.5f);


        // ハンドルスケール
        handle[0].transform.localScale = new Vector2(2 * handleSize, 2 * handleSize);
        handle[1].transform.localScale = new Vector2(2 * handleSize, 2 * handleSize);
        handle[2].transform.localScale = new Vector2(2 * handleSize, 2 * handleSize);
        handle[3].transform.localScale = new Vector2(2 * handleSize, 2 * handleSize);

        handle[4].transform.localScale = new Vector2(handleSize, objScale.y);
        handle[5].transform.localScale = new Vector2(handleSize, objScale.y);
        handle[6].transform.localScale = new Vector2(objScale.x, handleSize);
        handle[7].transform.localScale = new Vector2(objScale.x, handleSize);

        handle[8].transform.localScale = new Vector2(2 * handleSize, 2 * handleSize);

        // 仮想オブジェクト設定
        virtualObject.transform.position = virtualObjPosition;
        virtualObject.transform.localEulerAngles = new Vector3(0, 0, virtualObjRotation);
        virtualObject.transform.localScale = virtualObjScale;


        // マーカーポジションとローテーション
        // スケールはいじらない
        markerObject.transform.position = objPosition;
        markerObject.transform.localEulerAngles = new Vector3(0, 0, objRotation);



        // マウスクリック、ハンドル取得
        if (Input.GetMouseButtonDown(0))
        {
            nowHandlePriority = 0;
            foreach (RaycastHit2D hit2 in Physics2D.RaycastAll(MousePos(), Vector2.zero, handle_LayerMask))
            {
                if (hit2.collider.gameObject.TryGetComponent<HandleSign>(out var _handleSign))
                {
                    clickStartPos = MousePos();
                    isHandleGrab = true;

                    // プライオリティが一番高いものを取得
                    if (_handleSign.priority > nowHandlePriority)
                    {
                        scaleSign = _handleSign.handleSign;
                        isHandleRot = _handleSign.isRot;
                        nowHandlePriority = _handleSign.priority;
                    }
                }
            }
        }

        Vector2 nowMousePos = MousePos();

        // 変更前
        Vector2 mouseVec = nowMousePos - clickStartPos;

        // 角度計算式
        float mouseRad = Mathf.Atan2(mouseVec.y, mouseVec.x);
        float editRad = mouseRad - objRotation * Mathf.Deg2Rad;
        
        // 変更後
        Vector2 editVec = new Vector2(mouseVec.magnitude * Mathf.Cos(editRad), mouseVec.magnitude * Mathf.Sin(editRad));

        //Vector2 positionSign = new Vector2(Mathf.Abs(scaleSign.x), Mathf.Abs(scaleSign.y));
        //if (positionSign.x == 0 || positionSign.y == 0)
        //{
        //    positionSign = new Vector2(Mathf.Abs(scaleSign.x) * Mathf.Cos(objRotation * Mathf.Deg2Rad), Mathf.Abs(scaleSign.y) * Mathf.Sin(objRotation * Mathf.Deg2Rad));
        //}

        Vector2 rotMouseVec = nowMousePos - objPosition;
        float rotRad = Mathf.Atan2(rotMouseVec.y, rotMouseVec.x);

        // マウスドラッグ、オブジェクト大きさ変更中
        if (Input.GetMouseButton(0))
        {
            if (isHandleGrab)
            {
                if (isHandleRot)
                {
                    virtualObjRotation = rotRad * Mathf.Rad2Deg - 90;
                }
                else
                {
                    virtualObjPosition = objPosition + mouseVec / 2;
                    virtualObjScale = objScale + editVec * scaleSign;
                }
            }
        }

        // マウスクリック離す、仮オブジェクトのサイズに変更
        if (Input.GetMouseButtonUp(0))
        {
            if (isHandleGrab)
            {
                if (isHandleRot)
                {
                    objRotation = virtualObjRotation;
                }
                else
                {
                    objPosition = virtualObjPosition;
                    objScale = virtualObjScale;
                }
            }

            isHandleGrab = false;
            isHandleRot = false;
        }
        
    }

    // マウスポジション取得
    private Vector3 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }

}
