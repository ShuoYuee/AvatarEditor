using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 返回上頁處理
/// </summary>
public class BackPage
{
    protected ScrollDlg _ScrollDlg;
    protected List<BackData> _aPageClass = new List<BackData>();

    protected class BackData
    {
        public int _iClass;
        public bool _bMultilayer = false;

        public BackData(int iClass, bool bMultilayer = false)
        {
            _iClass = iClass;
            _bMultilayer = bMultilayer;
        }
    }

    public BackPage(ScrollDlg scrollDlg)
    {
        _ScrollDlg = scrollDlg;
    }

    /// <summary>
    /// 新增返回內容
    /// </summary>
    /// <param name="iClass">頁碼</param>
    /// <param name="bMultilayer">是否為子層</param>
    public virtual void f_AddClass(int iClass, bool bMultilayer = false)
    {
        _aPageClass.Add(new BackData(iClass, bMultilayer));
    }

    /// <summary>返回上頁</summary>
    public virtual void f_BackPage()
    {
        if (!_ScrollDlg) { return; }
        if (_aPageClass.Count < 2)
        {
            _ScrollDlg.f_SetMenu(true);
            _ScrollDlg.f_SetScroll(false);
        }
        else
        {
            _ScrollDlg.gpView.f_Sort(_aPageClass[_aPageClass.Count - 2]._iClass, true);
            _aPageClass.RemoveRange(_aPageClass.Count - 1, 1);
        }
    }

    /// <summary>重置</summary>
    public virtual void f_Reset()
    {
        _aPageClass.Clear();
    }
}
