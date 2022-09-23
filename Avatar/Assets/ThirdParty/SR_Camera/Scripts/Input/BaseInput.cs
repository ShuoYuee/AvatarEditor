using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SR
{
    public class BaseInput : IIput
    {

        public double T1 = 0;
        public double T2 = 0;
        //Minimum double click interval
        public float doubleClickTime = 0.2f;

        public float longTime = 1f;//Long press to determine the time
        public float tapTime = 0.15f;//Click to determine the time
        public float LastTime = 0;//Time of last record
        public Vector2 originPos;
        public bool isLongTap = false;
        public bool isOverUI = false;
        public virtual bool IsAnyKeyDown()
        {
            return Input.anyKeyDown;
        }

        public virtual bool GetKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public virtual bool GetKeyUp(KeyCode key)
        {
            return Input.GetKeyUp(key);
        }

        public virtual bool GetKey(KeyCode key)
        {
            return Input.GetKey(key);
        }

        public virtual bool GetPointerDown(int button)
        {
            return Input.GetMouseButtonDown(button);
        }

        public virtual bool GetPointerUp(int button)
        {
            return Input.GetMouseButtonUp(button);
        }

        public virtual bool GetPointer(int button)
        {
            return Input.GetMouseButton(button);
        }

        public virtual float GetAxis(InputAxis axis)
        {
            switch (axis)
            {
                case InputAxis.X:
                    return Input.GetAxis("Mouse X");
                case InputAxis.Y:
                    return Input.GetAxis("Mouse Y");
                case InputAxis.Z:
                    return Input.GetAxis("Mouse ScrollWheel");
                case InputAxis.Horizontal:
                    return Input.GetAxis("Horizontal");
                case InputAxis.Vertical:
                    return Input.GetAxis("Vertical");
                default:
                    return 0;
            }
        }

        public virtual Vector3 GetPositon()
        {
            return Input.mousePosition;
        }

        public virtual Vector2 GetTouchPostion(int touchID)
        {
            return GetTouch(touchID).position;
        }

        public virtual int GetTouchCount()
        {
            return Input.touchCount;
        }

        public virtual Touch GetTouch(int touchID)
        {
            return default;
        }

        public virtual Vector2 GetTouchDetal(int touchID)
        {
            return GetTouch(touchID).deltaPosition;
        }
        /// <summary>
        /// IsLongTouch
        /// </summary>
        /// <param name="touchID"></param>
        /// <returns></returns>
        public virtual bool IsLongTouch()
        {
            return false;
        }

        public virtual bool IsTap()
        {
            return false;
        }
        /// <summary>
        /// Determine whether the mouse is clicked on the UI
        /// </summary>
        /// <returns></returns>
        public virtual bool IsPointerOverUIObject()
        {
            if (EventSystem.current == null)
            {
                Debug.LogError("Add EvetSystem to the scenario to listen for input events:UI->EventSystem");
                return false;
            }
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

        public virtual bool IsDoubleClick()
        {
            return false;
        }

        /// <summary>
        /// Click on the same location
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual ClickType IsClicksSationary()
        {
            return ClickType.NULL;
        }
    }
}


