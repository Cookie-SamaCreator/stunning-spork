using RPG.Combat;
using RPG.Definitions;
using RPG.Systems;
using UnityEngine;

namespace RPG.Player
{
    /// <summary>
    /// Handles all player combat logic, including melee, ranged, and magic attacks.
    /// </summary>
    public class PlayerCombat : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Where projectiles and effects are spawned from.")]
        public Transform firePoint;
        [Tooltip("Handles equipment logic.")]
        public EquipmentManager equipmentManager;
        [Tooltip("Player stats (HP, MP, etc).")]
        public PlayerStats stats;

        /// <summary>
        /// Handles the player's attack logic based on equipped weapon type.
        /// </summary>
        public void Attack()
        {
            // Get the equipped weapon, return if none
            if (equipmentManager.GetEquipped(EquipmentSlot.Weapon) is not Weapon weapon) return;

            // Prepare damage data
            var dmg = new DamageData(
                weapon.physicalDamage,
                weapon.elementType,
                weapon.elementalDamage,
                weapon.damageZonePrefab,
                gameObject
            );

            switch (weapon.weaponType)
            {
                case WeaponType.Melee:
                    PerformAttackZone(dmg);
                    break;
                case WeaponType.Ranged:
                    PerformRangedAttack(weapon, dmg);
                    break;
                case WeaponType.Magic:
                    PerformMagicAttack(weapon, dmg);
                    break;
            }
        }

        /// <summary>
        /// Spawns a damage zone at the weapon's position (used for melee and magic).
        /// </summary>
        private void PerformAttackZone(DamageData dmg)
        {
            if (dmg.damageZone == null) return;
            Transform weaponTransform = PlayerController.Instance.GetWeaponCollider();

            GameObject zone = Instantiate(dmg.damageZone, weaponTransform.position, weaponTransform.rotation);

            if (zone.TryGetComponent(out DamageAOE damageZone))
            {
                damageZone.Init(dmg, gameObject);
            }
        }

        /// <summary>
        /// Instantiates and initializes a projectile for ranged attacks.
        /// </summary>
        private void PerformRangedAttack(Weapon weapon, DamageData dmg)
        {
            if (weapon.projectilePrefab == null) return;
            GameObject proj = Instantiate(weapon.projectilePrefab, firePoint.position, firePoint.rotation);

            if (proj.TryGetComponent(out Projectile projectile))
            {
                projectile.Init(dmg);
            }
        }

        /// <summary>
        /// Deducts mana and performs a magic attack by spawning a damage zone.
        /// </summary>
        private void PerformMagicAttack(Weapon weapon, DamageData dmg)
        {
            if (dmg.damageZone == null) return;

            // Deduct mana, abort if not enough
            if (!stats.ModifyStat("MP", -weapon.manaCost)) return;

            PerformAttackZone(dmg);
        }
    }
}
