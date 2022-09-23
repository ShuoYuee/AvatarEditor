using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;

public enum SocketCommand
{
    /// <summary>
    /// PING
    /// </summary>
    PING = 10000,
    PING_Reps = 30000,

    /// <summary>
    /// --客户端<-->游戏服务器
    /// </summary>
    MSG_CGameMsg = 6,

    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 创建账户 
    /// </summary>
    CS_UserCreate = 10001,
    /// <summary>
    /// 登陆申请 CMsg_CTG_AccountEnter
    /// </summary>
    CS_UserLogin = 10002,

    //////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 创建账户结果 
    /// </summary>
    SC_UserCreate = 30001,
    /// <summary>
    /// 登陆结果返回 CMsg_AccountLoginRelt
    /// </summary>
    SC_UserLogin = 30002,
    SC_Kickout = 30003,

    /// <summary>
    /// 游戏开始通知  
    /// </summary>
    SC_RoleEnterGame = 30016,

    /// <summary>
    /// 操作结果回应
    /// </summary>
    CONTROL_CTG_OperateResult = 30017,

    SC_UserAttrInit = 30021,
    /// <summary>
    /// 角色：数据更新
    /// </summary>
    SC_UserAttr = 30022,
    SC_AvaterBuf = 30023,			// AvaterBuf

    SC_EnergyTime = 30030,          // 体力恢复计时节点


    CS_ItemInfo = 10200,            // 道具初始化
    CS_SellItem = 10201,            // 出售道具
    CS_UseItem = 10203,             // 使用道具
    CS_SetupItem = 10205,             // 装备道具

    CS_ShopBuy = 10301,		// 抽奖商店购买
    CS_ShopLotteryBuy = 10302,		// 抽奖商店购买
    SC_ShopLotteryInfo = 30300,		// 抽奖商店信息

    SC_ItemInfo = 30200,
    SC_ItemRemove = 30201,

    CS_UpdateAvater = 30400,             // 提交Avater数据
    CS_UpdateSpecialAvater = 30410,      // 提交Avater数据




    #region 军团战

    CS_LegionBattleInit = 13800,            // 军团战初始化
    CS_LegionBattleApply = 13802,           // 军团战报名
    CS_LegionBattleDefenceList = 13803,     // 军团战防守列表
    CS_LegionBattleChallenge = 13804,		// 军团战挑战
    CS_LegionBattleTable = 13805,			// 军团战对阵表

    SC_LegionBattleInit = 33800,            // 军团战初始化
    SC_LegionBattleView = 33801,            // 军团战信息预览
    SC_LegionBattleDefenceList = 33803,     // 军团战防守列表
    SC_LegionBattleChallengeRet = 33804,	// 军团战挑战
    SC_LegionBattleTable = 33805,           // 军团战对阵表

    #endregion


    #region 玩家

    /// <summary>
    /// 玩家基础信息
    /// </summary>
    RC_UserView = 40011,    // 
    /// <summary>
    /// 请求玩家扩展信息
    /// </summary>
    CR_UserExtra = 40012,   // 
    /// <summary>
    /// 玩家扩展信息
    /// </summary>
    RC_UserExtra = 40013,   // 

    #endregion


    #region 好友

    CR_InitFriend = 40200,  // 请求初始化好友
    RC_InitFriend = 40201,  // 好友
    CR_InitFriendApply = 40202, // 请求初始化申请列表
    RC_InitFriendApply = 40203, // 好友申请列表
    CR_RecomFriend = 40204,     // 请求推荐好友
    RC_RecomFriend = 40205,     // 推荐好友
    CR_ApplyFriend = 40206, // 申请好友
    CR_ReplyFriend = 40207, // 好友申请回应(双向)
    CR_RemoveFriend = 40210,    // 移除好友
    RC_RemoveFriend = 40211,    // 好友移除

    CR_InitBlack = 40300,   // 请求黑名单
    RC_InitBlack = 40301,   // 黑名单
    CR_Black = 40302,       // 拉黑
    CR_Disblack = 40303,    // 取消拉黑


    #endregion
}