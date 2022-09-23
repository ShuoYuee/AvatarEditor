using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class PageView : MonoBehaviour
{
    protected class ScrollTitle
    {
        public int m_iClass;
        public string m_strTitle;
        public GameObject m_oPage;
        public PageItem m_PageItem;
    }

    protected ccCallback _OnClickMenu = null;
    protected ccButton _CurSelButton = null;
    protected ScrollTitle _CurSelPage = null;
    protected List<ScrollTitle> _aScrollTitle = new List<ScrollTitle>();
    protected Dictionary<int, ccButton> _dirButton = new Dictionary<int, ccButton>();

    public GameObject m_MenuParent;
    public GameObject m_oBtnMenuModel;

    /// <summary>是否啟用ID升序</summary>
    public bool _UseSortID = false;
    /// <summary>是否啟用菜單多語言設定</summary>
    public bool _UseMenuLanguage = true;
    /// <summary>是否啟用菜單圖片設定</summary>
    public bool _UseMenuSprite = false;
    /// <summary>是否啟用菜單列表滾動</summary>
    public bool _UseMenuScroll = false;

    public virtual void f_Reset()
    {
        _aScrollTitle.Clear();
        _dirButton.Clear();
        _CurSelButton = null;
        _CurSelPage = null;
    }

    public void f_AddTitle(int iClass, string strTitle, GameObject oPage)
    {
        ScrollTitle tScrollTitle = new ScrollTitle();
        tScrollTitle.m_iClass = iClass;
        tScrollTitle.m_strTitle = strTitle;
        tScrollTitle.m_oPage = oPage;
        _aScrollTitle.Add(tScrollTitle);
    }

    private void CallBack_OnClick(object Obj)
    {
        GridItemDT tGridItemDT = (GridItemDT)Obj;
        MessageBox.DEBUG("Click " + tGridItemDT.f_GetName());
    }

    public virtual void CreateTitle(ccCallback ccCallback)
    {
        if (_aScrollTitle.Count == 0)
        {
            return;
        }

        _OnClickMenu = ccCallback;
        Vector3 Pos = m_oBtnMenuModel.transform.position;
        if (_UseSortID)
        {//ID升序
            _aScrollTitle.Sort(delegate (ScrollTitle itemDT1, ScrollTitle itemDT2)
            {
                return itemDT1.m_iClass.CompareTo(itemDT2.m_iClass);
            });
        }
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
            ccUIEventListener.Get(oBtn).f_RegClick(CallBack_OnTitleClick, _aScrollTitle[i], null, "");

            _aScrollTitle[i].m_PageItem = _aScrollTitle[i].m_oPage.GetComponent<PageItem>();
            GameTools.f_SetGameObject(_aScrollTitle[i].m_oPage, false);

            //啟用菜單圖片設置
            if (_UseMenuSprite)
            {//資源名和菜單名字必須一致
                tccButton.f_Init(glo_Main.GetInstance().m_ResourceManager.f_CreateIcon(LogicUI_Const.AvatarAB, _aScrollTitle[i].m_strTitle));
            }
        }
        m_oBtnMenuModel.SetActive(false);

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

    public virtual void f_Sort(int iClass)
    {
        if (_CurSelButton != null)
        {
            _CurSelButton.f_UnSelect();
        }
        _dirButton.TryGetValue(iClass, out _CurSelButton);
        _CurSelButton.f_Select();
    }

    public virtual void CallBack_OnTitleClick(GameObject go, object obj1, object obj2)
    {
        ScrollTitle tScrollTitle = (ScrollTitle)obj1;
        f_Sort(tScrollTitle.m_iClass);

        if (_CurSelPage != null)
        {
            GameTools.f_SetGameObject(_CurSelPage.m_oPage, false);
        }
        
        GameTools.f_SetGameObject(tScrollTitle.m_oPage, true);
        _CurSelPage = tScrollTitle;
        if (_OnClickMenu != null)
        {
            _OnClickMenu(tScrollTitle.m_iClass);
        }
    }

    /// <summary>開關菜單列表</summary>
    public void f_SetMenu(bool bSet)
    {
        m_MenuParent.transform.parent.gameObject.SetActive(bSet);
    }

    /// <summary>開關項目列表</summary>
    public void f_SetScroll(bool bSet)
    {
        GameTools.f_SetGameObject(_CurSelPage.m_oPage, bSet);
    }

    /// <summary>開啟指定菜單</summary>
    public void f_OpenPage(int iClass)
    {
        ScrollTitle tScrollTitle = _aScrollTitle.Find(delegate (ScrollTitle title)
        {
            if (title.m_iClass == iClass) { return true; }
            return false;
        });
        if (tScrollTitle != null)
        {
            CallBack_OnTitleClick(null, tScrollTitle, null);
        }
    }

    public List<GameObject> f_GetCurPageObj()
    {
        return _CurSelPage.m_PageItem._UIGameObject;
    }

    public GameObject f_GetObject(string strName)
    {
        return GameTools.f_GetGameObject(_CurSelPage.m_PageItem._UIGameObject, strName);
    }
}
