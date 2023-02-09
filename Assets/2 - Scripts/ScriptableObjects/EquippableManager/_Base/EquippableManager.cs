using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipSlot
{
    public Item item;
    public BodyPart bodyPart;
}

public enum BodyPart
{
    Head = 0,
    LeftArm = 1,
    Chest = 2,
    RightArm = 3,
    LeftWrist = 4,
    Waist = 5,
    RightWrist = 6,
    LeftHand = 7,
    RightHand = 8,
    Legs = 9,
    Feet = 10
}

public enum EquipType
{
    Equipment,
    Implant
}

[CreateAssetMenu(fileName = "EquippableManager", menuName = "EquippableManager", order = 1)]
public class EquippableManager : ScriptableObject
{
    [SerializeField]
    public List<EquipSlot> slots;
    [SerializeField]
    public EquipType type;
}
