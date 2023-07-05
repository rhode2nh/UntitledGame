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
    Vector3[] path;
    int targetIndex;
    public bool findNewPath = false;
    public bool findRandomPaths = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();       
        previousPosition = rb.velocity;
        // PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }
    void Update() {
        if (findNewPath) {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            findNewPath = false;
        }
    }

    void FixedUpdate() {
        // float positionPID = CalculatePositionPID();
        // rb.AddForce((target.position - rb.position).normalized * thrust * positionPID);

        // float yCurrentAngle = Vector3.SignedAngle(Vector3.forward, rb.rotation * Vector3.forward, Vector3.up);
        // float yTargetAngle = Vector3.SignedAngle(Vector3.forward, (targetTransform.position - rb.position).normalized, Vector3.up);
        // float yRotationPID = CalculateRotationPID(yCurrentAngle, yTargetAngle);
        // Debug.Log(yCurrentAngle);
        // Debug.Log(yTargetAngle);
        // rb.AddTorque(new Vector3(0, yRotationPID * torque, 0)); 

        // RotateToVelocity(torque, false);

        // Hover();
    }

    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];

        while (true) {
            if (Vector3.Distance(transform.position, currentWaypoint) < 1.0f) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    path = new Vector3[0];
                    targetIndex = 0;
                    if (findRandomPaths) {
                        PathRequestManager.RequestRandomPath(transform.position, OnPathFound);
                    }
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            float positionPID = CalculatePositionPID();
            rb.AddForce((currentWaypoint - rb.position).normalized * thrust * positionPID);
            RotateToVelocity(torque, false);
            yield return new WaitForFixedUpdate();
        }
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
                Quaternion dirQ = Quaternion.LookRotation (dir);
                Quaternion slerp = Quaternion.Slerp (transform.rotation, dirQ, turnSpeed * Time.deltaTime);
                rb.MoveRotation(slerp);
            }
            else {
                Quaternion dirQ = Quaternion.LookRotation (dir);
                Quaternion slerp = Quaternion.Slerp (transform.rotation, dirQ, dir.magnitude * turnSpeed * Time.deltaTime);
                rb.MoveRotation(slerp);
            }
        }
    }

    float CalculatePositionPID() {
        float P = CalculateP(rb.position, target.position);
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
                Gizmos.color = Color.black;
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
}
