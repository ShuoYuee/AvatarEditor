using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SR
{
    [CustomEditor(typeof(ParameterConfiguration))]
    public class ParameterConfigurationEditor : Editor
    {
        private ParameterConfiguration parameter { get { return target as ParameterConfiguration; } }
        private TabsBlock tabs;

        private void OnEnable()
        {
            tabs = new TabsBlock(new Dictionary<string, System.Action>()
            {
                {"PCConfig", PCTap},
                {"MobileConfig", MobileTap},

            });
            tabs.SetCurrentMethod(parameter.lastTab);
        }
     
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(parameter, "ParameterConfiguration");
            tabs.Draw();
            if (GUI.changed)
            {
                parameter.lastTab = tabs.curMethodIndex;
                EditorUtility.SetDirty(parameter);
            }
        }


        private void PCTap()
        {
            parameter.scrollWheelZoomingSensitivity = EditorGUILayout.FloatField("ZoomingSensitivity: ", parameter.scrollWheelZoomingSensitivity);
            parameter.mouseRotationSpeed = EditorGUILayout.FloatField("The rotation speed of mouse: ", parameter.mouseRotationSpeed);
            using (new HorizontalBlock())
            {
                parameter.minDistance = EditorGUILayout.FloatField("Minimum dis: ", parameter.minDistance);
                parameter.maxDistance = EditorGUILayout.FloatField("Maximum  dis: ", parameter.maxDistance);
            }

            parameter.targetDistance = EditorGUILayout.FloatField("Select the current distance of the object/set to 0 default middle position: ", parameter.targetDistance);

            parameter.targetWeight = EditorGUILayout.FloatField("Adjust the camera focus weight based on the target size: ", parameter.targetWeight);

      

            using (new HorizontalBlock())
            {
                GUILayout.Label("Set Rotate If true select target Use under  rotateValue: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                parameter.isSettingRotate = EditorGUILayout.Toggle(parameter.isSettingRotate);
            }

            if (parameter.isSettingRotate)
            {
                parameter.currentRotate = EditorGUILayout.Vector3Field("RotateValue: ", parameter.currentRotate);
            }

        }
        private void MobileTap()
        {
            parameter.MobileZoomingSensitivity = EditorGUILayout.FloatField("Mobile Zoom of strength: ", parameter.MobileZoomingSensitivity);
            parameter.MobileRotationSpeed = EditorGUILayout.FloatField("Mobile Rotate of Speed: ", parameter.MobileRotationSpeed);
            using (new HorizontalBlock())
            {
                parameter.minDistance = EditorGUILayout.FloatField("Minimum dis: ", parameter.minDistance);
                parameter.maxDistance = EditorGUILayout.FloatField("Maximum  dis: ", parameter.maxDistance);
            }

            parameter.targetDistance = EditorGUILayout.FloatField("Select the current distance of the object/set to 0 default middle position: ", parameter.targetDistance);

            parameter.targetWeight = EditorGUILayout.FloatField("Adjust the camera focus weight based on the target size: ", parameter.targetWeight);



            using (new HorizontalBlock())
            {
                GUILayout.Label("Set Rotate If true select target Use under  rotateValue: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                parameter.isSettingRotate = EditorGUILayout.Toggle(parameter.isSettingRotate);
            }

            if (parameter.isSettingRotate)
            {
                parameter.currentRotate = EditorGUILayout.Vector3Field("RotateValue: ", parameter.currentRotate);
            }

        }

        private void OnSceneGUI()
        {
            if (parameter.isSettingRotate)
            {
                if (parameter.targetDistance == 0)
                {
                    parameter.targetDistance = (parameter.maxDistance / parameter.minDistance) / 2;
                }
                Handles.color = Color.green;
                Handles.SphereHandleCap(1, parameter.transform.position, Quaternion.identity, 0.5f, Event.current.type);
                Vector3 pos = Quaternion.Euler(parameter.currentRotate) * -Vector3.forward;
                Handles.SphereHandleCap(1, parameter.transform.position + pos * parameter.minDistance, Quaternion.identity, 0.5f, Event.current.type);
                Handles.SphereHandleCap(1, parameter.transform.position + pos * parameter.maxDistance, Quaternion.identity, 0.5f, Event.current.type);
                Handles.SphereHandleCap(1, parameter.transform.position + pos * parameter.targetDistance, Quaternion.identity, 1f, Event.current.type);
                Handles.color = Color.blue;
                GUIStyle style = new GUIStyle();
                style.fontSize = 22;
                Handles.Label(parameter.transform.position + pos * parameter.minDistance, parameter.minDistance.ToString(), style);
                Handles.Label(parameter.transform.position + pos * parameter.targetDistance, parameter.targetDistance.ToString(), style);
                Handles.Label(parameter.transform.position + pos * parameter.maxDistance, parameter.maxDistance.ToString(), style);
                Handles.color = Color.red;
                Handles.DrawLine(parameter.transform.position, parameter.transform.position + pos * parameter.minDistance);
                Handles.color = Color.green;
                Handles.DrawLine(parameter.transform.position + pos * parameter.minDistance, parameter.transform.position + pos * parameter.maxDistance);
            }
        }
    }

}
