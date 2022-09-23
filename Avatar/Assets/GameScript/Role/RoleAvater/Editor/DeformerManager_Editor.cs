//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(DeformerManager))]
//[CanEditMultipleObjects]
//public class DeformerManager_Editor : Editor
//{
//    private SerializedObject _Manager;
//    private DeformerManager _DeformerManager;
//    private Animator _Animator;

//    private void OnEnable()
//    {
//        _Manager = new SerializedObject(target);
//        _DeformerManager = (DeformerManager)target;

//        DressRoleControl tRoleControl = _DeformerManager.GetComponent<DressRoleControl>();
//        _DeformerManager.f_Init(tRoleControl, new DressRoleActionController(tRoleControl));
//    }

//    public override void OnInspectorGUI()
//    {
//        _Manager.Update();
//        _DeformerManager.fBodyHight = EditorGUILayout.IntSlider("身高", (int)_DeformerManager.fBodyHight, 0, 100);
//        _DeformerManager.f_Deformer(GameEM.EM_Deformer.BodyHeight.ToString(), _DeformerManager.fBodyHight);

//        _DeformerManager.fSpine = EditorGUILayout.IntSlider("身長", (int)_DeformerManager.fSpine, 0, 100);
//        _DeformerManager.f_Deformer(GameEM.EM_Deformer.Spine.ToString(), _DeformerManager.fSpine);

//        _DeformerManager.fLegHight = EditorGUILayout.IntSlider("腿長", (int)_DeformerManager.fLegHight, 0, 100);
//        _DeformerManager.f_Deformer(GameEM.EM_Deformer.LegHight.ToString(), _DeformerManager.fLegHight);

//        _DeformerManager.fHandHight = EditorGUILayout.IntSlider("手臂長", (int)_DeformerManager.fHandHight, 0, 100);
//        _DeformerManager.f_Deformer(GameEM.EM_Deformer.HandHight.ToString(), _DeformerManager.fHandHight);

//        _DeformerManager.fShoulder = EditorGUILayout.IntSlider("肩寬", (int)_DeformerManager.fShoulder, 0, 100);
//        _DeformerManager.f_Deformer(GameEM.EM_Deformer.Shoulder.ToString(), _DeformerManager.fShoulder);

//        _Manager.ApplyModifiedProperties();
//    }
//}
