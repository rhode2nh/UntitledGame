using CrashKonijn.Goap.Behaviours;
using UnityEngine;

public class AgentBrain : MonoBehaviour
{
    private AgentBehaviour agent;
    private ThreatBehaviour threatBehaviour;
    private AgentMoveBehaviour agentMoveBehaviour;

    private void Awake()
    {
        this.agent = this.GetComponent<AgentBehaviour>();   
        this.threatBehaviour = this.GetComponent<ThreatBehaviour>();
        this.agentMoveBehaviour = this.GetComponent<AgentMoveBehaviour>();
    }

    // Update is called once per frame
    void Start()
    {
        this.agent.SetGoal<WanderGoal>(false);       
    }

    private void Update()
    {
        if (this.threatBehaviour.isEnemyInThreatArea)
        {
            Debug.Log("In threat");
            this.agent.SetGoal<DestroyEnemyGoal>(true);
            this.agentMoveBehaviour.navMeshAgent.destination = this.agent.transform.position;
        }
        else
        {
            this.agent.SetGoal<WanderGoal>(true);
        }
    }
}
