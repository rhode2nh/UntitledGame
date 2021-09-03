using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsObject : ScriptableObject
{
    [Header("Base Stats")]
    [SerializeField] private int agility = 0;
    [SerializeField] private int strength = 0;
    [SerializeField] private int stamina = 0;
    [SerializeField] private int machineLearning = 0;

    public int Agility => agility;
    public int Strength => strength;
    public int Stamina => stamina;
    public int MachineLearning => machineLearning;
}
