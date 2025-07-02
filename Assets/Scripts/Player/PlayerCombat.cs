using RPG.Combat;
using RPG.Definitions;
using RPG.Systems;
using UnityEngine;

namespace RPG.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("References")]
        public Transform firePoint;
        public EquipmentManager equipmentManager;
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
                gameObject
            );

            switch (weapon.weaponType)
            {
                case WeaponType.Melee:
                    PerformMeleeAttack(dmg);
                    break;
                case WeaponType.Ranged:
                    PerformRangedAttack(weapon, dmg);
                    break;
                case WeaponType.Magic:
                    PerformMagicAttack(weapon);
                    break;
            }
        }

        /// <summary>
        /// Performs a melee attack using a raycast.
        /// </summary>
        private void PerformMeleeAttack(DamageData dmg)
        {
            Transform weaponTransform = PlayerController.Instance.GetWeaponCollider();
            //TODO rework Melee combat to spawn a slash Collider
            RaycastHit2D hit = Physics2D.Raycast(transform.position, firePoint.right, 1.5f);
            if (hit.collider && hit.collider.TryGetComponent(out DamageController dc))
            {
                dc.ApplyDamage(dmg);
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
        /// Deducts mana for magic attacks.
        /// </summary>
        private void PerformMagicAttack(Weapon weapon)
        {
            stats.ModifyStat("MP", -weapon.manaCost);
        }
    }
}
