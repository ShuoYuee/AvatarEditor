using UnityEngine;
using System.Collections;

public class MessageDef
{
    /// <summary>
    /// 开始加载资源脚本消息
    /// </summary>
    public static string STARTLOADSC = "STARTLOADSC";
    public static string LOADSCSUC = "LOADSCSUC";
    
    public static string PAUSEGAME = "PAUSEGAME";
    public static string RESUMEGAME = "RESUMEGAME";
        
    public static string LOGINEROINFOR = "LOGINEROINFOR";

    /// <summary>
    /// 全局错误提示
    /// </summary>
    public static string GAMEMESSAGEBOX = "GAMEMESSAGEBOX";

    /// <summary>
    /// 游戏结束消息
    /// </summary>
    public static string GAMEOVER = "GAMEOVER";
    public static string GameStart = "GameStart";

    /// <summary>鎖住玩家行動</summary>
    public static string LockPlayerControll = "LockPlayerControll";
}
