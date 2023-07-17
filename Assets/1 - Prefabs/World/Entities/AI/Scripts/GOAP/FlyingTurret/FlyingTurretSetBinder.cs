using CrashKonijn.Goap.Behaviours;
using UnityEngine;

public class FlyingTurretSetBinder : MonoBehaviour
{
    public void Start()
    {
        var runner = FindObjectOfType<GoapRunnerBehaviour>();
        var agent = GetComponent<AgentBehaviour>();
        agent.GoapSet = runner.GetGoapSet("FlyingTurretAISet");
    }
}