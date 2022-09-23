using UnityEngine;
using System.Collections;



public class ResourceTools
{

    public static string f_GetLocalMainPath()
    {
        //if (GloData.glo_iPackType == 0)
        //{
        //    return Application.persistentDataPath + "/" + GloData.glo_ProName;
        //}
        //else
        //{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            return Application.streamingAssetsPath + "/" + GloData.glo_ProName;
#elif UNITY_IPHONE
        return Application.streamingAssetsPath + "/" + GloData.glo_ProName;
#elif UNITY_ANDROID
        return Application.dataPath + "!assets/" + GloData.glo_ProName;
#elif UNITY_WEBGL
        return Application.dataPath + "!assets/" + GloData.glo_ProName;
#endif
        //}
    }



    public static void f_UpdateURL()
    {


    }
    
}