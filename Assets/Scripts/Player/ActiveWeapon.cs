using System.Collections;
using RPG.Player;
using RPG.Systems;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public Weapon CurrentActiveWeapon { get; private set; }
    private PlayerControls playerControls;
    private float timeBetweenAttacks;

    private bool attackButtonDown, isAttacking = false;


    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
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
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        MeleeWeapon currentWeapon = GetComponentInChildren<MeleeWeapon>();
        if (currentWeapon)
        {
            NewWeapon(currentWeapon.GetWeaponInfo());
        }
        else
        {
            AttackCooldown();
        }
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(Weapon newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        EquipmentManager.Instance.UpdateWeapon(newWeapon);
        AttackCooldown();
        timeBetweenAttacks = CurrentActiveWeapon.attackRate;
    }

    private void AttackCooldown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }
    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            AttackCooldown();
            PlayerCombat.Instance.Attack();
            MeleeWeapon meleeWeapon = GetComponentInChildren<MeleeWeapon>();
            if (meleeWeapon)
            {
                meleeWeapon.Attack();
            }
        }
    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
        EquipmentManager.Instance.Unequip(RPG.Definitions.EquipmentSlot.Weapon);
    }
}
