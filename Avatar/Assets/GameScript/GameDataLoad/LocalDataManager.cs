using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 本地数据保存和获取工具
/// </summary>
public class LocalDataManager
{
    /// <summary>
    /// 本地数据前缀
    /// </summary>
    private static readonly string localDataStr = "localdata";


    private static string GetKey(string strKey)
    {
        return localDataStr + strKey;
    }

    #region 设置本地数据 设置值
    /// <summary>
    /// 设置本地数据 设置值
    /// </summary>
    /// <typeparam name="T">数据类型（int,float,string,bool等基本数据类型，或由基本数据类型封装的类）</typeparam>
    /// <param name="dataType">数据名字</param>
    /// <param name="value">数据值</param>
    public static void f_SetLocalData<T>(string strKey, T value)
    {
        Type type = typeof(T);
        string key = GetKey(strKey);
        if (type.Equals(typeof(int)))
        {
            PlayerPrefs.SetInt(key, (int)Convert.ChangeType(value, typeof(int)));
        }
        else if (type.Equals(typeof(float)))
        {
            PlayerPrefs.SetFloat(key, (float)Convert.ChangeType(value, typeof(float)));
        }
        else if (type.Equals(typeof(string)))
        {
            PlayerPrefs.SetString(key, (string)Convert.ChangeType(value, typeof(string)));
        }
        else if (type.Equals(typeof(bool)))
        {
            int data = ((bool)Convert.ChangeType(value, typeof(bool))) ? 1 : 0;
            PlayerPrefs.SetInt(key, (int)Convert.ChangeType(data, typeof(int)));
        }
        else
        {
            MessageBox.ASSERT("不支持的数据类型");
        }
        PlayerPrefs.Save();

        //Debug.Log("Value: " + value + " , Get: " + f_GetLocalData<T>(strKey, value));
    }
    #endregion
        
    #region 检查本地数据中是否已经有该数据
    /// <summary>
    /// 检查本地数据中是否已经有该数据
    /// </summary>
    /// <param name="dataType">数据名字</param>
    /// <returns>是否已经有该数据</returns>
    public static bool f_HasLocalData(string strKey)
    {
        string key = GetKey(strKey);
        return PlayerPrefs.HasKey(key);
    }
    #endregion

    #region 从本地数据中删除该数据值
    /// <summary>
    /// 从本地数据中删除该数据值
    /// </summary>
    /// <param name="dataType">数据名字</param>
    public static void f_DeleteLocalData(string strKey)
    {
        string key = GetKey(strKey);
        PlayerPrefs.DeleteKey(key);
    }
    #endregion

    #region 删除所有本地数据
    /// <summary>
    /// 删除所有本地数据
    /// </summary>
    public static void f_DeleteAllLocalData()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion

    #region 获取本地数据中某个数据值
    /// <summary>
    /// 获取本地数据中某个数据值
    /// </summary>
    /// <typeparam name="T">数据类型（int,float,string,bool等基本数据类型，或由基本数据类型封装的类）</typeparam>
    /// <param name="dataType">数据名字</param>
    /// <param name="Defaultalue">如果未发现对应的关健数据则返回此数据</param>
    /// <returns>数据中该数据值</returns>
    public static T f_GetLocalData<T>(string strKey, T Defaultalue = default(T))
    {
        T returnValue = default(T);
        string key = GetKey(strKey);
        Type type = typeof(T);
        if (PlayerPrefs.HasKey(key))
        {           
            if (type.Equals(typeof(int)))
            {
                returnValue = (T)Convert.ChangeType(PlayerPrefs.GetInt(key), type);
            }
            else if (type.Equals(typeof(float)))
            {
                returnValue = (T)Convert.ChangeType(PlayerPrefs.GetFloat(key), type);
            }
            else if (type.Equals(typeof(string)))
            {
                returnValue = (T)Convert.ChangeType(PlayerPrefs.GetString(key), type);
            }
            else if (type.Equals(typeof(bool)))
            {
                //bool value = PlayerPrefs.GetInt(key) == 1 ? true : false;
                //returnValue = (T)Convert.ChangeType(value, type);
                returnValue = (T)Convert.ChangeType(PlayerPrefs.GetInt(key), type);
            }
            else
            {
                MessageBox.ASSERT("不支持的数据类型");
            }
        }
        else
        {
            return Defaultalue;
        }
        return returnValue;
    }
    #endregion

    

}