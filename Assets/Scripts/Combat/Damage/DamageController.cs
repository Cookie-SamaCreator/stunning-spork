using UnityEngine;
using RPG.Definitions;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class DamageController : MonoBehaviour
    {
        public bool IsDead { get; private set; }
        [SerializeField] private float knockBackThrustAmount = 10f;
        [SerializeField] private float damageRecoveryTime = 1f;
        [SerializeField] private int maxHP = 100;
        private Slider healthSlider;
        const string HEALTH_SLIDER_TEXT = "Health Slider";
        readonly int DEATH_HASH = Animator.StringToHash("Death");
        private int currentHP;
        private bool canTakeDamage = true;
        private bool isPlayer;
        private Knockback knockback;
        private Flash flash;
        private Animator animator;

        // Stores resistance values for each element type (0 = no resistance, positive = resistance, negative = weakness)
        public Dictionary<ElementType, float> elementalResistances = new();

        private void Awake()
        {
            flash = GetComponent<Flash>();
            knockback = GetComponent<Knockback>();
            animator = GetComponent<Animator>();
            // Initialize all elemental resistances to 0 (neutral)
            foreach (ElementType elem in System.Enum.GetValues(typeof(ElementType)))
            {
                elementalResistances[elem] = 0f;
            }
            isPlayer = gameObject.CompareTag("Player");
        }

        private void Start()
        {
            IsDead = false;
            currentHP = maxHP;
            if (isPlayer)
            {
                UpdateHealthUI();
            }
        }

        /// <summary>
        /// Sets a weakness for a specific element (negative resistance).
        /// </summary>
        public void SetWeakness(ElementType type, float bonus) => elementalResistances[type] = -Mathf.Abs(bonus);

        /// <summary>
        /// Sets a resistance for a specific element (positive resistance).
        /// </summary>
        public void SetResistance(ElementType type, float resistance) => elementalResistances[type] = Mathf.Abs(resistance);


        public void Heal(int amount)
        {
            if (currentHP < maxHP)
            {
                if ((currentHP + amount) >= maxHP)
                {
                    currentHP = maxHP;
                }
                else
                {
                    currentHP += amount;
                }
            }
            if (isPlayer)
            {
                UpdateHealthUI();
            }
        }

        /// <summary>
        /// Applies incoming damage, factoring in elemental resistances/weaknesses.
        /// </summary>
        public void ApplyDamage(DamageData dmgData)
        {
            if (!canTakeDamage) return;

            knockback.GetKnockedBack(dmgData.source, knockBackThrustAmount);
            StartCoroutine(flash.FlashRoutine());
            canTakeDamage = false;

            int total = dmgData.physicalDamage;

            // Apply elemental modifier if applicable
            if (dmgData.element != ElementType.None && dmgData.elementalDamage > 0)
            {
                float mod = 1f - elementalResistances[dmgData.element];
                total += Mathf.RoundToInt(dmgData.elementalDamage * mod);
            }

            currentHP -= total;
            StartCoroutine(DamageRecoveryRoutine());
            Debug.Log($"{gameObject.name} took {total} damage. Remaining HP: {currentHP}");
            DetectDeath();
        }

        /// <summary>
        /// Checks if the entity is dead and logs a warning.
        /// </summary>
        private void DetectDeath()
        {
            if (currentHP > 0) return;
            IsDead = true;
            currentHP = 0;
            //animator.SetTrigger(DEATH_HASH);

            GetComponent<PickupSpawner>()?.DropItems();
            StartCoroutine(DeathRoutine());
            Debug.LogWarning($"{gameObject.name} is dead!");
        }

        private IEnumerator DamageRecoveryRoutine()
        {
            yield return new WaitForSeconds(damageRecoveryTime);
            canTakeDamage = true;
        }

        private IEnumerator DeathRoutine()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
            Stamina.Instance.ReplenishStamina();
            //SceneManager.LoadScene(TOWN_SCENE_STRING);
        }

        private void UpdateHealthUI()
        {
            if (healthSlider == null)
            {
                healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
            }

            healthSlider.maxValue = maxHP;
            healthSlider.value = currentHP;
        }
    }
}
