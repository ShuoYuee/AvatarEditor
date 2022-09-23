using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

//协议操作类型
public enum eMsgOperateType
{
    OT_NULL = -99,      //不存在错误码

    OT_CreateAccount = 0, // 创建帐号
    OT_LoginGame = 1, // 登陆游戏
};


//角色属性枚举
public enum eChangeRoleDataType
{
    eDefault = 0,
    eAccountId = 1000,      //账号ID AccountId=UserId

    eGUID = 1001,			//uid
    eLevel = 1002,       //等级	
    eRank = 1003,		//玩家rank
    eLastTime = 1004,	//Last_Login 最后离线时间	
    eCityId = 1005,	//看板娘模板Id

    eExp = 1007,		//经验

    eGold = 1008,		//金幣
    eToken = 1009,		//魔法石

    eAdvanecPP = 1010,			//AP
    eBp = 1011,			//BP

    eNoobStep = 1015,
    eBan = 1016,

    eActive = 1017,
    eVisitor = 1018,
    eMoney = 1019,

}
