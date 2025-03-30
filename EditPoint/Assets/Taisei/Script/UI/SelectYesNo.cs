using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectYesNo : MonoBehaviour
{
    [SerializeField] private GameObject SelectPanel;
    private bool isOnClick = false; //選択ボタンが押されたか
    /// <summary>
    /// yes=true / no=false
    /// </summary>
    private bool isSelect = false;  //はいかいいえか

    //ボタンアニメーションスクリプト
    [SerializeField] private ButtonHover[] hover;

    /// <summary>
    /// セレクト画面を表示するか
    /// </summary>
    /// <param name="_OnOff">非表示=false / 表示=true</param>
    public void SelectPanelActive(bool _OnOff)
    {
        //非表示にするときは初期状態に戻す
        if (!_OnOff)
        {
            isOnClick = false;
            for(int i = 0; i < hover.Length; i++)
            {
                hover[i].ResetButton();
            }
        }
        SelectPanel.SetActive(_OnOff);
    }

    //はいボタンを押したとき
    public void OnYesButton()
    {
        isSelect = true;
        isOnClick = true;
    }

    //いいえボタンを押したとき
    public void OnNoButton()
    {
        isSelect = false;   
        isOnClick = true;
    }

    /// <summary>
    /// 選択ボタンが押されたかどうかの結果を返す
    /// </summary>
    /// <returns>false=押されてない / true=押された</returns>
    public bool ReturnOnClick() => isOnClick;

    /// <summary>
    /// どちらを選択したか
    /// </summary>
    /// <returns>false=いいえ / true=はい</returns>
    public bool ReturnSelect() => isSelect;
}
