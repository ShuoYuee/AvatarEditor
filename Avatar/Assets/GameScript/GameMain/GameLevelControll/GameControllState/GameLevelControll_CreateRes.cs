using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
public class GameLevelControll_CreateRes : GameControllBaseState
{
    public GameLevelControll_CreateRes()
        : base((int)EM_GameLevelAction.CreateRes)
    {


    }


    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
                
        //CreateMap();
    }

    private void InitPlayerPos()
    {        
        
    }

    private void CreateMap()
    {
        //string[] strMapName = ccMath.f_String2ArrayString(StaticValue.m_GameLevelDT.szRes, ";");
        glo_Main.GetInstance().m_ResourceManager.f_CreateForAB("gamemap.bundle", "GameMap01", OnCreateMapResult);
    }

    private void OnCreateMapResult(object Obj)
    {
        GameObject oObj = (GameObject)Obj;
        GameMain.GetInstance().f_SetGameMap((GameObject)Obj);
      
        CreateCharacter();
        InitPlayerPos();

        f_SetComplete((int)EM_GameLevelAction.Gaming);
    }

    private void CreateCharacter()
    {

    }

    private void OnCreateChararcterMapResult(object Obj)
    {
        GameObject oObj = (GameObject)Obj;
        oObj.transform.parent = GameMain.GetInstance().f_GetGameTable();
    }
}
