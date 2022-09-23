using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ccU3DEngine;

namespace GameLogic
{
    public class UI_EditorInputDlg : ccUILogicBase
    {
        private InputField _InputField;
        private SocketCallbackDT _SocketDT;

        protected override void On_Init()
        {
            MessageBox.DEBUG("啟用遊戲包中的UI_EditorInputDlg腳本");
            _InputField = f_GetObject("InputField").GetComponent<InputField>();
            f_RegClickEvent(f_GetObject("BtnYes"), CallBackBtn_Yes);
            f_RegClickEvent(f_GetObject("BtnNo"), CallBackBtn_No);
        }

        protected override void On_Open(object e)
        {
            _SocketDT = (SocketCallbackDT)e;
        }

        protected override void On_Close()
        {
            
        }

        protected override void On_Destory()
        {
            
        }

        protected override void On_UpdateGUI()
        {
            
        }

        private void CallBackBtn_Yes(GameObject go, object obj1, object obj2)
        {
            if (_InputField.text.Length > 16)
            {
                _SocketDT.m_ccCallbackFail(_InputField.text);
            }
            else
            {
                _SocketDT.m_ccCallbackSuc(_InputField.text);
            }
            f_Close();
        }

        private void CallBackBtn_No(GameObject go, object obj1, object obj2)
        {
            f_Close();
        }
    }
}
