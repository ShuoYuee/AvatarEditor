using System;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class AnimPageView : PageView
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
            bool bPlay = GameTools.f_PlayAnimator(_ScrollPanelAnimator, "Panel_Out");
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

    public override void CreateTitle(ccCallback ccCallback)
    {
        base.CreateTitle(ccCallback);
        //播放菜單面板淡入動畫
        GameTools.f_PlayAnimator(_MenuPanelAnimator, "Panel_In");
    }

    public override void f_Sort(int iClass)
    {
        //播放選項面板淡入動畫
        GameTools.f_PlayAnimator(_ScrollPanelAnimator, "Panel_In");
        base.f_Sort(iClass);
    }
}
