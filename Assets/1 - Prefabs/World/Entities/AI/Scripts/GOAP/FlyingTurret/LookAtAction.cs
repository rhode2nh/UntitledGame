using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Classes;
using UnityEngine;

public class LookAtAction : ActionBase<LookAtAction.Data>
{
    public override void Created()
    {
    }

    // Start is called before the first frame update
    public override void Start(IMonoAgent agent, Data data)
    {
        data.ThreatBehaviour = agent.GetComponent<ThreatBehaviour>();
        data.RotateTowardsBehaviour = agent.GetComponent<IRotateTowardsBehaviour>();
        data.pidController = agent.GetComponent<PIDController>();
        data.RotateTowardsBehaviour.GoToPosition(data.ThreatBehaviour.GetLastEnemyPos());
    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
    {
        if (data.ThreatBehaviour == null || data.RotateTowardsBehaviour == null)
        {
            return ActionRunState.Stop;
        }
        // if  (!data.ThreatBehaviour.isEnemyInThreatArea) {
        //     return ActionRunState.Stop;
        // }

        return ActionRunState.Continue;
    }

    public override void End(IMonoAgent agent, Data data)
    {
        data.pidController.StopAllCoroutines();
        // data.pidController.StopGoingToPosition();
    }

    public class Data : IActionData
    {
        public ITarget Target { get; set; }
        public ThreatBehaviour ThreatBehaviour { get; set; }
        public IRotateTowardsBehaviour RotateTowardsBehaviour { get; set; }
        public PIDController pidController { get; set; }
    }
}
