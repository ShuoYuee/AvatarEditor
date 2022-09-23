using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollDlg : MonoBehaviour
{
    protected class ScrollTitle
    {
        public int m_iClass;
        public string m_strTitle;
    }

    public GridPageView gpView;
    GameObject _oScroll;

    protected ccButton _CurSelButton = null;
    protected List<GridItemDT> _aGridItem = new List<GridItemDT>();
    protected List<ScrollTitle> _aScrollTitle = new List<ScrollTitle>();
    protected Dictionary<int, ccButton> _dirButton = new Dictionary<int, ccButton>();

    [Space(10)]
    public GameObject m_MenuParent;
    public GameObject m_oBtnMenuModel;
    /// <summary>菜單間距</summary>
    public float _MenuSpacing = 100;

    [Space(10)]
    /// <summary>是否啟用ID升序</summary>
    public bool _UseSortID = false;
    /// <summary>是否啟用菜單多語言設定</summary>
    public bool _UseMenuLanguage = true;
    /// <summary>是否啟用菜單圖片設定</summary>
    public bool _UseMenuSprite = false;
    /// <summary>是否啟用菜單列表滾動</summary>
    public bool _UseMenuScroll = false;

    private void Awake()
    {
        //_oScroll = GameTools.f_GetGameObject(transform, "scroll");
        //gpView = _oScroll.GetComponent<GridPageView>();
    }

    public virtual void f_Reset()
    {
        _aScrollTitle.Clear();
        _dirButton.Clear();
        _aGridItem.Clear();
        gpView.f_Reset();
    }

    public virtual void f_AddScrollItem(long iId, string strName, string strBundle, string strRes, int iClass, object Obj, bool bMultilayer = false)//, ccCallback CallBackClick)
    {
        GridItemDT tGridItemDT = new GridItemDT();
        tGridItemDT.f_AddScrollItem(iId, strName, strBundle, strRes, iClass, Obj, bMultilayer);
        _aGridItem.Add(tGridItemDT);
    }

    public void f_AddTitle(int iClass, string strTitle)
    {
        ScrollTitle tScrollTitle = new ScrollTitle();
        tScrollTitle.m_iClass = iClass;
        tScrollTitle.m_strTitle = strTitle;
        _aScrollTitle.Add(tScrollTitle);
    }

    public virtual void f_Create(int iMaxSelNum, ccCallback CallBack_OnClick,
                        int iItemWidth, int iItemHeight, int rows, int columns,
                        float gapH, float gapV, bool isHorizontal, bool bLockDrag)
    {
        if (_aGridItem.Count == 0)
        {
            _aGridItem = null;
            MessageBox.ASSERT("列表未装入数据，请用f_AddScrollItem装入数据。");
        }
        if (_UseSortID)
        {//ID升序
            _aGridItem.Sort(delegate (GridItemDT itemDT1, GridItemDT itemDT2)
            {
                return itemDT1.m_iId.CompareTo(itemDT2.m_iId);
            });
        }

        CreateTitle();

        //生成显示的建筑清单列表
        this.gpView.f_Init(iMaxSelNum, -99, _aGridItem, iItemWidth, iItemHeight, rows,
                     columns,
                     CallBack_OnClick,
                     isHorizontal,
                     gapH,
                     gapV,
                    bLockDrag);

        //f_Sort(0);
    }

    private void CallBack_OnClick(object Obj)
    {
        GridItemDT tGridItemDT = (GridItemDT)Obj;
        MessageBox.DEBUG("Click " + tGridItemDT.f_GetName());
    }

    public virtual void CreateTitle()
    {
        if (_aScrollTitle.Count == 0)
        {
            return;
        }

        Vector3 Pos = m_oBtnMenuModel.transform.position;
        for (int i = 0; i < _aScrollTitle.Count; i++)
        {
            string strTitle = "";
            if (_UseMenuLanguage)
            {
                strTitle = LanguageManager.GetInstance().f_GetText(_aScrollTitle[i].m_strTitle);
            }
            else
            {
                strTitle = _aScrollTitle[i].m_strTitle;
            }
            GameObject oBtn = GameObject.Instantiate(m_oBtnMenuModel);
            oBtn.name = strTitle;

            oBtn.transform.SetParent(m_MenuParent.transform);
            Pos.x = Pos.x + 100;
            oBtn.transform.position = Pos;
            oBtn.transform.localScale = Vector3.one;

            ccButton tccButton = oBtn.GetComponent<ccButton>();
            tccButton.f_Init(null, strTitle);
            _dirButton.Add(_aScrollTitle[i].m_iClass, tccButton);
            //GameUITools.f_SetButtonText(oBtn, aScrollTitle[i].m_strTitle);
            ccUIEventListener.Get(oBtn).f_RegClick(CallBack_OnTitleClick, _aScrollTitle[i], null, "");

            //啟用菜單圖片設置
            if (_UseMenuSprite)
            {//菜單路徑在Assets/Resources/Icon/Menu底下，資源名和菜單名字必須一致
                tccButton.f_Init(glo_Main.GetInstance().m_ResourceManager.f_CreateIcon(LogicUI_Const.AvatarAB, _aScrollTitle[i].m_strTitle));
            }
        }
        m_oBtnMenuModel.SetActive(false);

        //啟用菜單滾輪功能
        if (_UseMenuScroll)
        {
            RectTransform MenuParent = (RectTransform)m_MenuParent.transform;
            RectTransform Top_TitleRect = (RectTransform)_dirButton[_aScrollTitle[0].m_iClass].transform;

            float fPos = Top_TitleRect.sizeDelta.y + _MenuSpacing;
            fPos *= _aScrollTitle.Count;
            MenuParent.sizeDelta = new Vector2(MenuParent.sizeDelta.x, fPos);
            //MenuParent.sizeDelta += new Vector2(0, fPos);
        }
    }

    public virtual void f_Sort(int iClass)
    {
        if (_CurSelButton != null)
        {
            _CurSelButton.f_UnSelect();
        }
        _dirButton.TryGetValue(iClass, out _CurSelButton);
        _CurSelButton.f_Select();
        this.gpView.f_Sort(iClass, true);
    }

    public virtual void CallBack_OnTitleClick(GameObject go, object obj1, object obj2)
    {
        ScrollTitle tScrollTitle = (ScrollTitle)obj1;
        f_Sort(tScrollTitle.m_iClass);
    }

    /// <summary>
    /// 获得当前被选择的项目的列表
    /// </summary>
    /// <returns></returns>
    public List<GridItem> f_GetCurSelItem()
    {
        return gpView.f_GetCurSelItem();
    }

    public void f_ClearCurSelItem()
    {
        gpView.f_ClearCurSelItem();
    }

    /// <summary>開關菜單列表</summary>
    public void f_SetMenu(bool bSet)
    {
        m_MenuParent.transform.parent.gameObject.SetActive(bSet);
    }

    /// <summary>開關項目列表</summary>
    public void f_SetScroll(bool bSet)
    {
        gpView.gameObject.SetActive(bSet);
    }
}