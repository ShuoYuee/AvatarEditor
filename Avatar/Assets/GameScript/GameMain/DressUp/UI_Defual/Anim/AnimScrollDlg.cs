using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

/// <summary>
/// 背包格清單_動畫播放版
/// </summary>
/// 處理菜單和清單的動畫播放與回調
/// 增加返回鍵
public class AnimScrollDlg : ScrollDlg
{
    /// <summary>菜單面板動畫機</summary>
    public Animator _MenuPanelAnimator;
    /// <summary>清單面板動畫機</summary>
    public Animator _ScrollPanelAnimator;

    public bool _PlayAnim = true;

    /// <summary>動畫播放事件計時器ID</summary>
    protected int iAnimEndEvent = 0;

    /// <summary>點選菜單回調方法</summary>
    public override void CallBack_OnTitleClick(GameObject go, object obj1, object obj2)
    {
        if (_PlayAnim)
        {
            //播放選項面板淡入動畫，並等待動畫播放完畢再做回調方法
            bool bPlay = GameTools.f_PlayAnimator(_ScrollPanelAnimator, "Panel_Out", 0, true);
            GameTools.f_ReAnimEventTime();
            //開啟動畫偵測事件計時器
            if (bPlay)
            {
                iAnimEndEvent = ccTimeEvent.GetInstance().f_RegEvent(0.02f, 1, obj1, f_RegCall);
            }
            else
            {//如果動畫啟動失敗則直接進行回調
                CallBack_PanelAnimEnd(obj1);
            }
        }
        else
        {
            CallBack_PanelAnimEnd(obj1);
        }
    }

    /// <summary>動畫偵測事件計時</summary>
    public void f_RegCall(object Obj)
    {
        GameTools.OnAnimComplete(_ScrollPanelAnimator, CallBack_PanelAnimEnd, Obj, 0f);
    }

    /// <summary>清單動畫播放結束的回調</summary>
    public virtual void CallBack_PanelAnimEnd(object Obj)
    {
        //取消動畫偵測事件計時器
        ccTimeEvent.GetInstance().f_UnRegEvent(iAnimEndEvent);
        base.CallBack_OnTitleClick(null, Obj, null);

        //增加菜單按鈕回調方法
        ScrollTitle tScrollTitle = (ScrollTitle)Obj;
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_MenuBtn, tScrollTitle.m_iClass);
    }

    public override void CreateTitle()
    {
        base.CreateTitle();
        f_OpenAnim();
    }

    public override void f_Create(int iMaxSelNum, ccCallback CallBack_OnClick, int iItemWidth, int iItemHeight, int rows, int columns, float gapH, float gapV, bool isHorizontal, bool bLockDrag)
    {
        base.f_Create(iMaxSelNum, CallBack_OnClick, iItemWidth, iItemHeight, rows, columns, gapH, gapV, isHorizontal, bLockDrag);
        f_CloseAnim();
    }

    public override void f_Sort(int iClass)
    {
        f_OpenAnim();
        base.f_Sort(iClass);
    }

    public void f_OpenAnim(bool bRePlay = false)
    {
        //播放選項面板淡入動畫
        GameTools.f_PlayAnimator(_ScrollPanelAnimator, "Panel_In", 0.001f, bRePlay);
    }

    public void f_CloseAnim(bool bRePlay = false)
    {
        //播放選項面板淡出動畫
        GameTools.f_PlayAnimator(_ScrollPanelAnimator, "Panel_Out", 0.001f, bRePlay);
    }
}
