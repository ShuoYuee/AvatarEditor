using UnityEngine;
using System.Collections;
using ccU3DEngine;

/// <summary>
/// DT UserData(玩家資料).xlsx
/// </summary>
public class PlayerData : BaseProperty
{

    //public GoodsDT m_GoodsDT;
    /// <summary>
    /// 建号时间
    /// </summary>
    public int m_CreateTime;

    //是否已有角色  走创建角色流程 就为false
    private bool isHaveRole;
    public bool m_isHaveRole
    {
        get
        {
            return isHaveRole;
        }
    }
    /// <summary>
    /// 需要创建角色
    /// </summary>
    public void f_NeedCreateRole()
    {
        isHaveRole = false;
    }

    /// <summary>
    /// 玩家唯一Id
    /// </summary>
    public long m_iUserId;//用户id
    /// <summary>
    /// 玩家名字
    /// </summary>
    public string m_szRoleName;//名字   

    /// <summary>
    /// 保存Avater数据
    /// </summary>
    public byte[] m_aAvaterBuf;

    /// <summary>
    /// 保存特殊Avater数据
    /// </summary>
    public byte[] m_aSpecialAvaterBuf;

    /// <summary>
    /// 上次体力恢复时间
    /// </summary>
    public int m_lastEnergyRestoreTime = 0;


    public PlayerData() : base((int)EM_UserAttr.End)
    {
        m_iUserId = -99;
        //SC_RestoreTime scEnergyRestoreTime = new SC_RestoreTime();
        //SC_RestoreTime scVigorRestoreTime = new SC_RestoreTime();
        //SC_RestoreTime scCrusadeRestoreTime = new SC_RestoreTime();

        //GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrusadeTokenTime, scCrusadeRestoreTime, OnCrusadeRestoreTimeCallbakc);
        //GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_EnergyTime, scEnergyRestoreTime, OnEnergyRestoreTimeCallback);
        //GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_VigorTime, scVigorRestoreTime, OnVigorRestoreTimeCallbakc);

        //SC_ReturnRandRoleName tCS_QueryRandRoleName = new SC_ReturnRandRoleName();
        //GameSocket.GetInstance().f_AddListener((int)SocketCommand.SC_ReturnRandRoleName, tCS_QueryRandRoleName, On_SC_ReturnRandRoleName);

        //m_PlayerLvDT =(PlayerLvDT) glo_Main.GetInstance().m_SC_Pool.m_PlayerLvSC.f_GetSC(1);

        isHaveRole = true;
    }

    public int m_iServerId;

    private string serverName;
    public string m_strServerName
    {
        set
        {
            serverName = value;
        }

        get
        {
            return serverName == null ? string.Empty : serverName;
        }
    }

    public override void f_Copy(int[] aData, int iStartPos, int iLen)
    {
        base.f_Copy(aData, iStartPos, iLen);
        ////Vip信息更新事件
        //glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
        //   new int[] { (int)EM_AchievementTaskCondition.eAchv_VipLv, UITool.f_GetVipLv(f_GetProperty((int)EM_UserAttr.eUserAttr_Vip)) });
        //glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerVipUpdate, UITool.f_GetVipLv(f_GetProperty((int)EM_UserAttr.eUserAttr_Vip)));
    }

    public override void f_UpdateProperty(int iIndex, int iData)
    {

        //if (iIndex != (int)EM_UserAttr.eUserAttr_Vip && iIndex != (int)EM_UserAttr.eUserAttr_MainCard && iIndex != (int)EM_UserAttr.eUserAttr_IconFrame && iIndex != (int)EM_UserAttr.eUserAttr_Energy
        //    && iIndex != (int)EM_UserAttr.eUserAttr_Vigor)
        //{
        //    Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.Money, iIndex, iData - f_GetProperty(iIndex));
        //}

        base.f_UpdateProperty(iIndex, iData);

        ////Vip信息更新事件
        //if (iIndex == (int)EM_UserAttr.eUserAttr_Vip)
        //{
        //    glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
        //    new int[] { (int)EM_AchievementTaskCondition.eAchv_VipLv, UITool.f_GetVipLv(f_GetProperty((int)EM_UserAttr.eUserAttr_Vip)) });
        //    glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerVipUpdate, UITool.f_GetVipLv(f_GetProperty((int)EM_UserAttr.eUserAttr_Vip)));
        //}

    }
    public void InitDataDefault()
    {
        if (m_lastEnergyRestoreTime == 0)
        {
            m_lastEnergyRestoreTime = 0;        // GameSocket.GetInstance().f_GetServerTime();//设置默认值
        }
    }
    /// <summary>
    /// 其他正常获取，等级和经验获取在主角卡那里获取
    /// </summary>
    /// <param name="iIndex"></param>
    /// <returns></returns>
    public override int f_GetProperty(int iIndex)
    {
        //if (iIndex == (int)EM_UserAttr.eUserAttr_Level)
        //{
        //    return Data_Pool.m_CardPool.f_GetRoleLevel();
        //}
        //else if (iIndex == (int)EM_UserAttr.eUserAttr_Exp)
        //{
        //    return Data_Pool.m_CardPool.f_GetRoleExp();
        //}
        //else if (iIndex == (int)EM_UserAttr.eUserAttr_TaskScore)
        //{
        //    return Data_Pool.m_TaskDailyPool.mScore;
        //}
        //else
        return base.f_GetProperty(iIndex);
    }

    public void OnEnergyRestoreTimeCallback(object obj)
    {
        m_lastEnergyRestoreTime = ((SC_RestoreTime)obj).RestoreTime;
    }

    /// <summary>
    /// 更新角色的时装信息
    /// </summary>
    /// <param name="aBuf"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_UpdateAvaterBuf(byte[] aBuf, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_UpdateAvater, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(aBuf);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_UpdateAvater, bBuf);
    }

    /// <summary>
    /// 更新特殊时装
    /// </summary>
    /// <param name="aBuf"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_UpdateSpecialAvaterBuf(byte[] aBuf, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_UpdateSpecialAvater, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(aBuf);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_UpdateSpecialAvater, bBuf);
    }

}