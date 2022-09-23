#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// 編輯器模擬資料Editor(重新命名中文標題)
/// </summary>
[CustomPropertyDrawer(typeof(RenameAttribute))]
public class RenameEditor : PropertyDrawer
{
    private GUIContent _tLabel = null;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var att = (RenameAttribute)attribute;
        if (_tLabel == null)
        {
            string strName = att.strNmae;
            _tLabel = new GUIContent(strName);
        }
        EditorGUI.PropertyField(position, property, _tLabel);
    }
}
#endif
