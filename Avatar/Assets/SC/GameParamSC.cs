
//============================================
//
//    GameParam来自GameParam.xlsx文件自动生成脚本
//    2017/3/29 14:09:02
//    
//
//============================================
using ccU3DEngine;
using System;
using System.Collections.Generic;



public class GameParamSC : NBaseSC
{
    public GameParamSC()
    {
        Create("GameParamDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GameParamDT DataDT;
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
                DataDT = new GameParamDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id错误");
                }
                if (DataDT.iId >= 9) {

                }
                DataDT.iParam1 = ccMath.atoi(tData[a++]);
                DataDT.iParam2 = ccMath.atoi(tData[a++]);
                DataDT.iParam3 = ccMath.atoi(tData[a++]);
                DataDT.iParam4 = ccMath.atoi(tData[a++]);
                SaveItem(DataDT);
            }
            catch
            {
                MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
                continue;
            }
        }
    }

}
