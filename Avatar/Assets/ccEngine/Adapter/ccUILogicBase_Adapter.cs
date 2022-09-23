using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ccILR;
using System.Collections.Generic;
using ccU3DEngine;
using UnityEngine;

namespace ccU3DEngine
{
    public class ccUILogicBase_Adapter : CrossBindingAdaptor
    {

        public override Type BaseCLRType
        {
            get
            {
                return typeof(ccU3DEngine.ccUILogicBase);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(UILogicBase_Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new UILogicBase_Adapter(appdomain, instance);
        }

        class UILogicBase_Adapter : ccU3DEngine.ccUILogicBase, CrossBindingAdaptorType
        {
            private object[] m_aParams = new object[1];
            private ILTypeInstance instance;
            private ILRuntime.Runtime.Enviorment.AppDomain appdomain;


            public UILogicBase_Adapter()
            {

            }

            //public UILogicBase_Adapter(ccUIBase tccUIBase)
            //{
            //    m_ccUIControll = tccUIBase;
            //}
            //private IMethod m_SaveUIControll;
            //private bool m_SaveUIControllGot;
            //public void f_SaveUIControll(ccUIControll tccUIControll)
            //{
            //    m_ccUIControll = tccUIControll;
            //}

            public UILogicBase_Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
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

            private IMethod m_GetObject;
            private bool m_GetObjectGot;
            private void f_GetObject()
            {
                if (!m_GetObjectGot)
                {
                    m_GetObject = instance.Type.GetMethod("f_GetObject", 0);
                    m_GetObjectGot = true;
                }
                if (m_GetObject != null)
                {
                    appdomain.Invoke(m_GetObject, instance);
                }
            }

            private IMethod m_InitMessage;
            private bool m_InitMessageGot;
            private void InitMessage()
            {
                if (!m_InitMessageGot)
                {
                    m_InitMessage = instance.Type.GetMethod("InitMessage", 0);
                    m_InitMessageGot = true;
                }
                if (m_InitMessage != null)
                {
                    appdomain.Invoke(m_InitMessage, instance);
                }
            }

            private IMethod m_TTT;
            private bool m_TTTGot;
            private void TTT(object e)
            {
                if (!m_TTTGot)
                {
                    m_TTT = instance.Type.GetMethod("TTT", 1);
                    m_TTTGot = true;
                }
                if (m_TTT != null)
                {
                    m_aParams[0] = e;
                    appdomain.Invoke(m_TTT, instance, m_aParams);
                }
            }

            private IMethod m_OnInit;
            private bool m_OnInitGot;
            protected override void On_Init()
            {
                if (!m_OnInitGot)
                {
                    m_OnInit = instance.Type.GetMethod("On_Init", 0);
                    m_OnInitGot = true;
                }
                if (m_OnInit != null)
                {
                    appdomain.Invoke(m_OnInit, instance);
                }
            }

            private IMethod m_OnDestory;
            private bool m_OnDestoryGot;
            protected override void On_Destory()
            {
                if (!m_OnDestoryGot)
                {
                    m_OnDestory = instance.Type.GetMethod("On_Destory", 0);
                    m_OnDestoryGot = true;
                }
                if (m_OnDestory != null)
                {
                    appdomain.Invoke(m_OnDestory, instance);
                }
            }

            private IMethod m_OnUpdate;
            private bool m_OnUpdateGot;
            private bool m_OnUpdateInvokeing;
            protected override void On_Update()
            {
                if (!m_OnUpdateGot)
                {
                    m_OnUpdate = instance.Type.GetMethod("On_Update", 0);
                    m_OnUpdateGot = true;
                }
                if (!m_OnUpdateInvokeing)
                {
                    m_OnUpdateInvokeing = true;
                    appdomain.Invoke(m_OnUpdate, instance);
                    m_OnUpdateInvokeing = false;
                }
                else
                {
                    base.On_Update();
                }               
            }

            private IMethod m_OnUpdateGUI;
            private bool m_OnUpdateGUIGot;
            protected override void On_UpdateGUI()
            {
                if (!m_OnUpdateGUIGot)
                {
                    m_OnUpdateGUI = instance.Type.GetMethod("On_UpdateGUI", 0);
                    m_OnUpdateGUIGot = true;
                }
                if (m_OnUpdateGUI != null)
                {
                    appdomain.Invoke(m_OnUpdateGUI, instance);
                }
            }

            private IMethod m_OnOpen;
            private bool m_OnOpenGot;
            protected override void On_Open(object e)
            {
                if (!m_OnOpenGot)
                {
                    m_OnOpen = instance.Type.GetMethod("On_Open", 1);
                    m_OnOpenGot = true;
                }
                if (m_OnOpen != null)
                {
                    m_aParams[0] = e;
                    appdomain.Invoke(m_OnOpen, instance, m_aParams);
                }
            }

            private IMethod m_OnClose;
            private bool m_OnCloseGot;
            protected override void On_Close()
            {
                if (!m_OnCloseGot)
                {
                    m_OnClose = instance.Type.GetMethod("On_Close", 0);
                    m_OnCloseGot = true;
                }
                if (m_OnClose != null)
                {
                    appdomain.Invoke(m_OnClose, instance);
                }
            }

            private IMethod m_OnHold;
            private bool m_OnHoldGot;
            private bool m_OnHoldInvokeing;
            protected override void On_Hold(object e)
            {
                if (!m_OnHoldGot)
                {
                    m_OnHold = instance.Type.GetMethod("On_Hold", 0);
                    m_OnHoldGot = true;
                }
                if (m_OnHold != null)
                {
                    if (!m_OnHoldInvokeing)
                    {
                        m_OnHoldInvokeing = true;
                        m_aParams[0] = e;
                        appdomain.Invoke(m_OnHold, instance, m_aParams);
                        m_OnHoldInvokeing = false;
                    }
                    else
                    {
                        base.On_Hold(e);
                    }
                }
            }

            private IMethod m_OnUnHold;
            private bool m_OnUnHoldGot;
            private bool m_OnUnHoldInvokeing;
            protected override void On_UnHold(object e)
            {
                if (!m_OnUnHoldGot)
                {
                    m_OnUnHold = instance.Type.GetMethod("On_UnHold", 0);
                    m_OnUnHoldGot = true;
                }
                if (m_OnUnHold != null)
                {
                    if (!m_OnUnHoldInvokeing)
                    {
                        m_OnUnHoldInvokeing = true;
                        m_aParams[0] = e;
                        appdomain.Invoke(m_OnUnHold, instance, m_aParams);
                        m_OnUnHoldInvokeing = false;
                    }
                    else
                    {
                        base.On_UnHold(e);
                    }
                }
            }

            //private IMethod m_OnRaddot;
            //private bool m_OnRaddotGot;
            //private bool m_OnRaddotInvokeing;
            //protected override void OnRaddot()
            //{
            //    if (!m_OnRaddotGot)
            //    {
            //        m_OnRaddot = instance.Type.GetMethod("OnRaddot", 0);
            //        m_OnRaddotGot = true;
            //    }
            //    if (m_OnRaddot != null)
            //    {
            //        if (!m_OnRaddotInvokeing)
            //        {
            //            m_OnRaddotInvokeing = true;
            //            appdomain.Invoke(m_OnRaddot, instance);
            //            m_OnRaddotInvokeing = false;
            //        }
            //        else
            //        {
            //            base.OnRaddot();
            //        }
            //    }
            //}

            private IMethod m_OnUpdateReddotUI;
            private bool m_OnUpdateReddotUIGot;
            private bool m_OnUpdateReddotUIInvokeing;
            protected override void On_UpdateReddotUI()
            {
                if (!m_OnUpdateReddotUIGot)
                {
                    m_OnUpdateReddotUI = instance.Type.GetMethod("On_UpdateReddotUI", 0);
                    m_OnUpdateReddotUIGot = true;
                }
                if (m_OnUpdateReddotUI != null)
                {
                    if (!m_OnUpdateReddotUIInvokeing)
                    {
                        m_OnUpdateReddotUIInvokeing = true;
                        appdomain.Invoke(m_OnUpdateReddotUI, instance);
                        m_OnUpdateReddotUIInvokeing = false;
                    }
                    else
                    {
                        base.On_UpdateReddotUI();
                    }
                }
            }

            /*
            protected void f_RegClickEvent(string strObj, ccUIEventListener.VoidDelegateV2 aCallBackFuc, object oSaveData1 = null, object oSaveData2 = null, string strEffectSound = "");
            protected void f_RegClickEvent(GameObject Obj, ccUIEventListener.VoidDelegateV2 aCallBackFuc, object oSaveData1 = null, object oSaveData2 = null, string strEffectSound = "");
            */


            private IMethod m_RegClickEvent;
            private bool m_RegClickEventGot;
            protected void f_RegClickEvent(GameObject Obj, ccUIEventListener.VoidDelegateV2 aCallBackFuc, object oSaveData1 = null, object oSaveData2 = null, string strEffectSound = "")
            {
                if (!m_RegClickEventGot)
                {
                    m_RegClickEvent = instance.Type.GetMethod("f_RegClickEvent", 5);
                    m_RegClickEventGot = true;
                }
                if (m_RegClickEvent != null)
                {
                    appdomain.Invoke(m_RegClickEvent, instance);
                }
            }

            //public override void f_Save(object tILTypeInstance)
            //{
            //    throw new NotImplementedException();
            //}

            




        }


    }

}