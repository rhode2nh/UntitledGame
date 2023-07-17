using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

public class LookAtBehavior : MonoBehaviour, IRotateTowardsBehaviour
{
    private AgentBehaviour agent;
    private PIDController pidController;
    private ITarget currentTarget;
    private ThreatBehaviour threatBehaviour;
    private bool shouldRotate;

    private void Awake()
    {
        this.agent = this.GetComponent<AgentBehaviour>();
        this.pidController = GetComponent<PIDController>();
        this.threatBehaviour = GetComponent<ThreatBehaviour>();
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
        this.shouldRotate = true;
        pidController.StopFollowingPath();
    }

    private void OnTargetChanged(ITarget target, bool inRange)
    {
        this.currentTarget = target;
        this.shouldRotate = !inRange;
    }

    private void OnTargetOutOfRange(ITarget target)
    {
        this.shouldRotate = false;
    }

    public void FixedUpdate()
    {
        if (!this.shouldRotate)
        {
            return;
        }

        if (this.currentTarget == null)
        {
            return;
        }
        // if (!threatBehaviour.isEnemyInThreatArea) {
        //     pidController.StopGoingToPosition();
        //     pidController.coroutineStarted = false;
        //     return;
        // }

        // if (threatBehaviour.isEnemyInThreatArea && !pidController.coroutineStarted) {
        //     pidController.StartGoingToPosition(threatBehaviour.GetLastEnemyPos());
        //     pidController.coroutineStarted = true;
        // }
        // // pidController.LookAtPosition(currentTarget.Position, 5);
        if (threatBehaviour.HasEnemyMoved()) {
            pidController.testPosition = threatBehaviour.GetLastEnemyPos();
            // pidController.StartGoingToPosition(threatBehaviour.GetLastEnemyPos());
        }
        // pidController.StartGoingToPosition(threatBehaviour.GetLastEnemyPos());
        // pidController.LookAtPosition(threatBehaviour.GetLastEnemyPos(), 5);
    }

    public void RotateTowardsEnemy(Vector3 enemyPos) {
        pidController.LookAtPosition(enemyPos, 5);
    }

    public void GoToPosition(Vector3 position) {
        pidController.StartGoingToPosition(position);
    }

    public void StopGoingToPosition() {
        pidController.StopGoingToPosition();
    }
}
