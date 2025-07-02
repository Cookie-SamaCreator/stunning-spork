using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RPG.Combat;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }
    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private int timeBetweenStaminaRefresh = 3;
    private Transform staminaContainer;
    private readonly int startingStamina = 3;
    private int maxStamina;
    private DamageController damageController;
    const string STAMINA_CONTAINER_TEXT = "Stamina Container";

    protected override void Awake()
    {
        base.Awake();
        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
        damageController = GetComponent<DamageController>();
    }

    private void Start()
    {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
    }

    public bool UseStamina()
    {
        if (CurrentStamina > 0)
        {
            CurrentStamina--;
            UpdateStaminaImages();
            StopAllCoroutines();
            StartCoroutine(RefreshStaminaRoutine());
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddStamina(int staminaAmount = 1)
    {
        if (CurrentStamina < maxStamina && !damageController.IsDead)
        {
            if ((CurrentStamina + staminaAmount) >= maxStamina)
            {
                CurrentStamina = maxStamina;
            }
            else
            {
                CurrentStamina += staminaAmount;
            }
            UpdateStaminaImages();
        }
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            AddStamina();
        }
    }

    private void UpdateStaminaImages()
    {
        for (int i = 0; i < maxStamina; i++)
        {
            Transform child = staminaContainer.GetChild(i);
            Image image = child?.GetComponent<Image>();
            if (i <= CurrentStamina - 1)
            {
                image.sprite = fullStaminaImage;
            }
            else
            {
                image.sprite = emptyStaminaImage;
            }
        }
    }

    public void ReplenishStamina()
    {
        CurrentStamina = startingStamina;
        UpdateStaminaImages();
    }

}
