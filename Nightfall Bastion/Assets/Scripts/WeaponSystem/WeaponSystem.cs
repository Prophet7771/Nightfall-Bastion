using System.Collections;
using System.Collections.Generic;
using fLibrary;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    #region Enums

    public WeaponType weaponType = WeaponType.Bow;

    public FireMode fireMode = FireMode.Single;

    #endregion

    #region Basic Variables

    [SerializeField]
    float damage = 5f;

    [SerializeField]
    float fireRate = 0.2f;

    [Header("Projectile Data")]
    [SerializeField]
    GameObject projectile;

    [SerializeField]
    Transform projectileSpawnLoc;

    #endregion

    #region Getters & Setters

    public float GetDamage
    {
        get { return damage; }
    }

    #endregion

    #region Basic Functions

    public void UseWeapon(bool canUse)
    {
        switch (weaponType)
        {
            case WeaponType.Bow:
                FireProjectile();
                break;
            case WeaponType.Melee:
                break;
            case WeaponType.Pistol:
                break;
            case WeaponType.Rifle:
                break;
            case WeaponType.Special:
                break;
            case WeaponType.Tool:
                break;
            default:
                break;
        }
    }

    private void FireProjectile()
    {
        Debug.Log("Projectile Fired!");

        // GameObject tempProjectile = Instantiate(projectile, projectileSpawnLoc.transform.position, Quaternion.identity);
    }

    #endregion
}
