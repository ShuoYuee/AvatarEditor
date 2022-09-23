using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DressEditorManager))]
public class DressDataManager_Editor : Editor
{
    private SerializedObject _Manager;
    private DressEditorManager _DressEditorManager;

    private SerializedProperty _AvatarData;
    private SerializedProperty _RaceData;
    private SerializedProperty _DelAvatarName;
    private SerializedProperty _DelRaceName;
    private SerializedProperty _oLightObj;
    private SerializedProperty _MainCamera;

    private void OnEnable()
    {
        _Manager = new SerializedObject(target);
        _DressEditorManager = (DressEditorManager)target;

        _AvatarData = _Manager.FindProperty("_AvatarData");
        _RaceData = _Manager.FindProperty("_RoleData");
        _DelAvatarName = _Manager.FindProperty("_DelAvatarData");
        _DelRaceName = _Manager.FindProperty("_DelRaceData");
        _oLightObj = _Manager.FindProperty("_oLightObj");
        _MainCamera = _Manager.FindProperty("_MainCamera");
    }

    public override void OnInspectorGUI()
    {
        _Manager.Update();
        EditorGUILayout.PropertyField(_AvatarData, new GUIContent("時裝資料"), true);

        EditorGUILayout.PropertyField(_RaceData, new GUIContent("人種資料"), true);
        if (_DressEditorManager._RoleCount > 6)
        {
            EditorGUILayout.HelpBox("人種僅能有六個選項，包含白人、黑人、黃種人，並各分男女性！！！", MessageType.Warning);
        }
        EditorGUILayout.PropertyField(_DelAvatarName);
        EditorGUILayout.PropertyField(_DelRaceName);
        EditorGUILayout.PropertyField(_oLightObj);
        EditorGUILayout.PropertyField(_MainCamera);

        _Manager.ApplyModifiedProperties();
    }
}
