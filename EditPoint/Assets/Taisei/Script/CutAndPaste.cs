using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutAndPaste : MonoBehaviour
{
    private GameObject ChoiseObj;
    private GameObject CutObj;

    [SerializeField] private LayerChange layerChange;

    [SerializeField, Header("ペーストできる回数")] private int PasteNum = 1;

    void Start()
    {
        ChoiseObj = null;
        CutObj = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer1"))
            {
                layerChange.OutChangeLayerNum(1);
            }
            else if(hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer2"))
            {
                layerChange.OutChangeLayerNum(2);
            }
            else if(hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Layer3"))
            {
                layerChange.OutChangeLayerNum(3);
            }
            else
            {
                layerChange.OutChangeLayerNum(0);
            }

            if (hit2d)
            {
                ChoiseObj = hit2d.transform.gameObject;
            }
        }
    }

    //カットボタンを押した時
    public void OnCut()
    {
        if (PasteNum > 0)
        {
            CutObj = ChoiseObj;
            Destroy(ChoiseObj);
            Debug.Log(CutObj);
        }
    }

}
