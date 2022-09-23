using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCTools
{

    /// <summary>
    /// 手工设置当前工作的VVV几
    /// </summary>
    /// <param name="strVVV"></param>
    public static void f_SetVVV(string strVVV)
    {
        if (strVVV.Length > 0)
        {
            GloData.glo_ProName = strVVV;
            GloData.glo_strLoadAllSC = "http://" + GloData.glo_strHttpServerIP + "/" + GloData.glo_ProName + "/ver/";
        }
    }

}