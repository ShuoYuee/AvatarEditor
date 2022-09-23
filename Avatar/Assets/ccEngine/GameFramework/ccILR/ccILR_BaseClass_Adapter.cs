using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ccILR;
using System.Collections.Generic;

/// <summary>
/// 非继承关系接口不需要写对应的适配器
/// </summary>
public class ccILR_BaseClass_Adapter : CrossBindingAdaptor
{
    class CallClassFunctionDT
    {
        public IMethod m_IMethod = null;
        public bool m_bIsGetInvoking = false;
    }

    public override Type BaseCLRType
    {
        get
        {
            return typeof(ccILR_BaseClass);
        }
    }

    public override Type AdaptorType
    {
        get
        {
            return typeof(BaseClass_Adaptor);
        }
    }

    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
    {
        return new BaseClass_Adaptor(appdomain, instance);
    }

    class BaseClass_Adaptor : ccILR_BaseClass, CrossBindingAdaptorType
    {

        //IMethod mGetName;
        //bool mGetNameGot = false;

        private ILTypeInstance instance;
        private ILRuntime.Runtime.Enviorment.AppDomain appdomain;

        private Dictionary<string, CallClassFunctionDT> _aCreateFuncton = new Dictionary<string, CallClassFunctionDT>();

        public BaseClass_Adaptor()
        {

        }

        public BaseClass_Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }

        public ILTypeInstance ILInstance
        {
            get
            {
                return instance;
            }
        }

        //public string Name
        //{
        //    get
        //    {
        //        if (!mGetNameGot)
        //        {
        //            //属性的Getter编译后会以get_XXX存在，如果不确定的话可以打开Reflector等反编译软件看一下函数名称
        //            mGetName = instance.Type.GetMethod("get_Name", 1);
        //            mGetNameGot = true;
        //        }
        //        return base.Name;
        //    }
        //}

        public override string ToString()
        {
            IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
            m = instance.Type.GetVirtualMethod(m);
            if (m == null || m is ILMethod)
            {
                return instance.ToString();
            }
            else
                return instance.Type.FullName;
        }


        public T f_CallFunction<T>(string strFunctonName, params object[] p)
        {
            //CallClassFunctionDT tCallClassFunctionDT = null;
            //if (!_aCreateFuncton.TryGetValue(strFunctonName, out tCallClassFunctionDT))
            //{
            //    tCallClassFunctionDT = new CallClassFunctionDT();
            //    if (p == null)
            //    {
            //        tCallClassFunctionDT.m_IMethod = instance.Type.GetMethod(strFunctonName, 1);
            //    }
            //    else
            //    {
            //        tCallClassFunctionDT.m_IMethod = instance.Type.GetMethod(strFunctonName, p.Length);
            //    }
            //}
            ////对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.Value就会造成无限循环，最终导致爆栈
            //if (tCallClassFunctionDT.m_IMethod != null && !tCallClassFunctionDT.m_bIsGetInvoking)
            //{
            //    tCallClassFunctionDT.m_bIsGetInvoking = true;
            //    var res = (int)appdomain.Invoke(tCallClassFunctionDT.m_IMethod, instance, null);
            //    tCallClassFunctionDT.m_bIsGetInvoking = false;
            //    return res;
            //}
            //else
            //    return base.Value;

            //(IMethod m, object instance, params object[] p)
            IMethod tIMethod = instance.Type.GetMethod(strFunctonName);
            return (T)this.appdomain.Invoke(tIMethod, instance);
        }

    }


}