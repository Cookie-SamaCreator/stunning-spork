using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold = 0;
    private const int MAX_GOLD = 99999;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";

    public void AddGold(int amount)
    {
        if ((currentGold + amount) >= MAX_GOLD)
        {
            currentGold = MAX_GOLD;
        }
        else
        {
            currentGold += amount;
        }
        UpdateCurrentGold();
    }

    public bool RemoveGold(int amount)
    {
        if (amount > currentGold) { return false; }
        else
        {
            currentGold -= amount;
            UpdateCurrentGold();
            return true;
        }
    }
    private void UpdateCurrentGold()
    {
        if (goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        goldText.text = currentGold.ToString("D3");
    }
}
