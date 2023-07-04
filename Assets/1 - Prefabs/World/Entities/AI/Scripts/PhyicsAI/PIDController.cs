using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDController : MonoBehaviour
{
    [Header("PID Settings")]
    public float proportionalGain;
    public float integralGain;
    public float derivativeGain;

    public float targetHeight;
    public float currentHeight;
    public float thrust;
    public float previousPosition;
    public bool derivativeInitialized;

    [Header("Float Settings")]
    public float dampenFactor;   
    public float adjustFactor;

    [Header("Sensor Settings")]
    public float sensorDistance;
    public float avoidanceForce;
    public float avoidanceTorque;
    public float sensorAngle;

    [Header("Knockback Settings")]
    public float hitTorqueForce;
    public float knockback;

    private float min = -1f;
    private float max = 1f;
    private float error;
    private float intergrationStored = 0.0f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();       
        previousPosition = rb.velocity.y;
    }

    void FixedUpdate()
    {
        currentHeight = transform.position.y;
        float PID = CalculatePID();
        rb.AddForce(Vector3.up * thrust * PID);
        Hover();
        AvoidObstacles();
    }

    float CalculatePID()
    {
        float P = CalculateP();
        float D = CalculateD();
        float I = CalculateI();
        float PID = P + I + D;
        return Mathf.Clamp(PID, min, max);
    }

    float CalculateP()
    {
        error = targetHeight - currentHeight;
        return proportionalGain * error;
    }

    float CalculateI()
    {
        intergrationStored = Mathf.Clamp(intergrationStored + (error * Time.fixedDeltaTime), -1, 1);
        return integralGain * intergrationStored;
    }

    float CalculateD()
    {
        float valueRateOfChange = (rb.position.y - previousPosition) / Time.fixedDeltaTime;
        previousPosition = rb.position.y;
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

    public void AvoidObstacles()
    {
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-sensorAngle, transform.right) * transform.forward, sensorDistance))
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(-sensorAngle, transform.right) * transform.forward * sensorDistance, Color.red);
            rb.AddTorque(transform.right * avoidanceTorque);
            rb.AddForce(-transform.up * avoidanceForce);
            proportionalGain = 0;
            derivativeGain = 0;
            integralGain = 0;
        }
        else if (Physics.Raycast(transform.position, Quaternion.AngleAxis(sensorAngle, transform.right) * transform.forward, sensorDistance))
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(sensorAngle, transform.right) * transform.forward * sensorDistance, Color.red);
            rb.AddTorque(-transform.right * avoidanceTorque);
            rb.AddForce(transform.up * avoidanceForce);
            proportionalGain = 0;
            derivativeGain = 0;
            integralGain = 0;
        }
        else
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(-sensorAngle, transform.right) * transform.forward * sensorDistance, Color.green);
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(sensorAngle, transform.right) * transform.forward * sensorDistance, Color.green);
            proportionalGain = 1;
            derivativeGain = 1;
            integralGain = 1;
        }
    }
}
