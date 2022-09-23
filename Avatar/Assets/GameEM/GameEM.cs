using UnityEngine;
using ccU3DEngine;

//协议操作结果
public enum eMsgOperateResult
{
    OR_Succeed = 0, // 成功
    OR_Fail = 1, //未知原因失败
    OR_SocketConnectFail = 2, //网格无法连接     
    OR_VerFail = 3, //获取版本失败 
    OR_ScFail = 4, //获取脚本失败 
    OR_ResourceFail = 5, //加载资源失败

    OR_Error_AccountRepetition = 20, // 注册：账号重复
    OR_Error_NoAccount = 21, // 登陆：账号不存在
    OR_Error_Password = 22, // 登陆：密码错误
    OR_Error_AccountOnline = 24, // 登陆：账号在线
    OR_Error_NameRepetition = 23, // 改名：名称重复

    OR_Error_VersionNotMatch = 71, //版本不匹配 2016-7-8 
    OR_Error_ElseWhereLogin = 72, //异地登录 2016-7-8 
    OR_Error_SeverMaintain = 73, //服务器维护 2016-7-8 

    OR_Error_PosIsHavePlayer = 74, //位置上已经有玩家，操作失败


    OR_Error_WIFIConnectTimeOut = 993, //WIFI网络未开
    OR_Error_ConnectTimeOut = 994, //连接超时
    OR_Error_CreateAccountTimeOut = 995, //注册超时
    OR_Error_LoginTimeOut = 996, //登陆超时
    OR_Error_ExitGame = 997, //游戏出错，强制玩家离开
    OR_Error_ServerOffLine = 998, //服务器未开启
    OR_Error_Disconnect = 999, //游戏断开连接
    OR_Error_Default = 10000, //操作失败

    eOR_Default = 50001,    // 操作失败
    eOR_DuplicateAccount = 50002,   // 用户名重复
    eOR_IncorrectInfo = 50003,      // 账号或密码错误
    eOR_Otherwhere = 50004,         // 此账号在其他地方登录
    eOR_SeverMaintain = 50005,      // 服务器维护
    eOR_VersionNotMatch = 50006,    // 版本不匹配
    eOR_NeedRoleDIY = 50007,        // 需要角色定制
    eOR_IP_Forbidden = 50008,       // IP封禁
    eOR_Account_Forbidden = 50009,  // 账号封禁
    eOR_Chat_Forbidden = 50009,     // 聊天封禁
    eOR_TokenErr = 50010,			// token错误
    eOR_DuplicateRoleName = 50021,	// 重复角色名

    eOR_Money = 50033,              // 银币不足
    eOR_Gold = 50034,               // 金转蛋不足
    eOR_Energy = 50035,             // 银转蛋不足

    eOR_OutOfTimeRange = 50050,         //超出时间范围限制
    eOR_InCoolingTime = 50051,          //在限制时间内
    eOR_NotDT = 50052,                  //找不到对应DT	
    eOR_NotUserRecord = 50053,          //没有对应的UserRecord
    eOR_NotAllowBuyCond = 50054,        //没有满足条件
    eOR_FormationNULL = 50055,          //卡牌站位为空
    eOR_OutOfLimitRange = 50056,                //超出范围限制
    eOR_ParamErr = 50057,				//参数表没有配置
};

public enum EM_GameStatic
{
    Waiting = 100,
    Gaming,
    Win,
    Lost,
}

public enum EM_Camera
{
    MainCamera = 0,
    BGCamera,
    SRCamera,
    InforCamera,
}

public enum EM_Sound
{
    /// <summary>
    /// 聲道1-遊戲操作
    /// </summary>
    Effect,
    /// <summary>
    /// 聲道2-遊戲背景
    /// </summary>
    Music,
    /// <summary>
    /// 語音
    /// </summary>
    Voice,
}

public enum EM_GameLevelAction
{
    Loop = 0,
    LoadLevel,
    CreateRes,
    Gaming,
    GamePause,
    GameFail,
    GameSuc,
}

