using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class AgentMoveBehaviour : MonoBehaviour
{
    private AgentBehaviour agent;
    public NavMeshAgent navMeshAgent;
    private ITarget currentTarget;
    private bool shouldMove;
    public Transform goalPosition;
    public List<Transform> wayPoints;

    private void Awake()
    {
        this.agent = this.GetComponent<AgentBehaviour>();
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        this.agent.Events.OnTargetInRange += this.OnTargetInRange;
        this.agent.Events.OnTargetChanged += this.OnTargetChanged;
        this.agent.Events.OnTargetOutOfRange += this.OnTargetOutOfRange;
    }

    private void OnDisable()
    {
        this.agent.Events.OnTargetInRange -= this.OnTargetInRange;
        this.agent.Events.OnTargetChanged -= this.OnTargetChanged;
        this.agent.Events.OnTargetOutOfRange -= this.OnTargetOutOfRange;
    }

    private void OnTargetInRange(ITarget target)
    {
        this.shouldMove = false;
        Debug.Log("In Range");
    }

    private void OnTargetChanged(ITarget target, bool inRange)
    {
        this.currentTarget = target;
        this.shouldMove = !inRange;
        this.navMeshAgent.destination = currentTarget.Position;
    }

    private void OnTargetOutOfRange(ITarget target)
    {
        this.shouldMove = true;
    }

    public void Update()
    {
        if (!this.shouldMove)
        {
            return;
        }

        if (this.currentTarget == null)
        {
            return;
        }
    }
}
