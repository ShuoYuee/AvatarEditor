using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace SR
{
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("SR Camera")]
    public class SR_Camera : MonoBehaviour
    {
        public int lastTab = 0;
        private Camera m_camera;
        private bool useFixedUpdate = false;
        //use FixedUpdate() or LateUpdate()
        [Header("keyboard Movement Speed")]
        public float keyboardMovementSpeed = 10f;
        [Header("screen Edge Movement Speed")]
        public float screenEdgeMovementSpeed = 5f;

        [Header("keyboard Rotation Speed")]
        public float keyboardRotationSpeed = 3f;

        [Header("adaptive panning Speed")]
        public bool  adaptivepanningSpeed = false;

        [Header("panning Speed")]
        public float panningSpeed = 100f;
        [Header("mouse Rotation Speed")]
        public float mouseRotationSpeed = 200f;

        [Header("height Dampening 0-50")]
        public float heightDampening = 5f;
        [Header("key boardZooming Sensitivity")]
        public float keyboardZoomingSensitivity = 10f;
        [Header("scrollWheel Zooming Sensitivity")]
        public float scrollWheelZoomingSensitivity = 20f;

        [Header("adaptive ScrollWheel Speed")]
        public bool adaptiveScrollWheelSpeed = true;
        private float zoomPos = 0; //value in range (0, 1) used as t in Matf.Lerp

        [SerializeField]
        [Header("Check Tag")]
        public List<string> targetsTag;
        [Header("target Select")]
        public Transform targetSelect;

        private ParameterConfiguration parameter;

        [Header("target Offset")] 
        public Vector3 targetOffset = Vector3.zero;
        [Header("is Foollow Target")]
        public bool isFoollowTarget = true;

        [Header("click/doubleClick Select Target  Default click")]
        public bool doubleClick = false;
        [Header("is Check UI")]
        public bool isCheckUI = true;

        //缩放速度
        private float targetDistance = 0;
        [Header("max Distance")]
        public float maxDistance = 100f;
        [Header("min Distance")]
        public float minDistance = 2f;

        [Header("F key focus Speed")]
        public float focusSpeed = 10;
        [Header("target Weight")]
        public float targetWeight = 5;

        private bool isFocusing = false;
        private Vector3 targetPos;
        private float distance;
        private float juliCalc;

        public bool isCanSetCamera = true;
        private Vector3 setPostion;
        private Vector3 setRotate;
        private bool isSetting = false;
        Vector3 targetPosCalc = Vector3.zero;

        private float x;
        private float y;
        private Vector3 position;
        private Quaternion rotation;

        #region 事件Event
        public Action<GameObject> SelectTargetAction;//Select the callback for the object
        public Action ResetTargetAction;//Emitted when a target is lost or reset
        public Action<GameObject> FocusStartAction;//Focus on the beginning of the pullback
        public Action<GameObject> FocusEndAction;//Focus on the completed callback
        public Action ResetCameraTransfomCompleteAction;//Emitted when resetting camera position and rotation is complete
        #endregion
        //DefaultParamter
        private float RecordscrollWheelZoomingSensitivity;
        public float RecordmaxDistance = 100f;
        public float RecordminDistance = 2f;
        public float RecordtargetWeight = 5;
        public float RecordmouseRotationSpeed = 200f;

        #region 全局配置
        public Transform AimOriginPoint;
        [Header("Show Visual Areas")]
        public bool ShowVisualAreas = true;
        [Header("limit Map")]
        public bool limitMap = true;
        [Header("Scene center")]
        public Vector3 center = Vector3.zero;
        [Header("Check ground Mask")]
        public LayerMask groundMask = -1;
        [Header("aim Max Height")]
        public float aimMaxHeight = 100f;
        [Header("aim Min Height")]
        public float aimMinHeight = 0f;

        [Header("x limit of map")]
        public float aimlimitX = 100f; 
        [Header("z limit of map")]
        public float aimlimitZ = 100f; 

        [Header("Camera max Height")]
        public float maxHeight = 100f;
        [Header("Camera min Height")]
        public float minHeight = 1f;
        [Header("x limit of map")]
        public float limitX = 100f; 
        [Header("z limit of map")]
        public float limitZ = 100f; 
        [Header("Camera RotateLerp")]
        public bool CameraRotateLerp = false;

        private float LinerDistance;
        private float LinerAngle;
        [Header("move Lerp Speed")]
        public float moveLerpSpeed = 5.0f;
        [Header("rotate Lerp Speed")]
        public float rotateLerpSpeed = 5.0f;
        #endregion

        private bool isAligning = false;
        public bool FollowingTarget
        {
            get
            {
                return targetSelect != null;
            }
        }


        #region Input
        public IIput input;
        public bool useScreenEdgeInput = true;
        public float screenEdgeBorder = 100f;

        public bool useKeyboardInput = true;

        public bool usePanning = true;
        public KeyCode panningKey = KeyCode.Mouse2;

        public bool useKeyboardZooming = true;
        public KeyCode dropKey = KeyCode.E;
        public KeyCode riseKey = KeyCode.Q;

        public bool useScrollwheelZooming = true;
        public bool useKeyFocus = true;
        public KeyCode focusKey = KeyCode.F;

        [Header("Self Rotate")]
        public bool IsSelfRotate = false;
        public bool useKeyboardRotation = true;
        public KeyCode rotateRightKey = KeyCode.X;
        public KeyCode rotateLeftKey = KeyCode.Z;

        public bool useMouseRotation = true;
        public KeyCode mouseRotationKey = KeyCode.Mouse1;

        [Header("Is Fixed X Angle")]
        public bool IsFixedXAngle = true;
        public float xAngle = 45f;

        [Header("Clamp X Angle")]
        public bool isClampXAngle = true;
        public float clampMinXAngle = 0;
        public float clampMaxXAngle = 90;
        private Vector2 KeyboardInput
        {
            get { return useKeyboardInput ? new Vector2(input.GetAxis(InputAxis.Horizontal), input.GetAxis(InputAxis.Vertical)) : Vector2.zero; }
        }

        private float ScrollWheel
        {
            get { return input.GetAxis(InputAxis.Z); }
        }

        private Vector2 MouseAxis
        {
            get { return new Vector2(input.GetAxis(InputAxis.X), input.GetAxis(InputAxis.Y)); }
        }
        private int ZoomDirection
        {
            get
            {
                bool zoomIn = input.GetKey(dropKey);
                bool zoomOut = input.GetKey(riseKey);
                if (zoomIn && zoomOut)
                    return 0;
                else if (!zoomIn && zoomOut)
                    return 1;
                else if (zoomIn && !zoomOut)
                    return -1;
                else
                    return 0;
            }
        }

        private int RotationDirection
        {
            get
            {
                bool rotateRight = input.GetKey(rotateRightKey);
                bool rotateLeft = input.GetKey(rotateLeftKey);
                if (rotateLeft && rotateRight)
                    return 0;
                else if (rotateLeft && !rotateRight)
                    return -1;
                else if (!rotateLeft && rotateRight)
                    return 1;
                else
                    return 0;
            }
        }
        #endregion
        #region Unity_Methods

        void Awake()
        {
            if (EventSystem.current==null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
        }

        private void Start()
        {

            RecordscrollWheelZoomingSensitivity = scrollWheelZoomingSensitivity;
            RecordmaxDistance = maxDistance;
            RecordminDistance = minDistance;
            RecordtargetWeight = targetWeight;
            RecordmouseRotationSpeed = mouseRotationSpeed;
            input = new PCInput();
            m_camera = GetComponent<Camera>();
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
            GameObject go = new GameObject("AimPoint");
            AimOriginPoint = go.transform;
            CalcAimPosAndDistance();
        }

        void ResetParameters()
        {
            scrollWheelZoomingSensitivity = RecordscrollWheelZoomingSensitivity;
            maxDistance = RecordmaxDistance;
            minDistance = RecordminDistance;
            targetWeight = RecordtargetWeight;
            mouseRotationSpeed = RecordmouseRotationSpeed;
        }

        public void Update()
        {
            if (!doubleClick)
            {
                if (isCheckUI)
                {
                    if (input.IsTap() && !input.IsPointerOverUIObject())
                    {
                        Ray ray = m_camera.ScreenPointToRay(input.GetPositon());
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (targetsTag.Contains(hit.transform.tag))
                                SetTarget(hit.transform);
                            else
                                ResetTarget();
                        }
                    }
                }
                else
                {
                    if (input.IsTap())
                    {
                        Ray ray = m_camera.ScreenPointToRay(input.GetPositon());
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (targetsTag.Contains(hit.transform.tag))
                                SetTarget(hit.transform);
                            else
                                ResetTarget();
                        }
                    }
                }

            }
            else
            {
                if (isCheckUI)
                {
                    if (input.IsDoubleClick() && !input.IsPointerOverUIObject())
                    {
                        Ray ray = m_camera.ScreenPointToRay(input.GetPositon());
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (targetsTag.Contains(hit.transform.tag))
                                SetTarget(hit.transform);
                            else
                            {
                                ResetTarget();
                            }
                        }
                    }
                }
                else
                {
                    if (input.IsDoubleClick())
                    {
                        Ray ray = m_camera.ScreenPointToRay(input.GetPositon());
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (targetsTag.Contains(hit.transform.tag))
                                SetTarget(hit.transform);
                            else
                            {
                                ResetTarget();
                            }
                        }
                    }
                }


            }
        }
        private void LateUpdate()
        {
            if (!useFixedUpdate)
                CameraUpdate();
        }

        private void OnDrawGizmos()
        {
            if (ShowVisualAreas)
            {
                //Draw Gizmos
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(transform.position, Vector3.down * 100);
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(transform.position, transform.forward * 100);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, transform.right * 10);

                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.up * 10);

                Quaternion rot = Quaternion.AngleAxis(-transform.eulerAngles.x, transform.right);
                Vector3 dir = rot * transform.forward;
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(transform.position, dir * 100);
            }

        }
        private void FixedUpdate()
        {
            if (useFixedUpdate)
                CameraUpdate();
        }
        #endregion
        /// <summary>
        ///Update camera position and rotation
        /// </summary>
        private void CameraUpdate()
        {
            if (isCanSetCamera && isSetting)
            {
                UpdatePositonAndRotate();
                return;
            }
            //Focus Function Click to calculate the focus position
            if (input.GetKeyDown(focusKey) && targetSelect != null)
            {
                CalcFocusPositon();
            }
            if (isFocusing)
            {
                Focus();
            }
            else
            {
                //Follow the target
                if (isFoollowTarget&&targetSelect != null)
                {
                    AimOriginPoint.position = targetSelect.position;
                }
                Move();
                Rotation();
                HeightCalculation();
                rotation = Quaternion.Euler(y, x, 0.0f);
                //Move
                Vector3 disVector = new Vector3(0.0f, 0.0f, -targetDistance);
                position = rotation * disVector + AimOriginPoint.position;
               // Debug.DrawLine(AimOriginPoint.position, position,Color.green);
                LinerDistance = Vector3.Distance(transform.position, position + targetOffset);
                LinerAngle = Quaternion.Angle(transform.rotation, rotation);
                if (CameraRotateLerp)
                {
                    transform.position = Vector3.Lerp(transform.position, position + targetOffset, Time.deltaTime * moveLerpSpeed);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateLerpSpeed);
                }
                else
                {
                    //If the rotation button is pressed and the rotation occurs, interpolation is not performed
                    if (input.GetKey(mouseRotationKey)&&transform.rotation!=rotation)
                    {
                        transform.position = position + targetOffset;
                        transform.rotation = rotation;

                    }
                    else
                    {
                        transform.position = Vector3.Lerp(transform.position, position + targetOffset, Time.deltaTime * moveLerpSpeed);
                        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateLerpSpeed);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateLerpSpeed);
                        // 
                    }

                }

            }

            if (!isFocusing)
            {
                LimitPosition();
            }
            
        }

        /// <summary>
        /// Calculate the target point and distance
        /// </summary>
        void CalcAimPosAndDistance()
        {
            if (RayToGround(transform.forward, ref targetPosCalc))
            {
                AimOriginPoint.position = targetPosCalc;
            }
            else
            {
                Debug.LogError("You need to set the ground layer correctly and add Collider, otherwise you can't determine the target point!!  ");
                AimOriginPoint.position = transform.position + transform.forward * minDistance;

            }
            //Restrict the target point to the area
            if (limitMap)
            {
                AimOriginPoint.position = new Vector3(Mathf.Clamp(AimOriginPoint.position.x, center.x - aimlimitX, center.x + aimlimitX),
                    Mathf.Clamp(AimOriginPoint.position.y, aimMinHeight, aimMaxHeight),
                    Mathf.Clamp(AimOriginPoint.position.z, center.z - aimlimitZ, center.z + aimlimitZ));
            }
            targetDistance = Vector3.Distance(transform.position, AimOriginPoint.position);

        }
        void UpdatePositonAndRotate()
        {
            transform.position = Vector3.Lerp(transform.position, setPostion, Time.deltaTime * moveLerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(setRotate), Time.deltaTime * rotateLerpSpeed);
            if (Vector3.Distance(transform.position, setPostion) <= 0.01f && Quaternion.Angle(transform.rotation, Quaternion.Euler(setRotate)) <= 0.05f)
            {
                ResetCameraTransfomCompleteAction?.Invoke();
                transform.position = setPostion;
                transform.rotation = Quaternion.Euler(setRotate);
                isSetting = false;
                Vector3 angles = transform.eulerAngles;
                x = angles.y;
                y = angles.x;
                ResetTarget();
                isFocusing = false;
                CalcAimPosAndDistance();


            }
        }
        /// <summary>
        /// Calc Focus 
        /// </summary>
        void CalcFocusPositon()
        {
            Bounds b = CalculateBounds(targetSelect.gameObject);
            Vector3 max = b.size;
            float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));
            transform.rotation = rotation;
            Vector3 disVector = new Vector3(0.0f, 0.0f, -targetDistance);
            var pos = rotation * disVector + targetSelect.position;
            //   float fov = m_camera.fieldOfView * Mathf.Deg2Rad;
            //  distance = Mathf.Abs(radius / Mathf.Sin(fov / 2.0f));
            distance = Vector3.Distance(pos, targetSelect.position);
            juliCalc = distance - radius * targetWeight;
            targetPos = pos + transform.forward * juliCalc;
            isFocusing = true;
            if (FocusStartAction != null)
            {
                FocusStartAction.Invoke(targetSelect.gameObject);
            }
        }
        /// <summary>
        /// MoveCamera
        /// </summary>
        private void Move()
        {
            if (useKeyboardInput)
            {
                Vector3 desiredMove = new Vector3(KeyboardInput.x, 0, KeyboardInput.y);
                desiredMove *= keyboardMovementSpeed * Time.deltaTime;
                desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                AimOriginPoint.position += desiredMove;
                if (limitMap)
                {
                    AimOriginPoint.position = new Vector3(Mathf.Clamp(AimOriginPoint.position.x, center.x - aimlimitX, center.x + aimlimitX),
                        Mathf.Clamp(AimOriginPoint.position.y, aimMinHeight, aimMaxHeight),
                        Mathf.Clamp(AimOriginPoint.position.z, center.z - aimlimitZ, center.z + aimlimitZ));
                }
                if (KeyboardInput.x != 0 || KeyboardInput.y != 0)
                {
                    ResetTarget();
                }
            }
            if (useScreenEdgeInput)
            {
                if (!input.IsAnyKeyDown())
                {
                    Vector3 desiredMove = new Vector3();
                    Rect leftRect = new Rect(0, 0, screenEdgeBorder, Screen.height);
                    Rect rightRect = new Rect(Screen.width - screenEdgeBorder, 0, screenEdgeBorder, Screen.height);
                    Rect upRect = new Rect(0, Screen.height - screenEdgeBorder, Screen.width, screenEdgeBorder);
                    Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorder);
                    desiredMove.x = leftRect.Contains(input.GetPositon()) ? -1 : rightRect.Contains(input.GetPositon()) ? 1 : 0;
                    desiredMove.z = upRect.Contains(input.GetPositon()) ? 1 : downRect.Contains(input.GetPositon()) ? -1 : 0;
                    desiredMove *= screenEdgeMovementSpeed;
                    desiredMove *= Time.deltaTime;
                    desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                    AimOriginPoint.position += desiredMove;
                    if (limitMap)
                    {
                        AimOriginPoint.position = new Vector3(Mathf.Clamp(AimOriginPoint.position.x, center.x - aimlimitX, center.x + aimlimitX),
                            Mathf.Clamp(AimOriginPoint.position.y, aimMinHeight, aimMaxHeight),
                            Mathf.Clamp(AimOriginPoint.position.z, center.z - aimlimitZ, center.z + aimlimitZ));
                    }
                    if (desiredMove.x != 0 || desiredMove.z != 0)
                    {
                        ResetTarget();
                    }
                }

            }
            if (usePanning && input.GetKey(panningKey))
            {
                Vector3 desiredMove = new Vector3(-MouseAxis.x, 0, -MouseAxis.y);
                if (adaptivepanningSpeed)
                {
#if UNITY_EDITOR
                    panningSpeed = targetDistance * 2;
#else
                    panningSpeed = targetDistance * 4;
#endif
                    panningSpeed = Mathf.Clamp(panningSpeed, 2, maxDistance);
                }
                if (isCheckUI)
                {
                    if (input.IsPointerOverUIObject())
                    {
                        desiredMove *= 0;
                    }
                    else
                    {
                        desiredMove *= panningSpeed;
                    }
                }
                else
                {
                    desiredMove *= panningSpeed;
                }
                desiredMove *= Time.deltaTime;
                desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                AimOriginPoint.position += desiredMove;
                if (limitMap)
                {
                    AimOriginPoint.position = new Vector3(Mathf.Clamp(AimOriginPoint.position.x, center.x - aimlimitX, center.x + aimlimitX),
                        Mathf.Clamp(AimOriginPoint.position.y, aimMinHeight, aimMaxHeight),
                        Mathf.Clamp(AimOriginPoint.position.z, center.z - aimlimitZ, center.z + aimlimitZ));
                }
                if (desiredMove.magnitude!=0)
                {
                    ResetTarget();
                }
               
            }
        }

        /// <summary>
        /// Height/Scale
        /// </summary>
        private void HeightCalculation()
        {
            if (useScrollwheelZooming)
            {
                float targetSpeed = 0;
                if (adaptiveScrollWheelSpeed)
                {
                    targetSpeed = ScrollWheel * Mathf.Clamp(targetDistance,1,maxDistance);
                   
                }
                else
                {
                    targetSpeed = ScrollWheel * scrollWheelZoomingSensitivity;
                }
             
                if (isCheckUI)
                {
                    if (input.IsPointerOverUIObject())
                    {
                        targetSpeed = 0;
                    }
                    else
                    {
                        targetDistance -= targetSpeed;
                    }
                }
                else
                {
                    targetDistance -= targetSpeed;
                }
                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            }
            if (useKeyboardZooming)
            {
                zoomPos = ZoomDirection * keyboardZoomingSensitivity * Time.deltaTime;
                if (zoomPos != 0)
                {
                    ResetTarget();
                }
                AimOriginPoint.position += new Vector3(0, zoomPos, 0);
                if (limitMap)
                {
                    //Clamp Aim 
                    AimOriginPoint.position = new Vector3(Mathf.Clamp(AimOriginPoint.position.x, center.x - aimlimitX, center.x + aimlimitX),
                        Mathf.Clamp(AimOriginPoint.position.y, aimMinHeight, aimMaxHeight),
                        Mathf.Clamp(AimOriginPoint.position.z, center.z - aimlimitZ, center.z + aimlimitZ));
                }
            }

        }

        /// <summary>
        ///RotateCamera
        /// </summary>
        private void Rotation()
        {
            if (IsSelfRotate)
            {
                if (useKeyboardRotation)
                {
                    transform.Rotate(Vector3.up, RotationDirection * Time.deltaTime * keyboardRotationSpeed, Space.World);
                    if (RotationDirection != 0)
                    {
                        ResetTarget();
                    }
                }

                if (useMouseRotation && input.GetKey(mouseRotationKey))
                {
                    transform.Rotate(Vector3.up, -MouseAxis.x * Time.deltaTime * mouseRotationSpeed, Space.World);
                    ResetTarget();
                }
            }
            else
            {
                if (useMouseRotation && input.GetKey(mouseRotationKey))
                {
                    if (isCheckUI)
                    {
                        if (input.IsPointerOverUIObject())
                        {
                            x += 0;
                            y -= 0;
                        }
                        else
                        {
                            x += MouseAxis.x * mouseRotationSpeed * 0.02f;
                            y -= MouseAxis.y * mouseRotationSpeed * 0.02f;
                        }
                    }
                    else
                    {
                        x += MouseAxis.x * mouseRotationSpeed * 0.02f;
                        y -= MouseAxis.y * mouseRotationSpeed * 0.02f;
                    }

                }
                if (IsFixedXAngle)
                {
                    y = xAngle;
                }
                if (isClampXAngle)
                {
                    y = ClampAngle(y, clampMinXAngle, clampMaxXAngle);
                }

            }

        }

        /// <summary>
        /// Calculate the boundary size of the object
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        Bounds CalculateBounds(GameObject go)
        {
            Bounds b = new Bounds(go.transform.position, Vector3.zero);
            UnityEngine.Object[] rList = go.GetComponentsInChildren(typeof(Renderer));
            foreach (Renderer r in rList)
            {
                b.Encapsulate(r.bounds);
            }
            return b;
        }
        /// <summary>
        /// Focus
        /// </summary>
        void Focus()
        {
            if (targetSelect != null)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * focusSpeed);
                if (Vector3.Distance(transform.position, targetPos) <= 0.01f)
                {
                    transform.position = targetPos;
                    targetDistance = Vector3.Distance(transform.position, targetSelect.position);
                    isFocusing = false;
                    if (FocusEndAction != null)
                    {
                        FocusEndAction.Invoke(targetSelect.gameObject);
                    }
                }
            }
        }
        /// <summary>
        /// Limit the camera area to X and Z axes
        /// </summary>
        private void LimitPosition()
        {
            if (!limitMap)
                return;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, center.x - limitX, center.x + limitX),
                Mathf.Clamp(transform.position.y, minHeight, maxHeight),
                Mathf.Clamp(transform.position.z, center.z - limitZ, center.z + limitZ));
        }
        /// <summary>
        /// Ray Test Ground
        /// </summary>
        /// <returns></returns>
        private bool RayToGround(Vector3 dir, ref Vector3 point)
        {
            Ray ray = new Ray(transform.position, dir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask.value))
            {
                point = hit.point;
                return true;
            }
            return false;
        }
        //Limited rotation Angle
        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }



        /// <summary>
        /// SetTarget
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Transform target)
        {
            //Prevents repeated selection of the current object  
            if (target==targetSelect)
            {
               return; 
            }
            if (target.GetComponent<ParameterConfiguration>() != null)
            {
                 parameter = target.GetComponent<ParameterConfiguration>();
                scrollWheelZoomingSensitivity = parameter.scrollWheelZoomingSensitivity;
                maxDistance = parameter.maxDistance;
                minDistance = parameter.minDistance;
                targetWeight = parameter.targetWeight;
                mouseRotationSpeed = parameter.mouseRotationSpeed;
                targetDistance = parameter.targetDistance;
                if (parameter.isSettingRotate)
                {
                    x = parameter.currentRotate.y;
                    y = parameter.currentRotate.x;
                }
            }
            targetSelect = target;
            if (SelectTargetAction != null)
            {
                SelectTargetAction.Invoke(target.gameObject);
            }
            AimOriginPoint.position = targetSelect.position;


        }

        /// <summary>
        ///ResetTarget
        /// </summary>
        public void ResetTarget()
        {
            targetSelect = null;
            parameter = null;
            isFocusing = false;
            ResetParameters();
            if (ResetTargetAction != null)
            {
                ResetTargetAction();
            }
        }

        /// <summary>
        /// Set camera position and rotation
        /// </summary>
        /// <param name="positon"></param>
        /// <param name="rotation"></param>
        public void SetCameraPostionRotation(Vector3 positon, Vector3 rotation)
        {
            this.setPostion = new Vector3(Mathf.Clamp(positon.x, center.x - limitX, center.x + limitX), Mathf.Clamp(positon.y, minHeight, maxHeight), Mathf.Clamp(positon.z, center.z - limitZ, center.z + limitZ));

            if (IsFixedXAngle)
            {
                this.setRotate = new Vector3(xAngle, rotation.y, rotation.z);
            }else if (isClampXAngle)
            {
                if (rotation.x<clampMinXAngle||rotation.x>clampMaxXAngle)
                {
                    Debug.LogError("Outside the X-axis Angle limit:"+rotation.x);
                }
                this.setRotate = new Vector3(ClampAngle(rotation.x, clampMinXAngle, clampMaxXAngle), rotation.y, rotation.z);
            }
            else
            {
                this.setRotate = new Vector3(rotation.x, rotation.y, rotation.z);
            }

           
            isSetting = true;
        }

        private void OnDestroy()
        {
            if (AimOriginPoint.gameObject != null)
            {
                Destroy(AimOriginPoint.gameObject);
            }
        }
    }
}




