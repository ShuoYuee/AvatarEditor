using ccU3DEngine;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Intepreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ccILR
{

    public abstract class ccILR_BaseClass : ccBaseClass
    {

        public ILTypeInstance _Instance;
        //public override void f_Save(object tILTypeInstance)
        //{
        //    _Instance = (ILTypeInstance)tILTypeInstance;
        //}
        
        //public ccILR_BaseClass f_Clone()
        //{
        //    ccILR_BaseClass tccILR_BaseClass = (ccILR_BaseClass)MemberwiseClone();
        //    return tccILR_BaseClass;
        //}

        public T f_CallFunction<T>(string strFunctonName, params object[] p)
        {                      
            IMethod tIMethod = _Instance.Type.GetMethod(strFunctonName);
            return (T)ccILR_ClassFactory.GetInstance().f_GetAppDomain().Invoke(tIMethod, _Instance);
        }

        public void f_CallFunction(string strFunctonName, params object[] p)
        {
            IMethod tIMethod = _Instance.Type.GetMethod(strFunctonName);
            if (tIMethod != null)
            {
                ccILR_ClassFactory.GetInstance().f_GetAppDomain().Invoke(tIMethod, _Instance);
            }
            else
            {
                if (p == null)
                {
                    MessageBox.ASSERT(string.Format("f_CallFunction失败：类{0}中不存在此方法", Name));
                }
                else
                {
                    MessageBox.ASSERT(string.Format("f_CallFunction失败：类{0}中不存在此方法或参数不匹配。", Name));
                }
            }
        }

    }



}