using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Classes;
using UnityEngine;

public class IdleAction : ActionBase<IdleAction.Data>
{
    public override void Created()
    {

    }

    // Start is called before the first frame update
    public override void Start(IMonoAgent agent, Data data)
    {

    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
    {
        return ActionRunState.Continue;
    }

    public override void End(IMonoAgent agent, Data data)
    {
    }

    public class Data : IActionData
    {
        public ITarget Target { get; set; }
    }
}
