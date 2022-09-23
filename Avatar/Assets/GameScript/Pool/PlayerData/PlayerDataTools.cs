using ccU3DEngine;

public class PlayerDataTools
{

    /// <summary>
    /// 登陆成功后要把服务器传送来的服务器数据转换成游戏数据
    /// </summary>
    public static void f_ServerData2GameData(CMsg_GTC_AccountLoginRelt tCMsg_GTC_AccountLoginRelt)
    {
        Data_Pool.GetInstance().m_PlayerData.m_CreateTime = tCMsg_GTC_AccountLoginRelt.createTime;
        Data_Pool.GetInstance().m_PlayerData.f_Copy(tCMsg_GTC_AccountLoginRelt.attr, 0, tCMsg_GTC_AccountLoginRelt.attr.Length);
        Data_Pool.GetInstance().m_PlayerData.m_szRoleName = tCMsg_GTC_AccountLoginRelt.szRoleName;

    }

    public static void f_ChangeAvaterBuf(byte[] aBuf)
    {
        Data_Pool.GetInstance().m_PlayerData.m_aAvaterBuf = aBuf;
    }

    public static void f_ChangeSpecialAvaterBuf(byte[] aBuf)
    {
        Data_Pool.GetInstance().m_PlayerData.m_aSpecialAvaterBuf = aBuf;
    }

    public static void f_ChangePlayerData(EM_UserAttr tEM_UserAttr, int nValues)
    {
        Data_Pool.GetInstance().m_PlayerData.f_UpdateProperty((int)tEM_UserAttr, nValues);
    }

    #region 時裝相關
    /// <summary>
    /// 確認衣櫥是否存有該時裝
    /// </summary>
    /// <param name="iId">時裝ID</param>
    /// <returns></returns>
    public static bool f_CheckWordrobeHaveObj(int iId)
    {
        return Data_Pool.GetInstance().m_ItemPool.f_CheckHaveForTempleteId(iId);
    }

    /// <summary>
    /// 確認是否正在裝備該時裝
    /// </summary>
    /// <param name="iId">時裝ID</param>
    /// <returns></returns>
    public static bool f_CheckUseAvatar(int iId)
    {
        return Data_Pool.GetInstance().m_ItemPool.f_CheckSetup(iId);
    }

    /// <summary>
    /// 確認和玩家性別與尺寸相符
    /// </summary>
    /// <param name="iPart">時裝類型</param>
    /// <param name="iValue">車種尺寸</param>
    /// <param name="tRole">換裝控制器</param>
    /// <returns></returns>
    public static bool f_CheckGenderAndSize(int iPart, int iValue, DressRoleControl tRole)
    {
        if (iPart == (int)GameEM.EM_BikePart.bike || iPart == (int)GameEM.EM_RolePart.wheel || iPart == (int)GameEM.EM_RolePart.frame)
        {//若時裝為車種、車輪、車架，則確認角色身高是否符合尺寸
            return f_CheckBodyHight(iValue, tRole);
        }
        else
        {
            return f_CheckPlayerGender(iValue);
        }
    }

    /// <summary>
    /// 確認和玩家性別相符
    /// </summary>
    /// <param name="iGender">性別</param>
    /// <returns></returns>
    public static bool f_CheckPlayerGender(int iGender)
    {
        return Data_Pool.GetInstance().m_PlayerData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sex).Equals(iGender) || iGender.Equals((int)GameEM.EM_Gender.Common);
    }

    /// <summary>
    /// 確認和玩家人種相符
    /// </summary>
    /// <param name="iRace">人種</param>
    /// <returns></returns>
    public static bool f_CheckPlayerRace(int iRace)
    {
        return Data_Pool.GetInstance().m_PlayerData.f_GetProperty((int)EM_UserAttr.eUserAttr_Race).Equals(iRace);
    }

    /// <summary>
    /// 確認是否為新的商品
    /// </summary>
    /// <param name="iId">時裝ID</param>
    /// <returns></returns>
    public static bool f_CheckNewGoods(object iId)
    {//要用紅點系統
        return true;
    }

    /// <summary>
    /// 確認角色身高是否符合
    /// </summary>
    /// <param name="iSize">尺寸</param>
    /// <param name="tRole">換裝控制器</param>
    /// <returns></returns>
    public static bool f_CheckBodyHight(int iSize, DressRoleControl tRole)
    {//確認尺寸符合(人物身高和車型尺寸)
        float fValue = tRole.f_GetDeformerValue(GameEM.EM_Deformer.BodyHeight.ToString());
        int iPlayerSize = fValue <= 50 ? (int)GameEM.EM_BikeSize.Little : (int)GameEM.EM_BikeSize.Large;
        return iSize == iPlayerSize || iSize == (int)GameEM.EM_BikeSize.Common;
    }

    /// <summary>
    /// 檢測時裝數據是否存在
    /// </summary>
    /// <returns></returns>
    public static bool f_CheckAvatar()
    {
        if (Data_Pool.GetInstance().m_PlayerData.m_aAvaterBuf == null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 检测是否启用特殊时装
    /// </summary>
    /// <returns></returns>
    public static bool f_CheckUseSpecial()
    {
        if (Data_Pool.GetInstance().m_PlayerData.m_aSpecialAvaterBuf == null)
        {
            return false;
        }
        if (Data_Pool.GetInstance().m_PlayerData.m_aSpecialAvaterBuf[0] <= 0)
        {
            return false;
        }
        return true;
    }
    #endregion
}
