using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Stats/Player Stats")]
public class PlayerStats : StatsObject
{
    [Header("Player Specific Stats")]
    [SerializeField] private float distanceTraveled = 0.0f;

    public float DistanceTraveled
    {
        get { return distanceTraveled; }
        set { distanceTraveled = value; }
    }
}
