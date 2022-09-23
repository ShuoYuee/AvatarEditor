using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using ccU3DEngine;
using System;

public class SaveAvater2PHP
{

    private static string _strName = "";
    private static byte[] _aBuf = null;

    /// <summary>
    /// 保存编辑的基础模型数据
    /// </summary>
    /// <param name="strName"></param>
    /// <param name="aBuf"></param>
    public static void f_Save(string strName, byte[] aBuf)
    {
        _strName = strName;
        _aBuf = aBuf;
        
        glo_Main.GetInstance().f_StartCoroutine(DoWWW());
    }

    private static IEnumerator DoWWW()
    {
        string ppSQL = Convert.ToBase64String(_aBuf, 0, _aBuf.Length);
        //string ppSQL = System.Text.Encoding.UTF8.GetString(_aBuf, 0, _aBuf.Length);
        //string strDD = Base64.EncodeBase64(Encoding.UTF8, ppSQL);


        //string strURL = "http://192.168.0.227/BicycleWorld//Log/SaveAvater2PHP.php";
        string strURL = string.Format("http://123.207.87.187/bike/avatar?Name={0}&AvaterBuf={1}", _strName, ppSQL);

        MessageBox.DEBUG("DoWWW..." + strURL);
        
        WWW www = new WWW(strURL);
        yield return www;
        if (www.error != null)
        {
            yield return null;
        }
        if (www.isDone && www.error == null)
        {
            if (www.text != null && www.text != "")
            {
                MessageBox.DEBUG(www.text);
            }
        }
        MessageBox.DEBUG("... ...");        

    }

    /// <summary>
    /// 加载基础模型列表
    /// </summary>
    /// <param name="CallBack_LoadResult"></param>
    public static void f_LoadWWW(ccCallback CallBack_LoadResult)
    {
        glo_Main.GetInstance().f_StartCoroutine(StartLoadWWW(CallBack_LoadResult));
    }

    /// <summary>
    /// 检测公告消息
    /// </summary>
    /// <returns></returns>
    static IEnumerator StartLoadWWW(ccCallback CallBack_LoadResult)
    {
        string strURL = "http://123.207.87.187/bike/avatar/all";
        WWW www = new WWW(strURL);
        yield return www;
        if (www.error != null)
        {
            yield return null;
        }
        if (www.isDone && www.error == null)
        {
            if (www.text != null && www.text != "")
            {
                //byte[] aa = System.Text.Encoding.ASCII.GetBytes(www.text);
                //MessageBox.DEBUG(www.text);
                if (CallBack_LoadResult != null)
                {
                    CallBack_LoadResult(www.text);
                }
            }
        }
    }



}