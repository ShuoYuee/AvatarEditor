using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SR
{
    public enum FingerType
    {
        SingleFingerTranslation,//单指平移
        TwoFingerTranslation   //双指平移
    }

    public enum FingerState
    {
        SingleFinger,//单指状态
        TwoFinger,//双指状态
        Default  //默认状态
    }

    /// <summary>
    /// MobileInput
    /// </summary>
    public class MobileInput : BaseInput
    {
        public FingerState fingerState = FingerState.Default;

        public override Vector2 GetTouchPostion(int touchID)
        {
            return GetTouch(touchID).position;
        }

        public override int GetTouchCount()
        {
            return Input.touchCount;
        }

        public override Touch GetTouch(int touchID)
        {
            if (GetTouchCount() >= 1 && GetTouchCount() > touchID)
            {
                return Input.GetTouch(touchID);
            }
            Debug.LogError("There is no current Touch subscript out of bounds:" + touchID);
            return default;
        }

        public override Vector2 GetTouchDetal(int touchID)
        {
            return GetTouch(touchID).deltaPosition;
        }
        /// <summary>
        /// Long press the gesture to determine
        /// </summary>
        /// <param name="touchID"></param>
        /// <returns></returns>
        public override bool IsLongTouch()
        {
            if (GetTouchCount() == 1)
            {
                Touch touch = GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    fingerState = FingerState.SingleFinger;
                    LastTime = Time.realtimeSinceStartup;
                    isLongTap = false;
                    //Determine if the touch point is on the UI
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        // Debug.Log("处于UI上");
                        isOverUI = true;
                    }
                    else
                    {
                        // Debug.Log("不处于UI上");
                        isOverUI = false;
                    }

                }
                else if (!isOverUI && !isLongTap && touch.phase == TouchPhase.Stationary && Time.realtimeSinceStartup - LastTime >= longTime)
                {
                    isLongTap = true;
                    LastTime = Time.realtimeSinceStartup;
                    return true;
                }
                else if (!isOverUI && touch.phase == TouchPhase.Moved)
                {
                    isLongTap = true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tap
        /// </summary>
        /// <returns></returns>
        public override bool IsTap()
        {
            if (GetTouchCount() == 1)
            {
                Touch touch = GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    fingerState = FingerState.SingleFinger;
                    LastTime = Time.realtimeSinceStartup;
                    originPos = touch.position;
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        // Debug.Log("处于UI上");
                        isOverUI = true;
                    }
                    else
                    {
                        // Debug.Log("不处于UI上");
                        isOverUI = false;
                    }

                }
                if (!isOverUI && Vector2.Distance(touch.position, originPos) <= 0.1f && touch.phase == TouchPhase.Ended && Time.realtimeSinceStartup - LastTime <= tapTime)
                {
                    LastTime = Time.realtimeSinceStartup;
                    return true;

                }
            }
            return false;
        }

        /// <summary>
        /// DoubleClick
        /// </summary>
        /// <returns></returns>
        public override bool IsDoubleClick()
        {
            if (GetTouchCount() > 0 && GetTouch(0).phase == TouchPhase.Began)
            {
                T2 = Time.realtimeSinceStartup;
                if (IsPointerOverUIObject())
                {
                    // Debug.Log("在UI上");
                }
                else
                {
                    //Debug.Log("当前没有触摸在UI上");
                    if (T2 - T1 < doubleClickTime)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                T1 = T2;
            }
            return false;
        }
    }

}

