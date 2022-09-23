using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DressEditorData))]
[CanEditMultipleObjects]
public class DressData_Editor : Editor
{
    private SerializedObject _Data;
    private DressEditorData _DressEditorData;

    private SerializedProperty _EditorData;
    private SerializedProperty _ID;
    private SerializedProperty _Name;

    private SerializedProperty _Avatar;
    private SerializedProperty _Race;
    private SerializedProperty _Action;
    private SerializedProperty _Scene;

    private void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        foreach (Object obj in targets)
        {
            _Data = new SerializedObject(obj);
            _DressEditorData = (DressEditorData)obj;

            _EditorData = _Data.FindProperty("_EditorAction");
            _ID = _Data.FindProperty("_ID");
            _Name = _Data.FindProperty("_AvatarName");

            _Avatar = _Data.FindProperty("_AvatarData");
            _Race = _Data.FindProperty("_Race");
            _Action = _Data.FindProperty("_Action");
            _Scene = _Data.FindProperty("_SceneData");
            EditorUtility.SetDirty(obj);

            _Data.Update();
            EditorGUILayout.PropertyField(_EditorData);
            EditorGUILayout.PropertyField(_ID);
            EditorGUILayout.PropertyField(_Name);

            switch (_EditorData.enumValueIndex)
            {
                case (int)DressEditorData.EM_EditorAction.Avatar:
                    EditorGUILayout.PropertyField(_Avatar);
                    break;

                case (int)DressEditorData.EM_EditorAction.RoleRace:
                    EditorGUILayout.PropertyField(_Race);
                    break;

                case (int)DressEditorData.EM_EditorAction.Action:
                    EditorGUILayout.PropertyField(_Action);
                    break;

                case (int)DressEditorData.EM_EditorAction.Lens:
                    EditorGUILayout.PropertyField(_Scene);
                    break;
            }
            _Data.ApplyModifiedProperties();
        }
    }
}
