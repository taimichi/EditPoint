using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerChange : MonoBehaviour
{
    [SerializeField] private Text LayerNo;
    //layerNum : 1 = 8.layer1, 2 = 9.layer2, 3 = 10.layer3
    private int layerNum = 1;
    [SerializeField, Header("���C���[�̍ő�l")] private int maxLayerNum;
    [SerializeField, Header("���C���[�̍ŏ��l")] private int minLayerNum;

    private GameObject[] Layer1AllObj;
    private GameObject[] Layer2AllObj;
    private GameObject[] Layer3AllObj;

    // Start is called before the first frame update
    void Start()
    {
        Layer1AllObj = GetLayerAllObj(8);
        Layer2AllObj = GetLayerAllObj(9);
        Layer3AllObj = GetLayerAllObj(10);
    }

    // Update is called once per frame
    void Update()
    {
        Wheel();
    }

    //�}�E�X�z�C�[��
    private void Wheel()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            layerNum++;
            if (layerNum >= maxLayerNum)
            {
                layerNum = maxLayerNum;
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            layerNum--;
            if (layerNum <= minLayerNum)
            {
                layerNum = minLayerNum;
            }
        }

        LayerNo.text = layerNum.ToString();
        
    }

    //���C���[�ύX��
    private void ChangeLayer()
    {
        switch (layerNum)
        {
            default:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    //�S�ẴI�u�W�F�N�g�̃��C���[���擾
    private GameObject[] GetLayerAllObj(LayerMask layerMask)
    {
        GameObject[] goArray = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> goList = new List<GameObject>();
        foreach (GameObject go in goArray)
        {
            // LayerMask bit check
            if (((1 << go.layer) & layerMask.value) != 0)
            {
                goList.Add(go);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }

    //���C���[�̔ԍ���Ԃ�
    public int ReturnLayerNum()
    {
        return layerNum;
    }
}
