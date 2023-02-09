using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TestStats
{
    public int agility, strength;
    public TestStats(int agility, int strength)
    {
        this.agility = agility;
        this.strength = strength;
    }

    public TestStats(TestStats testStats)
    {
        this.agility = testStats.agility;
        this.strength = testStats.strength;
    }
}
