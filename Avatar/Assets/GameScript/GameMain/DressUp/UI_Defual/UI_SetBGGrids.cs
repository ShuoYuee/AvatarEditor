/// <summary>
/// 背景背包格
/// </summary>
/// 開關格子背景
/// 可設定是否開關Logo
public class UI_SetBGGrids : UI_LogoGrids
{
    public bool _ShowLogo = true;

    public override void f_Show()
    {
        if (_ShowLogo)
        {
            base.f_Show();
        }
        m_ccButton.f_ShowBG();
    }

    public override void f_UnShow()
    {
        if (_ShowLogo)
        {
            base.f_UnShow();
        }
        m_ccButton.f_UnShowBG();
    }
}
