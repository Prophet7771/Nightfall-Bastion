using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fLibrary
{
    #region Classes

    public class FunctionLibrary : MonoBehaviour { }

    #endregion

    #region Enums

    public enum WeaponType
    {
        Bow,
        Rifle,
        Pistol,
        Melee,
        Tool,
        Special
    };

    public enum FireMode
    {
        Single,
        Rappid,
        Burst
    };

    #endregion
}
