using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 變形工具Item
/// </summary>
/// 將變形功能串接
public class DeformerGridItem : UI_SetBGGrids
{
    [Space(10)]
    /// <summary>變形Slider</summary>
    public Slider _DeformSlider;
    private GameEM.EM_Deformer DeformerEM;

    protected override void InitComponent()
    {
        base.InitComponent();
        _DeformSlider.onValueChanged.AddListener(f_CallBackSlider);
    }

    public override void f_UpdateUI(GridItemDT tGridItemDT)
    {
        _GridItemDT = tGridItemDT;       
        m_ccButton.f_InitText(tGridItemDT.f_GetName());
        gameObject.name = tGridItemDT.f_GetName();
        f_InitSlider();
    }

    public override void f_UnShow()
    {
        _GridItemDT = null;
        m_ccButton.f_UnShow();
        m_ccButton.f_UnShowBG();
    }

    public void f_InitSlider()
    {//初始化Slider
        GameEM.EM_Deformer tEM = (GameEM.EM_Deformer)_GridItemDT.f_GetClass();
        DeformerEM = tEM;
        _DeformSlider.value = GameMain.GetInstance().m_DressRoleControl.f_GetDeformerValue(tEM.ToString()) / 100;
    }

    public void f_CallBackSlider(float fValue)
    {
        GameMain.GetInstance().m_DressRoleControl.f_Deformer(DeformerEM.ToString(), fValue * 100);
    }

    private void OnDestroy()
    {
        _DeformSlider.onValueChanged.RemoveAllListeners();
    }
}
