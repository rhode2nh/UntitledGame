using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public PlayerStats playerStats;

    void Start()
    {
        GameEvents.current.onCalculateBuffedStats += CalculateBuffedStats;
        playerStats.InitializeStats();
    }

    public void CalculateBuffedStats()
    {
        playerStats.buffedStats = playerStats.testStats;
        List<TestStats> implantStats = GameEvents.current.GetImplantStats();
        foreach (var implantStat in implantStats)
        {
            playerStats.buffedStats = playerStats.buffedStats + implantStat;
        }
    }
}
