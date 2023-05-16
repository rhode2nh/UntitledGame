using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Classes;
using UnityEngine;

public class LookTowardsEnemyAction : ActionBase<LookTowardsEnemyAction.Data>
{
    public override void Created()
    {

    }

    // Start is called before the first frame update
    public override void Start(IMonoAgent agent, Data data)
    {
        data.ThreatBehaviour = agent.GetComponent<ThreatBehaviour>();
        data.RotateTowardsBehaviour = agent.GetComponent<RotateTowardsBehaviour>();
    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
    {
        if (data.ThreatBehaviour == null || data.RotateTowardsBehaviour == null)
        {
            return ActionRunState.Stop;
        }
        if (data.ThreatBehaviour.threat > data.ThreatBehaviour.threshold || data.ThreatBehaviour.isLookingAtEnemy)
        {
            return ActionRunState.Stop;
        }

        data.RotateTowardsBehaviour.RotateTowardsEnemy(data.ThreatBehaviour.GetLastEnemyPos());

        return ActionRunState.Continue;
    }

    public override void End(IMonoAgent agent, Data data)
    {
    }

    public class Data : IActionData
    {
        public ITarget Target { get; set; }
        public ThreatBehaviour ThreatBehaviour { get; set; }
        public RotateTowardsBehaviour RotateTowardsBehaviour { get; set; }
    }
}
