using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

/// <summary>
/// 時裝ScrollDlg
/// </summary>
public class AvatarScrollDlg : AnimScrollDlg
{
    /// <summary>返回鍵</summary>
    public ccButton _BtnMenuExit;
    /// <summary>返回上頁處理</summary>
    protected AnimBackPage _BackPage;

    protected virtual void Awake()
    {
        _BackPage = new AnimBackPage(this);
        if (_BtnMenuExit != null)
        {
            ccUIEventListener.Get(_BtnMenuExit.gameObject).f_RegClick(f_BackPage, null, null, "");
            _BtnMenuExit.f_UnShowBG();
        }
        f_InitMessage();
    }

    protected virtual void f_InitMessage()
    {
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_ReturnMenuBtn, CallBack_OnClickReturnMenuBtn);
    }

    public override void f_Reset()
    {
        base.f_Reset();

        //重置菜單選項(刪除選項)
        m_oBtnMenuModel = null;
        ccMathEx.f_CreateChild(m_MenuParent, 1);
        m_oBtnMenuModel = m_MenuParent.GetComponentInChildren<ccButton>().gameObject;
    }

    public override void CallBack_OnTitleClick(GameObject go, object obj1, object obj2)
    {
        if (_CurSelButton)
        {//若已選取該菜單項目，則不做動作
            if (_CurSelButton == go.GetComponentInChildren<ccButton>()) { return; }
        }
        base.CallBack_OnTitleClick(go, obj1, obj2);
    }

    /// <summary>清單動畫播放結束的回調</summary>
    public override void CallBack_PanelAnimEnd(object Obj)
    {
        base.CallBack_PanelAnimEnd(Obj);
        ScrollTitle tScrollTitle = (ScrollTitle)Obj;
        _dirButton[tScrollTitle.m_iClass].f_UnHover();
        //設置返回清單
        _BackPage.f_Reset();
        _BackPage.f_AddClass(tScrollTitle.m_iClass);
        if (_BtnMenuExit != null)
        {
            _BtnMenuExit.f_UnShowBG();
        }
    }

    public override void f_Sort(int iClass)
    {
        List<GridItem> tList = f_GetCurSelItem();
        //若為子層則設置返回清單
        if (tList.Count > 0)
        {
            if (tList[0].f_GetGridItemDT() == null)
            {
                if (_BtnMenuExit != null)
                {
                    _BtnMenuExit.f_UnShowBG();
                }
            }
            else if (tList[0].f_GetGridItemDT().f_GetMultilayer())
            {
                _BackPage.f_AddClass(tList[0].f_GetGridItemDT().f_GetClass(), true);
            }
        }
        base.f_Sort(iClass);
    }

    /// <summary>若點選子層則出現返回鍵</summary>
    protected void CallBack_OnClickReturnMenuBtn(object obj)
    {
        if (_BtnMenuExit != null)
        {
            _BtnMenuExit.f_ShowBG();
        }
    }

    /// <summary>返回上頁</summary>
    protected void f_BackPage(GameObject go, object obj1, object obj2)
    {
        _BackPage.f_BackPage();
        if (_BtnMenuExit != null)
        {
            _BtnMenuExit.f_UnShowBG();
        }
    }
}
