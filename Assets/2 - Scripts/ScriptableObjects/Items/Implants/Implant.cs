using UnityEngine;

[CreateAssetMenu(fileName = "New Implant", menuName = "Items/Implants/New Implant", order = 1)]
public class Implant : Item, IImplant
{
    [SerializeField]
    private int qualityLevel = 1;
    [SerializeField]
    private BodyPart bodyPart;
    [SerializeField]
    private TestStats testStats = new TestStats(1, 1);

    public int QualityLevel { get => qualityLevel; set => qualityLevel = value;  }
    public BodyPart BodyPart { get => bodyPart; set => bodyPart = value;  }
    public TestStats TestStats { get => testStats; set => testStats = value; }

    private void Awake()
    {
        this.Name = Constants.IMPLANT;
        this.isStackable = false;
    }
}
