using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickObjectGetter : MonoBehaviour
{
    [SerializeField]
    GameObject ObjectScaleEditor;

    ObjectScaleEditor ose;

    [SerializeField]
    GameObject editObj;

    private void Start()
    {
        ose = ObjectScaleEditor.GetComponent<ObjectScaleEditor>();
        ObjectScaleEditor.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            GimmickObjGet(false);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GimmickObjGet(true);
        }
    }

    /// <summary>
    /// オブジェクトの選択処理　引数で左or右クリックを判別
    /// 左 false/右 true
    /// </summary>
    /// <param name="trigger">true/false</param>
    private void GimmickObjGet(bool trigger)
    {

        // ObjectScaleEdditor用のブロッククリック時取得プログラム
        if (editObj == null)
        {
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(MousePos(), Vector2.zero))
            {
                // コピペ時はうごかないように
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Gimmick") && ModeData.ModeEntity.mode != ModeData.Mode.copy　&& ModeData.ModeEntity.mode != ModeData.Mode.paste)
                {
                    ObjectScaleEditor.SetActive(true);
                    ose.GetObjTransform(hit.collider.gameObject, trigger);
                    editObj = hit.collider.gameObject;
                }
            }
        }
        else
        {
            bool isEditCancel = true;
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(MousePos(), Vector2.zero))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Handle") || hit.collider.gameObject == editObj)
                {
                    isEditCancel = false;
                }
            }

            if (isEditCancel)
            {
                editObj = null;
                ose.DeleteButtonChange(false);
                ObjectScaleEditor.SetActive(false);
            }

        }
    }

    private Vector3 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }
}
