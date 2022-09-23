using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Editor_Scene))]
public class SceneData_Editor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.LabelField(position, label);

        SerializedProperty _EditorSecene = property.FindPropertyRelative("_EditorSecene");
        EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 1, position.width, 20), _EditorSecene, new GUIContent());

        if (_EditorSecene.enumValueIndex == 1)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 2, position.width, 20), property.FindPropertyRelative("_oSceneModel"), new GUIContent());
        }
        else if(_EditorSecene.enumValueIndex == 0)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 2, position.width, 20), property.FindPropertyRelative("_strScene"), new GUIContent());
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 12 + 32;
    }
}