/// <summary>
/// 玩家个人属性
/// </summary>
public enum EM_UserAttr
{
    eUserAttr_None = 0,
    eUserAttr_MainCard = 1,     // 主卡(确定头像)
    eUserAttr_IconFrame = 2,    // 头像边框
    eUserAttr_CardPower = 3,    // (上阵)卡牌战力
    eUserAttr_Level = 4,        // 等级
    eUserAttr_Exp = 5,          // 经验	
    eUserAttr_Money = 6,        // 银币(OG)
    eUserAttr_Sycee = 7,		// 元宝(G)

    /// <summary>
    /// 金转蛋
    /// </summary>
    eUserAttr_Gold,
    /// <summary>
    /// 银转蛋
    /// </summary>
    eUserAttr_Energy,
    /// <summary>
    /// 人种
    /// </summary>
    eUserAttr_Race,     //
    /// <summary>
    /// 男女(1男2女)
    /// </summary>
    eUserAttr_Sex,		//

    eUserAttr_Lottery99,
    eUserAttr_Lottery100,

    /// <summary>
    /// 國籍(新增)
    /// </summary>
    eUserAttr_Country,

    End1,
    /// <summary>
    /// 结束
    /// </summary>
    End,

};

public class GameEM
{

    public enum TeamType
    {
        Ero = 0,
        A = 10,
        B = 20,
        C = 30,
        D = 40,
        E = 50,
        Computer = 60,
    }


    public enum emRoleType
    {
        Player = 0,       //玩家
        Pet = 3,        //寵物
    }

    #region 換裝
    /// <summary>
    /// 角色身上的部件
    /// </summary>
    public enum EM_RolePart
    {
        //-----模型自身部件----------------------------------
        /// <summary>
        /// 模型自身部件
        /// </summary>
        Default = 0,

        //-------身体组成--------------------------------
        /// <summary>
        /// 頭部
        /// </summary>       
        head = 1,
        /// <summary>
        /// 手臂
        /// </summary>
        arm = 2,
        /// <summary>
        /// 腿部
        /// </summary>
        leg = 3,
        /// <summary>
        /// 鬍子
        /// </summary>
        beard = 4,
        /// <summary>
        /// 髮型
        /// </summary>
        hair = 5,
        /// <summary>
        /// 髮色
        /// </summary>
        hair_color = 6,
        /// <summary>
        /// 頭盔
        /// </summary>
        helmet = 7,
        /// <summary>
        /// 眼鏡
        /// </summary>
        goggles = 11,
        /// <summary>
        /// 上半身
        /// </summary>
        jersey = 12,
        /// <summary>
        /// 下半身
        /// </summary>
        shorts = 13,
        /// <summary>
        /// 手套
        /// </summary>
        gloves = 14,
        /// <summary>
        /// 襪子
        /// </summary>
        socks = 15,
        /// <summary>
        /// 鞋子
        /// </summary>
        shoes = 16,

        //--------配件-------------------------------
        /// <summary>
        /// 配件
        /// </summary>
        accessories = 17,
        /// <summary>
        /// 寵物
        /// </summary>
        pet = 18,
        /// <summary>
        /// 特殊造型
        /// </summary>
        cosplay = 19,

        skin = 20,          //膚色

        RoleEnd,

        //-------自行车部件--------------------------------
        /// <summary>
        /// 限量款車種
        /// </summary>
        bike = 50,  //腳踏車底層(腳踏車物件最底部的參考點，用來計算角色和地面的高度)
        /// <summary>
        /// 車架
        /// </summary>
        frame = 51,
        /// <summary>
        /// 車輪
        /// </summary>
        wheel = 52,

        dummy_all = 60,

        BikeEnd,

        //--------------------------------------
        animation = 99,       //動作


        //--------------------------------------
        /// <summary>
        /// 眼睛
        /// </summary>
        eyes = 101,
        /// <summary>
        /// 臉
        /// </summary>
        face = 102,
        /// <summary>
        /// 手指
        /// </summary>
        finger = 103,

        //--------------------------------------
        
