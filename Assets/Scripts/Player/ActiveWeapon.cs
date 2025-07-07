using System.Collections;
using RPG.Player;
using RPG.Systems;
using UnityEngine;

/// <summary>
/// Manages the player's currently active weapon and handles attack logic.
/// </summary>
public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public Weapon CurrentActiveWeapon { get; private set; }
    private PlayerControls playerControls;
    private MouseFollow mouseFollow;
    private float timeBetweenAttacks;
    private bool attackButtonDown, isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
        mouseFollow = GetComponent<MouseFollow>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls?.Disable();
    }

    private void Start()
    {
        // Bind attack input events
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        // Try to equip the first available weapon
        TryEquipFirstAvailableWeapon();
    }

    private void Update()
    {
        Attack();
    }

    /// <summary>
    /// Sets the new active weapon and updates the equipment manager.
    /// </summary>
    public void NewWeapon(Weapon newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        EquipmentManager.Instance.UpdateWeapon(newWeapon);
        AttackCooldown();
        timeBetweenAttacks = CurrentActiveWeapon.attackRate;
        mouseFollow.enabled = CurrentActiveWeapon.weaponType == RPG.Definitions.WeaponType.Ranged;
    }

    /// <summary>
    /// Starts the attack cooldown coroutine.
    /// </summary>
    private void AttackCooldown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    /// <summary>
    /// Coroutine to handle time between attacks.
    /// </summary>
    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    /// <summary>
    /// Called when the attack button is pressed.
    /// </summary>
    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    /// <summary>
    /// Called when the attack button is released.
    /// </summary>
    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    /// <summary>
    /// Handles attack input and weapon-specific attack logic.
    /// </summary>
    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            AttackCooldown();
            PlayerCombat.Instance.Attack();

            // Get the active weapon component and call its Attack method
            var weaponComponent = GetActiveWeaponComponent();
            if (weaponComponent != null)
            {
                weaponComponent.Attack();
            }
            else
            {
                Debug.LogError("No active weapon detected");
            }
        }
    }

    /// <summary>
    /// Sets the current weapon to null and unequips it.
    /// </summary>
    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
        EquipmentManager.Instance.Unequip(RPG.Definitions.EquipmentSlot.Weapon);
        mouseFollow.enabled = false;
    }

    /// <summary>
    /// Tries to equip the first available weapon found as a child component.
    /// </summary>
    private void TryEquipFirstAvailableWeapon()
    {
        var weaponComponent = GetActiveWeaponComponent();
        if (weaponComponent is IWeapon weapon)
        {
            NewWeapon(weapon.GetWeaponInfo());
        }
        else
        {
            AttackCooldown();
        }
    }

    /// <summary>
    /// Returns the first found weapon component (melee, ranged, or magic).
    /// </summary>
    private IWeapon GetActiveWeaponComponent()
    {
        // Try to get any weapon type attached to the player
        return (IWeapon)GetComponentInChildren<MeleeWeapon>() ??
               (IWeapon)GetComponentInChildren<RangedWeapon>() ??
               (IWeapon)GetComponentInChildren<MagicWeapon>();
    }
}
