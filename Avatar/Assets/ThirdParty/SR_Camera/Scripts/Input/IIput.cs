using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    public enum InputAxis
    {
        X,
        Y,
        Z,
        Horizontal,
        Vertical,
    }
    public interface IIput
    {
        /// <summary>
        /// 任意键按下
        /// </summary>
        /// <returns></returns>
        bool IsAnyKeyDown();
        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool GetKeyDown(KeyCode key);
        /// <summary>
        /// 按键弹起
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool GetKeyUp(KeyCode key);
        /// <summary>
        /// 任意键按住
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool GetKey(KeyCode key);
        /// <summary>
        /// 指定下标的按键按下
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        bool GetPointerDown(int button);
        /// <summary>
        /// 指定下标的按键弹起
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        bool GetPointerUp(int button);
        /// <summary>
        /// 指定下标的按键按住
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        bool GetPointer(int button);
        /// <summary>
        /// 任意轴的值
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        float GetAxis(InputAxis axis);
        /// <summary>
        /// 当前鼠标的位置
        /// </summary>
        /// <returns></returns>
        Vector3 GetPositon();
        /// <summary>
        /// 获取点击的位置
        /// </summary>
        /// <param name="touchID"></param>
        /// <returns></returns>
        Vector2 GetTouchPostion(int touchID);
        /// <summary>
        /// 获取点击的数量
        /// </summary>
        /// <returns></returns>
        int GetTouchCount();
        /// <summary>
        /// 获取当前手指
        /// </summary>
        /// <param name="touchID"></param>
        /// <returns></returns>
        Touch GetTouch(int touchID);
        /// <summary>
        /// 获取当前手指的增量信息
        /// </summary>
        /// <param name="touchID"></param>
        /// <returns></returns>
        Vector2 GetTouchDetal(int touchID);
        /// <summary>
        /// 是否长按
        /// </summary>
        /// <returns></returns>
        bool IsLongTouch();
        /// <summary>
        /// 是否点击
        /// </summary>
        /// <returns></returns>
        bool IsTap();

        bool IsPointerOverUIObject();

        bool IsDoubleClick();

        ClickType IsClicksSationary();

    }
}

