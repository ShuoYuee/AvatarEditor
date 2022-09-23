using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class Socket_Login : Socket_StateBase
{
    private stIp _stIp;
    private ccCallback _LoginCallbackFunc;
    private System.DateTime _dtLoginTimeOut;
    private int _iReLogin = 0;
    private int _iReLoginTimeId = -99;
    public Socket_Login(BaseSocket tBaseSocket)
        : base((int)EM_Socket.Login, tBaseSocket)
    {

    }

    public void f_Login(ccCallback func = null)
    {
        _LoginCallbackFunc = func;

        CMsg_GTC_LoginRelt tCMsg_GTC_LoginRelt = new CMsg_GTC_LoginRelt();
        _BaseSocket.f_AddListener((int)SocketCommand.SC_UserLogin, tCMsg_GTC_LoginRelt, On_LoginSuc);
        //basicNode1 tsGameStartNotfy = new basicNode1();
        //_GameSocket.f_AddListener((int)SocketCommand.GTC_GameStartNotfy, tsGameStartNotfy, On_GTC_GameStartNotfy);
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        MessageBox.DEBUG("登陆....");

        _stIp.m_szIp = GloData.glo_strSvrIP;
        _stIp.m_iPort = GloData.glo_iSvrPort;

        Login(null);
    }

    private void Login(object Obj)
    {
        _iReLoginTimeId = -99;
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.ConnectSuc)
        {
            SendLogin(_iReLogin);
        }
        else
        {
            Socket_Connect tSocket_Connect = (Socket_Connect)f_GetOtherStateBase((int)EM_Socket.Connect);
            tSocket_Connect.f_SetIpInfor(_stIp);
            f_SetComplete((int)EM_Socket.Connect, this);
        }
    }

    public override void f_Execute()
    {
        //if ((System.DateTime.Now - _dtLoginTimeOut).TotalSeconds > 30)
        //{
        //    CMsg_GTC_LoginRelt tCMsg_GTC_LoginRelt = new CMsg_GTC_LoginRelt();
        //    tCMsg_GTC_LoginRelt.m_result = (int) eMsgOperateResult.OR_Error_LoginTimeOut;
        //    On_LoginSuc(tCMsg_GTC_LoginRelt);
        //}
    }
    
    private void SendLogin(int iReLogin = 0)
    {        
        _dtLoginTimeOut = System.DateTime.Now;

        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iReLogin);
        tCreateSocketBuf.f_Add(0);          // Data_Pool.m_UserData.m_iServerId);
        tCreateSocketBuf.f_Add(GloData.glo_iVer);
        tCreateSocketBuf.f_Add(0);
        tCreateSocketBuf.f_Add(0);
        tCreateSocketBuf.f_Add(StaticValue.m_LoginName, 32);
        tCreateSocketBuf.f_Add(StaticValue.m_LoginPwd, 28);
        _BaseSocket.f_SendBuf2Force((int)SocketCommand.CS_UserLogin, tCreateSocketBuf.f_GetBuf());

        _BaseSocket.f_SetSocketStatic(EM_SocketStatic.Logining);

        //CMsg_GTC_LoginRelt tCMsg_GTC_LoginRelt = new CMsg_GTC_LoginRelt();
        //tCMsg_GTC_LoginRelt.m_result = (int)eMsgOperateResult.OR_Succeed;
        //On_LoginSuc(tCMsg_GTC_LoginRelt);
    }

    private void On_LoginSuc(object Obj)
    {
		/*
        if (_GameSocket.f_GetSocketStatic() == EM_SocketStatic.LoginEro)
        {
            return;
        }
        */
        CMsg_GTC_LoginRelt tCMsg_GTC_LoginRelt = (CMsg_GTC_LoginRelt)Obj;
        //_GameSocket.f_RemoveListener(SocketCommand.GTC_AccountEnterResult);
        MessageBox.DEBUG("登陆返回：" + tCMsg_GTC_LoginRelt.m_result);
        if (_LoginCallbackFunc != null)
        {
            _LoginCallbackFunc(tCMsg_GTC_LoginRelt.m_result);
            _LoginCallbackFunc = null;            
        }
        if (tCMsg_GTC_LoginRelt.m_result == (int)eMsgOperateResult.OR_Succeed)
        {
            _iReLogin = 1;
            _BaseSocket.f_SetSocketStatic(EM_SocketStatic.OnLine);
            _BaseSocket.UpdateServerTime(tCMsg_GTC_LoginRelt.m_iServerTime);

           
            //MessageBox.DEBUG("登陆成功, 玩家Id:" + Data_Pool.m_UserData.m_iUserId);

            _BaseSocket.f_Ping();
            f_SetComplete((int)EM_Socket.Loop);
        }     
        else
        {
            if (_iReLoginTimeId <= 0)
            {
                _BaseSocket.f_Close();
                //if ((eMsgOperateResult)tCMsg_GTC_LoginRelt.m_result == eMsgOperateResult.OR_Error_AccountRepetition ||    // = 20, // 注册：账号重复
                //    (eMsgOperateResult)tCMsg_GTC_LoginRelt.m_result == eMsgOperateResult.OR_Error_NoAccount ||    // = 21 ||    //, // 登陆：账号不存在
                //    (eMsgOperateResult)tCMsg_GTC_LoginRelt.m_result == eMsgOperateResult.OR_Error_Password ||    // = 22 ||    //, // 登陆：密码错误
                //    (eMsgOperateResult)tCMsg_GTC_LoginRelt.m_result == eMsgOperateResult.OR_Error_AccountOnline)    // = 24)    //, // 登陆：账号在线)
                //{
                //    _GameSocket.f_SetSocketStatic(EM_SocketStatic.LoginEro);
                //}
                _BaseSocket.f_SetSocketStatic(EM_SocketStatic.LoginEro);


                eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)tCMsg_GTC_LoginRelt.m_result;
                MessageBox.DEBUG("登陆失败。 " + teMsgOperateResult.ToString());
                MessageBox.DEBUG("重新登陆。 ");
                _iReLoginTimeId = ccTimeEvent.GetInstance().f_RegEvent(10f, false, null, Login);
            }            
        }

    }


#region 游戏开始协议

    private void On_GTC_GameStartNotfy(object Obj)
    {
        //basicNode0 tsForceOffline = (basicNode0)Obj;
        MessageBox.DEBUG("游戏开始 1111111111111。 ");
        //if (_LoginCallbackFunc != null)
        //{
        //    _LoginCallbackFunc((object)eMsgOperateResult.OR_Succeed);
        //    _LoginCallbackFunc = null;
        //}
    }

#endregion


}
