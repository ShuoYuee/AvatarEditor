using UnityEngine;

/// <summary>
/// 寵物動作器
/// </summary>
public class PetActionController : BaseActionController
{
    public PetActionController(BaseRoleControllV2 tBaseRoleControl) : base(tBaseRoleControl)
    {
        _Animator = _BaseRoleControllV2.GetComponent<Animator>();
    }

    public void f_SetPlayerAnimClip(string strAnim)
    {
        if (strAnim == "") { return; }
        GameTools.f_PlayAnimator(_Animator, strAnim);
    }
}
