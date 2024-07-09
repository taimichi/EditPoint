using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayer : MonoBehaviour
{
    [SerializeField, Range(1,3), Header("初期で設定するプレイヤーのレイヤー")] private int i_plLayer = 1;


    public int ReturnPLLayer()
    {
        return i_plLayer;
    }
}
