using System;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

namespace GameLogic
{
    public class UI_Load : ccUILogicBase
    {
        protected override void On_Init()
        {
            MessageBox.DEBUG("啟用遊戲包中的UI_Load腳本");
        }

        protected override void On_Open(object e)
        {
            MessageBox.DEBUG("開啟UI_Load腳本");
        }

        protected override void On_Close()
        {
            MessageBox.DEBUG("關閉UI_Load腳本");
        }

        protected override void On_Destory()
        {
            
        }

        protected override void On_UpdateGUI()
        {
            
        }
    }
}
