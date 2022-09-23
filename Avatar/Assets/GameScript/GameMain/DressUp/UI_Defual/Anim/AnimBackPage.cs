using UnityEngine;


/// <summary>
/// 返回上頁(動畫版)
/// </summary>
public class AnimBackPage : BackPage
{
    public AnimBackPage(ScrollDlg scrollDlg) : base(scrollDlg)
    {

    }

    public override void f_BackPage()
    {
        if (!_ScrollDlg) { return; }
        if (_aPageClass.Count >= 2)
        {
            _ScrollDlg.gpView.f_Sort(_aPageClass[_aPageClass.Count - 2]._iClass, true);
            _aPageClass.RemoveRange(_aPageClass.Count - 1, 1);
        }
        else if (_aPageClass.Count == 1)
        {
            _ScrollDlg.gpView.f_Sort(_aPageClass[0]._iClass, true);
        }
        else
        {
            _aPageClass.Clear();
        }
    }
}
