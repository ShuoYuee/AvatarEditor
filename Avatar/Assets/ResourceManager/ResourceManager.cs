using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ccU3DEngine;
using ccU3DEngine.ccEngine.ThingManager;

/// <summary>
/// 异步加载资源回调
/// </summary>
/// <param name="Obj">申请资源对象</param>
/// <param name="asset">异步加载成功返回后的资源，根据需要通过GameObject.Instantiate创建</param>
public delegate void ResourceCatchDelegate(object Obj, UnityEngine.Object asset);

public class ResourceManager
{  
    
    public ResourceManager()
    {
    }


    public void f_Update()
    {
           
    }

    #region 创建资源

    #region 創建Icon
    public Sprite f_CreateIcon(string szIcon)
    {
        Texture2D tTexture2D = null;
        if (!string.IsNullOrEmpty(szIcon))
        {
            tTexture2D = f_CreateResourceTexture2D(szIcon);
        }
        if (tTexture2D == null)
        {
            MessageBox.ASSERT("Sprite未找到創建失敗 " + szIcon);
        }
        return Sprite.Create(tTexture2D, new Rect(0, 0, tTexture2D.width, tTexture2D.height), Vector2.zero);
    }
    public Sprite f_CreateIcon(string strBundle, string strAB)
    {
        Texture2D tTexture2D = null;
        if (string.IsNullOrEmpty(strBundle))
        {
            tTexture2D = f_CreateResourceTexture2D(strAB);
        }
        else
        {
            tTexture2D = AssetHelper.LoadAssetImmediate<Texture2D>(strBundle + ".bundle", strAB);
        }
        if (tTexture2D == null)
        {
            MessageBox.ASSERT("Sprite未找到創建失敗 " + strAB);
        }
        return Sprite.Create(tTexture2D, new Rect(0, 0, tTexture2D.width, tTexture2D.height), Vector2.zero);
    }
    #endregion

    #region
    public Sprite f_CreateMenuIcon(string szIcon)
    {
        Texture2D tTexture2D = f_CreateResourceTexture2D("Icon\\Menu\\" + szIcon);
        if (tTexture2D == null)
        {
            MessageBox.ASSERT("菜單ICON未找到创建失败 " + szIcon);
        }
        return Sprite.Create(tTexture2D, new Rect(0, 0, tTexture2D.width, tTexture2D.height), Vector2.zero);
    }
    #endregion

    #region Resource
    public AudioClip f_CreateAudio(string strOther, bool bResetPosition = true)
    {
        AudioClip tAudioClip = f_LoadAudioClip(strOther);
        if (tAudioClip == null)
        {
            MessageBox.ASSERT("f_Create Fail.." + strOther);
        }
        return tAudioClip;
    }

    public GameObject f_CreateRes(string strRes, bool bResetPosition = true, bool bCreate = true)
    {
        GameObject oOther = CreateResource(strRes, bResetPosition, bCreate);
        if (!bCreate)
        {
            return null;
        }
        if (oOther == null)
        {
            MessageBox.ASSERT("f_Create Fail.." + strRes);
        }
        return oOther;
    }

    public GameObject f_CreateResource(string resPath)
    {
        string ppSQL = resPath;
        if (!ccResourceManager.GetInstance().f_CheckIsHave(ppSQL))
        {
            GameObject oProfab = (GameObject)Resources.Load(ppSQL);
            if (oProfab == null)
            {
                MessageBox.ASSERT("Profab没找到 " + ppSQL);
                return null;
            }
            GameObject oModel = (GameObject)GameObject.Instantiate(oProfab, Vector3.zero, Quaternion.identity);
            ccResourceManager.GetInstance().f_RegResource(ppSQL, oModel, null, null, true);
        }
        GameObject oBullet = ccResourceManager.GetInstance().f_Instantiate(ppSQL);
        return oBullet;
    }

