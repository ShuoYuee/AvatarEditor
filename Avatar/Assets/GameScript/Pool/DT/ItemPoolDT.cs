using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;



/// <summary>
/// 基础物品PoolDT
/// </summary>
public class ItemPoolDT : BasePoolDT<long>
{
    public int m_iNum;              // 数量

    private int _iTempleteId;
    public int m_iTempleteId
    {
        get
        {
            return _iTempleteId;
        }

        set
        {
            if (_iTempleteId != value)
            {
                _iTempleteId = value;
                m_BaseGoodsDT = (AvaterPartDT)glo_Main.GetInstance().m_SC_Pool.m_AvaterPartSC.f_GetSC(_iTempleteId);
            }
        }
    }

    public AvaterPartDT m_BaseGoodsDT;

    /// <summary>
    /// 是否已装备 0未装备 1已装备
    /// </summary>
    public int m_iSetup;

    /// <summary>
    /// 如果是从Shop购买的则保存Shop对应的Id
    /// </summary>
    public int m_iShopId;
}