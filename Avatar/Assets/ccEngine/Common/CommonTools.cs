using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;

public class CommonTools
{

    private static int _iKeyIdIndex = 0;
    public static long f_CreateLocalKey()
    {
        long iNewKeyId = ccMath.f_GetTimeStamp(System.DateTime.Now) * 100 + _iKeyIdIndex;
        if (_iKeyIdIndex > 80)
        {
            _iKeyIdIndex = 0;
        }
        _iKeyIdIndex++;
        return iNewKeyId;
    }

    /// <summary>
    /// 获取时间
    /// </summary>
    /// <param name="strStartTime">20181224</param>
    /// <returns></returns>
    public static DateTime f_GetDateTimeByTimeStr(string strStartTime)
    {
        if (strStartTime.Length < 8)
        {
            MessageBox.ASSERT("时间格式错误：" + strStartTime);
        }
        else if (strStartTime.Length > 8)
        {
            MessageBox.DEBUG("时间格式超长：" + strStartTime);
        }
        int year1 = int.Parse(strStartTime.Substring(0, 4));//20181224
        int month1 = int.Parse(strStartTime.Substring(4, 2));
        int day1 = int.Parse(strStartTime.Substring(6, 2));
        return new DateTime(year1, month1, day1);
    }

    /// <summary>
    /// 检查活动时间是否处于时间段内
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    public static bool f_CheckTime(DateTime startTime, DateTime endTime)
    {
        int timeNow = GameSocket.GetInstance().f_GetServerTime();
        long timeStart = ccMathEx.DateTime2time_t(startTime);
        long timeEnd = ccMathEx.DateTime2time_t(endTime);
        if (timeStart < timeNow && timeNow < timeEnd)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 检查活动时间是否处于时间段内
    /// </summary>
    /// <param name="strStartTime">格式：20181224</param>
    /// <param name="strEndTime">格式：20181224</param>
    /// <returns></returns>
    public static bool f_CheckTime(string strStartTime, String strEndTime)
    {
        DateTime startTime = f_GetDateTimeByTimeStr(strStartTime);
        DateTime endTime = f_GetDateTimeByTimeStr(strEndTime);
        return f_CheckTime(startTime, endTime);
    }

}