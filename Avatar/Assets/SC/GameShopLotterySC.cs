
//============================================
//
//    GameShopLottery来自GameShopLottery.xlsx文件自动生成脚本
//    2022/5/7 下午 09:19:46
//    
//
//============================================
using System;
using System.Collections.Generic;
using ccU3DEngine;


public class GameShopLotterySC : NBaseSC
{
    public GameShopLotterySC()
    {
        Create("GameShopLotteryDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GameShopLotteryDT DataDT;
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
                DataDT = new GameShopLotteryDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id错误");
                }
                DataDT.iSetId = ccMath.atoi(tData[a++]);
                DataDT.iGoodsType = ccMath.atoi(tData[a++]);
                DataDT.iGoodsId = ccMath.atoi(tData[a++]);
                DataDT.iGoodsNum = ccMath.atoi(tData[a++]);
                DataDT.iWeight = ccMath.atoi(tData[a++]);
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
