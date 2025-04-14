using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectYesNo : MonoBehaviour
{
    [SerializeField] private GameObject SelectPanel;
    private bool isOnClick = false; //�I���{�^���������ꂽ��
    /// <summary>
    /// yes=true / no=false
    /// </summary>
    private bool isSelect = false;  //�͂�����������

    //�{�^���A�j���[�V�����X�N���v�g
    [SerializeField] private ButtonHover[] hover;

    /// <summary>
    /// �Z���N�g��ʂ�\�����邩
    /// </summary>
    /// <param name="_OnOff">��\��=false / �\��=true</param>
    public void SelectPanelActive(bool _OnOff)
    {
        //��\���ɂ���Ƃ��͏�����Ԃɖ߂�
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

    //�͂��{�^�����������Ƃ�
    public void OnYesButton()
    {
        isSelect = true;
        isOnClick = true;
    }

    //�������{�^�����������Ƃ�
    public void OnNoButton()
    {
        isSelect = false;   
        isOnClick = true;
    }

    /// <summary>
    /// �I���{�^���������ꂽ���ǂ����̌��ʂ�Ԃ�
    /// </summary>
    /// <returns>false=������ĂȂ� / true=�����ꂽ</returns>
    public bool ReturnOnClick() => isOnClick;

    /// <summary>
    /// �ǂ����I��������
    /// </summary>
    /// <returns>false=������ / true=�͂�</returns>
    public bool ReturnSelect() => isSelect;
}
