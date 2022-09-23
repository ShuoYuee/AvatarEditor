using System;
using UnityEngine;
/// <summary>
/// 数学扩展  扩展Excel数学的函数
/// </summary>
public static class MathEx
{
    /// <summary>
    /// 类似Excel的RoundUp
    /// </summary>
    /// <param name="value">原始数据</param>
    /// <param name="digits">取整小数位置，负数向前,负数向后</param>
    /// <returns></returns>
    public static decimal RoundUp(this decimal value, sbyte digits)
    {
        if (digits == 0)
        {
            return (value >= 0 ? decimal.Ceiling(value) : decimal.Floor(value));
        }
        decimal multiple = System.Convert.ToDecimal(System.Math.Pow(10, digits));
        return (value >= 0 ? decimal.Ceiling(value * multiple) : decimal.Floor(value * multiple)) / multiple;
    }

    /// <summary>
    /// 类似Excel的RoundDown
    /// </summary>
    /// <param name="value">原始数据</param>
    /// <param name="digits">取整小数位置，负数向前,负数向后</param>
    /// <returns></returns>
    public static decimal RoundDown(this decimal value, sbyte digits)
    {
        if (digits == 0)
        {
            return (value >= 0 ? decimal.Floor(value) : decimal.Ceiling(value));
        }
        decimal multiple = System.Convert.ToDecimal(System.Math.Pow(10, digits));
        return (value >= 0 ? decimal.Floor(value * multiple) : decimal.Ceiling(value * multiple)) / multiple;
    }
}

public static class ccMathEx
{
    /// <summary>
    /// 检测Pos2在Pos1的左还是右  -1左 0中 1右
    /// </summary>
    /// <param name="Pos1">坐标参照点</param>
    /// <param name="Pos2">坐标2</param>
    /// <returns></returns>
    public static int f_CheckPointIsLeftRight(Vector3 Pos1, Vector3 Pos2)
    {
        if (Pos1.x < Pos2.x)
        {
            return 1;
        }
        else if (Pos1.x == Pos2.x)
        {
            return 0;
        }
        else
            return -1;
    }

    /// <summary>
    /// 將System.DateTime轉換成長整型時間格式
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long DateTime2time_t(System.DateTime dateTime)
    {
        long time_t;
        System.DateTime dt1 = new System.DateTime(1970, 1, 1, 0, 0, 0);
        System.TimeSpan ts = dateTime - dt1;
        time_t = ts.Ticks / 10000000 - 28800;
        return time_t;
    }

    /// <summary>
    /// 将长整型时间转换成System.DateTime格式
    /// </summary>
    /// <param name="iTime">长整型时间</param>
    /// <returns></returns>
    public static System.DateTime time_t2DateTime(long iTime)
    {
        System.DateTime dt = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime);
        dt = dt.AddHours(8);
        return dt;
    }



    /// <summary>
    /// 检测是否是同一天,跨天重置（延时15秒）
    /// </summary>
    /// <param name="time1">时间1</param>
    /// <param name="time2">时间2</param>
    /// <returns></returns>
    public static bool f_CheckSameDay(long time1, long time2)
    {
        //time1 -= 15;
        //time2 -= 15;
        DateTime dataTime1 = time_t2DateTime(time1);
        DateTime dataTime2 = time_t2DateTime(time2);
        if (dataTime1.Year == dataTime2.Year &&
            dataTime1.Month == dataTime2.Month &&
            dataTime1.Day == dataTime2.Day)
        {
            //Debug.LogError("跨天重置");
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测是否是同一天,跨天重置（延时15秒）
    /// </summary>
    /// <param name="time1">时间1</param>
    /// <returns></returns>
    public static bool f_CheckSameDayForNow(long time1)
    {
        DateTime dataTime1 = time_t2DateTime(time1);
        DateTime dataTime2 = DateTime.Now;
        if (dataTime1.Year == dataTime2.Year &&
            dataTime1.Month == dataTime2.Month &&
            dataTime1.Day == dataTime2.Day)
        {
            //Debug.LogError("跨天重置");
            return true;
        }
        return false;
    }
    

    /// <summary>
    /// 保持指定数量的子对象
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="iNewItemNum"></param>
    public static void f_CreateChild(GameObject obj, int iNewChhildNum, bool bDelTail = false)
    {
        int iNum = obj.transform.childCount - iNewChhildNum;
        if (iNum > 0)
        {
            if (bDelTail)
            {
                for (int i = 0; i < iNum; i++)
                {
                    GameObject.DestroyImmediate(obj.transform.GetChild(obj.transform.childCount - 1).gameObject);
                }
            }
            else
            {
                for (int i = 0; i < iNum; i++)
                {
                    GameObject.DestroyImmediate(obj.transform.GetChild(0).gameObject);
                }
            }
        }
        else if (iNum < 0)
        {
            GameObject NewObj = null;
            if (obj.transform.childCount > 0)
            {
                NewObj = obj.transform.GetChild(0).gameObject;
            }
            else
            {
                NewObj = new GameObject();
                iNum += 1;
            }

            for (int i = iNum; i < 0; i++)
            {
                GameObject tItem = GameObject.Instantiate(NewObj);
                tItem.transform.SetParent(obj.transform);
                tItem.transform.position = obj.transform.position;
            }
            //glo_Main.GetInstance().f_DoDestroyObj(NewObj);
        }
    }

    /// <summary>
    /// 查找子对象
    /// </summary>
    /// <param name="oObj"></param>
    /// <param name="strChild"></param>
    /// <returns></returns>
    public static Transform f_FindChild(Transform oObj, string strChild)
    {
        Transform oFind = oObj.Find(strChild);
        if (oFind == null)
        {
            for (int i = 0; i < oObj.childCount; i++)
            {
                oFind = f_FindChild(oObj.GetChild(i), strChild);
                if (oFind != null)
                {
                    break;
                }
            }
        }
        return oFind;
    }

    public static Vector2 f_Operand(int iValue)
    {
        Vector2 vOperand = Vector2.zero;
        int iDivision = iValue / 4;
        int iRemain = iValue % 4;
        if (iValue < 4)
        {
            return new Vector2(iValue, 0);
        }
        else
        {
            if (iRemain == 0)
            {
                int iNew = iDivision + 1;
                vOperand = new Vector2(iNew, iNew);
            }
            else
            {
                int iNew = iDivision + 2;
                vOperand = new Vector2(iNew - 1, iNew);
                //vOperand = new Vector2(iNew, iNew - 1);
            }
        }
        return vOperand;
    }
}