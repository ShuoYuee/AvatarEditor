using UnityEditor;

[CustomEditor(typeof(RoleAvaterDT))]
public class RoleAvaterDT_Editor : Editor
{
    private SerializedObject _RoleAvaterDT_Editor;
    private RoleAvaterDT _Data;

    private SerializedProperty _RoleRace;
    private SerializedProperty _RoleGender;
    private SerializedProperty _SkinMaterial;
    private SerializedProperty _Texture;
    private SerializedProperty _Color;
    private SerializedProperty _ColorName;
    private SerializedProperty _LowModel;
    private SerializedProperty _HightModel;

    private void OnEnable()
    {
        _RoleAvaterDT_Editor = new SerializedObject(target);
        _Data = (RoleAvaterDT)target;

        _RoleRace = _RoleAvaterDT_Editor.FindProperty("_RoleRace");
        _RoleGender = _RoleAvaterDT_Editor.FindProperty("_RoleGender");
        _SkinMaterial = _RoleAvaterDT_Editor.FindProperty("_SkinMaterial");
        _Texture = _RoleAvaterDT_Editor.FindProperty("_Texture");
        _Color = _RoleAvaterDT_Editor.FindProperty("_Color");
        _ColorName = _RoleAvaterDT_Editor.FindProperty("_ColorName");
        _LowModel = _RoleAvaterDT_Editor.FindProperty("_LowModel");
        _HightModel = _RoleAvaterDT_Editor.FindProperty("_HightModel");
    }

    public override void OnInspectorGUI()
    {
        _RoleAvaterDT_Editor.Update();
        EditorGUILayout.PropertyField(_RoleRace);
        EditorGUILayout.PropertyField(_RoleGender);
        EditorGUILayout.PropertyField(_SkinMaterial);
        EditorGUILayout.PropertyField(_Texture);
        EditorGUILayout.PropertyField(_Color);
        EditorGUILayout.PropertyField(_ColorName);
        EditorGUILayout.PropertyField(_LowModel);
        EditorGUILayout.PropertyField(_HightModel);
        if (_Data._SkinMaterial.Count > 3)
        {
            EditorGUILayout.HelpBox("SkinMaterial超過數量，一個人種僅有三個膚色！！！", MessageType.Warning);
        }
        else if (_Data._SkinMaterial.Count == 0)
        {
            EditorGUILayout.HelpBox("SkinMaterial尚未設定", MessageType.Info);
        }
        if (_Data._Texture.Count > 3)
        {
            EditorGUILayout.HelpBox("Texture超過數量，一個人種僅有三個膚色！！！", MessageType.Warning);
        }
        else if (_Data._Texture.Count == 0)
        {
            EditorGUILayout.HelpBox("Texture尚未設定", MessageType.Info);
        }
        if (_Data._Color.Count > 3)
        {
            EditorGUILayout.HelpBox("Color超過數量，一個人種僅有三個膚色！！！", MessageType.Warning);
        }
        else if (_Data._Color.Count == 0)
        {
            EditorGUILayout.HelpBox("Color尚未設定", MessageType.Info);
        }
        if (_Data._HightModel.Count > 2)
        {
            EditorGUILayout.HelpBox("HightModel超過數量，一個人種僅有兩種身高！！！", MessageType.Warning);
        }
        else if (_Data._HightModel.Count == 0)
        {
            EditorGUILayout.HelpBox("HightModel尚未設定", MessageType.Info);
        }
        if (_Data._LowModel.Count > 2)
        {
            EditorGUILayout.HelpBox("LowModel超過數量，一個人種僅有兩種身高！！！", MessageType.Warning);
        }
        else if (_Data._LowModel.Count == 0)
        {
            EditorGUILayout.HelpBox("LowModel尚未設定", MessageType.Info);
        }
        _RoleAvaterDT_Editor.ApplyModifiedProperties();
    }
}
