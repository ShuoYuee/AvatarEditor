using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using SR;

namespace SR
{
    [CustomEditor(typeof(SR_Camera))]
    public class SR_CameraEditor : Editor
    {
        private SR_Camera camera { get { return target as SR_Camera; } }

        private SerializedProperty targetsTag;
        private TabsBlock tabs;

        private void OnEnable()
        {
            targetsTag = serializedObject.FindProperty("targetsTag");
            tabs = new TabsBlock(new Dictionary<string, System.Action>()
            {
                {"SelectTarget", TargetTab},
                {"Paning", MovementTab},
                {"Rotate", RotationTab},
                {"Height/Scale", HeightTab},
                {"GlobalConfig", ConfigTab}

            });
            tabs.SetCurrentMethod(camera.lastTab);
        }
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(camera, "SR_Camera");
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
            GUILayout.Label("SelectTarget:", EditorStyles.boldLabel);
            camera.targetSelect = EditorGUILayout.ObjectField("Target: ", camera.targetSelect, typeof(Transform)) as Transform;
            camera.targetOffset = EditorGUILayout.Vector3Field("Offset: ", camera.targetOffset);

            using (new HorizontalBlock())
            {
                GUILayout.Label("Whether to follow the target after it is selected: ", EditorStyles.boldLabel );
                camera.isFoollowTarget = EditorGUILayout.Toggle(camera.isFoollowTarget);
            }

