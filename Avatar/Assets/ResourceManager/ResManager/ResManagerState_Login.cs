using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class ResManagerState_Login : ccMachineStateBase<ccMachineParamentBase>
{
    static EM_ResManagerStatic m_EM_AIStatic = EM_ResManagerStatic.Login;

    ccCallback _ccCallback = null;
    public ResManagerState_Login(ccCallback tccCallback)
        : base((int)m_EM_AIStatic)
    {
        _ccCallback = tccCallback;
    }

    public override void f_Enter(object Obj)
    {
        _ccCallback(Obj);
    }





}