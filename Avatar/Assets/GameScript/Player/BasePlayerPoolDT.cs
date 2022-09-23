using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class BasePlayerPoolDT : BasePoolDT<long>

{
    public BasePlayerPoolDT()
    {
        //m_eReadPlayerStep = EM_ReadPlayerStep.Base;
        //f_ResetVigorData();
    }

    #region 基础信息

    //玩家ID

    private int _iSex;
    /// <summary>
    /// 玩家性别
    /// </summary>
    public int m_iSex
    {
        get
        {
            return _iSex;
        }
    }

    public int m_CardId
    {
        get
        {
            //if (m_iSex == (int)EM_RoleSex.Man)
            //{
            //    return GameParamConst.ManCardId;
            //}
            //else if (m_iSex == (int)EM_RoleSex.Woman)
            //{
            //    return GameParamConst.WomanCardId;
            //}
            //else
            //{
            //    return m_iSex;
            //}
            return 0;
        }
    }

    private string _szName;
    /// <summary>
    /// 玩家名称
    /// </summary>
    public string m_szName
    {
        get
        {
            return _szName;
        }
    }

    private int _iVip;
    /// <summary>
    /// VIP标志
    /// </summary>
    public int m_iVip
    {
        get
        {
            return _iVip;
        }
    }

    private int _iTitle;
    /// <summary>
    /// 称号
    /// </summary>
    public int m_iTitle
    {
        get
        {
            return _iTitle;
        }
    }

    private long _iFrameId;
    /// <summary>
    /// 边框
    /// </summary>
    public long m_iFrameId
    {
        get
        {
            return _iFrameId;
        }
    }

    //public CardDT m_CardDT
    //{
    //    get
    //    {
    //        return glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(m_CardId) as CardDT;
    //    }
    //}
    #endregion

    //private EM_ReadPlayerStep _eReadPlayerStep;
    ///// <summary>
    ///// 资料读取进度
    ///// </summary>
    //public EM_ReadPlayerStep m_eReadPlayerStep
    //{
    //    get
    //    {
    //        return _eReadPlayerStep;
    //    }
    //    set
    //    {
    //        if((int)_eReadPlayerStep < (int)value)
    //        {
    //            _eReadPlayerStep = value;
    //        }
    //    }
    //}

    /// <summary>
    /// 保存基础信息
    /// </summary>
    /// <param name="szName"></param>
    /// <param name="iLogo"></param>
    /// <param name="iVip"></param>
    /// <param name="iTitle"></param>
    public void f_SaveBase(string szName, int iSex, long iFrameId, int iVip, int iTitle)
    {
        _szName = szName;
        //int id = Random.Range(0, 8);
        _iSex = iSex;
        _iVip = iVip;
        _iTitle = iTitle;
        _iFrameId = iFrameId;
    }

    public void f_SaveBikeUser(string szName, long iFrameId, byte iLv)
    {
        _szName = szName;
        _iFrameId = iFrameId;
    }

}