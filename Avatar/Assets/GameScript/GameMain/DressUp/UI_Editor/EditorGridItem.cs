/// <summary>
/// 角色編輯器選項
/// </summary>
/// 更新文字(未使用多語言)
public class EditorGridItem : GridItem
{
    /// <summary>是否已被點選</summary>
    public bool _CurSelect = false;

    public override void f_UpdateUI(GridItemDT tGridItemDT)
    {
        base.f_UpdateUI(tGridItemDT);
        m_ccButton.f_InitText(tGridItemDT.f_GetName());
    }

    public override void f_UnShow()
    {
        base.f_UnShow();
        m_ccButton.f_InitText("");
    }

    public override void f_UnSelect()
    {
        base.f_UnSelect();
        _CurSelect = false;
    }
}
