
//============================================
//
//    GameShop来自GameShop.xlsx文件自动生成脚本
//    2022/3/30 9:17:44
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GameShopDT : NBaseSCDT
{

    /// <summary>
    /// 售卖的提示文字
    /// </summary>
    public string szName;
    /// <summary>
    /// 售卖商品Id
    /// </summary>
    public int iGoodsId;
    /// 遊戲幣售卖价格
    /// </summary>
    public int iBuy;
    /// <summary>
    /// 遊戲幣特價價格
    /// </summary>
    public int iSpecialOffer;
    /// <summary>
    /// 金元寶售卖价格
    /// </summary>
    public int iBuy2;
    /// <summary>
    /// 金元寶特價價格
    /// </summary>
    public int iSpecialOffer2;
    /// <summary>
    /// 为空时普通商品  套装时保存套装数据商店Id-数量;商店Id-数量;
    /// </summary>
    public string szData;
    /// <summary>
    /// 回收价格
    /// </summary>
    public int iSale;
}
