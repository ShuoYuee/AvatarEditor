using UnityEngine;

/// <summary>
/// 一次性點擊按鈕
/// </summary>
/// 點擊完成後便取消點擊
public class ReturnBtn : AnimationBtn
{
    public override void f_Select()
    {
        base.f_Select();
        f_UnSelect();
    }
}
