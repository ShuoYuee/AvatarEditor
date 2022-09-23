using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;


public class GameSocket : BaseSocket
{
    private Socket_Loop _Socket_Loop = null;
    private static GameSocket _Instance = null;
    private System.DateTime m_fLastTime;

    public static GameSocket GetInstance()
    {
        if (null == _Instance)
        {
            _Instance = new GameSocket();
        }

        return _Instance;
    }

    public GameSocket()
    {
        m_strTTTT = "GameSocket";
    }

    protected override void InitMachine()
    {
        base.InitMachine();

        Socket_Wait tSocket_Wait = new Socket_Wait(this);
        _Socket_Loop = new Socket_Loop(this);
        _SocketMachineManger = new ccMachineManager<ccMachineParamentBase>(_Socket_Loop);
        _SocketMachineManger.f_RegState(new Socket_Connect(this));
        _SocketMachineManger.f_RegState(new Socket_Login(this));
        _SocketMachineManger.f_RegState(new Socket_Wait(this));
        _SocketMachineManger.f_ChangeState(tSocket_Wait);

    }

    public override void f_Close()
    {
        base.f_Close();

    }

    #region 登陆相关

    public void f_Login(ccCallback func = null)
    {
        Socket_Login tSocket_Login = (Socket_Login)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Login);
        tSocket_Login.f_Login(func);
        _SocketMachineManger.f_ChangeState(tSocket_Login, func);
    }

    #endregion

    #region 内部消息处理

    public override void Destroy()
    {
        base.Destroy();
        GameSocket._Instance = null;
    }

    public override void f_Update()
    {
        base.f_Update();

    }

    #endregion

    #region 外部消息处理

    protected override void InitMessage()
    {
        base.InitMessage();

        //m_GMSocketMessagePool.f_AddListener(SocketCommand.GM_SCK_UNBUILD.ToString(), On_GM_SCK_UNBUILD, null);
        stGameCommandReturn tGameCommandRet = new stGameCommandReturn();
        f_AddListener((int)SocketCommand.CONTROL_CTG_OperateResult, tGameCommandRet, On_CMsg_GameCommandReturn);

        CMsg_GTC_AccountLoginRelt tCMsg_GTC_AccountLoginRelt = new CMsg_GTC_AccountLoginRelt();
        f_AddListener((int)SocketCommand.SC_UserAttrInit, tCMsg_GTC_AccountLoginRelt, On_RoleData);

        SC_UserAttr tSC_UserAttr = new SC_UserAttr();
        f_AddListener((int)SocketCommand.SC_UserAttr, tSC_UserAttr, On_Data_CTG_ChangeRoleData, 1);
        SC_AvaterBuf tSC_AvaterBuf = new SC_AvaterBuf();
        f_AddListener((int)SocketCommand.SC_AvaterBuf, tSC_AvaterBuf, On_Data_SC_AvaterBuf);
        SC_AvaterBuf tSC_SpecialAvaterBuf = new SC_AvaterBuf();
        f_AddListener((int)SocketCommand.CS_UpdateSpecialAvater, tSC_SpecialAvaterBuf, On_Data_SC_SpecialAvaterBuf);

        basicNode1 tbasicNode1 = new basicNode1();
        f_AddListener((int)SocketCommand.SC_RoleEnterGame, tbasicNode1, On_StartGame);

        //////////////////////////////////////////////////////////////////////////
        //DATA

        basicNode1 tPing = new basicNode1();
        f_AddListener((int)SocketCommand.PING_Reps, tPing, On_Ping);


        basicNode1 tSC_KickoutReason = new basicNode1();
        f_AddListener((int)SocketCommand.SC_Kickout, tSC_KickoutReason, f_KickoutByServer);



    }

    #endregion

    #region 被踢下线相关


    private void f_KickoutByServer(object value)
    {
        basicNode1 node1 = (basicNode1)value;
        eMsgOperateResult result = (eMsgOperateResult)node1.value1;
        if (result == eMsgOperateResult.OR_Error_ElseWhereLogin)
        {
            //TopPopupMenuParams param = new TopPopupMenuParams("警告", "账号在其他地方登陆", "退出游戏", f_QuitGameSureHandle);
            //ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, param);
        }
        else if (result == eMsgOperateResult.OR_Error_SeverMaintain)
        {
            //TopPopupMenuParams param = new TopPopupMenuParams("警告", "服务器维护，请重新登录", "退出游戏", f_QuitGameSureHandle);
            //ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, param);
        }
        else if (result == eMsgOperateResult.eOR_Account_Forbidden)
        {
            //TopPopupMenuParams param = new TopPopupMenuParams("警告", "您的账号因故已被封，如有异议，请联系客服", "退出游戏", f_QuitGameSureHandle);
            //ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, param);
        }
        else
        {
            //TopPopupMenuParams param = new TopPopupMenuParams("警告", "被服务器踢下线", "退出游戏", f_QuitGameSureHandle);
            //ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, param);
        }
    }

    private void f_QuitGameSureHandle(object value)
    {
        //退出游戏
        Application.Quit();
    }

    #endregion


    #region 时间

    public override void f_Ping()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        int iNum = f_SendBuf2Force((int)SocketCommand.PING, bBuf);

        MessageBox.DEBUG("Game Ping");
    }


    #endregion

    #region 玩家相关

    private void On_RoleData(object Obj)
    {
        if (Obj == null)
        {
            return;
        }
        CMsg_GTC_AccountLoginRelt tCMsg_GTC_AccountLoginRelt = (CMsg_GTC_AccountLoginRelt)Obj;
        PlayerDataTools.f_ServerData2GameData(tCMsg_GTC_AccountLoginRelt);
    }

    /// <summary>
    /// 修改玩家信息操作
    /// </summary>
    /// <param name="Obj"></param>
    private void On_Data_CTG_ChangeRoleData(object Obj)
    {
        if (Obj == null)
        {
            return;
        }
        SC_UserAttr tSC_UserAttr = (SC_UserAttr)Obj;
        PlayerDataTools.f_ChangePlayerData((EM_UserAttr)tSC_UserAttr.attrEnum, tSC_UserAttr.iValue);
    }

    /// <summary>
    /// 修改玩家Avater操作
    /// </summary>
    /// <param name="Obj"></param>
    private void On_Data_SC_AvaterBuf(object Obj)
    {
        if (Obj == null)
        {
            return;
        }
        SC_AvaterBuf tSC_AvaterBuf = (SC_AvaterBuf)Obj;
        PlayerDataTools.f_ChangeAvaterBuf(tSC_AvaterBuf.szAvaterBuf);
    }

    /// <summary>
    /// 修改玩家Avater操作
    /// </summary>
    /// <param name="Obj"></param>
    private void On_Data_SC_SpecialAvaterBuf(object Obj)
    {
        if (Obj == null)
        {
            return;
        }
        SC_AvaterBuf tSC_AvaterBuf = (SC_AvaterBuf)Obj;
        PlayerDataTools.f_ChangeSpecialAvaterBuf(tSC_AvaterBuf.szAvaterBuf);
    }
    #endregion

    /// <summary>
    /// 接受服务器进入游戏指令
    /// </summary>
    /// <param name="Obj"></param>
    private void On_StartGame(object Obj)
    {

    }
}//END Class