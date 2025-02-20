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
            if (editObj == null)
            {
                foreach (RaycastHit2D hit in Physics2D.RaycastAll(MousePos(), Vector2.zero))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Gimmick"))
                    {
                        ObjectScaleEditor.SetActive(true);
                        ose.GetObjTransform(hit.collider.gameObject);
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
                    ObjectScaleEditor.SetActive(false);
                }

            }
        }
    }
    private Vector3 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }
}
