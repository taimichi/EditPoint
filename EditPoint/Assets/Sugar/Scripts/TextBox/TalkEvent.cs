using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkEvent : MonoBehaviour
{
    #region field
    // �L������\������ꏊ
    [SerializeField] Transform[] charaPos;
    // �o�ꂳ����L����
    [SerializeField] GameObject[] charaObj;

    // �ǂ̃{�b�N�X���g����
    [SerializeField] GameObject[] useTextBox;
    [SerializeField] Text[] nameText;
    [SerializeField] Text[] talkText;

    [SerializeField] GameObject CanvasFolder;


    [SerializeField] GameObject SelectButtonBox;
    // ��������L������ۑ�����
    GameObject gObj;

    // ��b�ԍ�
    int num=0;

    bool select = false;

    // Switch�Ŏg��
    int Snum=0;

    // �󂯎������b�Ɩ��O��ۑ�
    string resName;
    string resTalk;

    // �ꕶ�����Ƃ̕\���ɂ����鎞��
    [SerializeField] float delay = 0.3f; 

    // ��b�f�[�^
    TalkData tData;

    // enum�łǂ̃{�b�N�X���g�����I�ׂ�悤��
    enum SelectBox
    {
        Up,
        Center,
        Down,
    }

    SelectBox sBox;

    // enum�łǂ̃|�W�V�����ɂ��邩�I�ׂ�悤��
    enum SelectPos
    {
        Left,
        Cenetr,
        Right,
    }
    // �����̎��Ɏg��
    enum CharaName
    {
        AD,
    }
    #endregion
    void Start()
    {
        // �C���X�^���X����
        tData = new TalkData();
    }

    public int ParamSnum
    {
        set
        {
            // �ŏ���0
            Snum = value;
        }
    }

    void Update()
    {
        Debug.Log(resTalk);
        switch (Snum)
        {
            case 0: // ��b�ԍ�1�̃Z�b�g
                // ������
                ClearManager(true,true,true);

                sBox = SelectBox.Down;

                // ��b�Ɩ��O�̃Z�b�g
                DataSet(num, CharaName.AD);
                
                // �ǂ̒i�̃e�L�X�g�{�b�N�X���g����
                UseTalkBox(sBox);

                // �L�����̕\��
                gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);
                
                Snum++;
                break;
            case 1: // ��b����
                if(Input.GetMouseButtonDown(0))
                {
                    if (talkText[(int)sBox].text == resTalk)
                    {
                        Snum++;
                        resTalk = "";
                        num++;
                    }

                    // �ꕶ��������R���[�`���̒�~
                    StopAllCoroutines();
                    // �S���\��
                    talkText[(int)sBox].text = resTalk;
                                       
                }
                break;
            case 2: // ��b�ԍ�2�̃Z�b�g
                // ������
                ClearManager(true, true, true);

                // �g���e�L�X�g�{�b�N�X��ύX����Ȃ炱��ō��܂�
                // �g���Ă����̂��\����
                ActiveFalse();

                // �ǂ̒i���g�����w��
                sBox = SelectBox.Up;

                // ��b�Ɩ��O�̃Z�b�g
                DataSet(num, CharaName.AD);

                // �ǂ̒i�̃e�L�X�g�{�b�N�X���g����
                UseTalkBox(sBox);

                // �L�����̕\��
                gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Right].position, Quaternion.identity);

                Snum++;
                break;
            case 3:// �I����
                if (Input.GetMouseButtonDown(0))
                {
                    StopAllCoroutines();
                    // �S���\��
                    talkText[(int)sBox].text = resTalk;

                    resTalk = "";
                    SelectButtonBox.SetActive(true);
                }
                break;
            case 4: // Yes���[�g ��b�ԍ�3�̃Z�b�g
                    // ������
                ClearManager(true, true, true);
                // �g���e�L�X�g�{�b�N�X��ύX����Ȃ炱��ō��܂�
                // �g���Ă����̂��\����
                ActiveFalse();
                sBox = SelectBox.Down;

                // ��b�Ɩ��O�̃Z�b�g
                DataSet(num, CharaName.AD);

                // �ǂ̒i�̃e�L�X�g�{�b�N�X���g����
                UseTalkBox(sBox);

                // �L�����̕\��
                gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);

                Snum=6;
                break;
            case 5:// No���[�g
                Snum=2;
                break;
            case 6:
                if (Input.GetMouseButtonDown(0))
                {
                    if (talkText[(int)sBox].text == resTalk)
                    {
                        Snum++;
                        resTalk = "";
                        num++;
                    }
                    // �ꕶ��������R���[�`���̒�~
                    StopAllCoroutines();
                    // �S���\��
                    talkText[(int)sBox].text = resTalk;
                }
                break;
            case 7:// ��b�ԍ�4�̃Z�b�g
                // ������
                ClearManager(true, true, true);
                // �g���e�L�X�g�{�b�N�X��ύX����Ȃ炱��ō��܂�
                // �g���Ă����̂��\����
                ActiveFalse();
                sBox = SelectBox.Down;

                // ��b�Ɩ��O�̃Z�b�g
                DataSet(num, CharaName.AD);

                // �ǂ̒i�̃e�L�X�g�{�b�N�X���g����
                UseTalkBox(sBox);

                // �L�����̕\��
                gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);
                Snum = 999;
                break;
            case 999:
                if (Input.GetMouseButtonDown(0))
                {
                    // �ꕶ��������R���[�`���̒�~
                    StopAllCoroutines();
                    // �S���\��
                    talkText[(int)sBox].text = resTalk;
                    Snum++;
                    resTalk = "";
                }
                break;
            case 1000: // �I��
                if (Input.GetMouseButtonDown(0))
                {
                    StopAllCoroutines();
                    ActiveFalse();
                    Snum=0;
                    num=0;
                    resTalk = "";
                    ClearManager(true, true, true);
                    CanvasFolder.SetActive(false);
                }
                break;
        }
    }

    public void YesButton()
    {
        select = true;
        SelectButtonBox.SetActive(false);
        num++;
        Snum++;
    }

    public void NoButton()
    {
        select = false;
        SelectButtonBox.SetActive(false);
        num += 2;
        Snum+=2;
    }

    IEnumerator RevealText(SelectBox selectBox)
    {
        // �ꕶ�����\��
        foreach (char c in resTalk)
        {
            talkText[(int)selectBox].text += c;
            yield return new WaitForSecondsRealtime(delay); // �w�莞�ԑҋ@
        }
    }
    /// <summary>
    /// �ǂ̈ʒu�̃e�L�X�g�{�b�N�X���g���̂�
    /// </summary>
    /// <param name="selectBox">Up Center Down����I��</param>
    private void UseTalkBox(SelectBox selectBox)
    {
        // �\�����ĂȂ����݂̂Ɏ��s
        if (!useTextBox[(int)selectBox].activeSelf)
        {
            useTextBox[(int)selectBox].SetActive(true);
        }

        nameText[(int)selectBox].text = resName;

        StartCoroutine(RevealText(selectBox));
    }
    /// <summary>
    /// talk�̃f�[�^���Z�b�g
    /// </summary>
    /// <param name="num">��b�ԍ�</param>
    /// <param name="charaName">���O</param>
    private void DataSet(int num,CharaName charaName)
    {
        // ��b�Z�b�g
        resTalk = tData.SendText(num);
        // ���O�Z�b�g
        resName = tData.SendName((int)charaName);
    }

    /// <summary>
    /// ���̃��b�Z�[�W�����邽��Text�̒��g������
    /// </summary>
    /// <param name="talk">talk�̒��g���폜����Ȃ�true</param>
    /// <param name="name">name�̒��g���폜����Ȃ�true</param>
    /// <param name="chara">chara���폜����Ȃ�true</param>
    private void ClearManager(bool talk,bool name,bool chara)
    {
        // ���̃��b�Z�[�W�����邽��Text�̒��g������
        // talk�̒��g
        if (talk)
        {
            for (int i = 0; i < talkText.Length; i++)
            {
                talkText[i].text = "";
            }
        }
        // name�̒��g
        if (name)
        {
            for (int i = 0; i < nameText.Length; i++)
            {
                nameText[i].text = "";
            }
        }
        // �L��������������
        if(chara)
        {
            Destroy(gObj);
        }
    }

    /// <summary>
    /// �g���e�L�X�g�{�b�N�X��ύX���鎞�Ɍ��݂̃{�b�N�X���\����
    /// </summary>
    private void ActiveFalse()
    {
        for(int i=0;i<useTextBox.Length;i++)
        {
            // �����\����ԂȂ��\����
            if (useTextBox[i].activeSelf)
            {
                useTextBox[i].SetActive(false);
            }
        }
    }
}
