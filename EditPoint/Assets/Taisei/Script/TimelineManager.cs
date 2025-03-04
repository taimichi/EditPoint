using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineManager : MonoBehaviour
{
    [SerializeField, Header("�^�C�����C���̒����@�b�P��")] private int timelineLength = 15;
    [SerializeField, Header("����̒���(�^�C���o�[���~�܂�ʒu)�@�b�P��")] private int timebarLimit = 10;

    [SerializeField] private RectTransform Content;

    [SerializeField] private GameObject LimitlinePre;

    private Vector3 startPos;   //�ύX�O�̍��W
    private Vector3 pos;        //�ύX��̍��W
    private Vector2 startSize;  //�ύX�O�̃T�C�Y
    private Vector2 size;       //�ύX��̃T�C�Y

    /// <summary>
    /// ����̒���(�^�C���o�[���~�܂�b��)��Ԃ�
    /// </summary>
    public int ReturnTimebarLimit() => timebarLimit;

    private void Awake()
    {
        TimelineData TLData = TimelineData.TimelineEntity;

        //�ύX�O�̍��W�ƃT�C�Y���擾
        startPos = Content.localPosition;
        startSize = Content.sizeDelta;

        //�ύX��̍��W���v�Z�@x�ȊO�͕ύX���Ȃ�
        pos = startPos;
        pos.x = TLData.oneResize * timelineLength;
        //�ύX��̃T�C�Y���v�Z�@width�ȊO�͕ύX���Ȃ�
        //���0.5�b�ɂȂ��Ă���̂Ł~2�����Ă���
        size = startSize;
        size.x = (TLData.oneTickWidth * 2) * timelineLength;

        //���W�A�T�C�Y��ύX
        Content.localPosition = pos;
        Content.sizeDelta = size;
    }

    private void Start()
    {
        GameObject TimebarObj = GameObject.Find("Timebar");
        TimeBar timeBarScript = TimebarObj.GetComponent<TimeBar>();
        Vector3 limitPos = new Vector3(timeBarScript.ReturnLimitPos(), TimebarObj.transform.localPosition.y, 0);

        GameObject limit = Instantiate(LimitlinePre,Content);
        limit.transform.localPosition = limitPos;
        //�q�I�u�W�F�N�g�̏��Ԃ�ύX
        int childNum = Content.childCount;
        limit.transform.SetSiblingIndex(childNum - 1);

    }
}
