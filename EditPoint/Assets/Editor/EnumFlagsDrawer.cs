using UnityEditor;
using UnityEngine;
using System;

// EnumFlagsAttribute ��Ώۂɂ����J�X�^���v���p�e�B�h�����[
[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Enum�^�ɃL���X�g
        Enum targetEnum = (Enum)Enum.ToObject(fieldInfo.FieldType, property.intValue);
        string[] enumNames = targetEnum.GetType().GetEnumNames();           //Enum�̖��O�擾
        int[] enumValues = (int[])Enum.GetValues(targetEnum.GetType());     //Enum�̒l�擾

        // �`�F�b�N�{�b�N�X�̕`��
        int currentValue = property.intValue;   //���݂̏��
        int newValue = currentValue;            //�V�������

        EditorGUILayout.LabelField(label);      //�t�B�[���h���ǉ�
        EditorGUI.indentLevel++;                //�C���f���g�ǉ��Ō��₷��

        //�e�t���O���`�F�b�N�{�b�N�X�`���ŕ\��
        for (int i = 0; i < enumNames.Length; i++)
        {
            bool isSelected = (currentValue & enumValues[i]) != 0;                  //�t���O�������Ă��邩
            bool newSelected = EditorGUILayout.Toggle(enumNames[i], isSelected);    //�`�F�b�N�{�b�N�X�ŕ\��

            if (newSelected)
            {
                newValue |= enumValues[i]; // �t���O��ǉ�
            }
            else
            {
                newValue &= ~enumValues[i]; // �t���O���폜
            }
        }

        EditorGUI.indentLevel--;
        property.intValue = newValue;

        EditorGUI.EndProperty();
    }
}
