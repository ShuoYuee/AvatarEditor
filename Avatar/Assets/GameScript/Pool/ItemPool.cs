using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;
using System.Collections.Generic;

/// <summary>
/// 基础物品Pool
/// </summary>
public class ItemPool : BasePool
{
    public ItemPool() : base("ItemPoolDT")
    {

    }


    #region Pool数据管理

    protected override void f_Init()
    {
    }

    protected override void RegSocketMessage()
    {
        SC_ItemInit tSC_ItemInit = new SC_ItemInit();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_ItemInfo, tSC_ItemInit, Callback_SocketData_Update);
        stPoolDelData tstPoolDelData = new stPoolDelData();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ItemRemove, tstPoolDelData, Callback_Del);
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_ItemInit tServerData = (SC_ItemInit)Obj;
        ItemPoolDT tPoolDataDT = new ItemPoolDT();
        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_iNum = tServerData.iNum;
        tPoolDataDT.m_iSetup = tServerData.iSetup;
        tPoolDataDT.m_iShopId = tServerData.iShopId;
        f_Save(tPoolDataDT);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_ItemInit tServerData = (SC_ItemInit)Obj;
        ItemPoolDT tPoolDataDT = (ItemPoolDT)f_GetForId(tServerData.id);
        if (tPoolDataDT == null)
        {
            MessageBox.ASSERT("无此BaseGoods的资料，更新失败");
        }
        tPoolDataDT.m_iNum = tServerData.iNum;
    }

    protected void Callback_Del(object Obj)
    {
        stPoolDelData tServerData = (stPoolDelData)Obj;
        ItemPoolDT tPoolDataDT = (ItemPoolDT)f_GetForId(tServerData.iId);
        f_Delete(tServerData.iId);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //以下为外部调用接口

    /// <summary>
    /// 检测商店里的物品是否已购买过
    /// </summary>
    /// <param name="tGameShopDT"></param>
    /// <returns>true 已购买过 false未购买过</returns>
    public bool f_CheckIsBuy(GameShopDT tGameShopDT)
    {
        //前期直接采用循环处理，后期如何效能开消大要修改成缓存的方式来处理
        for (int i = 0; i < f_GetAll().Count; i++)
        {
            ItemPoolDT tItemPoolDT = (ItemPoolDT)f_GetAll()[i];
            if (tItemPoolDT.m_iShopId == tGameShopDT.iId)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 卖出接口
    /// </summary>
    /// <param name="Id">指定Id</param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Sell(ItemPoolDT tItemPoolDT, int iNum, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SellItem, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tItemPoolDT.iId);
        tCreateSocketBuf.f_Add(iNum);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SellItem, bBuf);
    }

    /// <summary>
    /// 使用接口
    /// </summary>
    /// <param name="Id">指定Id</param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Use(long id, int iNum, int param, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        //Data_Pool.m_AwardPool.m_GetLoginAward.Clear();
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_UseItem, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        tCreateSocketBuf.f_Add(iNum);
        tCreateSocketBuf.f_Add(param);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_UseItem, bBuf);
    }

    /// <summary>
    /// 裝備道具
    /// </summary>
    /// <param name="tItemPoolDT">道具資訊</param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Setup(ItemPoolDT tItemPoolDT, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SetupItem, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tItemPoolDT.iId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SetupItem, bBuf);
    }

    /// <summary>
    /// 確認背包裡是否有該道具
    /// </summary>
    /// <param name="iId">道具ID</param>
    /// <returns></returns>
    public bool f_CheckHaveForTempleteId(int iId)
    {
        List<BasePoolDT<long>> tList = f_GetAll();
        for (int i = 0; i < tList.Count; i++)
        {
            if (((ItemPoolDT)tList[i]).m_iTempleteId == iId)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 確認該道具是否被裝備
    /// </summary>
    /// <param name="iId">道具ID</param>
    /// <returns></returns>
    public bool f_CheckSetup(int iId)
    {
        ItemPoolDT tItemPoolDT = (ItemPoolDT)f_GetForId(iId);
        if (tItemPoolDT == null) { return false; }
        if (tItemPoolDT.m_iSetup.Equals(1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    ///// <summary>
    ///// 根据模板Id获取已拥有的数目
    ///// </summary>
    ///// <param name="templateId"></param>
    ///// <returns></returns>
    //public int f_GetHaveNumByTemplate(int templateId)
    //{
    //    if(f_GetForData5(templateId) == null)
    //        return 0;
    //    else
    //    {
    //        List<BasePoolDT<long>> tList = f_GetAllForData5(templateId);
    //        int m_Num = 0;
    //        for(int i = 0; i < tList.Count; i++)
    //        {
    //            m_Num += (tList[i] as BaseGoodsPoolDT).m_iNum;
    //        }
    //        return m_Num;
    //    }
    //}
}