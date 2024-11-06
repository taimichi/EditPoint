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

    //[SerializeField] Text[] UptextBox;
    //[SerializeField] Text[] CentertextBox;
    //[SerializeField] Text[] DowntextBox;
  
    // �������ڂ��J�E���g
     int texMaxNum = 0;

    // �e�L�X�g��\������ꏊ
    [SerializeField] Text[] nameText;
    [SerializeField] Text[] talkText;

    // ���r�U��(�ǂ݉���)�Ɏg��
    [SerializeField] Text SubText;

    [SerializeField] GameObject CanvasFolder;

    // �N���b�p�[�֘A
    [SerializeField] ClapperStart clapper;
    [SerializeField] GameObject clpObj;

    [SerializeField] GameObject SelectButtonBox;

    // ��������L������ۑ�����
    GameObject gObj;

    // ��������T�u�e�L�X�g�{�b�N�X
    Text textObj;

    List<Text> textList = new List<Text>();

    // ��b�ԍ�
    int num=0;

    // �ǂ݉���������^�C�~���O
    bool Isreading = false;

    string markerstart = "{";
    string markerend = "}";
    string marker = "#";

    enum Height
    {
        Up=30,
        Down=-60,
    };
    Height height=Height.Up;

    // �I����
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
                ClearManager(true, true, true);

                // �g���e�L�X�g�{�b�N�X��ύX����Ȃ炱��ō��܂�
                // �g���Ă����̂��\����
                ActiveFalse();

                // �ǂ̒i���g�����w��
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
                resTalk = RemoveTextBetweenMarkers(resTalk, markerstart, markerend);
                if (Input.GetMouseButtonDown(0))
                {
                    if (talkText[(int)sBox].text == resTalk)
                    {
                        Snum++;
                        resTalk = "";
                        num++;
                        // �ꕶ��������R���[�`���̒�~
                        StopAllCoroutines();
                    }

                    
                    // �S���\��
                    //talkText[(int)sBox].text = resTalk;
                                       
                }
                break;
            case 2: // ��b�ԍ�2�̃Z�b�g
                // ������
                ClearManager(true, true, false);

                // �g���e�L�X�g�{�b�N�X��ύX����Ȃ炱��ō��܂�
                // �g���Ă����̂��\����
                ActiveFalse();

                // �ǂ̒i���g�����w��
                sBox = SelectBox.Down;

                // ��b�Ɩ��O�̃Z�b�g
                DataSet(num, CharaName.AD);

                // �ǂ̒i�̃e�L�X�g�{�b�N�X���g����
                UseTalkBox(sBox);
                // �L�����̕\��
                //gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);

                Snum++;
                break;
            case 3:// �I����
                resTalk = RemoveTextBetweenMarkers(resTalk, markerstart, markerend);
                if (talkText[(int)sBox].text == resTalk)
                {
                    Debug.Log("������");
                    SelectButtonBox.SetActive(true);
                    // �ꕶ��������R���[�`���̒�~
                    StopAllCoroutines();

                }
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log(resTalk);
                   
                    // �S���\��
                    //talkText[(int)sBox].text = resTalk;

                    //resTalk = "";
                   
                }
                break;
            case 4: // Yes���[�g ��b�ԍ�3�̃Z�b�g
                    // ������
                ClearManager(true, true, false);
                // �g���e�L�X�g�{�b�N�X��ύX����Ȃ炱��ō��܂�
                // �g���Ă����̂��\����
                ActiveFalse();
                sBox = SelectBox.Down;

                // ��b�Ɩ��O�̃Z�b�g
                DataSet(num, CharaName.AD);

                // �ǂ̒i�̃e�L�X�g�{�b�N�X���g����
                UseTalkBox(sBox);

                // �L�����̕\��
                //gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);

                Snum=6;
                break;
            case 5:// No���[�g
                   // ������
                ClearManager(true, true, false);
                // �g���e�L�X�g�{�b�N�X��ύX����Ȃ炱��ō��܂�
                // �g���Ă����̂��\����
                ActiveFalse();
                sBox = SelectBox.Down;

                // ��b�Ɩ��O�̃Z�b�g
                DataSet(6, CharaName.AD);

                // �ǂ̒i�̃e�L�X�g�{�b�N�X���g����
                UseTalkBox(sBox);

                // �L�����̕\��
                //gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);

                Snum = 13;
                break;
            case 6:
                resTalk = RemoveTextBetweenMarkers(resTalk, markerstart, markerend);

                if (Input.GetMouseButtonDown(0))
                {
                    if (talkText[(int)sBox].text == resTalk)
                    {
                        Snum++;
                        resTalk = "";
                        num++;
                        // �ꕶ��������R���[�`���̒�~
                        StopAllCoroutines();
                    }
                   
                    // �S���\��
                   // talkText[(int)sBox].text = resTalk;
                }
                break;
            case 7:// ��b�ԍ�4�̃Z�b�g
                // ������
                ClearManager(true, true, false);
                // �g���e�L�X�g�{�b�N�X��ύX����Ȃ炱��ō��܂�
                // �g���Ă����̂��\����
                ActiveFalse();
                sBox = SelectBox.Down;

                // ��b�Ɩ��O�̃Z�b�g
                DataSet(num, CharaName.AD);

                // �ǂ̒i�̃e�L�X�g�{�b�N�X���g����
                UseTalkBox(sBox);

                // �L�����̕\��
                //gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);
                Snum ++;
                break;
            case 8:
                resTalk = RemoveTextBetweenMarkers(resTalk, markerstart, markerend);

                if (Input.GetMouseButtonDown(0))
                {
                    if (talkText[(int)sBox].text == resTalk)
                    {
                        Snum++;
                        resTalk = "";
                        num++;
                        // �ꕶ��������R���[�`���̒�~
                        StopAllCoroutines();
                    }
                   
                    // �S���\��
                    //talkText[(int)sBox].text = resTalk;
                }
                break;
            case 9:// ��b�ԍ�5�̃Z�b�g
                // ������
                ClearManager(true, true, false);
                // �g���e�L�X�g�{�b�N�X��ύX����Ȃ炱��ō��܂�
                // �g���Ă����̂��\����
                ActiveFalse();
                sBox = SelectBox.Down;

                // ��b�Ɩ��O�̃Z�b�g
                DataSet(num, CharaName.AD);

                // �ǂ̒i�̃e�L�X�g�{�b�N�X���g����
                UseTalkBox(sBox);

                // �L�����̕\��
                //gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);
                Snum++;
                break;
            case 10:
                resTalk = RemoveTextBetweenMarkers(resTalk, markerstart, markerend);

                if (Input.GetMouseButtonDown(0))
                {
                    if (talkText[(int)sBox].text == resTalk)
                    {
                        Snum++;
                        resTalk = "";
                        num++;
                        // �ꕶ��������R���[�`���̒�~
                        StopAllCoroutines();
                    }
                    
                    // �S���\��
                  //  talkText[(int)sBox].text = resTalk;
                }
                break;
            case 11: // ��b�ԍ�6�̃Z�b�g
                // ������
                ClearManager(true, true, false);
                // �g���e�L�X�g�{�b�N�X��ύX����Ȃ炱��ō��܂�
                // �g���Ă����̂��\����
                ActiveFalse();
                sBox = SelectBox.Down;

                // ��b�Ɩ��O�̃Z�b�g
                DataSet(num, CharaName.AD);

                // �ǂ̒i�̃e�L�X�g�{�b�N�X���g����
                UseTalkBox(sBox);

                // �L�����̕\��
                //gObj = Instantiate(charaObj[(int)CharaName.AD], charaPos[(int)SelectPos.Cenetr].position, Quaternion.identity);
                Snum=999;
                break;

                //���������[�g
            case 13:
                resTalk = RemoveTextBetweenMarkers(resTalk, markerstart, markerend);

                if (Input.GetMouseButtonDown(0))
                {
                    if (talkText[(int)sBox].text == resTalk)
                    {
                        Snum = 7;
                        resTalk = "";
                        num = 3;
                        // �ꕶ��������R���[�`���̒�~
                        StopAllCoroutines();
                    }
                    
                    // �S���\��
                    //talkText[(int)sBox].text = resTalk;
                }
                break;

            case 999:
                resTalk = RemoveTextBetweenMarkers(resTalk, markerstart, markerend);

                if (Input.GetMouseButtonDown(0))
                {
                    if (talkText[(int)sBox].text == resTalk)
                    {
                        // �ꕶ��������R���[�`���̒�~
                        StopAllCoroutines();
                        // �S���\��
                        //talkText[(int)sBox].text = resTalk;
                        Snum++;
                        resTalk = "";
                    }
                }
                break;
            case 1000: // �I��
               
                    StopAllCoroutines();
                    ActiveFalse();
                    Snum=0;
                    num=0;
                    resTalk = "";
                    ClearManager(true, true, true);
                    CanvasFolder.SetActive(false);
                    clpObj.SetActive(true);
                    clapper.SceneName = "Select";
                
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
        num = 1;
        Snum+=2;
    }

    IEnumerator RevealText(SelectBox selectBox)
    {
        // �ꕶ�����\��
        foreach (char c in resTalk)
        {
            // �e�L�X�g�{�b�N�X���ꕶ��������V�X�e��
            #region IF
            //switch ((int)selectBox)
            //{
            //    case 0:
            //        UptextBox[texMaxNum].text = c.ToString();
            //        texMaxNum++;
            //        break;
            //    case 1:
            //        CentertextBox[texMaxNum].text = c.ToString();
            //        texMaxNum++;
            //        break;
            //    case 2:
            //        DowntextBox[texMaxNum].text = c.ToString();
            //        texMaxNum++;
            //        break;
            //}
            #endregion

            // ���r�U��{�ǂ݉����j��t����
            if (c == '{')
            {
                Isreading = true;
                
                if(Isreading)
                {
                    // ��������
                    textObj=Instantiate(SubText);
                    // �q�I�u�W�F�N�g�ɓ����
                    textObj.transform.parent = useTextBox[(int)selectBox].transform;
                    // �����I�u�W�F�N�g�̍��W�����w��
                    textObj.rectTransform.anchoredPosition = new Vector3(-835+(texMaxNum*50),(float)height,0);
                    textObj.rectTransform.localScale = new Vector3(1,1,1);

                    // ���X�g�ɓ��ꍞ��
                    textList.Add(textObj);
                }
                yield return new WaitForSecondsRealtime(delay); // �w�莞�ԑҋ@
            }
            else if(c=='}')
            {
                Isreading = false;
            }
            else if (c == '#')
            {
                texMaxNum = 0;
                height = Height.Down;
                resTalk=resTalk.Replace(marker, "");
                yield return new WaitForSecondsRealtime(delay); // �w�莞�ԑҋ@
            }

            else if (Isreading)
            {
                textObj.text += c;
            }
            else
            {
                talkText[(int)selectBox].text += c;
                texMaxNum++;
            }
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
        texMaxNum = 0;
        height = Height.Up;
        for (int i=0;i<useTextBox.Length;i++)
        {
            // �����\����ԂȂ��\����
            if (useTextBox[i].activeSelf)
            {
                useTextBox[i].SetActive(false);
            }
        }

        DestroyAllGameObjects();
    }

    void DestroyAllGameObjects()
    {
        // �t���Ń��X�g�����[�v����i�C���f�b�N�X�̃Y����h�����߁j
        for (int i = textList.Count - 1; i >= 0; i--)
        {
            Destroy(textList[i].gameObject); // GameObject��Destroy
            textList.RemoveAt(i); // ���X�g������폜
        }
    }

    string RemoveTextBetweenMarkers(string text, string start, string end)
    {
        int startIndex = text.IndexOf(start);
        int endIndex = text.IndexOf(end);

        // �J�n�E�I���̃C���f�b�N�X��������Ȃ��ꍇ
        if (startIndex == -1 || endIndex == -1 || startIndex >= endIndex)
        {
            return text; // ���̕���������̂܂ܕԂ�
        }
        
        // �J�n�����ƏI���������܂߂č폜
        return text.Remove(startIndex, (endIndex - startIndex) + end.Length);
    }
}
