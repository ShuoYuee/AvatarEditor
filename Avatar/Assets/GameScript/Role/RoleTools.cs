using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleTools
{

    public static int f_GetRoleId()
    {
        return GloData.glo_iStartRoleId++;
    }

    private static int iIndex = 0;
    public static int f_CreateKeyId()
    {
        int iTT = (int)ccMathEx.DateTime2time_t(System.DateTime.Now);
        int iId = iTT + iIndex;
        iIndex++;
        if (iIndex > 90)
        {
            iIndex = 0;
        }
        return iId;
    }

    public static BaseRoleControllV2 f_CreateRoleControll(int iId, int iRole)
    {
        /*BaseRoleControllV2 tRoleControll = null;
        NBaseSCDT tNBaseSCDT = glo_Main.GetInstance().m_SC_Pool.m_CharacterSC.f_GetSC(iId);
        if (tNBaseSCDT == null)
        {
            MessageBox.ASSERT("創建關卡" + StaticValue.m_GameLevelDT.iLevel + "角色失敗，角色CharacterDT數據未找到, " + iId);
        }
        CharacterDT tCharacterDT = (CharacterDT)tNBaseSCDT;
        GameObject oRole = glo_Main.GetInstance().m_ResourceManager.f_CreateCharacter(tCharacterDT);
        oRole.transform.parent = GameMain.GetInstance().f_GetGameTable();

        if (tCharacterDT.iType == (int)GameEM.emRoleType.DressRole)
        {
            tRoleControll = oRole.GetComponent<DressRoleControl>();
            if (tRoleControll == null)
            {
                tRoleControll = oRole.AddComponent<DressRoleControl>();
            }
            tRoleControll.f_Init(iRole, null, GameEM.TeamType.Computer, tCharacterDT);
        }*/

        return null;
    }

    public static BaseRoleControllV2 f_CreateRoleControll(int iId, GameObject oModel)
    {
        if (oModel == null || iId < 0)
        {
            MessageBox.ASSERT("創建物件失敗，參數有誤：" + iId);
            return null;
        }

        oModel.transform.SetParent(GameMain.GetInstance().f_GetGameTable());
        BaseRoleControllV2 tRoleControll = null;
        AvaterPartDT tAvaterPartDT = (AvaterPartDT)glo_Main.GetInstance().m_SC_Pool.m_AvaterPartSC.f_GetSC(iId);
        if (tAvaterPartDT.iType == (int)GameEM.EM_DressType.Pet)
        {
            tRoleControll = oModel.GetComponent<PetControl>();
            if (tRoleControll == null)
            {
                tRoleControll = oModel.AddComponent<PetControl>();
            }
            ((PetControl)tRoleControll).f_Init(new PetActionController(tRoleControll), tAvaterPartDT);
            oModel.name = GameEM.EM_DressType.Pet.ToString();
        }

        return tRoleControll;
    }

#if UNITY_EDITOR
    public static BaseRoleControllV2 f_CreateRoleControll_Editor(int iId, int iPart, GameObject oModel)
    {//編輯器使用
        if (oModel == null || iPart < 0)
        {
            MessageBox.ASSERT("創建物件失敗，參數有誤：" + iPart);
            return null;
        }

        oModel.transform.SetParent(GameMain.GetInstance().f_GetGameTable());
        BaseRoleControllV2 tRoleControll = null;
        if (iPart == (int)GameEM.EM_RolePart.pet)
        {
            tRoleControll = oModel.GetComponent<PetControl>();
            if (tRoleControll == null)
            {
                tRoleControll = oModel.AddComponent<PetControl>();
            }
            AvaterPartDT tAvaterPartDT = new AvaterPartDT();
            tAvaterPartDT.iId = iId;
            tAvaterPartDT.iRolePart = iPart;
            tAvaterPartDT.iGender = (int)GameEM.EM_Gender.Common;
            ((PetControl)tRoleControll).f_Init(new PetActionController(tRoleControll), tAvaterPartDT);
        }

        return tRoleControll;
    }
#endif

    public static BaseRoleControllV2 f_CreateRoleControll(int iId)
    {
        BaseRoleControllV2 tRoleControll = null;
        NBaseSCDT tNBaseSCDT = glo_Main.GetInstance().m_SC_Pool.m_AvaterPartSC.f_GetSC(iId);
        if (tNBaseSCDT == null)
        {
            //MessageBox.ASSERT("創建關卡" + StaticValue.m_GameLevelDT.iLevel + "角色失敗，角色CharacterDT數據未找到, " + iId);
        }
        AvaterPartDT tCharacterDT = (AvaterPartDT)tNBaseSCDT;
        GameObject oRole = glo_Main.GetInstance().m_ResourceManager.f_CreateForAB(tCharacterDT.szBundleName, tCharacterDT.szResName);
        oRole.transform.parent = GameMain.GetInstance().f_GetGameTable();

        return tRoleControll;
    }

    #region Find Player

    /*public static BaseRoleControllV2 f_GetRoleControl2(int iRoleKeyId)
    {
        return GameMain.GetInstance().m_BattleRolePool.f_Get(iRoleKeyId);
    }*/

    #endregion
}