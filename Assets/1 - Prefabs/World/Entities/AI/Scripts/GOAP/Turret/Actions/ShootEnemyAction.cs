using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Classes;
using UnityEngine;

public class ShootEnemyAction : ActionBase<ShootEnemyAction.Data>
{
    public override void Created()
    {

    }

    // Start is called before the first frame update
    public override void Start(IMonoAgent agent, Data data)
    {
        data.Threat = agent.GetComponent<ThreatBehaviour>();
        data.ShootBehaviour = agent.GetComponent<ShootBehaviour>();
        data.RotateTowardsBehaviour = agent.GetComponent<IRotateTowardsBehaviour>();
    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
    {
        if (data.Threat == null && data.ShootBehaviour == null)
        {
            return ActionRunState.Stop;
        }

        if (GameEvents.current.IsPlayerDead())
        {
            return ActionRunState.Stop;
        }

        data.RotateTowardsBehaviour.RotateTowardsEnemy(data.Threat.GetLastEnemyPos());
        if (data.Threat.isLookingAtEnemy)
        {
            data.ShootBehaviour.Shoot();
        }

        return ActionRunState.Continue;
    }

    public override void End(IMonoAgent agent, Data data)
    {
    }

    public class Data : IActionData
    {
        public ITarget Target { get; set; }
        public ThreatBehaviour Threat { get; set; }
        public ShootBehaviour ShootBehaviour { get; set; }
        public IRotateTowardsBehaviour RotateTowardsBehaviour { get; set; }
    }
}
