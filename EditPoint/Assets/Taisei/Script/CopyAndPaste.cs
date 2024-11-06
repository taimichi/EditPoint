using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CopyAndPaste : MonoBehaviour
{
    [SerializeField] private Text copyModeText;

    //コピー元のオブジェクト
    private GameObject CopyObj;

    [SerializeField, Header("ペーストできる回数")] private int i_PasteNum = 1000;
    [SerializeField, Header("コピーできる回数")] private int i_CopyNum = 1000;

    //オブジェクトのペーストや移動するときにtrueにする
    private bool b_setOnOff = false;

    //ペースト時のオブジェクト
    private GameObject PasteObj;


    //マウスの座標関連
    private Vector3 v3_mousePos;
    private Vector3 v3_scrWldPos;

    //選択したオブジェクト
    private GameObject ClickObj;
    /// <summary>
    /// スクリプタブルオブジェクトでまとめてあるマテリアル
    /// "layerMaterials"というリストが入っている
    /// </summary>
    [SerializeField] private Materials materials;


    //条件を説明する変数
    private bool b_isNoHit;
    private bool b_isSpecificTag;

    private PlaySound playSound;

    [SerializeField] private bool b_Lock = false;

    private ClipPlay clipPlay;
    private GetCopyObj gco;


    void Start()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        CopyObj = null;
        PasteObj = null;
        copyModeText.enabled = false;
    }

    void Update()
    {
        //再生中は編集機能をロック
        if (GameData.GameEntity.b_playNow)
        {
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            return;
        }

        //コピーモードの時
        if (ModeData.ModeEntity.mode == ModeData.Mode.copy)
        {
            Copy();
            return;
        }

        if(b_setOnOff && ModeData.ModeEntity.mode == ModeData.Mode.paste)
        {
            v3_mousePos = Input.mousePosition;
            v3_mousePos.z = 10;

            v3_scrWldPos = Camera.main.ScreenToWorldPoint(v3_mousePos);

                PasteObj.transform.position = v3_scrWldPos;

            //UIの上じゃなかったら
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //クリックしたらカーソルの位置にオブジェクトを置く
                if (Input.GetMouseButtonDown(0))
                {
                    playSound.PlaySE(PlaySound.SE_TYPE.paste);
                    if (CopyObj.name.Contains("Blower"))
                    {
                        PasteObj.transform.GetChild(0).GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
                        PasteObj.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = false;

                    }
                    else
                    {
                        PasteObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
                        PasteObj.GetComponent<Collider2D>().isTrigger = false;
                    }

                    if(CopyObj.GetComponent<GetCopyObj>() == true)
                    {
                        gco = CopyObj.GetComponent<GetCopyObj>();
                        clipPlay = gco.ReturnAttachClip().GetComponent<ClipPlay>();
                        clipPlay.OutGetObj(PasteObj);
                    }

                    Paste();
                }
            }

            //右クリックでペーストモード解除
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("ペーストモード解除");
                playSound.PlaySE(PlaySound.SE_TYPE.cancell);
                    MaterialReset();
                    Destroy(PasteObj);
                    PasteObj = null;
                ModeData.ModeEntity.mode = ModeData.Mode.normal;
                b_setOnOff = false;
                copyModeText.enabled = false;
            }
        }

        //条件分が出てこなかったのでとりあえずこれで
        if(ModeData.ModeEntity.mode == ModeData.Mode.copy || ModeData.ModeEntity.mode == ModeData.Mode.paste)
        {

        }
        else
        {
            copyModeText.enabled = false;
        }
    }

    public bool ReturnSetOnOff()
    {
        return b_setOnOff;
    }

    private void GetObj()
    {
        //クリックしたときに選択したオブジェクトのレイヤーに変更
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            b_isNoHit = (hit2d == false);
            if (!b_isNoHit)
            {
                b_isSpecificTag = new List<string> { "Player", "UnTouch", "Markar" }.Contains(hit2d.collider.tag);
            }

            if (b_isNoHit || b_isSpecificTag)
            {
                if (ClickObj != null)
                {
                    MaterialReset();
                }
                ClickObj = null;
                return;
            }

            //新たなオブジェクトに更新する前に元のマテリアルに戻す
            if (ClickObj != null)
            {
                MaterialReset();
            }

            ClickObj = hit2d.collider.gameObject;
            if (ClickObj.transform.parent != null && ClickObj.transform.parent.gameObject.name.Contains("Blower"))
            {
                ClickObj = hit2d.collider.transform.parent.gameObject;
                ClickObj.transform.GetChild(0).GetComponent<SpriteRenderer>().material = materials.layerMaterials[1];
            }
            else
            {
                ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[1];
            }

            //コピーモードの時のみ
            if (ModeData.ModeEntity.mode == ModeData.Mode.copy)
            {
                //クリックしたオブジェクトが選択可能オブジェクトだったら
                if (ClickObj != null)
                {
                    playSound.PlaySE(PlaySound.SE_TYPE.copy);
                        if (i_CopyNum > 0)
                        {
                            CopyObj = ClickObj;
                            PasteObj = null;
                        }
                    Paste();
                    ModeData.ModeEntity.mode = ModeData.Mode.paste;
                }
            }
        }
    }

    private void Copy()
    {
        GetObj();

        //コピーモードを解除
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("コピーモード解除");
            playSound.PlaySE(PlaySound.SE_TYPE.cancell);
            MaterialReset();
            ModeData.ModeEntity.mode = ModeData.Mode.normal;
            ClickObj = null;
            CopyObj = null;
            copyModeText.enabled = false;
        }
    }

    //ペースト
    private void Paste()
    {
        copyModeText.text = "現在ペーストモードです";
        if (i_PasteNum > 0)
        {
            PasteObj = Instantiate(CopyObj);
            if(CopyObj.name.Contains("Blower"))
            {
                PasteObj.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = true;
            }
            else
            {
                PasteObj.GetComponent<Collider2D>().isTrigger = true;
            }
            i_PasteNum--;
        }
        b_setOnOff = true;
    }

    //コピーボタンを押した時
    public void OnCopy()
    {
        //再生中の時は編集機能をロック
        if (GameData.GameEntity.b_playNow)
        {
            return;
        }

        if (!b_Lock)
        {
            if (ModeData.ModeEntity.mode == ModeData.Mode.copy || ModeData.ModeEntity.mode == ModeData.Mode.paste)
            {
                ModeData.ModeEntity.mode = ModeData.Mode.normal;
                MaterialReset();
                if (PasteObj != null)
                {
                    Destroy(PasteObj);
                }
                ClickObj = null;
                CopyObj = null;
                PasteObj = null;
                b_setOnOff = false;
                copyModeText.enabled = false;
            }
            else
            {
                i_CopyNum--;
                ModeData.ModeEntity.mode = ModeData.Mode.copy;
                copyModeText.enabled = true;
                copyModeText.text = "現在コピーモードです";

            }
        }
        else
        {
            playSound.PlaySE(PlaySound.SE_TYPE.cancell);
        }
    }

    /// <summary>
    /// マテリアルを最初の状態に戻す
    /// </summary>
    private void MaterialReset()
    {
        if (ClickObj != null)
        {
            if(ClickObj.name.Contains("Blower"))
            {
                ClickObj.transform.GetChild(0).GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
            }
            else
            {
                ClickObj.GetComponent<SpriteRenderer>().material = materials.layerMaterials[0];
            }
        }
    }


}
