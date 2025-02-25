using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool isDragging = false;
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private Canvas canvas; // UI�̍��W�n���擾���邽�߂�Canvas

    public RectTransform targetImage; // �ڕW�n�_��Image��RectTransform
    public float snapDistance = 50f; // �X�i�b�v���鋗����臒l

    // �����ɕ\������\��̃L�����o�X�I�u�W�F�N�g��n��
    [SerializeField] LoadingProgressBar load;
    // ���[�h��\������
    [SerializeField] GameObject LoadObj;
    // �\������X�e�[�W�p�l��
    [SerializeField] GameObject StagePanel;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        // �e��Canvas���擾
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas��������܂���I���̃X�N���v�g��Canvas����UI�I�u�W�F�N�g�ł̂݋@�\���܂��B");
        }
        Vector2 checkPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);  // �`�F�b�N�������ʒu
        CheckUIObjectAtPosition(checkPosition, canvas);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && canvas != null)
        {
            // UI�̍��W�ϊ����l�����Ĉʒu���X�V
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );

            rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // �ڕW��Image�Ƃ̋������v�Z
        float distance = Vector2.Distance(rectTransform.anchoredPosition, targetImage.anchoredPosition);

        // �������l���Ȃ�ڕWImage�̈ʒu�ɃX�i�b�v
        if (distance < snapDistance)
        {
            rectTransform.anchoredPosition = targetImage.anchoredPosition;
            LoadObj.SetActive(true);
            load.SetObj = StagePanel;
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition; // ���̈ʒu�ɖ߂�
        }
    }

    // �w�肳�ꂽ���W�������RectTransform���ɂ��邩�`�F�b�N
    public static void CheckUIObjectAtPosition(Vector2 position, Canvas canvas, float tolerance = 0.1f)
    {
        // Canvas���̂��ׂĂ�UI�I�u�W�F�N�g���擾
        RectTransform[] rectTransforms = canvas.GetComponentsInChildren<RectTransform>();

        foreach (var rectTransform in rectTransforms)
        {
            // RectTransform���X�N���[�����W�n�Ɋ�Â��Ă��邩���m�F
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                position,
                canvas.worldCamera,
                out localPosition
            );

            // ����UI�I�u�W�F�N�g��Rect���Ɏw��ʒu���܂܂�Ă��邩�`�F�b�N
            if (rectTransform.rect.Contains(localPosition))
            {
                // �ʒu���܂܂�Ă���΃I�u�W�F�N�g�����o��
                Debug.Log("�w��ʒu�ɃI�u�W�F�N�g������܂�: " + rectTransform.gameObject.name);
            }
        }
    }

    /// <summary>
    /// ���̈ʒu�ɖ߂�
    /// </summary>
    public void OriginalPos()
    {
        rectTransform.anchoredPosition = originalPosition; // ���̈ʒu�ɖ߂�
    }
}
