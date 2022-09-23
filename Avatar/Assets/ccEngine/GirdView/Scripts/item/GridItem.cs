using UnityEngine;
using ccU3DEngine;

/// <summary>
/// 列表物件基础
/// </summary>
public class GridItem : MonoBehaviour
{
    protected GridItemDT _GridItemDT;
    public ccButton m_ccButton;

    void Awake()
    {
        InitComponent();
        InitMessage();
        m_ccButton.f_ShowBG();
    }

    public GridItemDT f_GetGridItemDT()
    {
        return _GridItemDT;
    }

    void InitMessage()
    {
        //ccUIEventListener.Get(this.gameObject).f_RegClick(GOClickHandle, null, null, "");
    }
      
    protected virtual void InitComponent()
    {
        
    }
    
    ///// <summary>
    ///// 点击Item处理
    ///// </summary>
    ///// <param name="go"></param>
    //private void GOClickHandle(GameObject go, object obj1, object obj2)
    //{
    //    //OnClick(_GridItemDT);
    //    _GridItemDT.f_BeClick();
    //}

    public virtual void f_UpdateUI(GridItemDT tGridItemDT)
    {
        _GridItemDT = tGridItemDT;       
    }

    public virtual void f_Show()
    {
        if (_GridItemDT == null)
        {
            m_ccButton.f_UnShow();
        }
        else
        {
            m_ccButton.f_Show();
        }
    }

    public virtual void f_UnShow()
    {
        _GridItemDT = null;
        m_ccButton.f_UnShow();
    }

    public virtual void f_Select()
    {
        m_ccButton.f_Select();
    }

    public virtual void f_UnSelect()
    {
        m_ccButton.f_UnSelect();
    }

    //protected virtual void OnClick(GridItemDT tGridItemDT)
    //{

    //}

}