        HairMenu = 1002,       //毛髮項目
        BikeMenu = 1003,    //腳踏車項目
        Deformer = 1004,     //變形
        Len = 1005,          //鏡頭
        GuildMenu = 1006,   //車隊商品
        WardMenu = 1007,    //換衣間總覽
        AllMenu = -99,
    }

    public enum EM_Deformer
    {
        //-------骨骼變形--------------------------------
        /// <summary>
        /// 身高
        /// </summary>
        BodyHeight = 1,
        /// <summary>
        /// 胸部
        /// </summary>
        Chest = 2,
        /// <summary>
        /// 手臂
        /// </summary>
        Hand = 3,
        /// <summary>
        /// 腰圍
        /// </summary>
        Waist = 4,
        /// <summary>
        /// 斜方肌
        /// </summary>
        Trapezius = 5,
        /// <summary>
        /// 臀部
        /// </summary>
        Buttocks = 6,
        /// <summary>
        /// 腿寬
        /// </summary>
        LegWidth = 7,
        /// <summary>
        /// 頭部
        /// </summary>
        Head = 8,

        LegHight = 9,
        HandHight = 10,
        Spine = 11,
        Shoulder = 12,
    }

    public enum EM_BikePart
    {
        //-------自行车部件--------------------------------
        dummy_all = 60,
        bike = 50,  //腳踏車底層(腳踏車物件最底部的參考點，用來計算角色和地面的高度)

        /// <summary>
        /// 前車輪
        /// </summary>
        dummy_wheel_front = 107,
        /// <summary>
        /// 後車輪
        /// </summary>
        dummy_wheel_back = 108,
        /// <summary>
        /// 左握把
        /// </summary>
        dummy_brakeL = 109,
        /// <summary>
        /// 右握把
        /// </summary>
        dummy_brakeR = 110,
        /// <summary>
        /// 龍頭車架
        /// </summary>
        mesh_bike_stem = 111,
        /// <summary>
        /// 主車架
        /// </summary>
        mesh_bike_frame = 112,
        /// <summary>
        /// 座位架
        /// </summary>
        mesh_bike_sadle = 113,
        /// <summary>
        /// 踏板
        /// </summary>
        dummy_crank = 114,
        /// <summary>
        /// 車架
        /// </summary>
        frame = 51,
        /// <summary>
        /// 車輪
        /// </summary>
        wheel = 52,
    }

    public enum EM_BikeIK
    {
        //自行車IK參考點
        bike = 1,  //腳踏車底層(腳踏車物件最底部的參考點，用來計算角色和地面的高度)
        LeftHandle = 2,    //左手把
        RightHandle = 3,   //右手把
        LeftFootBoard = 4, //左踏板
        RightFootBoard = 5,    //右踏板
        Seat = 6,     //座位
    }

    public enum EM_Gender
    {
#if UNITY_EDITOR
        [EnumAttirbute("通用")]
        Common = 0,
        [EnumAttirbute("男性")]
        Male = 1,
        [EnumAttirbute("女性")]
        Female = 2,
#else
        Common = 0,
        Male = 1,
        Female = 2,
#endif
        End,
    }

    public enum EM_BikeSize
    {
#if UNITY_EDITOR
        [EnumAttirbute("通用")]
        Common = 0,
        [EnumAttirbute("大車")]
        Large = 1,
        [EnumAttirbute("小車")]
        Little = 2,
#else
        Common = 0,
        Large = 1,
        Little = 2,
#endif
        End,
    }

    public enum EM_Race
    {
#if UNITY_EDITOR
        [EnumAttirbute("黑人")]
        Black,
        [EnumAttirbute("白人")]
        White,
        [EnumAttirbute("黃種人")]
        Asian,
#else
        Black,
        White,
        Asian,
#endif
        End,
    }

    public enum EM_DressType
    {
        /// <summary>
        /// 時裝
        /// </summary>
        Avatar = 1,
        /// <summary>
        /// 顏色
        /// </summary>
        Color = 2,
        /// <summary>
        /// 寵物
        /// </summary>
        Pet = 3,
        /// <summary>
        /// 組合包
        /// </summary>
        CombinPack = 4,
    }
#endregion
}