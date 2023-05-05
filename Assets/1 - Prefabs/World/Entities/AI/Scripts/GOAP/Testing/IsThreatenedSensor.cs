using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

public class IsThreatenedSensor : LocalWorldSensorBase
{
    public override void Created()
    {

    }

    public override void Update()
    {

    }

    public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
    {
        var threatBehaviour = references.GetComponent<ThreatBehaviour>();

        if (threatBehaviour == null)
        {
            return false;
        }

        return threatBehaviour.threat > threatBehaviour.threshold;
    }
}
