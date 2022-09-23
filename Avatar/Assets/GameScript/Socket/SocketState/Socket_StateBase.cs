using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class Socket_StateBase : ccMachineStateBase<ccMachineParamentBase>
{
    /// <summary>
    /// GameSocket
    /// </summary>
    protected BaseSocket _BaseSocket;
    public Socket_StateBase(int tiId, BaseSocket tBaseSocket)
        : base(tiId)
    {
        _BaseSocket = tBaseSocket;
    }
}