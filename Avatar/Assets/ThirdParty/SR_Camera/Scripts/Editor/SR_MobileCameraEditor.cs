using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SR
{
    [CustomEditor(typeof(SR_MobileCamera))]
    public class SR_MobileCameraEditor : Editor
    {
        private SR_MobileCamera camera { get { return target as SR_MobileCamera; } }

        private SerializedProperty targetsTag;
        //  private SerializedProperty fingerType;
        private TabsBlock tabs;

        private void OnEnable()
        {
            targetsTag = serializedObject.FindProperty("targetsTag");
            // fingerType = serializedObject.FindProperty("fingerType");
            tabs = new TabsBlock(new Dictionary<string, System.Action>()
            {
                {"TargetSetting", TargetTab},
                {"Panning", MovementTab},
                {"Rotate", RotationTab},
                {"Scale/Height", HeightTab},
                {"GlobalConfig", ConfigTab}

            });
            tabs.SetCurrentMethod(camera.lastTab);
        }

        public override void OnInspectorGUI()
        {
            Undo.RecordObject(camera, "SR_MobileCamera");
            tabs.Draw();
            if (GUI.changed)
            {
                camera.lastTab = tabs.curMethodIndex;
                EditorUtility.SetDirty(camera);
            }
            
        }

        /// <summary>
        /// Select Target Draw
        /// </summary>
        private void TargetTab()
        {
            using (new HorizontalBlock())
            {
                serializedObject.Update();
                EditorGUILayout.PropertyField(targetsTag, true);
                serializedObject.ApplyModifiedProperties();
            }
            GUILayout.Label("FollowTarget:", EditorStyles.boldLabel);
            camera.targetSelect = EditorGUILayout.ObjectField("Target: ", camera.targetSelect, typeof(Transform)) as Transform;
            camera.targetOffset = EditorGUILayout.Vector3Field("Offset: ", camera.targetOffset);
            using (new HorizontalBlock())
            {
                GUILayout.Label("Whether to follow the target after it is selected: ", EditorStyles.boldLabel);
                camera.isFoollowTarget = EditorGUILayout.Toggle(camera.isFoollowTarget);
            }
            using (new HorizontalBlock())
            {
                camera.focusSpeed = EditorGUILayout.FloatField("FocusSpeed: ", camera.focusSpeed);
                camera.targetWeight = EditorGUILayout.FloatField("Weights calculated size of object: ", camera.targetWeight);
            }
        }
        private void MovementTab()
        {
            using (new HorizontalBlock())
            {
                GUILayout.Label("Enable Pan: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.isCanpanning = EditorGUILayout.Toggle(camera.isCanpanning);
            }
            if (camera.isCanpanning)
            {
                using (new HorizontalBlock())
                {
                    serializedObject.Update();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("fingerType"), true);
                    serializedObject.ApplyModifiedProperties();
                }
                camera.panningSpeed = EditorGUILayout.FloatField("panning Speed: ", camera.panningSpeed);
            }


        }
        /// <summary>
        /// Rotation Draw
        /// </summary>
        private void RotationTab()
        {
            using (new HorizontalBlock())
            {
                GUILayout.Label("Enable Rotate: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.isCanRoatate = EditorGUILayout.Toggle(camera.isCanRoatate);
            }

            if (camera.isCanRoatate)
            {
                using (new HorizontalBlock())
                {
                    camera.RotationSpeed = EditorGUILayout.FloatField("Rotation Speed: ", camera.RotationSpeed);
                }

                using (new HorizontalBlock())
                {
                    GUILayout.Label("Use Fiexd Angle: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                    camera.IsFixedXAngle = EditorGUILayout.Toggle(camera.IsFixedXAngle);
                }
                if (camera.IsFixedXAngle)
                {
                    camera.xAngle = EditorGUILayout.FloatField("X axis Angle", camera.xAngle);
                }
                else
                {
                    using (new HorizontalBlock())
                    {
                        GUILayout.Label("Limit the X Angle: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                        camera.isClampXAngle = EditorGUILayout.Toggle(camera.isClampXAngle);
                    }

                    if (camera.isClampXAngle)
                    {
                        camera.clampMinXAngle = EditorGUILayout.FloatField("Min X Angle: ", camera.clampMinXAngle);
                        camera.clampMaxXAngle = EditorGUILayout.FloatField("Max X Angle: ", camera.clampMaxXAngle);
                    }
                }
            }


        }
        /// <summary>
        /// Height Scale Draw
        /// </summary>
        private void HeightTab()
        {
            using (new HorizontalBlock())
            {
                GUILayout.Label("Enable height/zoom Settings: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.isCanZooming = EditorGUILayout.Toggle(camera.isCanZooming);
            }

            if (camera.isCanZooming)
            {
                using (new HorizontalBlock())
                {
                    GUILayout.Label("The minimum Angle for zooming in and out of gestures. It is recommended that this value be limited to 30-90 degrees to distinguish between two fingers for translation or rotation ", EditorStyles.boldLabel);
                }
                using (new HorizontalBlock())
                {
                    camera.ZoomThreshold = Mathf.Clamp(EditorGUILayout.FloatField("To distinguish the threshold between(30-90): ", camera.ZoomThreshold ),30,90);
                }
                using (new HorizontalBlock())
                {
                    camera.ZoomingSensitivity = EditorGUILayout.FloatField("density values for scaling control: ", camera.ZoomingSensitivity);
                }

                using (new HorizontalBlock())
                {
                    camera.ZoomDirectionSensitivity = EditorGUILayout.FloatField("Increasing/decreasing density values: ", camera.ZoomDirectionSensitivity);
                }


            }

        }


        /// <summary>
        /// Global Config Draw
        /// </summary>
        private void ConfigTab()
        {
            using (new HorizontalBlock())
            {
                GUILayout.Label("ShowVisualAreas: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.ShowVisualAreas = EditorGUILayout.Toggle(camera.ShowVisualAreas);
            }
            using (new HorizontalBlock())
            {
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("groundMask"), true);
                serializedObject.ApplyModifiedProperties();
            }
            //using (new HorizontalBlock())
            //{
            //    GUILayout.Label("摄像机移动线性插值: ", EditorStyles.boldLabel, GUILayout.Width(170f));
            //    camera.CameraMoveLiner = EditorGUILayout.Toggle(camera.CameraMoveLiner);
            //}
            camera.LerpDamping = EditorGUILayout.FloatField("Moving interpolation velocity: ", camera.LerpDamping);
            
            using (new HorizontalBlock())
            {
                GUILayout.Label("Limit the camera area: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.limitMap = EditorGUILayout.Toggle(camera.limitMap);
            }
            if (camera.limitMap)
            {
                camera.center = EditorGUILayout.Vector3Field("Map Center: ", camera.center);
                camera.limitX = EditorGUILayout.FloatField("Camera limitX: ", camera.limitX);
                camera.limitZ = EditorGUILayout.FloatField("Camera limitZ: ", camera.limitZ);
                camera.aimlimitX = camera.limitX;
                camera.aimlimitZ = camera.limitZ;
                using (new HorizontalBlock())
                {
                    camera.minHeight = EditorGUILayout.FloatField("Camera Min height: ", camera.minHeight);
                    camera.maxHeight = EditorGUILayout.FloatField("Camera Max height: ", camera.maxHeight);
                    camera.aimMaxHeight = camera.maxHeight;
                }
                using (new HorizontalBlock())
                {
                    camera.aimlimitX = EditorGUILayout.FloatField("aim limitX: ", camera.aimlimitX);
                    camera.aimlimitZ = EditorGUILayout.FloatField("aim limitZ: ", camera.aimlimitZ);
                }
                using (new HorizontalBlock())
                {
                    camera.aimMinHeight = EditorGUILayout.FloatField("aim Min height: ", camera.aimMinHeight);
                    camera.aimMaxHeight = EditorGUILayout.FloatField("aim Max height: ", camera.aimMaxHeight);
                }


            }

            using (new HorizontalBlock())
            {
                GUILayout.Label("Distance Aim Setting: ", EditorStyles.boldLabel, GUILayout.Width(170f));

            }
            using (new HorizontalBlock())
            {
                camera.minDistance = EditorGUILayout.FloatField("MinDistance: ", camera.minDistance);
                camera.maxDistance = EditorGUILayout.FloatField("MaxDistance: ", camera.maxDistance);
            }

        }

        private void OnSceneGUI()
        {
            if (camera.ShowVisualAreas)
            {
                var widthOffset = Vector3.right * camera.limitX;
                var lengthOffset = Vector3.forward * camera.limitZ;
                var heightOffset = Vector3.forward * camera.maxHeight;
                var verts = new Vector3[]
                {
                camera.center + widthOffset + lengthOffset+new Vector3(0,camera.minHeight),
                camera.center - widthOffset + lengthOffset+new Vector3(0,camera.minHeight),
                camera.center  - widthOffset - lengthOffset+new Vector3(0,camera.minHeight),
                camera.center  + widthOffset - lengthOffset+new Vector3(0,camera.minHeight),
                };
                Handles.DrawSolidRectangleWithOutline(verts, new Color(0.2f, 0.5f, 0.5f, 0.2f), Color.green);
                verts = new Vector3[]
               {
                camera.center + widthOffset + lengthOffset+new Vector3(0,camera.maxHeight),
                camera.center - widthOffset + lengthOffset+new Vector3(0,camera.maxHeight),
                camera.center  - widthOffset - lengthOffset+new Vector3(0,camera.maxHeight),
                camera.center  + widthOffset - lengthOffset+new Vector3(0,camera.maxHeight)
               };
                Handles.DrawSolidRectangleWithOutline(verts, new Color(0.2f, 0.5f, 0.5f, 0.05f), Color.green);
                verts = new Vector3[]
                {
                camera.center  - widthOffset - lengthOffset+new Vector3(0,camera.minHeight),
                camera.center  - widthOffset - lengthOffset+new Vector3(0,camera.maxHeight),
                camera.center  + widthOffset - lengthOffset+new Vector3(0,camera.maxHeight),
                camera.center  + widthOffset - lengthOffset+new Vector3(0,camera.minHeight)

                };
                Handles.DrawSolidRectangleWithOutline(verts, new Color(0.2f, 0.5f, 0.5f, 0.05f), Color.green);
                verts = new Vector3[]
                {
                camera.center  - widthOffset + lengthOffset+new Vector3(0,camera.minHeight),
                camera.center  - widthOffset + lengthOffset+new Vector3(0,camera.maxHeight),
                camera.center  - widthOffset - lengthOffset+new Vector3(0,camera.maxHeight),
                camera.center  - widthOffset - lengthOffset+new Vector3(0,camera.minHeight),
                };
                Handles.DrawSolidRectangleWithOutline(verts, new Color(0.2f, 0.5f, 0.5f, 0.05f), Color.green);
                verts = new Vector3[]
                {
                camera.center  + widthOffset + lengthOffset+new Vector3(0,camera.minHeight),
                camera.center  + widthOffset + lengthOffset+new Vector3(0,camera.maxHeight),
                camera.center  + widthOffset - lengthOffset+new Vector3(0,camera.maxHeight),
                camera.center  + widthOffset - lengthOffset+new Vector3(0,camera.minHeight),
                };
                Handles.DrawSolidRectangleWithOutline(verts, new Color(0.2f, 0.5f, 0.5f, 0.05f), Color.green);
                verts = new Vector3[]
                {
                camera.center  - widthOffset + lengthOffset+new Vector3(0,camera.minHeight),
                camera.center  - widthOffset + lengthOffset+new Vector3(0,camera.maxHeight),
                camera.center  + widthOffset + lengthOffset+new Vector3(0,camera.maxHeight),
                camera.center  + widthOffset + lengthOffset+new Vector3(0,camera.minHeight),
                };
                Handles.DrawSolidRectangleWithOutline(verts, new Color(0.2f, 0.5f, 0.5f, 0.05f), Color.green);


                widthOffset = Vector3.right * camera.aimlimitX;
                lengthOffset = Vector3.forward * camera.aimlimitZ;
                heightOffset = Vector3.forward * camera.aimMaxHeight;
                verts = new Vector3[]
               {
                camera.center + widthOffset + lengthOffset+new Vector3(0,camera.aimMinHeight),
                camera.center - widthOffset + lengthOffset+new Vector3(0,camera.aimMinHeight),
                camera.center  - widthOffset - lengthOffset+new Vector3(0,camera.aimMinHeight),
                camera.center  + widthOffset - lengthOffset+new Vector3(0,camera.aimMinHeight),
               };
                Handles.DrawSolidRectangleWithOutline(verts, new Color(0.8f, 0.8f, 0.2f, 0.1f), Color.blue);
                Handles.color = Color.blue;
                if (camera.AimOriginPoint)
                {
                    Handles.SphereHandleCap(1, camera.AimOriginPoint.position, Quaternion.identity, 1f, Event.current.type);
                    var dir = camera.AimOriginPoint.position - camera.transform.position;
                    Handles.Label(camera.transform.position + dir.normalized * dir.magnitude / 2, dir.magnitude.ToString("F1"));
                }



            }

        }
    }

}

