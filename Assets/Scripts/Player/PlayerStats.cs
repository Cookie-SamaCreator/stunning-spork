using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    // Stores player stats with their names as keys
    private readonly Dictionary<string, int> stats = new();

    private void Awake()
    {
        // Initialize default stats
        stats["HP"] = 100;
        stats["MP"] = 100;
        stats["Strength"] = 10;
        stats["Intelligence"] = 10;
        stats["Agility"] = 10;
        stats["Luck"] = 10;
    }

    /// <summary>
    /// Modifies the value of a stat by a given amount.
    /// Returns true if the stat exists and was modified, false otherwise.
    /// Stat value cannot go below 0.
    /// </summary>
    public bool ModifyStat(string stat, int value)
    {
        if (!stats.TryGetValue(stat, out int current))
            return false;

        stats[stat] = Mathf.Max(0, current + value);
        return true;
    }

    /// <summary>
    /// Gets the value of a stat. Returns 0 if the stat does not exist.
    /// </summary>
    public int GetStat(string stat) => stats.TryGetValue(stat, out var val) ? val : 0;
}
