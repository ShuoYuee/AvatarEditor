using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace SR
{
    public enum ClickType
    {
        NULL = 0,//没有触发
        OneTap = 1,//单击
        TwoTap = 2,//双击
    }

    /// <summary>
    /// PC端的输入系统,可扩展
    /// </summary>
    public class PCInput : BaseInput
    {

        private ClickType clickType = ClickType.NULL;
        public override bool IsAnyKeyDown()
        {
            return base.IsAnyKeyDown();
        }

        public override bool GetKeyDown(KeyCode key)
        {
            return base.GetKeyDown(key);
        }

        public override bool GetKeyUp(KeyCode key)
        {
            return base.GetKeyUp(key);
        }

        public override bool GetKey(KeyCode key)
        {
            return base.GetKey(key);
        }

        public override bool GetPointerDown(int button)
        {
            return base.GetPointerDown(button);
        }

        public override bool GetPointerUp(int button)
        {
            return base.GetPointerUp(button);
        }

        public override bool GetPointer(int button)
        {
            return base.GetPointer(button);
        }

        public override float GetAxis(InputAxis axis)
        {
            return base.GetAxis(axis);
        }

        public override Vector3 GetPositon()
        {
            return base.GetPositon();
        }

        //左键单机的判定
        public override bool IsTap()
        {
            // Event e = Event.current;
            if (GetPointerDown(0))
            {
                LastTime = Time.realtimeSinceStartup;
                originPos = GetPositon();
            }
            if (GetPointerUp(0) && Vector2.Distance(GetPositon(), originPos) <= 1f && Time.realtimeSinceStartup - LastTime <= tapTime)
            {
              //  Debug.Log("Tap");
                LastTime = Time.realtimeSinceStartup;
                return true;
            }
            return false;
        }

        public override bool IsDoubleClick()
        {
            if (GetPointerDown(0))
            {
                T2 = Time.realtimeSinceStartup;
                if (T2 - T1 < doubleClickTime)
                {
                 //   Debug.Log("DoubleClick");
                    return true;
                }
                T1 = T2;
            }
            return false;
        }

        /// <summary>
        /// Determine whether to click once or twice
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public override ClickType IsClicksSationary()
        {
            if (GetPointerDown(0))
            {
                LastTime = Time.realtimeSinceStartup;
                T2 = Time.realtimeSinceStartup;
                originPos = GetPositon();
            }
            if (!IsPointerOverUIObject() && GetPointerUp(0) && Vector2.Distance(GetPositon(), originPos) <= 1.0f)
            {
                // Debug.Log(T2 - T1);
                if (T2 - T1 < doubleClickTime)
                {
                    clickType = ClickType.TwoTap;
                  //  Debug.Log("DoubleClick");
                }
                else
                {
                    if (Time.realtimeSinceStartup - LastTime <= longTime / 2 && Time.realtimeSinceStartup - LastTime > 0.08f)
                    {
                      //  Debug.Log("Tap");
                        LastTime = Time.realtimeSinceStartup;
                        clickType = ClickType.OneTap;
                    }

                }
                T1 = T2;
                return clickType;
            }
            clickType = ClickType.NULL;
            return clickType;
        }

        public override bool IsLongTouch()
        {
            if (GetPointer(0))
            {
                if (GetPointerDown(0))
                {
                    LastTime = Time.realtimeSinceStartup;
                    originPos = GetPositon();
                    isLongTap = false;
                }
                if (!IsPointerOverUIObject() && !isLongTap && Vector2.Distance(GetPositon(), originPos) <=1.0f && Time.realtimeSinceStartup - LastTime >= longTime)
                {
                   // Debug.Log("LongTouch");
                    isLongTap = true;
                    LastTime = Time.realtimeSinceStartup;
                    return true;
                }
            }
            return false;
        }

    }

}
