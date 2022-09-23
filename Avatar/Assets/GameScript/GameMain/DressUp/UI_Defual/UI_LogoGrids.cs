using UnityEngine;
using ccU3DEngine;

/// <summary>
/// Logo背包格
/// </summary>
/// 根據資料更新Logo圖和文字
public class UI_LogoGrids : GridItem
{
    public bool _UseLanguage = true;

    protected override void InitComponent()
    {
        base.InitComponent();
        m_ccButton.f_Init(null, "");
    }

    public override void f_UpdateUI(GridItemDT tGridItemDT)
    {
        base.f_UpdateUI(tGridItemDT);
        if (tGridItemDT != null)
        {
            string strName = "";
            if (_UseLanguage)
            {
                strName = LanguageManager.GetInstance().f_GetText(tGridItemDT.f_GetName());
            }
            else
            {
                strName = tGridItemDT.f_GetName();
            }
            m_ccButton.f_InitInfor(glo_Main.GetInstance().m_ResourceManager.f_CreateIcon(LogicUI_Const.GameIconAB, tGridItemDT.f_GetIcon()), strName);
        }
        else
        {
            f_UnShow();
            return;
        }
    }

    public override void f_UnShow()
    {
        base.f_UnShow();
        m_ccButton.f_InitText("");
    }
}
