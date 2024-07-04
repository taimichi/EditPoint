using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerController : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private CutAndPaste cp;

    [SerializeField] private Text LayerNo;
    //���ݑI�����Ă��郌�C���[�ԍ�
    //layerNum : 0 = 7.�S��, 1 = 8.layer1, 2 = 9.layer2, 3 = 10.layer3
    private int i_layerNum = 0;
    //�Ō�ɑI���������C���[�ԍ� 1�`3
    private int lastLayerNum = 1;
    [SerializeField, Header("���C���[�̍ő�l")] private int i_maxLayerNum;
    [SerializeField, Header("���C���[�̍ŏ��l")] private int i_minLayerNum;
    private bool b_changeLayer = false;

    //order in layer�̒l
    private int i_orderInLayer1Num = 5;
    private int i_orderInLayer2Num = 6;
    private int i_orderInLayer3Num = 7;

    private GameObject[] GroundLayer;
    private GameObject[] Layer1AllObj;
    private GameObject[] Layer2AllObj;
    private GameObject[] Layer3AllObj;

    [SerializeField] private GameObject LayerPanel;

    [SerializeField, Header("���C���[�\�����p�@���C���[1��")] private GameObject Layer1Rep;
    [SerializeField, Header("���C���[�\�����p�@���C���[2��")] private GameObject Layer2Rep;
    [SerializeField, Header("���C���[�\�����p�@���C���[3��")] private GameObject Layer3Rep;

    void Start()
    {
        LayerPanel.SetActive(false);
        GroundLayer = GetLayerAllObj(LayerMask.NameToLayer("Ground"));
        Layer1AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer1"));
        Layer2AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer2"));
        Layer3AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer3"));

        ChangeLayer();
    }

    void Update()
    {
        //�ҏW���[�h��ON�̎�
        if (gm.ReturnEditMode() == true)
        {
            if (!cp.ReturnSetOnOff())
            {
                Wheel();
                if (b_changeLayer)
                {
                    if (!LayerPanel.activeSelf)
                    {
                        LayerPanel.SetActive(true);
                    }
                }
                else
                {
                    if (LayerPanel.activeSelf)
                    {
                        LayerPanel.SetActive(false);
                    }
                }
                LayerReplacement();
                ChangeLayer();

            }
            else
            {

            }
        }
        LayerNo.text = i_layerNum.ToString();
    }

    //�}�E�X�z�C�[��
    private void Wheel()
    {
        if (Input.mouseScrollDelta.y > 0 && Input.GetKey(KeyCode.R))
        {
            i_layerNum++;
            if (i_layerNum >= i_maxLayerNum)
            {
                i_layerNum = i_maxLayerNum;
            }
            b_changeLayer = true;
        }
        else if (Input.mouseScrollDelta.y < 0 && Input.GetKey(KeyCode.R))
        {
            i_layerNum--;
            if (i_layerNum <= i_minLayerNum)
            {
                i_layerNum = i_minLayerNum;
            }
            b_changeLayer = true;
        }        
    }

    //���C���[�ύX��
    private void ChangeLayer()
    {
        GroundLayerChange();
        Layer1ColorChange();
        Layer2ColorChange();
        Layer3ColorChange();
        if (i_layerNum == 0)
        {
            LayerPanel.SetActive(false);
        }

        if (i_layerNum != 0)
        {
            lastLayerNum = i_layerNum;
        }
    }

    //�S�ẴI�u�W�F�N�g�̃��C���[���擾
    private GameObject[] GetLayerAllObj(LayerMask layerMask)
    {
        GameObject[] goArray = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> goList = new List<GameObject>();
        
        for(int i = 0; i < goArray.Length; i++)
        {
            if(goArray[i].layer == layerMask)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }

    //���̑��̃��C���[�I�u�W�F�N�g�̐F�����ύX
    private void GroundLayerChange()
    {
        if (i_layerNum == 0)
        {
            for(int i = 0; i < GroundLayer.Length; i++)
            {
                GroundLayer[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
        }
        else
        {
            for(int i = 0; i < GroundLayer.Length; i++)
            {
                GroundLayer[i].GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
    }

    //���C���[1�I�u�W�F�N�g�̐F�����ύX
    private void Layer1ColorChange()
    {
        //�I�u�W�F�N�g���z��ɑ��݂���Ƃ��̂�
        if (Layer1AllObj != null)
        {
            //���C���[���S�̂�1��I�����Ă���Ƃ�
            if (i_layerNum == 0)
            {
                for(int i = 0; i < Layer1AllObj.Length; i++)
                {
                    //
                    if(Layer1AllObj[i] != null)
                    {
                        Layer1AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = i_orderInLayer1Num;
                    }
                }
            }
            else if (i_layerNum == 1)
            {
                for (int i = 0; i < Layer1AllObj.Length; i++)
                {
                    if(Layer1AllObj[i] != null)
                    {
                        Layer1AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
                    }
                }
            }
            //����ȊO�̎�
            else
            {
                for (int i = 0; i < Layer1AllObj.Length; i++)
                {
                    if (Layer1AllObj[i] != null)
                    {
                        Layer1AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
                    }
                }
            }
        }
    }

    //���C���[�Q�I�u�W�F�N�g�̐F�����ύX
    private void Layer2ColorChange()
    {
        //�I�u�W�F�N�g���z��ɑ��݂���Ƃ��̂�
        if (Layer2AllObj != null)
        {
            //���C���[���S�̂�2��I�����Ă���Ƃ�
            if(i_layerNum == 0)
            {
                for (int i = 0; i < Layer2AllObj.Length; i++)
                {
                    Layer2AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = i_orderInLayer2Num;
                }
            }
            else if (i_layerNum == 2)
            {
                for (int i = 0; i < Layer2AllObj.Length; i++)
                {
                    Layer2AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
                }
            }
            //����ȊO�̎�
            else
            {
                for (int i = 0; i < Layer2AllObj.Length; i++)
                {
                    Layer2AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
            }
        }
    }

    //���C���[�R�I�u�W�F�N�g�̐F�����ύX
    private void Layer3ColorChange()
    {
        //�I�u�W�F�N�g���z��ɑ��݂���Ƃ��̂�
        if (Layer3AllObj != null)
        {
            //���C���[���S�̂�3��I�����Ă���Ƃ�
            if (i_layerNum == 0)
            {
                for (int i = 0; i < Layer3AllObj.Length; i++)
                {
                    Layer3AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = i_orderInLayer3Num;
                }

            }
            else if (i_layerNum == 3)
            {
                for (int i = 0; i < Layer3AllObj.Length; i++)
                {
                    Layer3AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
                }
            }
            //����ȊO�̎�
            else
            {
                for (int i = 0; i < Layer3AllObj.Length; i++)
                {
                    Layer3AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
            }
        }
    }

    //���C���[�̕\������ς�����
    private void LayerReplacement()
    {
        //���C���[1
        switch (Layer1Rep.transform.GetSiblingIndex() - 2)
        {
            case 0:
                i_orderInLayer1Num = 7;
                break;

            case 1:
                i_orderInLayer1Num = 6;
                break;

            case 2:
                i_orderInLayer1Num = 5;
                break;
        }

        //���C���[2
        switch (Layer2Rep.transform.GetSiblingIndex() - 2)
        {
            case 0:
                i_orderInLayer2Num = 7;
                break;

            case 1:
                i_orderInLayer2Num = 6;
                break;

            case 2:
                i_orderInLayer2Num = 5;
                break;
        }

        //���C���[3
        switch (Layer3Rep.transform.GetSiblingIndex() - 2)
        {
            case 0:
                i_orderInLayer3Num = 7;
                break;

            case 1:
                i_orderInLayer3Num = 6;
                break;

            case 2:
                i_orderInLayer3Num = 5;
                break;
        }
    }


    //���݂̃��C���[�ԍ���Ԃ�
    public int ReturnLayreNum()
    {
        return i_layerNum;
    }
    //�Ō�ɑI���������C���[�ԍ���Ԃ�
    public int ReturnLastLayerNum()
    {
        return lastLayerNum;
    }

    //�O������̃��C���[�ύX
    public void OutChangeLayerNum(int outNum)
    {
        b_changeLayer = true;
        i_layerNum = outNum;
        if (outNum != 0)
        {
            LayerPanel.SetActive(true);
        }
        ChangeLayer();
    }

    //�y�[�X�g���Ƀ��C���[���Ƃɕ������I�u�W�F�N�g�z����㏑��
    public void ChangeObjectList()
    {
        Layer1AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer1"));
        Layer2AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer2"));
        Layer3AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer3"));
    }

    //�e���C���[�̕\����\��
    //���C���[�P
    public void Layer1Active()
    {
        if(Layer1AllObj != null)
        {
            for(int i = 0; i < Layer1AllObj.Length; i++)
            {
                Layer1AllObj[i].SetActive(Layer1AllObj[i].activeSelf == false ? true : false);
            }
        }
    }
    //���C���[�Q
    public void Layer2Active()
    {
        if (Layer2AllObj != null)
        {
            for(int i = 0; i < Layer2AllObj.Length; i++)
            {
                Layer2AllObj[i].SetActive(Layer2AllObj[i].activeSelf == false ? true : false);
            }
        }
    }
    //���C���[�R
    public void Layer3Active()
    {
        if (Layer3AllObj != null)
        {
            for(int i = 0; i < Layer3AllObj.Length; i++)
            {
                Layer3AllObj[i].SetActive(Layer3AllObj[i].activeSelf == false ? true : false);
            }
        }
    }

    //�y�[�X�g���ɃI�u�W�F�N�g�̃��C���[�ɕύX
    public void PasteChangeLayer(LayerMask layerMask)
    {
        LayerPanel.SetActive(true);
        switch (layerMask)
        {
            case 8:
                i_layerNum = 1;
                ChangeLayer();
                break;

            case 9:
                i_layerNum = 2;
                ChangeLayer();
                break;

            case 10:
                i_layerNum = 3;
                ChangeLayer();
                break;
        }
    }

    

}
