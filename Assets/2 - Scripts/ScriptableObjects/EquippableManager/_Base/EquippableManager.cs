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
    Head,
    Chest,
    RightArm,
    LeftArm,
    RightWrist,
    LeftWrist,
    RightHand,
    LeftHand,
    Waist,
    RightLeg,
    LeftLeg,
    RightFoot,
    LeftFoot
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
