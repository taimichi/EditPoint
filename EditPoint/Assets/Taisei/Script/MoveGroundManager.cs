using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGroundManager : MonoBehaviour
{
    private List<GameObject> MoveGrounds = new List<GameObject>();
    private List<MoveGround> moveGroundScripts = new List<MoveGround>();

    void Update()
    {
        MoveGroundController();
    }

    /// <summary>
    /// MoveGroundをリストに追加
    /// </summary>
    /// <param name="_getObj">追加するオブジェクト</param>
    public void GetMoveGrounds(GameObject _getObj)
    {
        MoveGrounds.Add(_getObj);
        moveGroundScripts.Add(_getObj.GetComponent<MoveGround>());
    }

    /// <summary>
    /// MoveGroundをリストから削除
    /// </summary>
    /// <param name="_delObj">削除するオブジェクト</param>
    public void DeleteMoveGrounds(GameObject _delObj)
    {
        //削除したいオブジェクトをリストから探す
        for(int i = 0; i < MoveGrounds.Count; i++)
        {
            //同じオブジェクトの時
            if (MoveGrounds[i] == _delObj)
            {
                //消したい要素の場所に最後尾の要素を移し、
                //一番最後の要素を消す
                //こうする理由:リストで消す要素が先頭に近いほど、
                //コピー処理をする要素が多くなり処理が重くなるため
                //※ただしこの方法は順番が関係ないときに使える
                MoveGrounds[i] = MoveGrounds[MoveGrounds.Count - 1];
                MoveGrounds.RemoveAt(MoveGrounds.Count - 1);
                moveGroundScripts[i] = moveGroundScripts[moveGroundScripts.Count - 1];
                moveGroundScripts.RemoveAt(moveGroundScripts.Count - 1);
                break;
            }
        }

    }

    /// <summary>
    /// 条件が通るとき、MoveGround.csのCheckReset関数を実行
    /// </summary>
    private void MoveGroundController()
    {
        //動く床があり、リセットが入った時
        if (GameData.GameEntity.isTimebarReset && MoveGrounds.Count > 0)
        {
            for(int i = 0; i < MoveGrounds.Count; i++)
            {
                moveGroundScripts[i].CheckReset();
            }
            GameData.GameEntity.isTimebarReset = false;
        }
    }
}
