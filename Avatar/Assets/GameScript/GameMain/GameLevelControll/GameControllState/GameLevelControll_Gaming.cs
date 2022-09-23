using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelControll_Gaming : GameControllBaseState
{
    public GameLevelControll_Gaming( )
        : base((int)EM_GameLevelAction.Gaming)
    {
        

    }


    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.LockPlayerControll, false);
    }




}
