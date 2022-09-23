using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

/// <summary>
/// 脚本管理器
/// </summary>
public class SC_Pool
{
    int _iABVer = 0;

    private bool _bLoadSuc;

    public LanguageSC m_LanguageSC = new LanguageSC();
    public AvaterPartSC m_AvaterPartSC = new AvaterPartSC();

    List<NBaseSC> _aSCList = new List<NBaseSC>();
    public void f_LoadSC(byte[] bData)
    {
        _bLoadSuc = false;

        ///////////////////////////////////////////////////////////////

        _aSCList.Add(m_LanguageSC);      
        _aSCList.Add(m_AvaterPartSC);

        ///////////////////////////////////////////////////////////////

        int i = 0;
        MessageBox.DEBUG("解析脚本");

        string ppSQL;
        byte[] b = new byte[512];
        System.Array.Copy(bData, b, 5);
        int iHeadLen = int.Parse(System.Text.Encoding.UTF8.GetString(b));
        System.Array.Copy(bData, 5, b, 0, iHeadLen);
        string strHeadData = System.Text.Encoding.UTF8.GetString(b);
        string[] ttt = strHeadData.Split(new string[] { "," }, System.StringSplitOptions.None);
        int iMovePos = iHeadLen + 5;

        int iDataLen = int.Parse(ttt[i]);
        ppSQL = ZipTools.aaa556(bData, iMovePos, iDataLen);
        iMovePos = iMovePos + iDataLen;
        DispABVer(ppSQL);

        for (i = 0; i < _aSCList.Count; i++)
        {
            //yield return new WaitForSeconds(4.5f/_aSCList.Count);
            MessageBox.DEBUG("SC " + i + " " + _aSCList[i].m_strRegDTName);
            iDataLen = int.Parse(ttt[i + 1]);
            ppSQL = ZipTools.aaa556(bData, iMovePos, iDataLen);
            _aSCList[i].f_LoadSCForData(ppSQL);
            iMovePos = iMovePos + iDataLen;
        }

        _bLoadSuc = true;


        MessageBox.DEBUG("解析脚本成功");
    }


    private void DispABVer(string strData)
    {
        int[] aVer = ccMath.f_String2ArrayInt(strData, "-");

        if (aVer.Length == 5)
        {
            //0 $ProgrameVer = $_POST["ProgrameVer"]; 1 $AndriodVer = $_POST["AndriodVer"]; 2 $WindowVer = $_POST["WindowVer"]; 3 $IosVer = $_POST["IosVer"];
#if UNITY_EDITOR
            UnityEditor.BuildTarget tBuildTarget = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
            if (tBuildTarget == UnityEditor.BuildTarget.Android)
            {
                _iABVer = aVer[1];
            }
            else if (tBuildTarget == UnityEditor.BuildTarget.iOS)
            {
                _iABVer = aVer[3];
            }
            else if (tBuildTarget == UnityEditor.BuildTarget.StandaloneWindows || tBuildTarget == UnityEditor.BuildTarget.StandaloneWindows64)
            {
                _iABVer = aVer[2];
            }
            else if (tBuildTarget == UnityEditor.BuildTarget.WebGL)
            {
                _iABVer = aVer[4];
            }
#elif UNITY_IOS
            _iABVer = aVer[3];
#elif UNITY_ANDROID
           _iABVer = aVer[1];
#elif UNITY_STANDALONE
            _iABVer = aVer[2];
#elif UNITY_STANDALONE_OSX
           _iABVer = aVer[3];
#elif UNITY_WEBGL
           _iABVer = aVer[4];
#endif

        }
        else
        {
            MessageBox.DEBUG("AB资源版本版本错误:" + strData);
        }

        MessageBox.DEBUG("AB资源版本:" + _iABVer);
    }

    public int f_GetABVer()
    {
        return _iABVer;
    }


    public bool f_CheckLoadSuc()
    {
        return _bLoadSuc;
    }

}
