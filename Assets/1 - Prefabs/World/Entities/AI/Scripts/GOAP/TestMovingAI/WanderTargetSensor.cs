using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

public class WanderTargetSensor : LocalTargetSensorBase
{
    public override void Created()
    {
    }

    public override void Update()
    {
    }

    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        // var random = this.GetRandomPosition(agent);

        // return new PositionTarget(random);
        var wayPoints = references.GetCachedComponent<AgentMoveBehaviour>().wayPoints;
        var wayPoint = wayPoints[Random.Range(0, wayPoints.Count)];
        var targetPosition = new Vector3(wayPoint.position.x, agent.transform.position.y, wayPoint.position.z);
        return new PositionTarget(targetPosition);
    }

    public Vector3 GetRandomPosition(IMonoAgent agent)
    {
        var random = Random.insideUnitCircle * 5f;
        var position = agent.transform.position + new Vector3(random.x, 0f, random.y);

        return position;
    }
}
