using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BuffModifiers
{
    public StatTypes name;
    public float rawValue;
    public float buffPercentage;
    public float duration;
}

[CreateAssetMenu(fileName = "New Item Stats", menuName = "Stats/Item Stats")]
public class ItemStats : Stats
{
    // Add multipliers in the future
    [Header("Modifiers")]
    List<BuffModifiers> buffModifiers;
}
