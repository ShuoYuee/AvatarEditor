using UnityEngine;
using System.Collections;

public class SocketState
{
    public SocketState()
    {
        m_BattleTeamData = false;
        m_BuildData = false;
        m_GoodsData = false;
        m_NpcData = false;
        m_PlayerData = false;
        m_StateData = false;
    }

    public string f_IsSuc()
    {
        string ppSQL = "";
        if (!m_BattleTeamData)
            ppSQL += "m_BattleTeamData";
        if (!m_BuildData)
            ppSQL += "m_BuildData ";
        if (!m_GoodsData)
            ppSQL += "m_GoodsData ";
        if (!m_NpcData)
            ppSQL += "m_NpcData ";
        if (!m_PlayerData)
            ppSQL += "m_PlayerData ";
        if (!m_StateData)
            ppSQL += "m_StateData ";

        return ppSQL;        
    }

    /// <summary>
    /// 玩家数据
    /// </summary>
    public bool m_PlayerData;

    /// <summary>
    /// 我的世界已有建筑
    /// </summary>
    public bool m_BuildData;

    /// <summary>
    /// 已有英雄 已有怪物
    /// </summary>
    public bool m_NpcData;

    /// <summary>
    /// 玩家背包
    /// </summary>
    public bool m_GoodsData;

    /// <summary>
    /// 战斗编队
    /// </summary>
    public bool m_BattleTeamData;

    /// <summary>
    /// 关卡数据
    /// </summary>
    public bool m_StateData;




}
