using UnityEditor;
using UnityEngine;
using System;

// EnumFlagsAttribute を対象にしたカスタムプロパティドロワー
[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Enum型にキャスト
        Enum targetEnum = (Enum)Enum.ToObject(fieldInfo.FieldType, property.intValue);
        string[] enumNames = targetEnum.GetType().GetEnumNames();           //Enumの名前取得
        int[] enumValues = (int[])Enum.GetValues(targetEnum.GetType());     //Enumの値取得

        // チェックボックスの描画
        int currentValue = property.intValue;   //現在の状態
        int newValue = currentValue;            //新しい状態

        EditorGUILayout.LabelField(label);      //フィールド名追加
        EditorGUI.indentLevel++;                //インデント追加で見やすく

        //各フラグをチェックボックス形式で表示
        for (int i = 0; i < enumNames.Length; i++)
        {
            bool isSelected = (currentValue & enumValues[i]) != 0;                  //フラグがたっているか
            bool newSelected = EditorGUILayout.Toggle(enumNames[i], isSelected);    //チェックボックスで表示

            if (newSelected)
            {
                newValue |= enumValues[i]; // フラグを追加
            }
            else
            {
                newValue &= ~enumValues[i]; // フラグを削除
            }
        }

        EditorGUI.indentLevel--;
        property.intValue = newValue;

        EditorGUI.EndProperty();
    }
}
