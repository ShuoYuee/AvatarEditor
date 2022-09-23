
//============================================
//
//    AvaterPart来自AvaterPart.xlsx文件自动生成脚本
//    2022/5/30 14:29:04
//    
//
//============================================

public class AvaterPartDT : NBaseSCDT
{

    /// <summary>
    /// 時裝名
    /// </summary>
    public string szName;
    /// <summary>
    /// Bundle名
    /// </summary>
    public string szBundleName;
    /// <summary>
    /// 資源名
    /// </summary>
    public string szResName;
    /// <summary>
    /// 模型資源類型0Resource1AB
    /// </summary>
    public int iResType;
    /// <summary>
    /// 資料類型 時裝1 顏色(材質)2 寵物3 組合包4
    /// </summary>
    public int iType;
    /// <summary>
    /// 時裝說明(中文)
    /// </summary>
    public string szReadme;
    /// <summary>
    /// 時裝穿著部位
    /// </summary>
    public int iRolePart;
    /// <summary>
    /// 圖標資源
    /// </summary>
    public string szIcon;
    /// <summary>
    /// 材質資源類型0Resource1AB
    /// </summary>
    public int iResMaterials;
    /// <summary>
    /// 材質球
    /// </summary>
    public string szMaterials;
    /// <summary>
    /// 材質貼圖
    /// </summary>
    public string szTexture;
    /// <summary>
    /// 性別1男生 2女生 0或不填寫 通用
    /// </summary>
    public int iGender;
    /// <summary>
    /// 卸裝1-卸裝2-卸裝3;預覽資源類型-Bundle名-Prefab名
    /// </summary>
    public string szData;
    /// <summary>
    /// 時裝介紹文本Key
    /// </summary>
    public string szInfor;
    /// <summary>
    /// 是否為活動解鎖 填寫活動ID
    /// </summary>
    public int iLock;
    /// <summary>
    /// 解鎖條件文本Key
    /// </summary>
    public string szLockInfor;
    /// <summary>
    /// 色票R-G-B-A
    /// </summary>
    public string szHue;
    /// <summary>
    /// 基本回收价格当抽奖等其它方式获取时使用此价格
    /// </summary>
    public int iSale;

    /// <summary>
    /// 卸裝
    /// </summary>
    public string szTakeOff;
    /// <summary>
    /// 預覽物來源
    /// </summary>
    public string szDisplay;
}