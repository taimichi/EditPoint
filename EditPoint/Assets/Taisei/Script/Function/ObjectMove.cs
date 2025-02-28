using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private LayerMask lm;


    //オブジェクトを移動させる際のマウス座標
    private Vector3 scrWldPos;

    //クリックした位置とオブジェクトの座標との差
    private Vector3 mousePos;
    private Vector3 offset;

    private Vector3 nowPos;

    //単体移動
    private GameObject Obj;
    //オブジェクト移動中か
    private bool isObjMove = false;

    //選択時にアウトラインをつける
    private GameObject ClickObj;

    //条件を簡略化する変数
    private bool isNoHit;
    private bool isSpecificTag = false;

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

    //機能を使用可能かどうか
    [SerializeField] private bool isLook = false;


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
        if (GameData.GameEntity.isPlayNow || isLook)
        {
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            return;
        }

        if (!isLook)
        {
            if (Input.GetMouseButtonDown(0) &&
                (ModeData.ModeEntity.mode == ModeData.Mode.normal || ModeData.ModeEntity.mode == ModeData.Mode.moveANDdirect))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, Mathf.Infinity, lm);

                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                //ハンドルだった場合
                //if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Handle"))
                //{
                //    Debug.Log("ハンドルダヨーン");
                //    return;
                //}

                //オブジェクトが存在するかどうか
                isNoHit = (hit2d == false);
                //オブジェクトがあるとき
                if (!isNoHit)
                {
                    //特定のタグの時にフラグを立てる
                    isSpecificTag = new List<string> { "Player", "UnTouch", "Marcker", "MoveGround" }.Contains(hit2d.collider.tag);
                }

                //UIや動かしたくないオブジェクトだったらだったら何もしない
                //解除
                if (isNoHit || isSpecificTag)
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
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset = ClickObj.transform.position - mousePos;
                playSound.PlaySE(PlaySound.SE_TYPE.select);

                //オブジェクトムーブとサイズ・角度変更が重複して作動してしまうため、
                //クリエイトブロックの四隅の座標を取り、マウスの位置がそれらから内側に少しずらした座標なら
                //取得できるようにする
                //↓
                SpriteRenderer sr = ClickObj.GetComponent<SpriteRenderer>();
                Bounds bound = sr.bounds;

                //マウスの位置がオブジェクトを動かしていい範囲内かどうか
                if (mousePos.x >= bound.min.x + inLine && mousePos.x <= bound.max.x - inLine
                    && mousePos.y >= bound.min.y + inLine && mousePos.y <= bound.max.y - inLine)
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
                    isObjMove = true;
                    ModeData.ModeEntity.mode = ModeData.Mode.moveANDdirect;

                    // エディター追加
                    ObjectScaleEditor.SetActive(true);
                    //ObjectScaleEditor.GetComponent<ObjectScaleEditor>().GetObjTransform(Obj);
                }


            }

            if (isObjMove && isMove && ModeData.ModeEntity.mode == ModeData.Mode.moveANDdirect)
            {
                if (Input.GetMouseButton(0))
                {
                    scrWldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    scrWldPos.z = 10;
                    //Obj.transform.position = scrWldPos + offset;


                    //ObjectScaleEditor.GetComponent<ObjectScaleEditor>().GetObjTransform(Obj);

                }
                else if (Input.GetMouseButtonUp(0))
                {
                    checkHG = Obj.GetComponent<CheckHitGround>();
                    if (checkHG.ReturnHit())
                    {
                        Obj.transform.position = nowPos;
                    }

                    playSound.PlaySE(PlaySound.SE_TYPE.objMove);
                    isObjMove = false;
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

    public GameObject ReturnClickObj() => ClickObj;
}
