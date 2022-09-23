using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Editor_Action))]
[CanEditMultipleObjects]
public class ActionData_Editor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.LabelField(position, label);

        EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 1, position.width, 20), property.FindPropertyRelative("_strAnimClip"), new GUIContent());
        EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 2, position.width, 20), property.FindPropertyRelative("_strBikeAnimClip"), new GUIContent());
        EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 3, position.width, 20), property.FindPropertyRelative("_bFootIK"), new GUIContent());
        EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 4, position.width, 20), property.FindPropertyRelative("_bHandIK"), new GUIContent());
        EditorGUI.PropertyField(new Rect(position.x, position.y + 25 * 5, position.width, 20), property.FindPropertyRelative("_bBike"), new GUIContent());

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 18 + 32;
    }
}
