using fLibrary;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    PlayerMovement playerMove;

    [SerializeField]
    PlayerCharacter playerCharacter;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMovement>();
        playerCharacter = GetComponent<PlayerCharacter>();
    }

    #region Event Handlers

    private void OnEnable()
    {
        #region Player Movement Subscriptions

        playerMove.runningState += RunningState;
        playerMove.aimState += AimState;
        playerMove.locomotion += Locomotion;

        #endregion

        #region Player Character Subscriptions

        playerCharacter.onHolster += HolsterState;
        playerCharacter.onUseWeapon += UseWeapon;

        #endregion
    }

    private void OnDisable()
    {
        #region Player Movement Unsubscribe

        playerMove.runningState -= RunningState;
        playerMove.aimState -= AimState;
        playerMove.locomotion -= Locomotion;

        #endregion

        #region Player Character Unsubscribe

        playerCharacter.onHolster -= HolsterState;
        playerCharacter.onUseWeapon -= UseWeapon;

        #endregion
    }

    #endregion

    #region Player Movement Functions

    private void RunningState(bool val) => animator.SetBool("isRunning", val);

    private void AimState(bool val) => animator.SetBool("isAiming", val);

    private void Locomotion(float x, float y)
    {
        animator.SetFloat("posX", x, 0.1f, Time.deltaTime);
        animator.SetFloat("posY", y, 0.1f, Time.deltaTime);
    }

    #endregion

    #region Player Character Functions

    private void HolsterState(bool val)
    {
        switch (playerCharacter.GetWeaponType)
        {
            case WeaponType.Bow:
                if (!val)
                {
                    animator.SetTrigger("equipBow");
                    animator.ResetTrigger("holsterBow");
                }
                else
                {
                    animator.SetTrigger("holsterBow");
                    animator.ResetTrigger("equipBow");
                }
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

    private void UseWeapon()
    {
        switch (playerCharacter.GetWeaponType)
        {
            case WeaponType.Bow:
                animator.SetTrigger("fireBow");
                // animator.ResetTrigger("fireBow");
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

    private void ResetBowFire()
    {
        animator.ResetTrigger("fireBow");
    }

    #endregion
}
