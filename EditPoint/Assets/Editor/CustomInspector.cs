using UnityEditor;

[CustomEditor(typeof(ButtonSelect))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty typeProp = serializedObject.FindProperty("kindButton");
        EditorGUILayout.PropertyField(typeProp);

        var type = (ButtonSelect.SelectButton)typeProp.enumValueIndex;

        /* 共通表示 */
        // 選択中の表示オブジェクト
        EditorGUILayout.PropertyField(serializedObject.FindProperty("img"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fileObj"));

        // enumの値によって特定の表示を行う
        if (type == ButtonSelect.SelectButton.File)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LockPanel"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