    /// <summary>
    /// 產生在 Resource資料夾下的資源 (e.g. Model/Bullet/bullet)  )
    /// </summary>
    /// <param name="resPath"> 資源路徑 (範例: Model/Bullet/bullet ) </param>
    GameObject CreateResource(string resPath, bool bResetPosition = true, bool bCreate = true)
    {
        string ppSQL = resPath;
        if (!ccResourceManager.GetInstance().f_CheckIsHave(ppSQL))
        {
            GameObject oProfab = (GameObject)Resources.Load(ppSQL);
            if (oProfab == null){
                MessageBox.ASSERT("Profab没找到 " + ppSQL);
                return null;
            }
            GameObject oModel = null;
            if (bResetPosition)
            {
                oModel = (GameObject)GameObject.Instantiate(oProfab, Vector3.zero, Quaternion.identity);
            }
            else
            {
                oModel = (GameObject)GameObject.Instantiate(oProfab);
            }
            ccResourceManager.GetInstance().f_RegResource(ppSQL, oModel, null, null, true);
        }
        if (!bCreate)
        {
            return null;
        }

        GameObject oBullet = ccResourceManager.GetInstance().f_Instantiate(ppSQL);
        return oBullet;
    }

    
    /// <summary>
    /// 创建的GameObject通过此方法进行回收
    /// </summary>
    /// <param name="Obj"></param>
    public void f_DestorySD(GameObject Obj)
    {
        try
        {
            if (!ccResourceManager.GetInstance().f_UnInstantiate(Obj))
            {
                GameObject.Destroy(Obj);
            }
        }
        catch 
        {
            GameObject.Destroy(Obj);
        }

    }
        
	/// <summary>
    /// 加载Resource里的Texture2D资源
    /// </summary>
    /// <param name="strFile"></param>
    /// <returns></returns>
    Texture2D f_CreateResourceTexture2D(string strFile)//,object data)
    {
        Texture2D tTexture2D = Resources.Load(strFile) as Texture2D;
        return tTexture2D;
    }

    /// <summary>
    /// 获取音效
    /// </summary>
    /// <returns></returns>
    public AudioClip f_LoadAudioClip(string strFile)
    {
        AudioClip tAudioClip = Resources.Load<AudioClip>("Audio/" + strFile) as AudioClip;
        if (tAudioClip == null)
        {
            MessageBox.ASSERT("无此音樂" + strFile);
        }
        return tAudioClip;
    }

    /// <summary>
    /// 獲取圖片
    /// </summary>
    /// <param name="strFile">圖片路徑</param>
    /// <returns></returns>
    public Sprite f_LoadResourceImage(string strFile)
    {
        Sprite oLoadObj = Resources.Load<Sprite>(strFile) as Sprite;
        if (oLoadObj == null)
        {
            MessageBox.ASSERT("無此資源" + strFile);
        }
        return oLoadObj;
    }

    /// <summary>
    /// 獲取紋理
    /// </summary>
    /// <param name="strFile">紋理路徑</param>
    /// <returns></returns>
    public Texture f_LoadResourceTexture(string strFile)
    {
        Texture oLoadObj = Resources.Load<Texture>(strFile) as Texture;
        if (oLoadObj == null)
        {
            MessageBox.ASSERT("無此資源" + strFile);
        }
        return oLoadObj;
    }

    /// <summary>
    /// 獲取材質
    /// </summary>
    /// <param name="strFile">材質路徑</param>
    /// <returns></returns>
    public Material f_LoadResourceMaterial(string strFile)
    {
        Material oLoadObj = Resources.Load<Material>(strFile) as Material;
        if (oLoadObj == null)
        {
            MessageBox.ASSERT("無此資源" + strFile);
        }
        return oLoadObj;
    }
    #endregion

    #region AB
    /// <summary>
    /// AB中的资源
    /// </summary>
    /// <param name="strBundle"></param>
    /// <param name="strAB"></param>
    /// <param name="OnCreateResult"></param>
    public void f_CreateForAB(string strBundle, string strAB, ccCallback OnCreateResult)
    {
        AssetHelper.LoadAsset(strBundle, strAB, (string strName, GameObject go, object callBackData) =>
        {
            MessageBox.DEBUG(string.Format("创建UI{0}加载完成", strAB));
            if (go == null)
            {
                MessageBox.ASSERT("Profab没找到 " + strAB);
            }
            OnCreateResult(go);
        });
    }

    /// <summary>
    /// AB中的资源
    /// </summary>
    public void f_CreateForAB(string strBundle, string strAB, ccUIEventListener.VoidDelegateV2 OnCreateResult, object obj1, object obj2)
    {
        AssetHelper.LoadAsset(strBundle, strAB, (string strName, GameObject go, object callBackData) =>
        {
            MessageBox.DEBUG(string.Format("创建UI{0}加载完成", strAB));
            if (go == null)
            {
                MessageBox.ASSERT("Profab没找到 " + strAB);
            }
            OnCreateResult(go, obj1, obj2);
        });
    }

