using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{

    [SerializeField] private ScrollRect scrollRect; 
    [SerializeField] private RectTransform content; 
    [SerializeField] private RectTransform target;
    [SerializeField] private float scrollAmount_auto = 0.022789f;    //�ړ���(0�`1)
    private float scrollAmount_manual = 0.005f;

    [SerializeField] private RectTransform viewport;

    private void Start()
    {
        scrollAmount_auto = Screen.width / content.rect.width;
    }

    void Update()
    {

        // �^�[�Q�b�g��UI�I�u�W�F�N�g�̍��W���擾
        Vector3[] targetCorners = new Vector3[4];
        target.GetWorldCorners(targetCorners);

        // ScrollView��Viewport�̍��W���擾
        Vector3[] viewportCorners = new Vector3[4];
        scrollRect.viewport.GetWorldCorners(viewportCorners);

        if (!TimeData.TimeEntity.b_DragMode)
        {
            // �E�[�̔���
            if (targetCorners[2].x > viewportCorners[2].x)
            {
                ScrollToPositionRight(scrollAmount_auto); // �E�ɃX�N���[��
            }
        }
        else
        {
            // �E�[�̔���
            if (targetCorners[2].x > viewportCorners[2].x)
            {
                ScrollToPositionRight(scrollAmount_manual); // �E�ɃX�N���[��
            }
            else if (targetCorners[1].x < viewportCorners[1].x)
            {
                ScrollToPositionLeft(scrollAmount_manual); //���ɃX�N���[��
            }

        }

    }

    // �E�ɃX�N���[��
    private void ScrollToPositionRight(float normalizedPosition)
    {
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + normalizedPosition);
    }

    //���ɃX�N���[��
    private void ScrollToPositionLeft(float normalizedPosition)
    {
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition - normalizedPosition);
    }

}
