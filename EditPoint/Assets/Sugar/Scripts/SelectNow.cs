using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;    // UI
public class SelectNow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /*----Vector----*/
    Vector3 startRot = new Vector3(0, 0, 0);

    /*----int�ϐ�----*/
    int I_state = 0;  // Switch���Ŏg��

    /*----float----*/
    const float F_rotSpd = 2.0f;
    float F_timer ;
    const float F_settimer = 2.0f;

    /*----string----*/

    /*----bool----*/
    bool B_onUI = false;

    /*----���̑��ϐ��i�R���|�[�l���g�Ƃ��X�N���v�g�j----*/
    [SerializeField]
    RectTransform rct;  // �������Ώۂ�UI

    ClassUIAnim UAnim;  // UI�̃A�j���[�V�����N���X

    [SerializeField]
    GameObject Obj; // �c�[���@�\�̏Љ��I�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        // �C���X�^���X����
        UAnim = new ClassUIAnim();

        // �^�C�}�[�Z�b�g
        F_timer = F_settimer;
    }

    // Update is called once per frame
    void Update()
    { 
        SelectButton();
        UIName();
    }

    void UIName()
    {
        if (!B_onUI) { return; }
        if (Obj.activeSelf == true) { return; }

        F_timer -= Time.deltaTime;
        if(F_timer<=0)
        {
            Obj.SetActive(true);
            F_timer = F_settimer;
        }
    }

    void SelectButton()
    {
        switch (I_state)
        {
            case 0: // �����͏����ɖ߂����
                // �I�����O�ꂽ�ꍇ�ɂ����̏���
                rct.rotation = Quaternion.Euler(0, 0, 0);
                startRot.z = 0;
                break;

            // �P�ƂQ�̏��������݂ɓ��삳����
            case 1: // ����]
                if (startRot.z <= 30)
                {
                    startRot.z += F_rotSpd;
                    rct = UAnim.R_anim_rotation(rct, 0, 0, startRot.z);
                }
                else
                {
                    I_state++;
                }
                break;
            case 2: // �E��]
                if (startRot.z >= -30)
                {
                    startRot.z -= F_rotSpd;
                    rct = UAnim.R_anim_rotation(rct, 0, 0, startRot.z);
                }
                else
                {
                    I_state--;
                }
                break;
        }
    }

    // UI��ɃJ�[�\�����G��Ă��邩
    public void OnPointerEnter(PointerEventData eventData)
    {
        I_state++;
        B_onUI = true;
    }
    // ���ꂽ�ꍇ
    public void OnPointerExit(PointerEventData eventData)
    {
        I_state = 0;
        F_timer = F_settimer;
        if (Obj.activeSelf == true) { Obj.SetActive(false); }
        B_onUI = false;
    }
}
