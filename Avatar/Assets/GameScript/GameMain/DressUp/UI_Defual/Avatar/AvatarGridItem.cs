using UnityEngine;
using UnityEngine.UI;
using ccU3DEngine;


/// <summary>
/// 時裝相關背包格
/// </summary>
public class AvatarGridItem : UI_LogoGrids
{
    [Header("特殊圖像")]
    /// <summary>當前裝備之圖像</summary>
    public ccButton _CurSelectImage;
    /// <summary>格子上鎖之圖像</summary>
    public ccButton _LockImage;
    /// <summary>資料指引之圖像</summary>
    public ccButton _InforImage;
    /// <summary>紅點之圖像</summary>
    public ccButton _ReddotImage;

    /// <summary>是否已被點選</summary>
    public bool _CurSelect = false;
    /// <summary>是否為換圖模式</summary>
    public bool _UseChangeImage;

    protected override void InitComponent()
    {
        base.InitComponent();
        f_ChangeImage(_CurSelectImage, false);
        f_ChangeImage(_LockImage, false);
        f_ChangeImage(_ReddotImage, false);
        if (_InforImage)
        {
            ccUIEventListener.Get(_InforImage.gameObject).f_RegClick(f_OnClick_BtnInfor, null, null, "");
        }
    }

    public override void f_UnSelect()
    {
        base.f_UnSelect();
        _CurSelect = false;
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
            m_ccButton.f_InitInfor(glo_Main.GetInstance().m_ResourceManager.f_CreateIcon(LogicUI_Const.AvatarAB, tGridItemDT.f_GetIcon()), strName);
        }
        else
        {
            f_UnShow();
            return;
        }
        if (tGridItemDT == null)
        {
            f_SetExtraData(tGridItemDT, true);
            return;
        }
        if (tGridItemDT.f_GetMultilayer())
        {
            f_SetExtraData(tGridItemDT, true);
        }
        else
        {
            f_SetExtraData(tGridItemDT);
        }
    }

    /// <summary>設定其餘資料</summary>
    /// <param name="bClose">是否直接關閉圖像</param>
    /// <param name="tGridItemDT">Item資料</param>
    protected virtual void f_SetExtraData(GridItemDT tGridItemDT, bool bClose = false)
    {
        if (bClose)
        {
            f_ChangeImage(_CurSelectImage, false);
            f_ChangeImage(_LockImage, false);
            f_ChangeImage(_InforImage, false);
            return;
        }
        else
        {
            //設定是否為活動解鎖、是否為當前裝備的時裝
            GameShopDT tData = (GameShopDT)tGridItemDT.f_GetObj();
            AvaterPartDT tAvaterPartDT = (AvaterPartDT)glo_Main.GetInstance().m_SC_Pool.m_AvaterPartSC.f_GetSC(tData.iGoodsId);
            f_ChangeImage(_CurSelectImage, PlayerDataTools.f_CheckUseAvatar(tData.iGoodsId));
            bool bLock = tAvaterPartDT.iLock.Equals(1);
            f_ChangeImage(_LockImage, bLock);
        }
    }

    /// <summary>
    /// 設定特殊圖案
    /// </summary>
    /// <param name="oButton">圖案</param>
    /// <param name="bSet">是否啟用</param>
    protected void f_ChangeImage(ccButton oButton, bool bSet)
    {
        if (!oButton) { return; }
        if (_UseChangeImage)
        {
            if (bSet)
            {
                oButton.f_Select();
            }
            else
            {
                oButton.f_UnSelect();
            }
        }
        else
        {
            if (bSet)
            {
                oButton.f_ShowBG();
            }
            else
            {
                oButton.f_UnShowBG();
            }
        }
    }

    /// <summary>點選資訊按鈕後回調</summary>
    private void f_OnClick_BtnInfor(GameObject go, object obj1, object obj2)
    {
        MessageBox.DEBUG("Click " + _InforImage.name);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ShopInforBtn, _GridItemDT);
    }
}
