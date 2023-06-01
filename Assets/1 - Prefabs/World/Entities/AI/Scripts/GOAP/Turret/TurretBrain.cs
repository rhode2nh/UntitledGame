using CrashKonijn.Goap.Behaviours;
using UnityEngine;

public class TurretBrain : MonoBehaviour
{
    private AgentBehaviour agent;
    
    private ThreatBehaviour threatBehaviour;

    private void Awake()
    {
        this.agent = this.GetComponent<AgentBehaviour>();
        this.threatBehaviour = this.GetComponent<ThreatBehaviour>();
    }

    private void Start()
    {
        this.agent.SetGoal<IdleGoal>(true);
    }

    private void Update()
    {
        if (this.threatBehaviour.isEnemyInThreatArea || this.threatBehaviour.isLookingAtEnemy)
        {
            this.agent.SetGoal<DestroyEnemyGoal>(true);
        }
        else 
        {
            this.agent.SetGoal<IdleGoal>(true);
        }
    }
}
