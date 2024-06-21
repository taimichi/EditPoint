using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerController : MonoBehaviour
{
    [SerializeField] private CutAndPaste cp;

    [SerializeField] private Text LayerNo;
    //���ݑI�����Ă��郌�C���[�ԍ�
    //layerNum : 0 = 7.�S��, 1 = 8.layer1, 2 = 9.layer2, 3 = 10.layer3
    private int layerNum = 0;
    //�Ō�ɑI���������C���[�ԍ� 1�`3
    private int lastLayerNum = 1;
    [SerializeField, Header("���C���[�̍ő�l")] private int maxLayerNum;
    [SerializeField, Header("���C���[�̍ŏ��l")] private int minLayerNum;
    private bool changeLayer = false;

    private GameObject[] GroundLayer;
    private GameObject[] Layer1AllObj;
    private GameObject[] Layer2AllObj;
    private GameObject[] Layer3AllObj;

    [SerializeField] private GameObject LayerPanel;

    // Start is called before the first frame update
    void Start()
    {
        LayerPanel.SetActive(false);
        GroundLayer = GetLayerAllObj(LayerMask.NameToLayer("Ground"));
        Layer1AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer1"));
        Layer2AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer2"));
        Layer3AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer3"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!cp.ReturnSetOnOff())
        {
            Wheel();
            if (changeLayer)
            {
                if (!LayerPanel.activeSelf)
                {
                    LayerPanel.SetActive(true);
                }
                ChangeLayer();
            }
            else
            {
                if (LayerPanel.activeSelf)
                {
                    LayerPanel.SetActive(false);
                }
            }
        }
        else
        {

        }
    }

    //�}�E�X�z�C�[��
    private void Wheel()
    {
        if (Input.mouseScrollDelta.y > 0 && Input.GetKey(KeyCode.R))
        {
            layerNum++;
            if (layerNum >= maxLayerNum)
            {
                layerNum = maxLayerNum;
            }
            changeLayer = true;
        }
        else if (Input.mouseScrollDelta.y < 0 && Input.GetKey(KeyCode.R))
        {
            layerNum--;
            if (layerNum <= minLayerNum)
            {
                layerNum = minLayerNum;
            }
            changeLayer = true;
        }

        LayerNo.text = layerNum.ToString();
        
    }

    //���C���[�ύX��
    private void ChangeLayer()
    {
        GroundLayerChange();
        Layer1ColorChange();
        Layer2ColorChange();
        Layer3ColorChange();
        if (layerNum == 0)
        {
            LayerPanel.SetActive(false);
        }

        if (layerNum != 0)
        {
            lastLayerNum = layerNum;
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
        if (layerNum == 0)
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
            if (layerNum == 1 || layerNum == 0)
            {
                for (int i = 0; i < Layer1AllObj.Length; i++)
                {
                    Layer1AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
                }
            }
            //����ȊO�̎�
            else
            {
                for (int i = 0; i < Layer1AllObj.Length; i++)
                {
                    Layer1AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
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
            if (layerNum == 2 || layerNum == 0)
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
            if (layerNum == 3 || layerNum == 0)
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

    //���݂̃��C���[�ԍ���Ԃ�
    public int ReturnLayreNum()
    {
        return layerNum;
    }
    //�Ō�ɑI���������C���[�ԍ���Ԃ�
    public int ReturnLastLayerNum()
    {
        return lastLayerNum;
    }

    //�O������̃��C���[�ύX
    public void OutChangeLayerNum(int outNum)
    {
        changeLayer = true;
        layerNum = outNum;
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
}
