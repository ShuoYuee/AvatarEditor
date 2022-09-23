using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


public struct stIp
{
    public string m_szIp;
    public int m_iPort;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct basicNode1 : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int value1;
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct stGameCommandReturn : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iCommand;
    /// <summary>
    /// 操作结果
    /// </summary>
    public int iResult;
    //public int iData2;
    //public int iData3;
}


/// <summary>
/// 登陆封包
/// </summary>
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_CTG_AccountEnter
{
    /// <summary>
    /// 0：正常登陆 1：重新连接
    /// </summary>
    public int m_state;									// 服务器UID

    /// <summary>
    /// 账户名
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strAccount;                      //// 机器码		

    /// <summary>
    /// 密码
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strPassword;                      //// 机器码		
    //服务器ID： 默认1
    public int m_dwServerID;									// 服务器UID
    //GamePlayer标示，服务器用
    public long m_GamePlayerPoint;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_GTC_LoginRelt : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    /// <summary>
    /// 账户ID
    /// </summary>
    public long m_PlayerId;                                  // 账户id

    /// <summary>
    /// OR_Succeed = 0, // 成功 OR_Error_NoAccount = 21, // 登陆：账号不存在 OR_Error_Password = 22, // 登陆：密码错误
    /// </summary>
    public int m_result;									// 账户id
    public long userPtr;
    public int m_iServerTime;

    public int m_iServerId;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]//SocketDT.MAX_USER_NAME)]
    public string m_strServerName;

    /// <summary>
    /// 服务器保留
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 96)]//SocketDT.MAX_USER_NAME)]
    public string uniqueLoginId;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_GTC_AccountLoginRelt : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    public int createTime;			// 建号时间

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.I4)]       //eUserAttr_Enum.eUserAttr_INVALID - 1)]
    public int[] attr;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string szRoleName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_UserAttr : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte attrEnum;
    public int iValue;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_AvaterBuf : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.U1)]
    public byte[] szAvaterBuf;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RestoreTime : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int RestoreTime;    //体力、精力恢复时间
}


#region 商店道具相关

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ItemInit : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;            // 唯一Id
    public int tempId;        // 模板Id
    public int iNum;            // 等级
    public int iSetup;          //是否已装备 0未装备 1已装备
    public int iShopId;         //如果是从Shop购买的则保存Shop对应的Id
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CS_ShopBuy : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;            // 唯一Id
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct stPoolDelData : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long iId;
}
#endregion

#region 玩家

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct UserView : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uSex;      //0男1女   IConID  
    public byte uFrameId;
    public byte uVipLv;
    public byte uTitleId;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_szName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct UserExtra : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short iLv;
    public int iBattlePower;
    public int offlineTime;
    public string szLegionName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_UserExtra : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;
    public UserExtra m_UserExtra;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_UserView : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;
    public UserView m_UserView;
}

#endregion


#region 好友相关
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct F_UserView : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uSex;      //0男1女   IConID  
    public byte uFrameId;
    public byte uVipLv;
    public byte uTitleId;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_szName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct F_UserExtra : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short iLv;
    public int iBattlePower;
    public int offlineTime;
    public string szLegionName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_InitFriend : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public UserView m_UserView;
    public UserExtra m_UserExtra;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RTC_RecomFriend : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I8)]       //eUserAttr_Enum.eUserAttr_INVALID - 1)]
    public long[] recomUserId;   //推荐好友id
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RTC_ReplyFriend : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public byte uAccept; // 0=拒绝
}
#endregion