using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelControll_Pause : GameControllBaseState
{
    public GameLevelControll_Pause() : base((int)EM_GameLevelAction.GamePause)
    {

    }

    public override void f_Enter(object Obj)
    {
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.LockPlayerControll, true);
    }
}
