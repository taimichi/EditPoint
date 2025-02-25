using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Slider�ɃA�N�Z�X���邽�߂ɕK�v

public class LoadingProgressBar : MonoBehaviour
{
    public Slider progressBar; // �i�s�󋵃o�[

    public float loadTime = 5f; // ���[�h�����܂ł̎��ԁi�b�j


    private GameObject SetCanvasObj;

    private void OnEnable()
    {
        // ������Ԃł͐i�s�󋵂�0�ɐݒ�
        progressBar.value = 0f;

        // �X���C�_�[�𑀍�s�ɂ��ăn���h�����\���ɂ���
        SetSliderInactive();

        // �񓯊��ɐi�s�󋵂��X�V����R���[�`�����J�n
        StartCoroutine(LoadProcess());
    }

    private void SetSliderInactive()
    {
        // �X���C�_�[�𖳌���
        progressBar.interactable = false;

        //// �X���C�_�[���C���^���N�V��������Ȃ��悤�ɂ���
        //GraphicRaycaster raycaster = progressBar.GetComponentInParent<GraphicRaycaster>();
        //if (raycaster != null)
        //{
        //    raycaster.enabled = false;
        //}
    }

    private IEnumerator LoadProcess()
    {
        float timeElapsed = 0f;

        while (timeElapsed < loadTime)
        {
            // �o�ߎ��Ԃ��v�Z���A�i�s�󋵂��X�V
            timeElapsed += Time.deltaTime;
            progressBar.value = timeElapsed / loadTime; // �i�s�󋵁i0-1�͈̔́j

            yield return null; // ���̃t���[���܂őҋ@
        }

        // ���[�h������ɐi�s�󋵃o�[��100%�ɐݒ�
        progressBar.value = 1f;

        if (SetCanvasObj == null) { Debug.Log("�Z�b�g����ĂȂ���"); }
        SetCanvasObj.SetActive(true);
        this.gameObject.SetActive(false);
        Debug.Log("���[�h�����I");

        yield break;
    }

    /// <summary>
    /// �\������UI
    /// </summary>
    public GameObject SetObj
    {
        set { SetCanvasObj = value; }
    }
}
