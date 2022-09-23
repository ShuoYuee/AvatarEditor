using System.Collections.Generic;
using ccU3DEngine;
using UnityEngine;


/// <summary>
/// 多層菜單
/// </summary>
/// 增加多個子層設定
public class ScrollDlg_Multilayer : ScrollDlg
{
    /// <summary>多層Item</summary>
    protected class MultilayerItem
    {
        public int iClass;
        public GridPageView gpView;
        public List<GridItemDT> _aGridItem = new List<GridItemDT>();

        public MultilayerItem(int _iClass, GridPageView gp)
        {
            iClass = _iClass;
            gpView = gp;
        }

        public void f_Create(int iMaxSelNum, ccCallback CallBack_OnClick, int iItemWidth, int iItemHeight, int rows, int columns, float gapH, float gapV, bool isHorizontal, bool bLockDrag)
        {
            if (_aGridItem.Count == 0)
            {
                _aGridItem = null;
                MessageBox.DEBUG("列表未装入数据，请用f_AddScrollItem装入数据：" + gpView.name);
            }
            gpView.f_Init(iMaxSelNum, -99, _aGridItem, iItemWidth, iItemHeight, rows, columns, CallBack_OnClick, isHorizontal, gapH, gapV, bLockDrag);
        }

        public void f_Reset()
        {
            gpView.f_Reset();
        }
    }
    /// <summary>子層清單</summary>
    protected List<MultilayerItem> _aGridPagesList = new List<MultilayerItem>();
    /// <summary>當前子層</summary>
    protected GridPageView _CurItem = null;
    //---------------------------------------------------------

    #region 頁面功能
    public override void f_Reset()
    {
        base.f_Reset();
        for (int i = 0; i < _aGridPagesList.Count; i++)
        {
            _aGridPagesList[i].f_Reset();
        }
    }

    /// <summary>
    /// 增加子層
    /// </summary>
    /// <param name="iClass">子層ID</param>
    /// <param name="tGridPage">Page組件</param>
    public virtual void f_AddPagesItem(int iClass, GridPageView tGridPage)
    {
        _aGridPagesList.Add(new MultilayerItem(iClass, tGridPage));
    }

    /// <summary>
    /// 增加子層項目
    /// </summary>
    /// <param name="iPageClass">子層ID</param>
    /// <param name="iId">項目ID</param>
    /// <param name="strName">項目名稱</param>
    /// <param name="strBundle">項目Bundle來源</param>
    /// <param name="strRes">項目來源</param>
    /// <param name="iClass">頁碼</param>
    /// <param name="Obj">項目資料</param>
    public virtual void f_AddPagesScrollItem(int iPageClass, long iId, string strName, string strBundle, string strRes, int iClass, object Obj)
    {
        List<GridItemDT> tGridPageView = _aGridPagesList.Find(delegate (MultilayerItem Item)
        {
            if (Item.iClass == iPageClass) { return true; }
            return false;
        })._aGridItem;

        GridItemDT tGridItemDT = new GridItemDT();
        tGridItemDT.f_AddScrollItem(iId, strName, strBundle, strRes, iClass, Obj);
        tGridPageView.Add(tGridItemDT);
    }

    /// <summary>
    /// 開啟子層頁面
    /// </summary>
    /// <param name="iPageClass">子層ID</param>
    /// <param name="iClass">頁碼</param>
    public virtual void f_SortPage(int iPageClass, int iClass)
    {
        f_SetPage(false);
        GridPageView tGridPageView = _aGridPagesList.Find(delegate (MultilayerItem Item)
        {
            if (Item.iClass == iPageClass) { return true; }
            return false;
        }).gpView;

        tGridPageView.f_Sort(iClass, true);
        _CurItem = tGridPageView;
        f_SetPage(true);
    }

    public override void f_Sort(int iClass)
    {
        base.f_Sort(iClass);
        f_SetScroll(true);
        f_SetPage(false);
    }

    public override void f_Create(int iMaxSelNum, ccCallback CallBack_OnClick, int iItemWidth, int iItemHeight, int rows, int columns, float gapH, float gapV, bool isHorizontal, bool bLockDrag)
    {
        base.f_Create(iMaxSelNum, CallBack_OnClick, iItemWidth, iItemHeight, rows, columns, gapH, gapV, isHorizontal, bLockDrag);
        for (int i = 0; i < _aGridPagesList.Count; i++)
        {
            _aGridPagesList[i].f_Create(iMaxSelNum, CallBack_OnClick, iItemWidth, iItemHeight, rows, columns, gapH, gapV, isHorizontal, bLockDrag);
        }
    }

