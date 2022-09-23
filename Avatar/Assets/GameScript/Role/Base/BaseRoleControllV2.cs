using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using static GameEM;

public class TileNode
{

}

/// <summary>
/// 角色控制器基类
/// </summary>
public class BaseRoleControllV2 : MonoBehaviour
{
    public string ppSQL;

    /// <summary>
    /// 是否显示测试用的Tile信息
    /// </summary>
    public bool m_ShowTile = false;

    public Transform m_RoleModel
    {
        get
        {
            return transform;
        }
    }

    /// <summary>
    /// AI控制器
    /// </summary>
    protected BaseAIControl _BaseAI = null;
    protected BaseActionController _BaseActionController;
    public BaseRoleProtecy _BaseRoleProtecy;

    protected bool _bInitOK = false;
    private bool _bLockAI = false;

    /// <summary>
    /// 身體被攻擊的碰撞體
    /// </summary>
    private Collider[] _Colliders;

    protected Rigidbody _Rigidbody = null;

    public int m_iId
    {
        get
        {
            return _BaseRoleProtecy.m_iId;
        }
    }



    void Awake()
    {
        
    }

    public Transform f_GetRoleModel()
    {
        return transform;
    }

    public Rigidbody f_GetRigidbody()
    {
        return _Rigidbody;
    }


    void Update()
    {
        if (!_bInitOK)
        {
            return;
        }
        if (!_bLockAI)
        {
            if (_BaseAI != null)
            {
                _BaseAI.f_Update();
            }
        }
        if (_BaseActionController != null)
        {
            _BaseActionController.f_Update();
        }
        f_Excute();
    }

    protected virtual void f_Excute()
    {

    }

