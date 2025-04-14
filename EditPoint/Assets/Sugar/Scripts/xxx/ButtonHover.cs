using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;    // UI

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // �{�^��UI�Ƀ}�E�X�J�[�\�����G��Ă��邩
    bool onUI = false;

    // UI�𓮂����N���X
    ClassUIAnim moveUI;

    // �T�C�Y�ύX
    float sclX=1, sclY = 1;
    // �ύX���x
    float sclSpd = 0.02f;
    // �ڕW
    float sclGoalMax = 1.3f;
    float sclGoalMin = 1;
    [SerializeField] RectTransform myRct;

    // Switch����
    enum numUI
    {
        sclUP,
        sclDown,
        end
    }

    numUI num;
    void Start()
    {
        // �C���X�^���X����
        moveUI = new ClassUIAnim();

        num = numUI.sclUP;
    }

    void Update()
    {
        if (onUI)
        {
            SCL();
            myRct.localScale = new Vector2(sclX, sclY);
        }
        else
        {
            myRct.localScale = new Vector2(sclGoalMin, sclGoalMin);
        }
    }
    void SCL()
    {
        switch(num)
        {
            case numUI.sclUP: // �X�P�[���A�b�v
                if (sclX <= sclGoalMax || sclY <= sclGoalMax)
                {
                    sclX += sclSpd;
                    sclY += sclSpd;
                }
                else
                {
                    sclX = sclGoalMax;
                    sclY = sclGoalMax;

                    num = numUI.sclDown;
                }
                break;
            case numUI.sclDown: // �X�P�[���_�E��
                if (sclX >= sclGoalMin || sclY >= sclGoalMin)
                {
                    sclX -= sclSpd;
                    sclY -= sclSpd;
                }
                else
                {
                    sclX = sclGoalMin;
                    sclY = sclGoalMin;

                    num = numUI.sclUP;
                }
                break;
        }
    }
    #region Method_IPointer
    // UI��ɃJ�[�\�����G��Ă��邩
    public void OnPointerEnter(PointerEventData eventData)
    {
        onUI = true;
    }
    // ���ꂽ�ꍇ
    public void OnPointerExit(PointerEventData eventData)
    {
        onUI = false;
    }
    #endregion

    /// <summary>
    /// �O�����狭���I�ɓ������I������
    /// </summary>
    public void ResetButton()
    {
        onUI = false;
    }
}
