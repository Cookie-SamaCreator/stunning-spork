using RPG.Combat;
using RPG.Definitions;
using RPG.Systems;
using UnityEngine;

namespace RPG.Player
{
    /// <summary>
    /// Handles all player combat logic, including melee, ranged, and magic attacks.
    /// </summary>
    public class PlayerCombat : Singleton<PlayerCombat>
    {
        private EquipmentManager equipmentManager;
        private PlayerStats stats;

        protected override void Awake()
        {
            base.Awake();
            equipmentManager = GetComponent<EquipmentManager>();
            stats = GetComponent<PlayerStats>();
        }
        
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
                gameObject,
                weapon.ignoreOwner
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
        private void PerformAttackZone(DamageData dmg, Transform attackLocation = null)
        {
            if (dmg.damageZone == null) return;
            if (attackLocation == null)
            {
                attackLocation = PlayerController.Instance.GetAttackSpawnPoint();
            }

            GameObject zone = Instantiate(dmg.damageZone, attackLocation.position, Quaternion.identity);

            if (zone.TryGetComponent(out DamageAOE damageAOE))
            {
                damageAOE.Init(dmg, gameObject);
            }
            else
            {
                zone.GetComponent<DamageZone>().Init(dmg, gameObject);
            }
        }

        /// <summary>
        /// Instantiates and initializes a projectile for ranged attacks.
        /// </summary>
        private void PerformRangedAttack(Weapon weapon, DamageData dmg)
        {
            if (weapon.projectilePrefab == null) return;

            Transform projectileSpawnPoint = PlayerController.Instance.GetAttackSpawnPoint();
            GameObject proj = Instantiate(weapon.projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

            if (proj.TryGetComponent(out Projectile projectile))
            {
                projectile.Init(dmg);
            }
        }

        /// <summary>
        /// Deducts mana and performs a magic attack by spawning a damage zone at the mouse position.
        /// </summary>
        private void PerformMagicAttack(Weapon weapon, DamageData dmg)
        {
            // Abort if no damage zone prefab is set
            if (dmg.damageZone == null) return;

            // Try to deduct mana; abort if not enough
            if (!stats.ModifyStat("MP", -weapon.manaCost)) return;

            // Get mouse position in world space (on XY plane)
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            // Use a temporary GameObject to provide a Transform at the mouse position
            using var temp = new TempTransform(mouseWorldPos);
            PerformAttackZone(dmg, temp.Transform);
        }

        /// <summary>
        /// Utility class for creating and automatically destroying a temporary Transform.
        /// </summary>
        private class TempTransform : System.IDisposable
        {
            public Transform Transform { get; }
            private readonly GameObject _gameObject;

            public TempTransform(Vector3 position)
            {
                _gameObject = new GameObject("MagicAttackTarget");
                _gameObject.transform.position = position;
                Transform = _gameObject.transform;
            }

            public void Dispose()
            {
                Object.Destroy(_gameObject);
            }
        }
    }
}
