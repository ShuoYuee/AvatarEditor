using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using System;

public class RoleModelCallbackEvent : MonoBehaviour
{
    BaseRoleControllV2 _BaseRoleControl;               //獲取自己用

    private ccCallback _CallBack_Grab;              //抓玩家
    private ccCallback _CallBack_Attacking;         //攻擊
    private ccCallback _CallBack_AttackComplete;    //攻擊完成
    private ccCallback _CallBack_RecvAnimatorEvent; //動畫結束

    private ccCallback _CallBack_ThrowStart;        // 開始投擲
    private ccCallback _CallBack_ThrowEnd;          //結束投擲

    public void f_RegCallbackEvent(
        ccCallback tAttacking, 
        ccCallback tAttackComplete, 
        //ccCallback tRecvAnimatorEvent,
        ccCallback tThrowStart,
        ccCallback tThrowEnd
        )
    {
        //_CallBack_Grab = tGrab;
        _CallBack_Attacking = tAttacking;
        _CallBack_AttackComplete = tAttackComplete;
        //_CallBack_RecvAnimatorEvent = tRecvAnimatorEvent;
        _CallBack_ThrowStart = tThrowStart;
        _CallBack_ThrowEnd = tThrowEnd;
    }


    void Start(){
        _BaseRoleControl = this.GetComponent<BaseRoleControllV2>();  //獲取自己
    }


    ////(在 Animation的 Key上添加以下事件)
    #region 01. 攻击相关 ======================================================================
    // OnAttacking()：
    // 從 BaseActionController.cs 來指定上面 f_RegCallbackEvent() 裡的項目
    // 而 指定的項目來自 BaseRoleControl.cs 的 f_CallBack_Attacking(object Obj) 
    // 而 f_CallBack_Attacking()  又去執行 BaseAIControl.cs 的 f_Attack()
    // 然後 f_Attack() 又會去調用 角色身上 AI類型是Attack 的 AI_(名字).cs 上的 f_Attack()
    
    public void OnAttacking(){
        if (_CallBack_Attacking != null){
            _CallBack_Attacking(null);
        }
    }

    public void OnAttackComplete(){
        if (_CallBack_AttackComplete != null){
            _CallBack_AttackComplete(null);
            
        }
    }
    #endregion



    #region 丟東西相关 ======================================================================

    public void Mv_ThrowStart()
    {
        if (_CallBack_ThrowStart != null)
        {
            _CallBack_ThrowStart(null);
        }
    }

    public void Mv_ThrowEnd()
    {
        if (_CallBack_ThrowEnd != null)
        {
            _CallBack_ThrowEnd(null);
        }
    }

    #endregion





    #region 02. 動畫相关 ======================================================================
    /// <summary>
    /// 接收动画自定义事件
    /// </summary>
    /// <param name="strData">动画传入参数</param>
    public void OnRecvAnimatorEvent(string strData){
        if (_CallBack_RecvAnimatorEvent != null){
            _CallBack_RecvAnimatorEvent(null);
        }
    }

    /// <summary>
    /// 設置 Animator 上 (parameter)"BeAttack" 的值
    /// </summary>
    /// <param name="value">開關狀態 (1為開啟，0為關閉)</param>
    public void setBeAttack(int value){
        _BaseRoleControl.GetComponent<Animator>().SetInteger("BeAttack", value);
    }

    /// <summary>
    /// 控制動畫能不能位移 (0=關 / 1=開)
    /// </summary>
    public void SetRootMotion(int tmp) {
        if (tmp == 0) {
            _BaseRoleControl.GetComponent<Animator>().applyRootMotion = false;
        }
        if (tmp == 1) {
            _BaseRoleControl.GetComponent<Animator>().applyRootMotion = true;
        }
    }
    #endregion


    #region 03. 遊戲相关 ======================================================================
    /// <summary> 銷毀 </summary>
    public void aniDestroy(){
        //_BaseRoleControl.m_bIsComplete = true;
        //GameMain.GetInstance().f_RoleDie2(_BaseRoleControl);
        ccTimeEvent.GetInstance().f_RegEvent(3, false, null, CallBack_Destory);
    } void CallBack_Destory(object Obj){ _BaseRoleControl.f_Destory();}


    /// <summary> 遊戲以勝利結束 </summary>
    public void GameWin(){
        //如果遊戲還在進行中，以勝利結束遊戲
        //if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming) {
        //    glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEOVER, EM_GameResult.Win);
        //}
        //GameMain.GetInstance().OpenUI_Win();
    }

    /// <summary> 遊戲以失敗結束 </summary>
    public void GameOver(){
        //如果遊戲還在進行中，以失敗結束遊戲
        MessageBox.DEBUG("RoleModelCallbackEvent.GameOver");
        //if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming) {
        //    glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEOVER, EM_GameResult.Lost);
        //}
    }


   

    /// <summary>
    /// 普通刪除
    /// </summary>
    public void LocalDestroy(){
        Destroy(this.gameObject);
    }
    #endregion





}


