using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class Socket_Wait : Socket_StateBase
{

    public Socket_Wait(BaseSocket tBaseSocket)
        : base((int)EM_Socket.Wait, tBaseSocket)
    {

    }

    public override void f_Enter(object Obj)
    {

        base.f_Enter(Obj);
    }
    


}
