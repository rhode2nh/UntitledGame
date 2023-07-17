using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDController : MonoBehaviour
{
    [Header("PID Settings")]
    public float proportionalGain;
    public float integralGain;
    public float derivativeGain;
    public Transform target;
    public float thrust;
    public float torque;
    public Vector3 previousPosition;
    public float previousAngle;
    public bool derivativeInitialized;
    public float stabilizeThreshold;

    [Header("Float Settings")]
    public float dampenFactor;   
    public float adjustFactor;

    [Header("Knockback Settings")]
    public float hitTorqueForce;
    public float knockback;

    private float min = -1f;
    private float max = 1f;
    private float positionError;
    private float angleError;
    private float rotationError;
    private float intergrationStored = 0.0f;

    private Rigidbody rb;
    public Vector3[] path;
    public int targetIndex;
    public bool findNewPath = false;
    public bool findRandomPaths = false;
    public bool hit = false;
    public bool stabilize = false;
    public float stabilizeTime = 0.0f;
    public Vector3 testPosition;
    public bool coroutineStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();       
        previousPosition = rb.velocity;
        // PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }
    void Update() {
        // if (findNewPath) {
        //     PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        //     findNewPath = false;
        // }
    }

    void FixedUpdate() {
        // if (stabilize) {
            // Hover();
        // }
    }

    IEnumerator FollowPath() {
        // TODO: I think there are instances where a path cannot be found because the turret is in
        // an unwalkable node. We should try and find a neighbor that is walkable or temporarily mark
        // that node as walkable.
        Vector3 currentWaypoint = path[0];

        while (true) {
            if (Vector3.Distance(transform.position, currentWaypoint) < 1.0f) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    path = new Vector3[0];
                    targetIndex = 0;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            if (hit) {
                hit = false;
                yield return new WaitForSeconds(0.2f);
                stabilize = true;
                yield return new WaitForSeconds(stabilizeTime);
                stabilize = false;
            }
            float positionPID = CalculatePositionPID(path[path.Length - 1]);
            rb.AddForce((currentWaypoint - rb.position).normalized * thrust * positionPID);
            RotateToVelocity(torque, false);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator GoToPosition(Vector3 position) {
        // while (true) {
        //     if (position.Equals(rb.position)) {
        //         yield break;
        //     }
        //     if (hit) {
        //         hit = false;
        //         yield return new WaitForSeconds(0.2f);
        //         stabilize = true;
        //         yield return new WaitForSeconds(stabilizeTime);
        //         stabilize = false;
        //     }
        //     if (!hit) {
        //         Vector3 a = position;
        //         Vector3 b = transform.position;
        //         Vector3 pointToGoTo = 5 * Vector3.Normalize(b - a) + a;
        //         float positionPID = CalculatePositionPID(pointToGoTo);
        //         rb.AddForce((pointToGoTo - rb.position).normalized * thrust * positionPID);
        //         LookAtPosition(position, torque);
        //         yield return new WaitForFixedUpdate();
        //     } else {
        //         hit = false;
        //     }
        // }
        while (true) {
            if (position.Equals(rb.position)) {
                yield break;
            }
            if (hit) {
                hit = false;
                yield return new WaitForSeconds(0.2f);
                stabilize = true;
                yield return new WaitForSeconds(stabilizeTime);
                stabilize = false;
            }
            if (!hit) {
                Vector3 a = testPosition;
                Vector3 b = transform.position;
                Vector3 pointToGoTo = 5 * Vector3.Normalize(b - a) + a;
                float positionPID = CalculatePositionPID(pointToGoTo);
                rb.AddForce((pointToGoTo - rb.position).normalized * thrust * positionPID);
                LookAtPosition(testPosition, torque);
                yield return new WaitForFixedUpdate();
            } else {
                hit = false;
            }
        }
    }

    public void GetNewPath(Vector3 destination) {
        PathRequestManager.RequestPath(transform.position, destination, OnPathFound);
    }

    public Vector3 GetRandomDestination() {
        return PathRequestManager.GetRandomNode().worldPosition;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if (pathSuccessful) {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    //rotates rigidbody to face its current velocity
    public void RotateToVelocity(float turnSpeed, bool ignoreY)
    {
        Vector3 dir;
        if(ignoreY)
        dir = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        else
        dir = rb.velocity;

        Vector3 stabilizeDir = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (dir.magnitude > 0.1)
        {
            if (dir.magnitude <  stabilizeThreshold) {
                dir = stabilizeDir; 
                Quaternion dirQ = Quaternion.LookRotation(dir);
                Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, turnSpeed * Time.deltaTime);
                rb.MoveRotation(slerp);
            }
            else {
                Quaternion dirQ = Quaternion.LookRotation(dir);
                Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, dir.magnitude * turnSpeed * Time.deltaTime);
                rb.MoveRotation(slerp);
            }
        }
    }

    public void StopFollowingPath() {
        StopCoroutine("FollowPath");
    }

    public void StopGoingToPosition() {
        StopCoroutine("GoToPosition");
    }

    public void StartGoingToPosition(Vector3 position) {
        StartCoroutine(GoToPosition(position));
    }

    public void LookAtPosition(Vector3 position, float turnSpeed) {
        Quaternion dirQ = Quaternion.LookRotation(position - transform.position);
        Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, turnSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(slerp);
    }

    float CalculatePositionPID(Vector3 targetPosition) {
        float P = CalculateP(rb.position, targetPosition);
        float I = CalculateI(positionError);
        float D = CalculateD(rb.position);
        float PID = P + I + D;
        return Mathf.Clamp(PID, min, max);
    }

    float CalculateRotationPID(float currentAngle, float targetAngle) {
        float P = CalculateP(currentAngle, targetAngle);
        float I = CalculateI(angleError);
        float D = CalculateD(currentAngle);
        float PID = P + I + D;
        return Mathf.Clamp(PID, min, max);
    }
    
    float CalculateP(float currentAngle, float targetAngle) {
        angleError = AngleDifference(targetAngle, currentAngle);
        return proportionalGain * angleError;
    }

    float CalculateP(Vector3 currentPosition, Vector3 targetPosition) {
        positionError = (targetPosition - currentPosition).magnitude;
        return proportionalGain * positionError;
    }

    float CalculateI(float error)
    {
        intergrationStored = Mathf.Clamp(intergrationStored + (error * Time.fixedDeltaTime), -1, 1);
        return integralGain * intergrationStored;
    }

    float CalculateD(float currentAngle)
    {
        float valueRateOfChange = AngleDifference(currentAngle, previousAngle) / Time.fixedDeltaTime;
        previousAngle = currentAngle;
        float deriveMeasure = 0;
        if (derivativeInitialized)
        {
            deriveMeasure = -valueRateOfChange;
        }
        else
        {
            derivativeInitialized = true;
        }

        return derivativeGain * deriveMeasure; 
    }

    float CalculateD(Vector3 currentPosition)
    {
        float valueRateOfChange = (currentPosition - previousPosition).magnitude / Time.fixedDeltaTime;
        previousPosition = rb.position;
        float deriveMeasure = 0;
        if (derivativeInitialized)
        {
            deriveMeasure = -valueRateOfChange;
        }
        else
        {
            derivativeInitialized = true;
        }

        return derivativeGain * deriveMeasure; 
    }

    float AngleDifference(float a, float b) {
        return (a - b + 540) % 360 - 180;
    }

    private void Hover()
    {
        Quaternion deltaQuat = Quaternion.FromToRotation(rb.transform.up, Vector3.up);

        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);

        // Applies negative forces until the torque is zeroed out
        rb.AddTorque(-rb.angularVelocity * dampenFactor, ForceMode.Acceleration);

        // Determines how much torque should be applied between local up and world up angle
        rb.AddTorque(axis.normalized * angle * adjustFactor, ForceMode.Acceleration);
    }

    public void ApplyForces(Vector3 torqueDir, Vector3 forceDir)
    {
        rb.AddTorque(torqueDir * hitTorqueForce, ForceMode.Force);
        rb.AddForce(forceDir * knockback, ForceMode.Impulse);
    }

    public void OnDrawGizmos() {
        if (path != null) {
            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    public void ClearPath() {
        path = new Vector3[0];
        targetIndex = 0;
    }
}
