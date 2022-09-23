using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 列表物件对应的数据结构
/// </summary>
public class GridItemDT
{
    protected long _iId;
    protected string _strName = "";
    protected string _strIcon = "";
    protected string _strBundle = "";
    protected int _iClass;
    protected object _Obj;
    protected bool _bMultilayer = false;

    //private ccCallback _CallBack_OnItemClick;
    public long m_iId
    {
        get
        {
            return _iId;
        }
    }


    public void f_AddScrollItem(long iId, string strName, string strBundle, string strIcon, int iClass, object Obj)//, ccCallback CallBackClick)
    {
        _iId = iId;
        _strName = strName;
        _strIcon = strIcon;
        _strBundle = strBundle;
        _iClass = iClass;
        _Obj = Obj;
        //_CallBack_OnItemClick = CallBackClick;
    }

    public void f_AddScrollItem(long iId, string strName, string strBundle, string strIcon, int iClass, object Obj, bool bMultilayer)
    {
        _iId = iId;
        _strName = strName;
        _strIcon = strIcon;
        _strBundle = strBundle;
        _iClass = iClass;
        _Obj = Obj;

        //若啟用子層設定，則該筆資料也作為菜單使用，Obj資料為對應Class
        _bMultilayer = bMultilayer;
    }

    public string f_GetName()
    {
        return _strName;
    }

    public string f_GetBundle()
    {
        return _strBundle;
    }

    public string f_GetIcon()
    {
        return _strIcon;
    }

    public object f_GetObj()
    {
        return _Obj;
    }

    public int f_GetClass()
    {
        return _iClass;
    }

    public bool f_GetMultilayer()
    {
        return _bMultilayer;
    }

    //public void f_BeClick()
    //{
    //    if (_CallBack_OnItemClick != null)
    //    {
    //        _CallBack_OnItemClick(this);
    //    }
    //}

}