    /// <summary>
    /// AB中的资源
    /// </summary>
    /// <param name="strBundle"></param>
    /// <param name="strAB"></param>
    /// <param name="OnCreateResult"></param>
    public GameObject f_CreateCharacter(CharacterDT tCharacterDT)
    {
        return AssetHelper.LoadAssetImmediate<GameObject>(tCharacterDT.szName, tCharacterDT.szResName);
    }

    public GameObject f_CreateForAB(string strBundle, string strAB)
    {
        return AssetHelper.LoadAssetImmediate<GameObject>(strBundle + ".bundle", strAB);
    }

    public void f_LoadABSprite(string strBundle, string strAB, ccUIEventListener.VoidDelegateV2 OnCreateResult, object obj1)
    {
        Texture2D tTexture2D = null;
        tTexture2D = AssetHelper.LoadAssetImmediate<Texture2D>(strBundle + ".bundle", strAB);
        if (tTexture2D == null)
        {
            MessageBox.ASSERT("Sprite没找到 " + strAB);
        }
        Sprite oObj = Sprite.Create(tTexture2D, new Rect(0, 0, tTexture2D.width, tTexture2D.height), Vector2.zero);
        OnCreateResult(null, oObj, obj1);
    }


    #region Texture
    public void f_LoadABTexture(string strBundle, string strAB, ccUIEventListener.VoidDelegateV2 OnCreateResult, object obj1)
    {
        AssetHelper.LoadAsset(strBundle, strAB, (string strName, Texture go, object callBackData) =>
        {
            MessageBox.DEBUG(string.Format("创建UI{0}加载完成", strAB));
            if (go == null)
            {
                MessageBox.ASSERT("Profab没找到 " + strAB);
            }
            OnCreateResult(null, go, obj1);
        });
    }

    public void f_LoadABTextureSign(string strBundle, string strSign, ccUIEventListener.VoidDelegateV2 OnCreateResult, object obj1)
    {
        string[] strTexture = ccMath.f_String2ArrayString(strBundle, strSign);
        AssetHelper.LoadAsset(strTexture[0], strTexture[1], (string strName, Texture go, object callBackData) =>
        {
            MessageBox.DEBUG(string.Format("创建UI{0}加载完成", strTexture[1]));
            if (go == null)
            {
                MessageBox.ASSERT("Profab没找到 " + strTexture[1]);
            }
            OnCreateResult(null, go, obj1);
        });
    }

    public Texture f_LoadABTexture(string strBundle, string strAB)
    {
        return AssetHelper.LoadAssetImmediate<Texture>(strBundle, strAB);
    }

    public void f_LoadABMaterial(string strBundle, string strAB, ccUIEventListener.VoidDelegateV2 OnCreateResult, object obj1)
    {
        AssetHelper.LoadAsset(strBundle, strAB, (string strName, Material go, object callBackData) =>
        {
            MessageBox.DEBUG(string.Format("创建UI{0}加载完成", strAB));
            if (go == null)
            {
                MessageBox.ASSERT("Profab没找到 " + strAB);
            }
            OnCreateResult(null, go, obj1);
        });
    }
    #endregion

    #region Material
    public void f_LoadABMaterialSign(string strBundle, string strSign, ccUIEventListener.VoidDelegateV2 OnCreateResult, object obj1)
    {
        string[] strMaterials = ccMath.f_String2ArrayString(strBundle, strSign);
        AssetHelper.LoadAsset(strMaterials[0], strMaterials[1], (string strName, Material go, object callBackData) =>
        {
            MessageBox.DEBUG(string.Format("创建UI{0}加载完成", strMaterials[1]));
            if (go == null)
            {
                MessageBox.ASSERT("Profab没找到 " + strMaterials[1]);
            }
            OnCreateResult(null, go, obj1);
        });
    }

    public Material f_LoadABMaterial(string strBundle, string strAB)
    {
        return AssetHelper.LoadAssetImmediate<Material>(strBundle, strAB);
    }
    #endregion

    public RoleAvaterList f_LoadRoleAvaterList(string strBundle, string strAB)
    {
        return AssetHelper.LoadAssetImmediate<RoleAvaterList>(strBundle, strAB);
    }
    #endregion

    #endregion



}
