using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelControll_LoadLevel : GameControllBaseState
{
    public GameLevelControll_LoadLevel( )
        : base((int)EM_GameLevelAction.LoadLevel)
    {
        

    }


    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);

        int iLevel = (int)Obj;
        //StaticValue.m_GameLevelDT = (GameLevelDT) glo_Main.GetInstance().m_SC_Pool.m_GameLevelSC.f_GetSC(iLevel);

        f_SetComplete((int)EM_GameLevelAction.CreateRes);
    }




}
