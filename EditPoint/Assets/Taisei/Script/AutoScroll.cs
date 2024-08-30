using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect; 
    [SerializeField] private RectTransform content; 
    [SerializeField] private RectTransform target;
    [SerializeField] private float scrollAmount = 0.022789f;    //�ړ���(0�`1)

    void Update()
    {
        // �^�[�Q�b�g��UI�I�u�W�F�N�g�̍��W���擾
        Vector3[] targetCorners = new Vector3[4];
        target.GetWorldCorners(targetCorners);

        // ScrollView��Viewport�̍��W���擾
        Vector3[] viewportCorners = new Vector3[4];
        scrollRect.viewport.GetWorldCorners(viewportCorners);

        // �E�[�̔���
        if (targetCorners[2].x > viewportCorners[2].x)
        {
            ScrollToPosition(scrollAmount); // �E�ɃX�N���[��
        }
    }

    // �X�N���[���ʒu��ݒ肷�郁�\�b�h
    private void ScrollToPosition(float normalizedPosition)
    {
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + normalizedPosition);
    }

}
