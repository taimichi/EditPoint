using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private LayerMask lm;


    //オブジェクトを移動させる際のマウス座標
    private Vector3 v3_scrWldPos;

    //クリックした位置とオブジェクトの座標との差
    private Vector3 v3_mousePos;
    private Vector3 v3_offset;

    private Vector3 nowPos;

    //単体移動
    private GameObject Obj;
    private bool b_objMove = false;

    //選択時にアウトラインをつける
    private GameObject ClickObj;

    //条件を簡略化する変数
    private bool b_isNoHit;
    private bool b_isSpecificTag = false;

    private PlaySound playSound;

    private CheckHitGround checkHG;

    // ObjectScaleEditor追加
    // アタッチすること
    [SerializeField]
    GameObject ObjectScaleEditor;

    //内側にずらす量
    [SerializeField, Range(0.1f,0.3f)] private float inLine = 0.1f;
    //動かせるかどうか
    private bool isMove = false;    


    void Start()
    {
        if (GameObject.Find("AudioCanvas") != null)
        {
            playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        }
        else
        {
            Debug.Log("audioなし");
        }
    }


    void Update()
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.isPlayNow)
        {
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            return;
        }

        if (Input.GetMouseButtonDown(0) &&
            (ModeData.ModeEntity.mode == ModeData.Mode.normal || ModeData.ModeEntity.mode == ModeData.Mode.moveANDdirect))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, Mathf.Infinity, lm);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (hit2d.collider != null && hit2d.collider.tag == "Handle")
            {
                Debug.Log("ハンドルダヨーン");
                return;
            }

            b_isNoHit = (hit2d == false);
            if (!b_isNoHit)
            {
                b_isSpecificTag = new List<string> { "Player", "UnTouch", "Marcker", "MoveGround"}.Contains(hit2d.collider.tag);
            }

            //UIや動かしたくないオブジェクトだったらだったら何もしない
            //解除
            if (b_isNoHit || b_isSpecificTag)
            {
                if (ClickObj != null)
                {
                    ObjectScaleEditor.SetActive(false);
                }
                ClickObj = null;
                isMove = false;

                return;
            }

            ClickObj = hit2d.collider.gameObject;
            if (ClickObj.transform.parent != null && ClickObj.transform.parent.gameObject.name.Contains("Blower"))
            {
                ClickObj = hit2d.collider.transform.parent.gameObject;

            }
            v3_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v3_offset = ClickObj.transform.position - v3_mousePos;
            playSound.PlaySE(PlaySound.SE_TYPE.select);

            //オブジェクトムーブとサイズ・角度変更が重複して作動してしまうため、
            //クリエイトブロックの四隅の座標を取り、マウスの位置がそれらから内側に少しずらした座標なら
            //取得できるようにする
            //↓
            SpriteRenderer sr = ClickObj.GetComponent<SpriteRenderer>();
            Bounds bound = sr.bounds;

            //マウスの位置がオブジェクトを動かしていい範囲内かどうか
            if(v3_mousePos.x >= bound.min.x + inLine && v3_mousePos.x <= bound.max.x - inLine
                && v3_mousePos.y >= bound.min.y + inLine && v3_mousePos.y <= bound.max.y - inLine)
            {
                isMove = true;
                Debug.Log("いけるよー");
            }

            if (hit2d)
            {
                Obj = ClickObj;

                if (Obj.name.Contains("Blower"))
                {
                    Obj.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = true;
                }
                else
                {
                    Obj.GetComponent<Collider2D>().isTrigger = true;
                }

                nowPos = Obj.transform.position;
                b_objMove = true;
                ModeData.ModeEntity.mode = ModeData.Mode.moveANDdirect;

                // エディター追加
                ObjectScaleEditor.SetActive(true);
                ObjectScaleEditor.GetComponent<ObjectScaleEditor>().GetObjTransform(Obj);
            }


        }

        if (b_objMove && isMove && ModeData.ModeEntity.mode == ModeData.Mode.moveANDdirect)
        {
            if (Input.GetMouseButton(0))
            {
                v3_scrWldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                v3_scrWldPos.z = 10;
                Obj.transform.position = v3_scrWldPos + v3_offset;


                ObjectScaleEditor.GetComponent<ObjectScaleEditor>().GetObjTransform(Obj);

            }
            else if (Input.GetMouseButtonUp(0))
            {
                checkHG = Obj.GetComponent<CheckHitGround>();
                if (checkHG.ReturnHit())
                {
                    Obj.transform.position = nowPos;
                }

                playSound.PlaySE(PlaySound.SE_TYPE.objMove);
                b_objMove = false;
                if (Obj.name.Contains("Blower"))
                {
                    Obj.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = false;
                }
                else
                {
                    Obj.GetComponent<Collider2D>().isTrigger = false;
                }
                isMove = false;
                Obj = null;
                ModeData.ModeEntity.mode = ModeData.Mode.normal;
            }
        }

        //deleteキーで選択してるオブジェクトを消す
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (ClickObj != null)
            {
                Destroy(ClickObj);
                ClickObj = null;
                isMove = false;
                ObjectScaleEditor.SetActive(false);
            }
        }
    }

    public bool ReturnObjMove()
    {
        return b_objMove;
    }

    public GameObject ReturnClickObj()
    {
        return ClickObj;
    }
}
