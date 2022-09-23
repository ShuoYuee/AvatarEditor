
//============================================
//
//    AvaterPart来自AvaterPart.xlsx文件自动生成脚本
//    2022/5/30 14:29:04
//    
//
//============================================
using ccU3DEngine;
using System;
using System.Collections.Generic;



public class AvaterPartSC : NBaseSC
{
    public AvaterPartSC()
    {
        Create("AvaterPartDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        AvaterPartDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
                    MessageBox.DEBUG(m_strRegDTName + "脚本存在空记录, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new AvaterPartDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id错误");
                }
                DataDT.szName = tData[a++];
                DataDT.szBundleName = tData[a++];
                DataDT.szResName = tData[a++];
                DataDT.iResType = ccMath.atoi(tData[a++]);
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.szReadme = tData[a++];
                DataDT.iRolePart = ccMath.atoi(tData[a++]);
                DataDT.szIcon = tData[a++];
                DataDT.iResMaterials = ccMath.atoi(tData[a++]);
                DataDT.szMaterials = tData[a++];
                DataDT.szTexture = tData[a++];
                DataDT.iGender = ccMath.atoi(tData[a++]);
                DataDT.szData = tData[a++];
                DataDT.szInfor = tData[a++];
                DataDT.iLock = ccMath.atoi(tData[a++]);
                DataDT.szLockInfor = tData[a++];
                DataDT.szHue = tData[a++];
                DataDT.iSale = ccMath.atoi(tData[a++]);
                f_SetData(DataDT);
                SaveItem(DataDT);
            }
            catch
            {
                MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
                continue;
            }
        }
    }

    private void f_SetData(AvaterPartDT tData)
    {
        string[] strData = ccMath.f_String2ArrayString(tData.szData, ";");
        if (strData.Length == 2)
        {
            tData.szDisplay = strData[1];
        }
        tData.szTakeOff = strData[0];
    }
}