    #region 初始相关
    /// <summary>
    /// 初始控制器信息
    /// </summary>
    /// <param name="iId"></param>
    /// <param name="tBasePlayerControll"></param>
    /// <param name="tCharacterDT"></param>
    /// <param name="tTileNode"></param>
    /// <param name="bUpdatePos"></param>
    public virtual void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT)
    {
        _BaseRoleProtecy = new BaseRoleProtecy(iId, tCharacterDT, tTeamType);
        _BaseAI = new BaseAIControl(this);
        gameObject.name = _BaseRoleProtecy.m_iId + "_" + tCharacterDT.szName;
        _BaseActionController = tBaseActionController;

        InitAI();
        InitModel();
        InitMessage();

        _bInitOK = true;
    }

    protected virtual void InitMessage()
    {
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.LockPlayerControll, OnLockPlayerControll);
    }

    void OnLockPlayerControll(object Obj)
    {
        _bLockAI = (bool)Obj;
    }

    protected virtual void InitModel()
    {
        _Rigidbody = m_RoleModel.GetComponent<Rigidbody>();

        f_ChangeBodySize(_BaseRoleProtecy.f_BodySize());

    }

    public void f_ChangeBodySize(float fSize)
    {
        _BaseRoleProtecy.f_ChangeBodySize(fSize);
        CapsuleCollider tCapsuleCollider = m_RoleModel.GetComponent<CapsuleCollider>();
        if (tCapsuleCollider == null)
        {
            tCapsuleCollider = m_RoleModel.gameObject.AddComponent<CapsuleCollider>();
        }
        //tCapsuleCollider.height = _BaseRoleProtecy.f_GetHeight();
        tCapsuleCollider.radius = fSize;
    }

    #region AI

    protected virtual void InitAI()
    {

    }

    /// <summary>
    /// 向控制器增加新的AI动作
    /// </summary>
    protected void RegNewAIState(AI_BaseState tAI_BaseState)
    {
        _BaseAI.f_RegNewAIState(tAI_BaseState);
    }

    /// <summary>
    /// 讓控制器切換狀態
    /// </summary>
    /// <param name="tEM_AIState">切換的狀態</param>
    /// <param name="Obj">傳送的參數</param>
    public virtual void f_ChangeAIState(AI_EM.EM_AIState tEM_AIState, object Obj = null)
    {
        _BaseAI.f_ChangeAIState(tEM_AIState, Obj);
    }

    public AI_EM.EM_AIState f_GetCurAIState()
    {
        if (_BaseAI == null)
        {
            MessageBox.ASSERT("BaseAI 错误" + m_iId);
            return AI_EM.EM_AIState.None;
        }
        return _BaseAI.f_GetRoleCurState();
    }

    #endregion

    #region 碰撞相关
    ///// <summary>
    ///// 獲取所有碰撞
    ///// </summary>
    //public void f_GetBodyCollider()
    //{
    //    _Colliders = GetComponentsInChildren<Collider>(); //獲取所有碰撞
    //}

    public Collider f_GetBodyCollider()
    {
        return m_RoleModel.GetComponent<Collider>();
    }

    /// <summary>
    /// 開啟或關閉身體碰撞
    /// </summary>
    /// <param name="tmp"> true=開 / false=關 </param>
    public void f_SetBodyCollider(bool tmp)
    {
        for (int i = 0; i < _Colliders.Length; i++)
        {
            _Colliders[i].enabled = tmp;
        }
    }
    #endregion

    #endregion


    #region 动作相关

    /// <summary>
    /// 操作相应动作
    /// </summary>
    /// <param name="tAIState"></param>
    public void f_PlayAction(AI_EM.EM_AIState tAIState, ccCallback tccCallback = null)
    {
        bool bNeedStop = false;

        if (_BaseActionController.f_GetCurAIStatic() == AI_EM.EM_AIState.Walk && tAIState != AI_EM.EM_AIState.Walk)
        {
            bNeedStop = true;
        }
        _BaseActionController.f_PlayAction(tAIState, tccCallback);
        if (bNeedStop)
        {
            f_StopWalk();
        }
    }

    public virtual void f_Walk()
    {

    }

    public virtual void f_Attack(ccCallback tccCallback = null)
    {

    }

    public virtual void f_StopAttack()
    {
        _BaseActionController.f_StopAttack();
    }

    public virtual void f_StopWalk()
    {

    }

    public virtual void f_PlayAnim(object Obj, ccCallback CallBack_Complete = null)
    {

    }
    #endregion

    #region 死亡相关
    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void f_Die()
    {
        MessageBox.DEBUG("死亡:" + m_iId);

        f_PlayDie();
        //_BaseRoleProtecy.f_LostHP(999999);
    }

    /// <summary>
    /// 是否死亡了
    /// </summary>
    /// <returns></returns>
    /// -----------------------------------------------------
    public bool f_IsDie()
    {
        /*if (_BaseRoleProtecy.f_GetHp() <= 0)
        {
            return true;
        }*/

        return false;
    }

    /// <summary>
    /// 执行角色回收销毁
    /// </summary>
    public void f_Destory()
    {
        if (this == null)
        {
            return;
        }

        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.LockPlayerControll, OnLockPlayerControll);
        if (_BaseAI != null)
        {
            _BaseAI = null;
        }
        if (_BaseActionController != null)
        {
            _BaseActionController = null;
        }
        Destroy(gameObject);

    }

    #endregion


    #region 被攻击血量相关
    /*/// <summary>
    /// 被攻击
    /// </summary>
    /// <param name="iHP">受到的傷害</param>
    /// <param name="AttackRole">攻击发起者</param>
    /// <param name="tBodyPart">被攻击部位</param>
    public virtual void f_BeAttack(int iHP, BaseRoleControllV2 AttackRole, GameEM.EM_BodyPart tBodyPart)
    {
        if (f_IsDie())
        {
            return;
        }
        else if (f_IsInv())
        {
            return;
        }
        LostHP(iHP);

        //处理死亡
        if (f_GetHp() <= 0)
        {
            f_Die();
            DispScore(AttackRole.m_iId, m_iId, tBodyPart, true);
        }
        else
        {
            DispScore(AttackRole.m_iId, m_iId, tBodyPart, false);
        }
    }*/

    public virtual void f_PlayDie()
    {

    }

    /*/// 加血
    /// </summary>
    /// <param name="iHP"></param>
    public virtual void f_AddHp(int iHP)
    {
        if (f_IsDie())
        {
            return;
        }
        _BaseRoleProtecy.f_AddHp(iHP);
        UpdateHP();
    }

    public virtual void f_ChangeHP(int iHP)
    {
        if (f_IsDie())
        {
            return;
        }
        _BaseRoleProtecy.f_ChangeHP(iHP);
        UpdateHP();
    }

    /// <summary>
    /// 恢复满血
    /// </summary>
    public void f_FullHP()
    {
        _BaseRoleProtecy.f_FullHP();
        UpdateHP();
    }


    /// <summary>
    /// 掉血
    /// </summary>
    /// <param name="iHP"></param>
    protected void LostHP(int iHP)
    {
        _BaseRoleProtecy.f_LostHP(iHP);
        //更新血条
        UpdateHP();
    }

    /// <summary>
    /// 鎖血
    /// </summary>
    /// <param name="iHP"></param>
    public void f_LockHp(float iHP)
    {
        if (f_IsDie())
        {
            return;
        }
        _BaseRoleProtecy.f_LockHp(iHP);
    }

    /// <summary>
    /// 更新血量显示
    /// </summary>
    protected virtual void UpdateHP()
    {

    }

    /// <summary>
    /// 無視無敵強制扣血
    /// </summary>
    /// <param name="iHP"></param>
    public virtual void f_ForceBeAttack(int iHP)
    {
        LostHP(iHP);
        //处理死亡
        if (f_GetHp() <= 0)
        {
            f_Die();
        }
    }

    /// <summary>
    /// 獲取當前血量
    /// </summary>
    public int f_GetHp()
    {
        return _BaseRoleProtecy.f_GetHp();
    }

    public int f_GetMaxHp()
    {
        return _BaseRoleProtecy.f_GetMaxHp();
    }*/

    #endregion


    #region 无敌相关
    private bool _bIsInvincible = false;

    /// <summary>
    /// 是否為無敵
    /// </summary>
    public bool f_IsInv()
    {
        return _BaseRoleProtecy.f_IsInv();
    }

    /// <summary>
    /// 設定是否為無敵狀態, 此无敌设置权限比脚本中的无敌权限大
    /// </summary>
    public void f_SetInv(bool bInvincible)
    {
        _BaseRoleProtecy.f_SetInv(bInvincible);
    }
    #endregion

    #region 生命
    /// <summary>
    /// 查询还有几点生命
    /// </summary>
    public int f_GetHaveLife()
    {
        return _BaseRoleProtecy.f_GetHaveLife();
    }


    /// <summary>
    /// 重生(?)
    /// </summary>
    public virtual void f_Rebirth()
    {
        //f_FullHP();
        _bInitOK = true;
    }


    /// <summary>
    /// 失去生命
    /// </summary>
    public void f_LostLife(int value)
    {
        _BaseRoleProtecy.f_LostLife(value);
    }

    #endregion

    #region 属性相关
    public GameEM.TeamType f_GetTeamType()
    {
        return _BaseRoleProtecy.f_GetTeamType();
    }

    public GameEM.emRoleType f_GetRoleType()
    {
        return _BaseRoleProtecy.f_GetRoleType();
    }

    public float f_BodySize()
    {
        return _BaseRoleProtecy.f_BodySize();
    }

    /*public float f_GetAttackSize()
    {
        return _BaseRoleProtecy.f_GetAttackSize();
    }

    public float f_GetViewSize()
    {
        return _BaseRoleProtecy.f_GetViewSize();
    }

    public float f_GetWalkSpeed()
    {
        return _BaseRoleProtecy.f_GetWalkSpeed();
    }*/

    public float f_GetRunSpeed()
    {
        return _BaseRoleProtecy.f_GetRunSpeed();
    }

    public bool f_CheckIsNoFind()
    {
        return _BaseRoleProtecy.f_CheckIsNoFind();
    }

    public string f_GetAI()
    {
        return _BaseRoleProtecy.f_GetAI();
    }

    /// <summary>
    /// 獲得說明
    /// </summary>
    public string f_GetReadme()
    {
        return _BaseRoleProtecy.f_GetReadme();
    }

    #endregion

    #region AI動作相關
    /// <summary>獲取是否無視警戒時間</summary>
    public bool f_GetIgnoreWarnTime()
    {
        return _BaseAI.f_GetIgnoreWarnTime();
    }

    /// <summary>
    /// 設定無視警戒時間
    /// </summary>
    /// <param name="bIgnoreWarnTime">是否無視</param>
    public virtual void f_SetIgnoreWarnTime(bool bIgnoreWarnTime)
    {
        
    }

    /// <summary>獲取警戒時長</summary>
    public float f_GetMaxAIWarnTime()
    {
        return _BaseAI.f_GetMaxAIWarnTime();
    }

    /// <summary>
    /// 設定警戒時長
    /// </summary>
    /// <param name="fMaxWarnTime">警戒時長</param>
    public virtual void f_SetMaxAIWarnTime(float fMaxWarnTime)
    {
        
    }

    /// <summary>獲取追逐時長</summary>
    public float f_GetMaxAIChaseTime()
    {
        return _BaseAI.f_GetMaxAIChaseTime();
    }

    /// <summary>
    /// 設定追逐時長
    /// </summary>
    /// <param name="fMaxChaseTime">追逐時長</param>
    public virtual void f_SetMaxAIChaseTime(float fMaxChaseTime)
    {
        
    }

    /// <summary>獲取巡邏點列表</summary>
    public List<Vector3> f_GetPathWalk()
    {
        return _BaseAI.f_GetPathWalk();
    }

    /// <summary>
    /// 設定巡路位置
    /// </summary>
    /// <param name="szPath"></param>
    public virtual void f_SetPathWalk(string szPath, string szAnimId = null, string szVarName = null)
    {
        
    }

    /// <summary>
    /// 增加巡邏點
    /// </summary>
    /// <param name="Path">巡邏點位置</param>
    /// <param name="iAnimId">播放動畫ID</param>
    /// <param name="VarId">觸發指令</param>
    public virtual void f_AddPathWalk(Vector3 Path, int iAnimId = 0, int VarId = -1)
    {
        
    }

    /// <summary>獲取巡邏點動畫ID</summary>
    public List<int> f_GetPathAnimId()
    {
        return _BaseAI.f_GetPathAnimId();
    }

    /// <summary>
    /// 獲取巡邏點停留時長
    /// </summary>
    /// <returns></returns>
    public float f_GetAutoMoveSleepTime()
    {
        return _BaseAI.f_GetAutoMoveSleepTime();
    }

    /// <summary>
    /// 設定巡邏點停留時長
    /// </summary>
    /// <param name="fAutoMoveTime">停留時長</param>
    public virtual void f_SetAutoMoveSleepTime(float fAutoMoveTime)
    {
        
    }
    #endregion

    class RoleEnterColliderDT
    {
        public RoleEnterColliderDT(float fRang, GameEM.TeamType tTargetTeamType, ccCallback CallBack_RoleEnter)
        {
            m_TargetTeamType = tTargetTeamType;
            m_ccCallback = CallBack_RoleEnter;
            m_fRang = fRang;
        }

        public GameEM.TeamType m_TargetTeamType;
        public ccCallback m_ccCallback;
        public float m_fRang;
    }

    #region 范围Collider

    List<RoleEnterColliderDT> _aCallBack_RoleEnter = new List<RoleEnterColliderDT>();

    private RoleEnterColliderDT GetCallBack(ccCallback CallBack_RoleEnter)
    {
        RoleEnterColliderDT tFind = _aCallBack_RoleEnter.Find(delegate (RoleEnterColliderDT tItem)
        {
            if (CallBack_RoleEnter == tItem.m_ccCallback)
            {
                return true;
            }
            return false;
        });
        return tFind;
    }

    public void f_RegRoleEnterCollider(float fRang, GameEM.TeamType tTargetTeamType, ccCallback CallBack_RoleEnter)
    {
        if (GetCallBack(CallBack_RoleEnter) == null)
        {
            _aCallBack_RoleEnter.Add(new RoleEnterColliderDT(fRang, tTargetTeamType, CallBack_RoleEnter));
        }
        CapsuleCollider tCapsuleCollider = m_RoleModel.GetComponent<CapsuleCollider>();
        //tCapsuleCollider.radius = _BaseRoleProtecy.f_BodySize();
        tCapsuleCollider.radius = fRang;
    }

    public void f_UnRegRoleEnterCollider(ccCallback CallBack_RoleEnter)
    {
        for (int i = 0; i < _aCallBack_RoleEnter.Count; i++)
        {
            if (CallBack_RoleEnter == _aCallBack_RoleEnter[i].m_ccCallback)
            {
                _aCallBack_RoleEnter.RemoveAt(i);
                return;
            }
        }
    }


    public void f_RoleEnter(BaseRoleControllV2 tBaseRoleControllV2)
    {
        for (int i = 0; i < _aCallBack_RoleEnter.Count; i++)
        {
            if (tBaseRoleControllV2.f_GetTeamType() == _aCallBack_RoleEnter[i].m_TargetTeamType)
            {
                _aCallBack_RoleEnter[i].m_ccCallback(tBaseRoleControllV2);
            }
        }
    }

    #endregion

    #region 位置與方向

    public virtual Vector3 f_GetHomePos()
    {
        return _BaseRoleProtecy.f_GetHomePos();
    }

    public virtual void f_SetHomePos(Vector3 Pos)
    {
        _BaseRoleProtecy.f_SetHomePos(Pos);
    }

    public virtual Vector3 f_GetPosttion()
    {
        return transform.position;
    }

    public virtual void f_SetPosttion(Vector3 Pos, Vector3 v3Angles)
    {
        transform.position = Pos;
        transform.eulerAngles = v3Angles;
    }

    public virtual void f_SetPosttion(Vector3 Pos)
    {
        transform.position = Pos;       
    }

    public virtual Transform f_GetTransform()
    {
        return transform;
    }

    public virtual Quaternion f_GetRotation()
    {
        return transform.rotation;
    }

    public virtual void f_SetRotation(Quaternion tQuaternion)
    {
        transform.rotation = tQuaternion;
    }

    public virtual Vector3 f_GetForward()
    {
        return transform.forward;
    }

    public float f_GetHeight()
    {
        return _BaseRoleProtecy.f_GetHeight();
    }
    #endregion

    public void f_LookAt(Transform tTransform)
    {
        transform.LookAt(tTransform);
    }

    #region Show

    public void f_Show(bool bShow)
    {
        f_GetRoleModel().gameObject.SetActive(bShow);
    }

    #endregion

    #region 碰撞相关

    public virtual void f_BoxColliderOpen()
    {

    }

    public virtual void f_BoxColliderClose()
    {

    }

    public virtual void f_OnTriggerEnter(BaseRoleControllV2 tBaseRoleControllV2)
    {
        
    }

    public virtual void f_OnTriggerExit(BaseRoleControllV2 tBaseRoleControllV2)
    {

    }

    public virtual void f_ShowUI()
    {

    }

    #endregion
}