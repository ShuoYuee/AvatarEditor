using UnityEngine;
using System.Collections;
using ccU3DEngine;



//数据节点更新类型枚举
public enum eUpdateNodeType
{
    node_default,//默认，第一次进入游戏
    node_add,//添加
    node_update,
    node_delete,
}

/// <summary>
/// 抽象池类
/// </summary>
public abstract class BasePool : ccBasePool<long>
{
    public BasePool(string strRegDTName):base(strRegDTName)
    {
        f_Init();
        RegSocketMessage();
    }

    public virtual void f_Destory()
    {
    }

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected abstract void f_Init();
    protected abstract void RegSocketMessage();

    protected void Callback_SocketData_Update(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            if (iData1 == (int)eUpdateNodeType.node_add)
            {
                f_Socket_AddData(tData, true);
            }
            else if (iData1 == (int)eUpdateNodeType.node_update)
            {
                f_Socket_UpdateData(tData);
            }
            else if (iData1 == (int)eUpdateNodeType.node_default)
            {
                f_Socket_AddData(tData, false);
            }
            //else if (iData1 == (int)eUpdateNodeType.node_delete)
            //{
            //    f_Socket_DelData(tData);
            //}
        }
    }
    protected abstract void f_Socket_AddData(SockBaseDT Obj, bool bNew);
    protected abstract void f_Socket_UpdateData(SockBaseDT Obj);
    //protected abstract void f_Socket_DelData(SockBaseDT Obj);

}