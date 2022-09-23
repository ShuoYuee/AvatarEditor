using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class Socket_Loop : Socket_StateBase
{
    private int _iPingId = -99;
    private System.DateTime m_dtPingTime;
    private bool _bTestTime = false;

    public Socket_Loop(BaseSocket tBaseSocket)
        : base((int)EM_Socket.Loop, tBaseSocket)
    {

    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        f_UpdateTestTime();
        if (_iPingId == -99)
        {
            _iPingId = ccTimeEvent.GetInstance().f_RegEvent(GloData.glo_iPingTime, true, null, Callback_Ping);
        }        
    }

    public override void f_Execute()
    {
        base.f_Execute();

        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OnLine)
        {
            if (TestSocketOnline())// && _BaseSocket.f_CheckHaveBuf())
            {
                if (!_bTestTime)
                {
                    _BaseSocket.f_DispSendCatchBuf();
                }
            }
            else
            {
                ccTimeEvent.GetInstance().f_UnRegEvent(_iPingId);
                _iPingId = -99;
                _BaseSocket.f_Close();
            }
        }
        else if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OffLine)
        {
            f_SetComplete((int)EM_Socket.Login, -99);
        }
        else 
        {
            MessageBox.ASSERT("Socket状态错误 " + _BaseSocket.f_GetSocketStatic().ToString());
        }        
    }

    private bool TestSocketOnline()
    {
        if (_BaseSocket.f_TestSocket())       
        {
            //if (!glo_Main.GetInstance().m_StarSDK.f_CheckIsPaying())
            //{
                if (PingTimeOut())
                {
                    return false;
                }
            //}
        }
        else
        {           
            return false;
        }
        return true;
    }

    public void f_UpdateTestTime()
    {
        _iRetryTimes = 0;
        _bTestTime = false;
        m_dtPingTime = System.DateTime.Now;
    }
    
    private void Callback_Ping(object Obj)
    {
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OnLine)
        {
            //int iTime = (int)(System.DateTime.Now - _BaseSocket.m_dtSocketTimeout).TotalSeconds;
            //if (iTime > GloData.glo_iPingTime)
            //{
                _iRetryTimes = 0;
                _bTestTime = true;
                m_dtPingTime = System.DateTime.Now;
                _BaseSocket.f_Ping();
            //}
        }
    }

    private int _iRetryTimes = 0;
    private bool PingTimeOut()
    {
        if (_bTestTime)
        {
            int iTime = (int)(System.DateTime.Now - m_dtPingTime).TotalSeconds;
            if (iTime > 5)
            {
                if (_iRetryTimes > 2)
                {
                    f_UpdateTestTime();
                    MessageBox.DEBUG("PingTimeOut 网络超时。");
                    //_BaseSocket.f_Close();
                    return true;
                }
                else
                {
                    m_dtPingTime = System.DateTime.Now;
                    _BaseSocket.f_Ping();
                    _iRetryTimes++;
                }
            }           
        }
        return false;
        //int iTime = (int)(System.DateTime.Now - _BaseSocket.m_dtSocketTimeout).TotalSeconds;
        //if (iTime > GloData.glo_iRecvPingTime)
        //{
        //    MessageBox.DEBUG("BufTimeOut 网络超时。");
        //    //_BaseSocket.f_Close();
        //    return true;
        //}
        //return false;
    }

}
