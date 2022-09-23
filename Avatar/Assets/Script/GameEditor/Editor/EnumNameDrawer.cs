#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

/// <summary>
/// 編輯器模擬資料Editor(重新命名中文Enum)
/// </summary>
[CustomPropertyDrawer(typeof(EnumAttirbute))]
public class EnumNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 替換屬性名稱
        EnumAttirbute enumAttirbute = (EnumAttirbute)attribute;
        label.text = enumAttirbute.strName;

        bool isElement = Regex.IsMatch(property.displayName, "Element \\d+");
        if (isElement)
        {
            label.text = property.displayName;
        }

        if (property.propertyType == SerializedPropertyType.Enum)
        {
            DrawEnum(position, property, label);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    /// <summary>
    /// 重新繪製枚舉類型屬性
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    private void DrawEnum(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        Type type = fieldInfo.FieldType;

        string[] names = property.enumNames;
        string[] values = new string[names.Length];
        while (type.IsArray)
        {
            type = type.GetElementType();
        }

        for (int i = 0; i < names.Length; ++i)
        {
            FieldInfo info = type.GetField(names[i]);
            EnumAttirbute[] enumAttributes = (EnumAttirbute[])info.GetCustomAttributes(typeof(EnumAttirbute), false);
            values[i] = enumAttributes.Length == 0 ? names[i] : enumAttributes[0].strName;
        }

        int index = EditorGUI.Popup(position, label.text, property.enumValueIndex, values);
        if (EditorGUI.EndChangeCheck() && index != -1)
        {
            property.enumValueIndex = index;
        }
    }
}
#endif
