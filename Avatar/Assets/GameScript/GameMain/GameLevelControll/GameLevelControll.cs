using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelControll
{
    private ccMachineManager<ccMachineParamentBase> _GameLevelControllMachineManager = null;

    public GameLevelControll()
    {
        _GameLevelControllMachineManager = new ccMachineManager<ccMachineParamentBase>(new GameLevelControll_Loop()); //Loop

        RegState(new GameLevelControll_LoadLevel());
        RegState(new GameLevelControll_CreateRes());
        RegState(new GameLevelControll_Gaming());
        RegState(new GameLevelControll_Pause());

    }

    void RegState(GameControllBaseState tccMachineStateBase)
    {
        _GameLevelControllMachineManager.f_RegState(tccMachineStateBase);
    }

    public void f_Start(int iStateId)
    {
        _GameLevelControllMachineManager.f_ChangeState((int)EM_GameLevelAction.LoadLevel, iStateId);
    }

    public void f_Update()
    {
        _GameLevelControllMachineManager.f_Update();
    }

    /// <summary>
    /// 更改遊戲狀態
    /// </summary>
    /// <param name="iStateId">狀態ID</param>
    public void f_CangeGameState(int iStateId, object Obj = null)
    {
        _GameLevelControllMachineManager.f_ChangeState(iStateId, Obj);
    }

    public bool f_IsGaming()
    {
        ccMachineStateBase<ccMachineParamentBase> tccMachineStateBase = _GameLevelControllMachineManager.f_GetCurMachineState();
        if (tccMachineStateBase != null)
        {
            if (tccMachineStateBase.iId == (int)EM_GameLevelAction.Gaming)
            {
                return true;
            }
        }
        return false;
    }


}
