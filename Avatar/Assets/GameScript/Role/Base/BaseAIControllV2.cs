using ccU3DEngine;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI控制器基类
/// </summary>
public class BaseAIControllV2
{
    //List<AI_RunBaseStateV2> _aRunAIAction = new List<AI_RunBaseStateV2>();
    protected ccMulMachineManager _ConditionAIMachineManger = null;
    //protected ccMulMachineManager _RunAIMachineManger = null;
    private BaseRoleControllV2 _RoleControl;

    public BaseAIControllV2(BaseRoleControllV2 tRoleControl)
    {
        _RoleControl = tRoleControl;
        InitDefaultAI();
        //CharacterAIConditionDT tCharacterAIConditionDT = null;
        //if (_RoleControl.f_GetAI() != null)
        //{
        //    int[] aAI = ccMath.f_String2ArrayInt(_RoleControl.f_GetAI(), ";");
        //    for (int i = 0; i < aAI.Length; i++)
        //    {
        //        if (aAI[i] <= 0)
        //        {
        //            continue;
        //        }
        //        CharacterAIDT tCharacterAIDT = (CharacterAIDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAISC.f_GetSC(aAI[i]);
        //        tCharacterAIConditionDT = (CharacterAIConditionDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAIConditionSC.f_GetSC(ccMath.atoi(tCharacterAIDT.szMainAI));
        //        AI_ConditionBaseStateV2 tAI_ConditionBaseStateV2 = AIManager.GetInstance().f_CreateConditionAI(_RoleControl, tCharacterAIDT, tCharacterAIConditionDT);
        //        f_RegConditionAIState(tAI_ConditionBaseStateV2);

        //        //注册子任务相关
        //        if (tCharacterAIDT.szSubAI != null && tCharacterAIDT.szSubAI.Length > 0)
        //        {
        //            int[] aSubAI = ccMath.f_String2ArrayInt(tCharacterAIDT.szSubAI, ";");
        //            for (int a = 0; a < aSubAI.Length; a++)
        //            {
        //                if (aSubAI[a] <= 0)
        //                {
        //                    continue;
        //                }
        //                CharacterAIDT tSubCharacterAIDT = (CharacterAIDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAISC.f_GetSC(aSubAI[a]);
        //                if (tSubCharacterAIDT.szSubAI != null && tSubCharacterAIDT.szSubAI.Length > 0)
        //                {
        //                    MessageBox.ASSERT("子条件AI不能再包含子条件，创建子条件AI失败 Id = " + tSubCharacterAIDT.iId);
        //                }
        //                tCharacterAIConditionDT = (CharacterAIConditionDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAIConditionSC.f_GetSC(ccMath.atoi(tSubCharacterAIDT.szMainAI));
        //                AI_ConditionBaseStateV2 tAI_SubConditionBaseStateV2 = AIManager.GetInstance().f_CreateConditionAI(_RoleControl, tSubCharacterAIDT, tCharacterAIConditionDT);
        //                tAI_ConditionBaseStateV2.f_RegSubState(tAI_SubConditionBaseStateV2);
        //            }
        //        }

        //    }
        //}
       
    }

    /// <summary>
    /// 默认AI，必须AI自动增加
    /// </summary>
    void InitDefaultAI() {

        ////玩家AI
        //if (_RoleControl.f_GetRoleType() == GameEM.emRoleType.Player) {
        //    CharacterAIRunDT tCharacterAIRunDT = null;

        //    //AI_PlayerDie tmpPlayerAI_1 = new AI_PlayerDie();
        //    //tCharacterAIRunDT = (CharacterAIRunDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterAIRunSC.f_GetSC(1000);
        //    //tmpPlayerAI_1.f_Init(_RoleControl, null, tCharacterAIRunDT);
        //    //f_RegRunAIState(tmpPlayerAI_1);

        //    //AI_PlayerIdle tmpPlayerAI_2 = new AI_PlayerIdle();
        //    //tmpPlayerAI_2.f_Init(_RoleControl, null, null);
        //    //f_RegRunAIState(tmpPlayerAI_2);
        //}

        ////怪物默认AI
        //else {
        //    AI_WaitActionRun tAI_Wait = new AI_WaitActionRun();
        //    tAI_Wait.f_Init(_RoleControl, null, null);
        //    f_RegRunAIState(tAI_Wait);
        //    AI_Die tAI_Die = new AI_Die();
        //    tAI_Die.f_Init(_RoleControl, null, null);
        //    f_RegRunAIState(tAI_Die);
        //}
    }

    //#region 条件状态机
    ///// <summary>
    ///// 向控制器增加新的AI动作
    ///// </summary>
    //void f_RegConditionAIState(AI_ConditionBaseStateV2 tAI_BaseState)
    //{
    //    if (_ConditionAIMachineManger == null)
    //    {
    //        _ConditionAIMachineManger = new ccMulMachineManager(new AI_WaitAction2(_RoleControl), false, true);
    //    }
    //    _ConditionAIMachineManger.f_RegState(tAI_BaseState);
    //}

    //#endregion
    
    public void f_Update()
    {
        if (_ConditionAIMachineManger != null)
        {
            _ConditionAIMachineManger.f_Update();
        }
        //if (_RunAIMachineManger != null)
        //{
        //    _RunAIMachineManger.f_Update();
        //}
    }

  
    


}
