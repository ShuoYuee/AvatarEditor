using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Editor_AvatarData))]
[CanEditMultipleObjects]
public class AvatarData_Editor : PropertyDrawer
{
    private Editor_AvatarData _Data;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.LabelField(position, label);

        SerializedProperty AvatarData = property.FindPropertyRelative("_Part");
        EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 1, position.width, 20), property.FindPropertyRelative("_AvatarLowObj"), new GUIContent());
        EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 2, position.width, 20), property.FindPropertyRelative("_AvatarHightObj"), new GUIContent());

        if (AvatarData.enumValueIndex != 12)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 3, position.width, 20), property.FindPropertyRelative("_AvatarMaterial"), new GUIContent());
            EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 4, position.width, 20), property.FindPropertyRelative("_AvatarTexture"), new GUIContent());
            EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 5, position.width, 20), property.FindPropertyRelative("_Color"), new GUIContent());
        }
        EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 7, position.width, 20), AvatarData, new GUIContent());

        if (AvatarData.enumValueIndex >= 14)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 8, position.width, 20), property.FindPropertyRelative("_Size"), new GUIContent());
        }
        else
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 9, position.width, 20), property.FindPropertyRelative("_Gender"), new GUIContent());
        }
        EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 11, position.width, 20), property.FindPropertyRelative("_TakeOffData"), new GUIContent());

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 40 + 32;
    }
}