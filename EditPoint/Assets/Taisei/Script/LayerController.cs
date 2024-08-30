using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Live2D.Cubism.Rendering;

public class LayerController : MonoBehaviour
{
    #region Field
    [SerializeField] private CopyAndPaste cp;

    //���ݑI�����Ă��郌�C���[�ԍ�
    /// <summary>
    /// ���ݑI������Ă��郌�C���[
    /// layerNum : 0 = �S��, 1 = layer1, 2 = layer2, 3 = layer3
    /// </summary>
    private int i_layerNum = 0;
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

    /// <summary>
    /// ���C���[�p�l��
    /// </summary>
    [SerializeField] private GameObject LayerPanel;
    private Image layerImg;

    [SerializeField, Header("���C���[�\�����p�@���C���[1��")] private GameObject Layer1Rep;
    [SerializeField, Header("���C���[�\�����p�@���C���[2��")] private GameObject Layer2Rep;
    [SerializeField, Header("���C���[�\�����p�@���C���[3��")] private GameObject Layer3Rep;
    private int i_layer1RepIndex;
    private int i_layer2RepIndex;
    private int i_layer3RepIndex;

    private GameObject Pl;
    //private SpriteRenderer PlSpriteRender;
    private PlayerLayer plLayer;
    private int i_plLayerNum;

    private CubismRenderController live2DRender;

    private Color layerColor;
    private float f_layerColorAlfa;
    #endregion

    void Start()
    {
        Pl = GameObject.Find("Player");
        //PlSpriteRender = Pl.GetComponent<SpriteRenderer>();
        plLayer = Pl.GetComponent<PlayerLayer>();
        i_plLayerNum = plLayer.ReturnPLLayer() - 1;

        live2DRender = GameObject.Find("ADChan").GetComponent<CubismRenderController>();

        layerImg = LayerPanel.GetComponent<Image>();
        f_layerColorAlfa = 90f / 255f;
        LayerPanel.SetActive(false);
        GroundLayer = GetLayerAllObj(LayerMask.NameToLayer("Ground"));
        Layer1AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer1"));
        Layer2AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer2"));
        Layer3AllObj = GetLayerAllObj(LayerMask.NameToLayer("Layer3"));

        i_layer1RepIndex = Layer1Rep.transform.GetSiblingIndex() - 2;
        i_layer2RepIndex = Layer2Rep.transform.GetSiblingIndex() - 2;
        i_layer3RepIndex = Layer3Rep.transform.GetSiblingIndex() - 2;

        ChangeLayer();
        LayerReplacement();
    }

