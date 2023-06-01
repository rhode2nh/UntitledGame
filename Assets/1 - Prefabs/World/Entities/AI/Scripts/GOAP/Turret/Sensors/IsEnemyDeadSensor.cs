using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

public class IsEnemyDeadSensor : LocalWorldSensorBase
{

    public override void Created()
    {
    }

    public override void Update()
    {

    }

    public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
    {
        return GameEvents.current.IsPlayerDead();
    }
}