    public virtual void f_CreatePage(int iPageClass, int iMaxSelNum, ccCallback CallBack_OnClick, int iItemWidth, int iItemHeight, int rows, int columns, float gapH, float gapV, bool isHorizontal, bool bLockDrag)
    {
        MultilayerItem tMultilayerItem = _aGridPagesList.Find(delegate (MultilayerItem Item)
        {
            if (Item.iClass == iPageClass) { return true; }
            return false;
        });

        tMultilayerItem.f_Create(iMaxSelNum, CallBack_OnClick, iItemWidth, iItemHeight, rows, columns, gapH, gapV, isHorizontal, bLockDrag);
    }

    public override void CreateTitle()
    {
        if (_aScrollTitle.Count == 0)
        {
            return;
        }

        Vector3 Pos = m_oBtnMenuModel.transform.position;
        for (int i = 0; i < _aScrollTitle.Count; i++)
        {
            GameObject oBtn = GameObject.Instantiate(m_oBtnMenuModel);
            oBtn.name = _aScrollTitle[i].m_strTitle;

            oBtn.transform.SetParent(m_MenuParent.transform);
            Pos.x = Pos.x + 100;
            oBtn.transform.position = Pos;
            oBtn.transform.localScale = Vector3.one;

            ccButton tccButton = oBtn.GetComponent<ccButton>();
            tccButton.f_Init(null, _aScrollTitle[i].m_strTitle);
            _dirButton.Add(_aScrollTitle[i].m_iClass, tccButton);
            ccUIEventListener.Get(oBtn).f_RegClick(CallBack_OnTitleClick, _aScrollTitle[i], null, "");

            //啟用菜單圖片設置
            if (_UseMenuSprite)
            {
                tccButton.f_Init(glo_Main.GetInstance().m_ResourceManager.f_CreateIcon(LogicUI_Const.AvatarAB, _aScrollTitle[i].m_strTitle));
            }
        }
        m_oBtnMenuModel.SetActive(false);

        if (_UseMenuScroll)
        {
            //啟用菜單滾輪功能
            if (_UseMenuScroll)
            {
                RectTransform MenuParent = (RectTransform)m_MenuParent.transform;
                RectTransform Top_TitleRect = (RectTransform)_dirButton[_aScrollTitle[0].m_iClass].transform;

                float fPos = Top_TitleRect.sizeDelta.y + 100;
                fPos *= _aScrollTitle.Count;
                MenuParent.sizeDelta = new Vector2(100, 100);
                MenuParent.sizeDelta += new Vector2(0, fPos);
            }
        }
    }

    /// <summary>
    /// 開關當前子層
    /// </summary>
    /// <param name="bSet">開關</param>
    public virtual void f_SetPage(bool bSet)
    {
        if (_CurItem != null)
        {
            _CurItem.gameObject.SetActive(bSet);
        }
    }

    /// <summary>
    /// 開關指定子層
    /// </summary>
    /// <param name="iClass">子層ID</param>
    /// <param name="bSet">開關</param>
    public virtual void f_SetPage(int iClass, bool bSet)
    {
        GridPageView tGridPageView = _aGridPagesList.Find(delegate (MultilayerItem Item)
        {
            if (Item.iClass == iClass) { return true; }
            return false;
        }).gpView;
        tGridPageView.gameObject.SetActive(bSet);
    }
    #endregion

    public override void CallBack_OnTitleClick(GameObject go, object obj1, object obj2)
    {//設定特殊頁面，有特殊需求則增加if
        ScrollTitle tScrollTitle = (ScrollTitle)obj1;
        if (tScrollTitle.m_iClass == (int)GameEM.EM_RolePart.Deformer)
        {//若為變形，則直接開啟變形菜單
            f_SetScroll(false);
            f_SortPage(2, -99);
        }
        else
        {
            f_Sort(tScrollTitle.m_iClass);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DeformerBtn);
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_MenuBtn, tScrollTitle.m_iClass);
    }
}