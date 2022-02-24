using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct KeyValuePair
{
    public StatTypes key;
    public Stat val;
}

public enum StatTypes
{
    Agility,
    Strength,
    Stamina,
    Intelligence,
    Speed
}

[Serializable]
public class Stat
{
    [SerializeField]
    private float rawValue;
    [SerializeField]
    private float buffPercentage;

    public float RawValue { get { return rawValue; } set { rawValue = value; } }
    public float BuffPercentage { get { return buffPercentage; } set { buffPercentage = value; } }
}
public class Stats : ScriptableObject
{
    [SerializeField]
    private List<KeyValuePair> _attributes;
    public Dictionary<StatTypes, Stat> attributes = new Dictionary<StatTypes, Stat>();

    private void OnEnable()
    {
        foreach (KeyValuePair kvp in _attributes)
        {
            attributes[kvp.key] = kvp.val;
        }
    }
} 