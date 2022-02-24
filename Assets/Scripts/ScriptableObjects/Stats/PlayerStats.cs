using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Stats/Player Stats")]
public class PlayerStats : Stats
{
    [Header("Player Specific Stats")]
    [SerializeField] private float distanceTraveled = 0.0f;

    public float DistanceTraveled
    {
        get { return distanceTraveled; }
        set { distanceTraveled = value; }
    }

    public void ApplyConsumable(ItemStats stats)
    {
        foreach (var attribute in stats.attributes)
        {
            this.attributes[attribute.Key].RawValue += attribute.Value.RawValue;
            this.attributes[attribute.Key].BuffPercentage += attribute.Value.BuffPercentage;
        }
    }
}
