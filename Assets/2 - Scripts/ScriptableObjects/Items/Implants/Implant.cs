using UnityEngine;

[CreateAssetMenu(fileName = "New Implant", menuName = "Items/Implants/New Implant", order = 1)]
public class Implant : Item, IImplant
{
    [SerializeField]
    private int qualityLevel = 1;
    [SerializeField]
    private BodyPart bodyPart;

    public int QualityLevel { get => qualityLevel; set => qualityLevel = value;  }
    public BodyPart BodyPart { get => bodyPart; set => bodyPart = value;  }

    private void Awake()
    {
        this.Name = Constants.IMPLANT;
        this.Id = Constants.IMPLANT_ID;
        this.isStackable = false;
    }
}
