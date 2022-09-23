
//============================================
//
//    GameShop来自GameShop.xlsx文件自动生成脚本
//    2022/3/30 9:17:44
//    
//
//============================================
using ccU3DEngine;
using System;
using System.Collections.Generic;



public class GameShopSC : NBaseSC
{
    public GameShopSC()
    {
        Create("GameShopDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GameShopDT DataDT;
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
                DataDT = new GameShopDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id错误");
                }
                DataDT.szName = tData[a++];
                DataDT.iGoodsId = ccMath.atoi(tData[a++]);
                DataDT.iBuy = ccMath.atoi(tData[a++]);
                DataDT.iSpecialOffer = ccMath.atoi(tData[a++]);
                DataDT.iBuy2 = ccMath.atoi(tData[a++]);
                DataDT.iSpecialOffer2 = ccMath.atoi(tData[a++]);
                DataDT.szData = tData[a++];
                DataDT.iSale = ccMath.atoi(tData[a++]);
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
