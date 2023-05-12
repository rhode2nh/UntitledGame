using CrashKonijn.Goap.Behaviours;
using UnityEngine;

public class AgentBrain : MonoBehaviour
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
        this.agent.SetGoal<WanderGoal>(false);
    }

    private void Update()
    {
        if (this.threatBehaviour.IsEnemyInThreatArea() || this.threatBehaviour.IsLookingAtEnemy())
        {
            this.agent.SetGoal<DestroyEnemyGoal>(true);
        }
        else if (this.threatBehaviour.threat < this.threatBehaviour.threshold)
        {
            this.agent.SetGoal<WanderGoal>(true);
        }
    }
}
