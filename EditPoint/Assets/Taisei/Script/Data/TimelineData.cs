using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TimeLineData")]
public class TimelineData : ScriptableObject
{
    [Header("1��ɃT�C�Y�ύX�����")] public float f_oneResize = 140f;
    [Header("1�ڐ���̕�(���[�J�����W)")] public float f_oneTickWidht = 140f;
    [Header("1���C���̍���(���[�J�����W)")] public float f_oneTickHeight = 74f;
    [Header("�^�C�����C���̍��[��X���W(���[�J�����W)")] public float f_timelineEndLeft = -166f;
    [Header("�^�C�����C���̉E�[��X���W(���[�J�����W)")] public float f_timelineEndRight = 83134f;
    [Header("�^�C�����C���̏��Y���W(���[�J�����W)")] public float f_timelineEndUp = 2f;
    [Header("�^�C�����C���̉���Y���W(���[�J�����W)")] public float f_timelineEndDown = -294f;
}
