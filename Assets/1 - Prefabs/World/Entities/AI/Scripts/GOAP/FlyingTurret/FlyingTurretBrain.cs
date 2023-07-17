using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using UnityEngine;

public class FlyingTurretBrain : MonoBehaviour
{
    private AgentBehaviour agent;
    private ThreatBehaviour threatBehaviour;
    private MoveBehavior agentMoveBehaviour;

    private void Awake()
    {
        this.agent = this.GetComponent<AgentBehaviour>();   
        this.threatBehaviour = this.GetComponent<ThreatBehaviour>();
        // this.agentMoveBehaviour = this.GetComponent<MoveBehavior>();
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
            this.agent.SetGoal<DestroyEnemyGoal>(true);
        }
        else
        {
            this.agent.SetGoal<WanderGoal>(true);
        }
    }
}
