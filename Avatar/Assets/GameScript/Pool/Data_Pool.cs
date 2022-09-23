using UnityEngine;
using System.Collections;



/// <summary>
/// 游戏数据数据结构
/// </summary>
public class Data_Pool
{



    #region POOL
    /// <summary>玩家資料</summary>
    public PlayerData m_PlayerData;
    /// <summary>物件資料</summary>
    public ItemPool m_ItemPool;
    #endregion

    private static Data_Pool _Instance = null;
    public static Data_Pool GetInstance()
    {
        if (_Instance == null)
        {
            _Instance = new Data_Pool();
        }

        return _Instance;
    }

    public void f_InitPool()
    {
        m_PlayerData = new PlayerData();
        m_ItemPool = new ItemPool();
    }

    public void f_LoadPool()
    {
        
    }

    public void f_Reset()
    {
        
    }

   
}
