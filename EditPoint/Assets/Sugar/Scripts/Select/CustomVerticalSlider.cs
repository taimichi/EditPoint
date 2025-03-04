using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomVerticalSlider : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform topLimit;  // ����i�X���C�_�[�j
    public RectTransform bottomLimit;  // �����i�X���C�_�[�j
    public RectTransform targetTopLimit;  // �ΏۃI�u�W�F�N�g�̏��
    public RectTransform targetBottomLimit;  // �ΏۃI�u�W�F�N�g�̉���
    public RectTransform targetRect;  // �Ώۂ� RectTransform
    public float scrollSpeed = 10f;  // �}�E�X�z�C�[���̊��x

    // �ړ����x�𒲐����邽�߂̃X�P�[�����O�t�@�N�^�[
    public float speedMultiplier = 2f; // �X���C�_�[�̈ړ����x�𒲐�

    private RectTransform handle; // �n���h���i���g�� RectTransform�j
    private bool isDragging = false;
    private float currentYPosition;  // ���݂̃X���C�_�[�ʒu
    private Vector2 pointerOffset;  // �N���b�N�ʒu�̃I�t�Z�b�g
    private Vector2 initialMousePosition; // �N���b�N�������̃}�E�X�̈ʒu

    void Start()
    {
        handle = GetComponent<RectTransform>();
        currentYPosition = handle.anchoredPosition.y;  // �����ʒu��ݒ�
    }

    // �N���b�N�����ʒu�ɃX���C�_�[���ړ�
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;

        // �}�E�X�ʒu����X���C�_�[�̈ʒu�������ăI�t�Z�b�g���v�Z
        RectTransformUtility.ScreenPointToLocalPointInRectangle(handle.parent as RectTransform, eventData.position, eventData.pressEventCamera, out pointerOffset);
        initialMousePosition = eventData.position;
    }

    // �}�E�X�{�^���𗣂�����h���b�O����
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    // �h���b�O����
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // ���݂̃}�E�X�̈ړ��ʂ��v�Z���āA�ړ����x�𒲐�
        float mouseDeltaY = eventData.position.y - initialMousePosition.y;

        // �X�s�[�h�����F�ړ��ʂ𑬓x�X�P�[���Œ���
        float adjustedDeltaY = mouseDeltaY * speedMultiplier;

        // �V����Y�ʒu���v�Z
        currentYPosition += adjustedDeltaY;
        currentYPosition = Mathf.Clamp(currentYPosition, bottomLimit.anchoredPosition.y, topLimit.anchoredPosition.y);

        // �n���h����V�����ʒu�Ɉړ�
        handle.anchoredPosition = new Vector2(handle.anchoredPosition.x, currentYPosition);

        // �V����Y���W���L�^���Ď��̃h���b�O�ɔ�����
        initialMousePosition = eventData.position;

        // �X���C�_�[�̈ʒu�Ɋ�Â��đΏۂ̈ʒu���X�V
        UpdateTargetRectPosition(currentYPosition);
    }

    void Update()
    {
        // �}�E�X�z�C�[���ŃX���C�_�[�𓮂���
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            currentYPosition += scroll * scrollSpeed * 10f;
            currentYPosition = Mathf.Clamp(currentYPosition, bottomLimit.anchoredPosition.y, topLimit.anchoredPosition.y);
            HandleSliderMovement(currentYPosition);
        }
    }

    // �X���C�_�[�̈ʒu�𓮂���
    private void HandleSliderMovement(float yPosition)
    {
        handle.anchoredPosition = new Vector2(handle.anchoredPosition.x, yPosition);

        // �X���C�_�[�̈ʒu�Ɋ�Â��đΏۂ̈ʒu���X�V
        UpdateTargetRectPosition(yPosition);
    }

    // �Ώۂ� RectTransform ���X���C�_�[�̈ʒu�Ɋ�Â��ē�����
    private void UpdateTargetRectPosition(float clampedY)
    {
        // �X���C�_�[�͈͓̔��ł̊������v�Z
        float normalizedSliderPosition = (clampedY - bottomLimit.anchoredPosition.y) / (topLimit.anchoredPosition.y - bottomLimit.anchoredPosition.y);

        // �Ώۂ� RectTransform �͈̔͂�ݒ�
        float targetMinY = targetBottomLimit.anchoredPosition.y;
        float targetMaxY = targetTopLimit.anchoredPosition.y;

        // �Ώۂ� Y ���W���X���C�_�[�̊����Ɋ�Â��ē�����
        float targetY = Mathf.Lerp(targetMinY, targetMaxY, normalizedSliderPosition);
        targetRect.anchoredPosition = new Vector2(targetRect.anchoredPosition.x, targetY);
    }
}