            using (new HorizontalBlock())
            {
                GUILayout.Label("Double click/Single select object Default single: ", EditorStyles.boldLabel);
                camera.doubleClick = EditorGUILayout.Toggle(camera.doubleClick);
            }
            using (new HorizontalBlock())
            {
                GUILayout.Label("Detect UI(If true, nothing happens when you click on the UI): ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.isCheckUI = EditorGUILayout.Toggle(camera.isCheckUI);
            }
            using (new HorizontalBlock())
            {
                GUILayout.Label("Focus: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.useKeyFocus = EditorGUILayout.Toggle(camera.useKeyFocus);
            }
            if (camera.useKeyFocus)
            {
                camera.focusKey = (KeyCode)EditorGUILayout.EnumPopup("FocusKey: ", camera.focusKey);
                camera.focusSpeed = EditorGUILayout.FloatField("FocusSpeed: ", camera.focusSpeed);
                camera.targetWeight = EditorGUILayout.FloatField("Weights calculated based on the size of the object: ", camera.targetWeight);
            }
         
        }
        /// <summary>
        /// Paning Draw
        /// </summary>
        private void MovementTab()
        {
            using (new HorizontalBlock())
            {
                GUILayout.Label("Enable keyboard Input: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.useKeyboardInput = EditorGUILayout.Toggle(camera.useKeyboardInput);
            }
            if (camera.useKeyboardInput)
            {
                EditorGUILayout.EnumPopup("Horizontal axis name: ", InputAxis.Horizontal);
                EditorGUILayout.EnumPopup("Vertical axis name: ", InputAxis.Vertical);
                camera.keyboardMovementSpeed = EditorGUILayout.FloatField("KeyboardMovementSpeed: ", camera.keyboardMovementSpeed);
            }

            using (new HorizontalBlock())
            {
                GUILayout.Label("UseScreenEdgeInput(Mouse moves when placed at the edge of the screen): ", EditorStyles.boldLabel);
                camera.useScreenEdgeInput = EditorGUILayout.Toggle(camera.useScreenEdgeInput);
            }

            if (camera.useScreenEdgeInput)
            {
                camera.screenEdgeBorder = EditorGUILayout.FloatField("ScreenEdgeBorder(Pixel): ", camera.screenEdgeBorder);
                camera.screenEdgeMovementSpeed = EditorGUILayout.FloatField("ScreenEdgeMovementSpeed: ", camera.screenEdgeMovementSpeed);
            }

            using (new HorizontalBlock())
            {
                GUILayout.Label("EnablePan: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.usePanning = EditorGUILayout.Toggle(camera.usePanning);
            }
            if (camera.usePanning)
            {
                camera.panningKey = (KeyCode)EditorGUILayout.EnumPopup("PanKey: ", camera.panningKey);

                using (new HorizontalBlock())
                {
                    GUILayout.Label("Adaptive translation speed: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                    camera.adaptivepanningSpeed = EditorGUILayout.Toggle(camera.adaptivepanningSpeed);
                }
                if (!camera.adaptivepanningSpeed)
                {
                    camera.panningSpeed = EditorGUILayout.FloatField("PanSpeed: ", camera.panningSpeed);
                }
            }

        }
        /// <summary>
        /// Rotate Draw
        /// </summary>
        private void RotationTab()
        {
            //using (new HorizontalBlock())
            //{
            //    GUILayout.Label("以自身旋转: ", EditorStyles.boldLabel, GUILayout.Width(170f));
            //    camera.IsSelfRotate = EditorGUILayout.Toggle(camera.IsSelfRotate);
            //}

            if (camera.IsSelfRotate)
            {
                using (new HorizontalBlock())
                {
                    GUILayout.Label("UseKeyboardRotation: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                    camera.useKeyboardRotation = EditorGUILayout.Toggle(camera.useKeyboardRotation);
                }
                if (camera.useKeyboardRotation)
                {
                    camera.rotateLeftKey = (KeyCode)EditorGUILayout.EnumPopup("RotateLeftKey: ", camera.rotateLeftKey);
                    camera.rotateRightKey = (KeyCode)EditorGUILayout.EnumPopup("RotateRightKey: ", camera.rotateRightKey);
                    camera.keyboardRotationSpeed = EditorGUILayout.FloatField("keyboardRotationSpeed", camera.keyboardRotationSpeed);
                }
            }
            using (new HorizontalBlock())
            {
                GUILayout.Label("UseMouseRotation: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.useMouseRotation = EditorGUILayout.Toggle(camera.useMouseRotation);
            }
            if (camera.useMouseRotation)
            {
                camera.mouseRotationKey = (KeyCode)EditorGUILayout.EnumPopup("MouseRotationKey: ", camera.mouseRotationKey);
                camera.mouseRotationSpeed = EditorGUILayout.FloatField("MouseRotationSpeed: ", camera.mouseRotationSpeed);
            }
            using (new HorizontalBlock())
            {
                GUILayout.Label("UseFixedXaxisAngle: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.IsFixedXAngle = EditorGUILayout.Toggle(camera.IsFixedXAngle);
            }
            if (camera.IsFixedXAngle)
            {
                camera.xAngle = EditorGUILayout.FloatField("XaxisAangle", camera.xAngle);
            }
            else
            {
                using (new HorizontalBlock())
                {
                    GUILayout.Label("isClampXAngle: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                    camera.isClampXAngle = EditorGUILayout.Toggle(camera.isClampXAngle);
                }

                if (camera.isClampXAngle)
                {
                    camera.clampMinXAngle = EditorGUILayout.FloatField("MinXAngle: ", camera.clampMinXAngle);
                    camera.clampMaxXAngle = EditorGUILayout.FloatField("MaxXAngle: ", camera.clampMaxXAngle);
                }
            }

        }
        /// <summary>
        /// Height/Scale Draw
        /// </summary>
        private void HeightTab()
        {
            using (new HorizontalBlock())
            {
                GUILayout.Label("HegihtSetting: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.useKeyboardZooming = EditorGUILayout.Toggle(camera.useKeyboardZooming);
            }
            if (camera.useKeyboardZooming)
            {
                camera.dropKey = (KeyCode)EditorGUILayout.EnumPopup("dropKey: ", camera.dropKey);
                camera.riseKey = (KeyCode)EditorGUILayout.EnumPopup("riseKey: ", camera.riseKey);
                camera.keyboardZoomingSensitivity = EditorGUILayout.FloatField("Keyboard sensitivity: ", camera.keyboardZoomingSensitivity);
                camera.heightDampening = EditorGUILayout.FloatField("heightDampening: ", camera.heightDampening);
            }

            using (new HorizontalBlock())
            {
                GUILayout.Label("useScrollwheelZooming: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.useScrollwheelZooming = EditorGUILayout.Toggle(camera.useScrollwheelZooming);
            }

            if (camera.useScrollwheelZooming)
            {
                using (new HorizontalBlock())
                {
                    GUILayout.Label("adaptiveScrollWheelSpeed: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                    camera.adaptiveScrollWheelSpeed = EditorGUILayout.Toggle(camera.adaptiveScrollWheelSpeed);
                }

                if (!camera.adaptiveScrollWheelSpeed)
                {
                    camera.scrollWheelZoomingSensitivity = EditorGUILayout.FloatField("Scrollwheel sensitivity: ", camera.scrollWheelZoomingSensitivity);
                }
              
            }
        }


        /// <summary>
        ///Global Config Draw
        /// </summary>
        private void ConfigTab()
        {
            using (new HorizontalBlock())
            {
                GUILayout.Label("Show visual areas: ", EditorStyles.boldLabel, GUILayout.Width(170f));
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
            camera.moveLerpSpeed = EditorGUILayout.FloatField("moveLerpSpeed(Consistent with rotation interpolation is recommended): ", camera.moveLerpSpeed);
            camera.rotateLerpSpeed = EditorGUILayout.FloatField("rotateLerpSpeed(The recommendation is consistent with the move interpolation): ", camera.rotateLerpSpeed);

            using (new HorizontalBlock())
            {
                GUILayout.Label("Camera Rotate Lerp: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.CameraRotateLerp = EditorGUILayout.Toggle(camera.CameraRotateLerp);
            }
            using (new HorizontalBlock())
            {
                GUILayout.Label("limitCameraMap: ", EditorStyles.boldLabel, GUILayout.Width(170f));
                camera.limitMap = EditorGUILayout.Toggle(camera.limitMap);
            }
            if (camera.limitMap)
            {
                camera.center = EditorGUILayout.Vector3Field("MapCenter: ", camera.center);
                using (new HorizontalBlock())
                {
                    camera.limitX = EditorGUILayout.FloatField("camera limitX: ", camera.limitX);
                    camera.limitZ = EditorGUILayout.FloatField("camera limitZ: ", camera.limitZ);
                    camera.aimlimitX = camera.limitX;
                    camera.aimlimitZ = camera.limitZ;
                }
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
