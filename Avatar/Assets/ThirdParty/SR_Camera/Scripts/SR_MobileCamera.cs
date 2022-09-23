using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace SR
{
    [RequireComponent(typeof(Camera))]
    public class SR_MobileCamera : MonoBehaviour
    {
        public int lastTab = 0;
        [Header("Object detected Tag")]
        public List<string> targetsTag;
        [Header("target Select")]
        public Transform targetSelect;
        [Header("target Offset")]
        public Vector3 targetOffset = Vector3.zero;
        [Header("is Foollow Target")]
        public bool isFoollowTarget = true;

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



        Vector3 targetPosCalc = Vector3.zero;



        private Camera m_camera;
        public bool useFixedUpdate = false; //use FixedUpdate() or LateUpdate()

        [Header("is Can Roatate")]
        public bool isCanRoatate = true;
        [Header("Rotation Speed")]
        public float RotationSpeed = 2f;


        [SerializeField]
        [Header("SingleFingerTranslation(DoubleFingerRotate)/DoubleTranslation(SingleRotate)")]
        public FingerType fingerType = FingerType.SingleFingerTranslation;

        //move
        [Header("is Can panning")]
        public bool isCanpanning = true;
        [Header("panning Speed")]
        public float panningSpeed = 0.2f;


        //Height
        [Header("is Can Zooming")]
        public bool isCanZooming = true;

        [Header("Zoom Direction Sensitivity")]
        public float ZoomDirectionSensitivity = 10f;
        [Header("Zooming Sensitivity")]
        public float ZoomingSensitivity = 20f;
        private float zoomPos = 0; //value in range (0, 1) used as t in Matf.Lerp

        [Header("Zoom Threshold")]
        public float ZoomThreshold = 30f;//放大缩小手势的最小角度,建议此值限制在30-90度之间 区分两指平移或者旋转



        public LayerMask groundMask = -1;
        public Transform AimOriginPoint;
        [Header("Lerp Damping")]
        public float LerpDamping = 5.0f;
        [Header("Scene center")]
        public Vector3 center = Vector3.zero;
        [Header("Show Visual Areas")]
        public bool ShowVisualAreas = true;
        [Header("limit Map")]
        public bool limitMap = true;
        [Header("camera max Height")]
        public float maxHeight = 100f;
        [Header("Camera min Height")]
        public float minHeight = 1f;
        [Header("limitX")]
        public float limitX = 100f; //x limit of map
        [Header("limitZ")]
        public float limitZ = 100f; //z limit of map
        [Header("aim limit X")]
        public float aimlimitX = 100f; //x limit of map
        [Header("aim limit Z")]
        public float aimlimitZ = 100f; //z limit of map
        [Header("aim Max Height")]
        public float aimMaxHeight = 100f;
        [Header("aim Min Height")]
        public float aimMinHeight = 0f;




        public bool isCanSetCamera = true;
        public float setSpeed = 5;
        private Vector3 setPostion;
        private Vector3 setRotate;
        private bool isSetting = false;


        //default
        private float RecordZoomingSensitivity;
        public float RecordmaxDistance = 100f;
        public float RecordminDistance = 2f;
        public float RecordtargetWeight = 5;
        public float RecordRotationSpeed = 200f;
        public float RecordtargetDistance = 10f;

        public bool FollowingTarget
        {
            get
            {
                return targetSelect != null;
            }
        }


        [Header("Is Fixed X Angle")]
        public bool IsFixedXAngle = true;
        public float xAngle = 45f;

        [Header("is Clamp X Angle")]
        public bool isClampXAngle = true;
        public float clampMinXAngle = 0;
        public float clampMaxXAngle = 90;


        private float x;
        private float y;
        private Vector3 position;

        public Action<GameObject> SelectTargetAction;
        public Action ResetTargetAction;
        public Action<GameObject> FocusStartAction;
        public Action<GameObject> FocusEndAction;
        public Action ResetCameraTransfomCompleteAction;

        #region Input
        private int ZoomDirection = 0;
        [HideInInspector]
        public MobileInput input;
        private Vector2 oldTouch1;  //上次触摸点1(手指1)  
        private Vector2 oldTouch2;  //上次触摸点2(手指2)  
        private Touch newTouch1;
        private Touch newTouch2;


        private Vector2 OneTouchAxis
        {
            get
            {
                if (input.GetTouchCount() == 1)
                {
                    if (input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        input.fingerState = FingerState.SingleFinger;
                    }
                    return input.GetTouchDetal(0);
                }
                else
                {
                    return Vector2.zero;
                }

            }
        }

        private Vector2 TwoTouchAxis = Vector2.zero;
        private float ZoomSensitivity = 0;
        /// <summary>
        /// Two FingerCalc
        /// </summary>

        void TwoFingerCalc()
        {
            if (input.GetTouchCount() == 2)
            {
                newTouch1 = input.GetTouch(0);
                newTouch2 = input.GetTouch(1);
                if (newTouch1.phase == TouchPhase.Began || newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2.position;
                    oldTouch1 = newTouch1.position;
                    input.fingerState = FingerState.TwoFinger;
                }
                else if (newTouch2.phase == TouchPhase.Moved || newTouch1.phase == TouchPhase.Moved)
                {
                    input.fingerState = FingerState.TwoFinger;
                    if (Vector2.Angle(newTouch1.position - oldTouch1, newTouch2.position - oldTouch2) < ZoomThreshold)
                    {
                        TwoTouchAxis = (newTouch1.deltaPosition + newTouch2.deltaPosition) / 2;//delta
                    }
                    else
                    {
                        //zoom 
                        float oldDistance = Vector2.Distance(oldTouch1, oldTouch2);//计算两点距离
                        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);//计算两点距离
                        ZoomSensitivity = newDistance - oldDistance;
                    }
                }
                else
                {
                    TwoTouchAxis = Vector2.zero;
                    ZoomSensitivity = 0;
                }
                oldTouch1 = newTouch1.position;
                oldTouch2 = newTouch2.position;
            }
            else
            {
                TwoTouchAxis = Vector2.zero;
                ZoomSensitivity = 0;
            }
        }

        #endregion




        #region Unity_Methods

        private void Start()
        {
            

            input = new MobileInput();
            m_camera = GetComponent<Camera>();
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
            GameObject go = new GameObject("AimPoint");
            AimOriginPoint = go.transform;
            CalcAimPosAndDistance();
            RecordZoomingSensitivity = ZoomingSensitivity;
            RecordmaxDistance = maxDistance;
            RecordminDistance = minDistance;
            RecordtargetWeight = targetWeight;
            RecordRotationSpeed = RotationSpeed;
        }
        void ResetParameters()
        {
            ZoomingSensitivity = RecordZoomingSensitivity;
            RotationSpeed = RecordRotationSpeed;
            maxDistance = RecordmaxDistance;
            minDistance = RecordminDistance;
            targetWeight = RecordtargetWeight;
            targetDistance = RecordtargetDistance;

        }
        void Update()
        {
            if (input.IsTap())
            {
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (targetsTag.Contains(hit.transform.tag))
                        SetTarget(hit.transform);
                    else
                        ResetTarget();
                }
            }
            if (input.IsLongTouch())
            {
                Debug.Log("long Touch");
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
        ///Update
        /// </summary>
        private void CameraUpdate()
        {
            if (input.GetTouchCount() == 0)
            {
                input.fingerState = FingerState.Default;
            }
            TwoFingerCalc();
            if (isCanSetCamera && isSetting)
            {
                UpdatePositonAndRotate();
                return;
            }
      
            if (isFocusing)
            {
                Focus();
            }
            else
            { 
                if (isFoollowTarget&&targetSelect != null)
                {
                    AimOriginPoint.position = targetSelect.position;
                }
                Move();
                Rotation();
                HeightCalculation();
                Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
             
                //Move
                Vector3 disVector = new Vector3(0.0f, 0.0f, -targetDistance);
                position = rotation * disVector + AimOriginPoint.position;

                //if (transform.rotation != rotation)
                //{
                //    transform.position = position + targetOffset;
                //    transform.rotation = rotation;
                //}
                //else
                //{
                //    transform.position = Vector3.Lerp(transform.position, position + targetOffset, Time.deltaTime * LerpDamping);
                //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * LerpDamping);
                //    // 
                //}
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * LerpDamping);
                transform.position = Vector3.Lerp(transform.position, position + targetOffset, Time.deltaTime * LerpDamping);
            }
            if (!isFocusing)
            {
                LimitPosition();
            }
        }

        void UpdatePositonAndRotate()
        {
            transform.position = Vector3.Lerp(transform.position, setPostion, Time.deltaTime * setSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(setRotate), Time.deltaTime * setSpeed);
            if (Vector3.Distance(transform.position, setPostion) <= 0.01f && Vector3.Angle(transform.eulerAngles, setRotate) <= 0.05f)
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

        void CalcFocusPositon()
        {
            Bounds b = CalculateBounds(targetSelect.gameObject);
            Vector3 max = b.size;
            float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));
            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            Vector3 disVector = new Vector3(0.0f, 0.0f, -targetDistance);
            var position = rotation * disVector + targetSelect.position;
            //   float fov = m_camera.fieldOfView * Mathf.Deg2Rad;
            //  distance = Mathf.Abs(radius / Mathf.Sin(fov / 2.0f));
            distance = Vector3.Distance(position, targetSelect.position);
            juliCalc = distance - radius * targetWeight;
            targetPos = position + transform.forward * juliCalc;
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, center.x - limitX, center.x + limitX),
                Mathf.Clamp(targetPos.y, minHeight, maxHeight),
                Mathf.Clamp(targetPos.z, center.z - limitZ, center.z + limitZ));
            isFocusing = true;
            if (FocusStartAction != null)
            {
                FocusStartAction.Invoke(targetSelect.gameObject);
            }
        }
        /// <summary>
        /// Move 
        /// </summary>
        private void Move()
        {
            if (isCanpanning)
            {
                //如果是单指平移,只有单指状态下才能平移
                if (fingerType == FingerType.SingleFingerTranslation && input.fingerState == FingerState.SingleFinger)
                {
                    if (Mathf.Abs(OneTouchAxis.x) >= float.Epsilon || Mathf.Abs(OneTouchAxis.y) >= float.Epsilon)
                    {
                        Vector3 desiredMove = new Vector3(-OneTouchAxis.x, 0, -OneTouchAxis.y);
                        desiredMove *= panningSpeed;
                        desiredMove *= Time.deltaTime;
                        desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                        AimOriginPoint.position += desiredMove;
                        if (limitMap)
                        {
                            AimOriginPoint.position = new Vector3(Mathf.Clamp(AimOriginPoint.position.x, center.x - aimlimitX, center.x + aimlimitX),
                                Mathf.Clamp(AimOriginPoint.position.y, aimMinHeight, aimMaxHeight),
                                Mathf.Clamp(AimOriginPoint.position.z, center.z - aimlimitZ, center.z + aimlimitZ));
                        }
                        ResetTarget();
                    }
                }//如果是双指平移,只有在双指平移状态下才能执行
                else if (fingerType == FingerType.TwoFingerTranslation && input.fingerState == FingerState.TwoFinger)
                {
                    if (Mathf.Abs(TwoTouchAxis.x) >= float.Epsilon || Mathf.Abs(TwoTouchAxis.y) >= float.Epsilon)
                    {
                        Vector3 desiredMove = new Vector3(-TwoTouchAxis.x, 0, -TwoTouchAxis.y);
                        desiredMove *= panningSpeed;
                        desiredMove *= Time.deltaTime;
                        desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                        AimOriginPoint.position += desiredMove;
                        if (limitMap)
                        {
                            AimOriginPoint.position = new Vector3(Mathf.Clamp(AimOriginPoint.position.x, center.x - aimlimitX, center.x + aimlimitX),
                                Mathf.Clamp(AimOriginPoint.position.y, aimMinHeight, aimMaxHeight),
                                Mathf.Clamp(AimOriginPoint.position.z, center.z - aimlimitZ, center.z + aimlimitZ));
                        }
                        ResetTarget();
                    }
                }
            }
        }
        /// <summary>
        /// Height calc
        /// </summary>
        private void HeightCalculation()
        {

            var targetSpeed = ZoomSensitivity / ZoomingSensitivity;
            targetDistance -= targetSpeed;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

            zoomPos = ZoomDirection * ZoomDirectionSensitivity * Time.deltaTime;
            if (zoomPos != 0)
            {
                ResetTarget();
            }
            AimOriginPoint.position += new Vector3(0, zoomPos, 0);
            if (limitMap)
            {
                AimOriginPoint.position = new Vector3(Mathf.Clamp(AimOriginPoint.position.x, center.x - aimlimitX, center.x + aimlimitX),
                    Mathf.Clamp(AimOriginPoint.position.y, aimMinHeight, aimMaxHeight),
                    Mathf.Clamp(AimOriginPoint.position.z, center.z - aimlimitZ, center.z + aimlimitZ));
            }

        }

        /// <summary>
        ///Camera Rotation
        /// </summary>
        private void Rotation()
        {
            if (isCanRoatate)
            {
                //双指旋转 双指状态
                if (fingerType == FingerType.SingleFingerTranslation && input.fingerState == FingerState.TwoFinger)
                {
                    x += TwoTouchAxis.x * RotationSpeed * 0.02f;
                    y -= TwoTouchAxis.y * RotationSpeed * 0.02f;
                }
                else if (fingerType == FingerType.TwoFingerTranslation && input.fingerState == FingerState.SingleFinger)
                {  //单指旋转,单指状态
                    x += OneTouchAxis.x * RotationSpeed * 0.02f;
                    y -= OneTouchAxis.y * RotationSpeed * 0.02f;
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

        //CalculateBounds
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
        /// Clamp Camera  X  Z
        /// </summary>
        private void LimitPosition()
        {
            if (!limitMap)
                return;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, center.x - limitX, center.x + limitX),
                Mathf.Clamp(transform.position.y, minHeight, maxHeight),
                Mathf.Clamp(transform.position.z, center.z - limitZ, center.z + limitZ));
        }
        //Calc Aim pos
        void CalcAimPosAndDistance()
        {
            if (RayToGround(transform.forward, ref targetPosCalc))
            {
                AimOriginPoint.position = targetPosCalc;
            }
            else
            {
                Debug.LogError("您需要正确设置地面层并添加Colloer,否则无法确定目标点!!!");
                AimOriginPoint.position = transform.position + transform.forward * minDistance;

            }

            if (limitMap)
            {
                AimOriginPoint.position = new Vector3(Mathf.Clamp(AimOriginPoint.position.x, center.x - aimlimitX, center.x + aimlimitX),
                    Mathf.Clamp(AimOriginPoint.position.y, aimMinHeight, aimMaxHeight),
                    Mathf.Clamp(AimOriginPoint.position.z, center.z - aimlimitZ, center.z + aimlimitZ));
            }
            targetDistance = Vector3.Distance(transform.position, AimOriginPoint.position);
        }

        /// <summary>
        /// RayToGround
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
        //ClampAngle
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
            RecordtargetDistance = targetDistance;
            if (target.GetComponent<ParameterConfiguration>() != null)
            {
                var parameter = target.GetComponent<ParameterConfiguration>();
                ZoomingSensitivity = parameter.MobileZoomingSensitivity;
                RotationSpeed = parameter.MobileRotationSpeed;
                maxDistance = parameter.maxDistance;
                minDistance = parameter.minDistance;
                targetWeight = parameter.targetWeight;
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
        ///Reset Target
        /// </summary>
        public void ResetTarget()
        {
            targetSelect = null;
            isFocusing = false;
            ResetParameters();
            if (ResetTargetAction != null)
            {
                ResetTargetAction();
            }
        }

        /// <summary>
        /// Set Camera Postion Rotation
        /// </summary>
        /// <param name="positon"></param>
        /// <param name="rotation"></param>
        public void SetCameraPostionRotation(Vector3 positon, Vector3 rotation)
        {
            this.setPostion = new Vector3(Mathf.Clamp(positon.x, center.x - limitX, center.x + limitX), Mathf.Clamp(positon.y, minHeight, maxHeight), Mathf.Clamp(positon.z, center.z - limitZ, center.z + limitZ));

            if (IsFixedXAngle)
            {
                this.setRotate = new Vector3(xAngle, rotation.y, rotation.z);
            }
            else if (isClampXAngle)
            {
                this.setRotate = new Vector3(ClampAngle(rotation.x, clampMinXAngle, clampMaxXAngle), rotation.y, rotation.z);
            }
            else
            {
                this.setRotate = new Vector3(rotation.x, rotation.y, rotation.z);
            }


            isSetting = true;
        }

        /// <summary>
        /// Focus Target
        /// </summary>
        public void ForcusTarget()
        {
            if (targetSelect != null)
            {
                CalcFocusPositon();
            }

        }
        /// <summary>
        ///Vertical ascent and descent
        /// </summary>
        /// <param name="dir"></param>
        public void SetZoomDirection(int dir)
        {
            this.ZoomDirection = dir;
        }
    }

}

