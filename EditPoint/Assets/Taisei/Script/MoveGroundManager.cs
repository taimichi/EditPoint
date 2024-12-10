using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGroundManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> MoveGrounds = new List<GameObject>();
    private List<MoveGround> moveGroundScripts = new List<MoveGround>();

    void Start()
    {

    }

    void Update()
    {
        MoveGroundController();
    }

    /// <summary>
    /// MoveGround�����X�g�ɒǉ�
    /// </summary>
    /// <param name="_getObj">�ǉ�����I�u�W�F�N�g</param>
    public void GetMoveGrounds(GameObject _getObj)
    {
        MoveGrounds.Add(_getObj);
        moveGroundScripts.Add(_getObj.GetComponent<MoveGround>());
    }

    private void MoveGroundController()
    {
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
