using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerChange : MonoBehaviour
{
    [SerializeField] private Text LayerNo;
    //layerNum : 0 = 7.�S��, 1 = 8.layer1, 2 = 9.layer2, 3 = 10.layer3
    private int layerNum = 0;
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
        //GroundLayerChange();
        Layer1ColorChange();
        Layer2ColorChange();
        Layer3ColorChange();
        if (layerNum == 0)
        {
            LayerPanel.SetActive(false);
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

    //�n�ʃ��C���[�I�u�W�F�N�g�̐F�����ύX
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
        //���C���[���S�̂�1��I�����Ă���Ƃ�
        if (layerNum == 1 || layerNum == 0)
        {
            for(int i = 0; i < Layer1AllObj.Length; i++)
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

    //���C���[�Q�I�u�W�F�N�g�̐F�����ύX
    private void Layer2ColorChange()
    {
        //���C���[���S�̂�2��I�����Ă���Ƃ�
        if (layerNum == 2 || layerNum == 0)
        {
            for(int i = 0; i < Layer2AllObj.Length; i++)
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

    //���C���[�R�I�u�W�F�N�g�̐F�����ύX
    private void Layer3ColorChange()
    {
        //���C���[���S�̂�3��I�����Ă���Ƃ�
        if (layerNum == 3 || layerNum == 0)
        {
            for(int i = 0; i < Layer3AllObj.Length; i++)
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

    public int ReturnLayreNum()
    {
        return layerNum;
    }

    //�O������̃��C���[�ύX
    public void OutChangeLayerNum(int outNum)
    {
        LayerPanel.SetActive(true);
        layerNum = outNum;
    }
}