    void Update()
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
    }

    //�}�E�X�z�C�[��
    private void Wheel()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            i_layerNum++;
            if (i_layerNum >= i_maxLayerNum)
            {
                i_layerNum = i_maxLayerNum;
            }
            b_changeLayer = true;
        }
        else if (Input.mouseScrollDelta.y < 0)
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
        PlayerLayerChange();
        GroundLayerChange();
        Layer1ColorChange();
        Layer2ColorChange();
        Layer3ColorChange();

        switch (i_layerNum)
        {
            case 0:
                LayerPanel.SetActive(false);
                break;

            case 1:
                layerColor = Color.red;
                break;

            case 2:
                layerColor = Color.blue;
                break;

            case 3:
                layerColor = Color.green;
                break;
        }
        layerColor.a = f_layerColorAlfa;
        layerImg.color = layerColor;

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

    //�v���C���[�̃��C���[
    private void PlayerLayerChange()
    {
        switch (i_plLayerNum)
        {
            case 0:
                if (i_layerNum == 0)
                {
                    live2DRender.SortingOrder = i_orderInLayer1Num;
                }
                else if (i_layerNum == 1)
                {
                    live2DRender.SortingOrder = 5;
                }
                else
                {
                    live2DRender.SortingOrder = 3;
                }
                break;

            case 1:
                if (i_layerNum == 0)
                {
                    live2DRender.SortingOrder = i_orderInLayer2Num;
                }
                else if (i_layerNum == 2)
                {
                    live2DRender.SortingOrder = 5;
                }
                else
                {
                    live2DRender.SortingOrder = 3;
                }
                break;

            case 2:
                if (i_layerNum == 0)
                {
                    live2DRender.SortingOrder = i_orderInLayer3Num;
                }
                else if (i_layerNum == 3)
                {
                    live2DRender.SortingOrder = 5;
                }
                else
                {
                    live2DRender.SortingOrder = 3;
                }
                break;
        }

    }

    //���̑��̃��C���[�I�u�W�F�N�g�̐F�����ύX
    private void GroundLayerChange()
    {
        if(GroundLayer != null)
        {
            if (i_layerNum == 0)
            {
                for (int i = 0; i < GroundLayer.Length; i++)
                {
                    GroundLayer[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
                }
            }
            else
            {
                for (int i = 0; i < GroundLayer.Length; i++)
                {
                    GroundLayer[i].GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
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
                    if (Layer2AllObj[i] != null)
                    {
                        Layer2AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = i_orderInLayer2Num;
                    }
                }
            }
            else if (i_layerNum == 2)
            {
                for (int i = 0; i < Layer2AllObj.Length; i++)
                {
                    if (Layer2AllObj[i] != null)
                    {
                        Layer2AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
                    }
                }
            }
            //����ȊO�̎�
            else
            {
                for (int i = 0; i < Layer2AllObj.Length; i++)
                {
                    if (Layer2AllObj[i] != null)
                    {
                        Layer2AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
                    }
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
                    if (Layer3AllObj[i] != null)
                    {
                        Layer3AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = i_orderInLayer3Num;
                    }
                }

            }
            else if (i_layerNum == 3)
            {
                for (int i = 0; i < Layer3AllObj.Length; i++)
                {
                    if (Layer3AllObj[i] != null)
                    {
                        Layer3AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 5;
                    }
                }
            }
            //����ȊO�̎�
            else
            {
                for (int i = 0; i < Layer3AllObj.Length; i++)
                {
                    if (Layer3AllObj[i] != null)
                    {
                        Layer3AllObj[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
                    }
                }
            }
        }
    }

    //���C���[�̕\������ς�����
    private void LayerReplacement()
    {
        i_layer1RepIndex = Layer1Rep.transform.GetSiblingIndex() - 2;
        i_layer2RepIndex = Layer2Rep.transform.GetSiblingIndex() - 2;
        i_layer3RepIndex = Layer3Rep.transform.GetSiblingIndex() - 2;
        //���C���[1
        switch (i_layer1RepIndex)
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
        switch (i_layer2RepIndex)
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
        switch (i_layer3RepIndex)
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

        //���C���[�̕\����(������O���A�E������)
        //123,132,213,231,312,321
        //�v���C���[��
        switch (i_plLayerNum)
        {
            case 0:
                switch (i_layer1RepIndex)
                {
                    case 0:
                        SetLayerCollision("Layer1", "Layer2", "Layer3", false, false, false);
                        break;
                    case 1:
                        if (i_layer2RepIndex == 0)
                        {
                            SetLayerCollision("Layer1", "Layer2", "Layer3", false, true, false);
                        }
                        else if (i_layer3RepIndex == 0)
                        {
                            SetLayerCollision("Layer1", "Layer2", "Layer3", false, false, true);
                        }
                        break;
                    case 2:
                        SetLayerCollision("Layer1", "Layer2", "Layer3", false, true, true);
                        break;
                }
                break;

            case 1:
                switch (i_layer2RepIndex)
                {
                    case 0:
                        SetLayerCollision("Layer1", "Layer2", "Layer3", false, false, false);
                        break;
                    case 1:
                        if (i_layer1RepIndex == 0)
                        {
                            SetLayerCollision("Layer1", "Layer2", "Layer3", true, false, false);
                        }
                        else if (i_layer3RepIndex == 0)
                        {
                            SetLayerCollision("Layer1", "Layer2", "Layer3", false, false, true);
                        }
                        break;
                    case 2:
                        SetLayerCollision("Layer1", "Layer2", "Layer3", true, false, true);
                        break;
                }
                break;

            case 2:
                switch (i_layer3RepIndex)
                {
                    case 0:
                        SetLayerCollision("Layer1", "Layer2", "Layer3", false, false, false);
                        break;
                    case 1:
                        if (i_layer1RepIndex == 0)
                        {
                            SetLayerCollision("Layer1", "Layer2", "Layer3", true, false, false);
                        }
                        else if (i_layer2RepIndex == 0)
                        {
                            SetLayerCollision("Layer1", "Layer2", "Layer3", false, true, false);
                        }
                        break;
                    case 2:
                        SetLayerCollision("Layer1", "Layer2", "Layer3", true, true, false);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// �v���C���[���C���[�Ɗe���C���[���Ƃ̓����蔻���L���ɂ��邩�ǂ���
    /// </summary>
    /// <param name="layer1">string�^"Layer1"�Z�b�g</param>
    /// <param name="layer2">string�^"Layer2"�Z�b�g</param>
    /// <param name="layer3">string�^"Layer3"�Z�b�g</param>
    /// <param name="collision1">�v���C���[�ƃ��C���[1�������邩�ǂ�����true/false�œn��</param>
    /// <param name="collision2">�v���C���[�ƃ��C���[2�������邩�ǂ�����true/false�œn��</param>
    /// <param name="collision3">�v���C���[�ƃ��C���[3�������邩�ǂ�����true/false�œn��</param>
    private void SetLayerCollision(string layer1, string layer2, string layer3, bool collision1, bool collision2, bool collision3)
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(layer1), collision1);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(layer2), collision2);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(layer3), collision3);
    }


    //���݂̃��C���[�ԍ���Ԃ�
    public int ReturnLayreNum()
    {
        return i_layerNum;
    }

    //�O������̃��C���[�ύX
    public void OutChangeLayerNum(int outNum)
    {
        b_changeLayer = true;
        i_layerNum = outNum;
        if (outNum == 0)
        {
            LayerPanel.SetActive(false);
        }
        else
        {
            LayerPanel.SetActive(true);
        }
        ChangeLayer();
    }

    ///<summary>
    ///�y�[�X�g���Ƀ��C���[���Ƃɕ������I�u�W�F�N�g�z����㏑��
    /// </summary>
    /// <remarks>���ӓ_</remarks>
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
