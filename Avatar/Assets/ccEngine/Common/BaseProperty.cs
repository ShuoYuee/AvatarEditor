using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;

/// <summary>
/// 基本属性管理
/// </summary>
public class BaseProperty
{
    int iTTT;
    private int[] _aData = null;


    public BaseProperty(int iLen)
    {
       _aData = new int[iLen];
        iTTT = ccMath.f_CreateKeyId();

    }

    /// <summary>
    /// 复制数据
    /// </summary>
    /// <param name="aData">来源数组</param>
    /// <param name="iStartPos">目录的开始位置</param>
    /// <param name="iLen">复制长度</param>
    virtual public void f_Copy(int[] aData, int iStartPos, int iLen)
    {
        Array.Copy(aData, 0, _aData, iStartPos, iLen);
    }

    /// <summary>
    /// 增加属性
    /// </summary>
    /// <param name="iIndex"></param>
    /// <param name="iData"></param>
    virtual public void f_AddProperty(int iIndex, int iData)
    {
        _aData[iIndex] += iData;
    }

    /// <summary>
    /// 减少属性
    /// </summary>
    /// <param name="iIndex"></param>
    /// <param name="iData"></param>
    virtual public void f_SubtractProperty(int iIndex, int iData)
    {
        _aData[iIndex] -= iData;
    }

    /// <summary>
    /// 更新属性
    /// </summary>
    /// <param name="iIndex"></param>
    /// <param name="iData"></param>
    virtual public void f_UpdateProperty(int iIndex, int iData)
    {
        _aData[iIndex] = iData;
    }

    /// <summary>
    /// 获取属性
    /// </summary>
    /// <param name="iIndex"></param>
    /// <returns></returns>
    virtual public int f_GetProperty(int iIndex)
    {
        return _aData[iIndex];
    }

    virtual public void f_Reset()
    {
        Array.Clear(_aData, 0, (int)EM_RoleProperty.End);
    }
    
}


/// <summary>
/// 角色属性类型
/// </summary>
public enum EM_RoleProperty
{
    /// <summary>
    /// 攻击
    /// </summary>
    Atk = 1,
    /// <summary>
    /// 生命
    /// </summary>
    Hp,
    /// <summary>
    /// 物防
    /// </summary>
    Def,
    /// <summary>
    /// 法防
    /// </summary>
    MDef,
    /// <summary>
    /// 攻击加成
    /// </summary>
    AddAtk,
    /// <summary>
    /// 生命加成
    /// </summary>
    AddHp,
    /// <summary>
    /// 物防加成
    /// </summary>
    AddDef,
    /// <summary>
    /// 法防加成
    /// </summary>
    AddMDef,
    /// <summary>
    /// 基础怒气
    /// </summary>
    Anger,
    /// <summary>
    /// 活力
    /// </summary>
    Energy = 10,
    /// <summary>
    /// 暴击率
    /// </summary>
    CritR,
    /// <summary>
    /// 抗爆率
    /// </summary>
    AntiknockR,
    /// <summary>
    /// 命中率
    /// </summary>
    HitR,
    /// <summary>
    /// 闪避率
    /// </summary>
    DodgeR,
    /// <summary>
    /// 伤害加成
    /// </summary>
    DamageR,
    /// <summary>
    /// 伤害减免
    /// </summary>
    InjurySR,
    /// <summary>
    /// PVP增伤
    /// </summary>
    PInjuryAR,
    /// <summary>
    /// PVP减伤
    /// </summary>
    PInjurySR,
    /// <summary>
    /// 致命一击
    /// </summary>
    Deadly,
    /// <summary>
    /// 忽视防御发生率
    /// </summary>
    IgDefCR = 20,
    /// <summary>
    /// 忽视防御比例
    /// </summary>
    IgDefR,
    /// <summary>
    /// 吸血发生率
    /// </summary>
    SuckCR,
    /// <summary>
    /// 吸血比例
    /// </summary>
    SuckR,
    /// <summary>
    /// 物理反弹发生率
    /// </summary>
    BounceCR,
    /// <summary>
    /// 物理反弹伤害比例
    /// </summary>
    BounceR,
    /// <summary>
    /// 法术反弹发生率
    /// </summary>
    MBounceCR,
    /// <summary>
    /// 法术反弹伤害比例
    /// </summary>
    MBounceR,
    /// <summary>
    /// 生命恢复发生率
    /// </summary>
    RcvHpCR,
    /// <summary>
    /// 生命恢复伤害比例
    /// </summary>
    RcvHpR,
    /// <summary>
    /// 怒气恢复
    /// </summary>
    RcvAnger = 30,
    /// <summary>
    /// 经验加成
    /// </summary>
    ExpR = 31,
    /// <summary>
    /// 结束
    /// </summary>
    End = 32,

}