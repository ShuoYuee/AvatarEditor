using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class ResManagerState_Loop : ccMachineStateBase<ccMachineParamentBase>
{
    static EM_ResManagerStatic m_EM_AIStatic = EM_ResManagerStatic.Loop;


    public ResManagerState_Loop()
        : base((int)m_EM_AIStatic)
    {
        
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
    }

    //public override void f_Execute()
    //{
    //    _fSleepTime += Time.deltaTime;
    //    if (_fSleepTime > _fMaxSleepTime)
    //    {
    //        f_SetComplete();
    //        return;
    //    }

    //    base.f_Execute();
    //}



}
