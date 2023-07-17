using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAIController : MonoBehaviour
{
    [Header("Ground Settings")]
    public float groundedRadius;
    public LayerMask layersToAvoid;
    public float groundThrust;
    public bool grounded = false;

    [Header("Float Settings")]
    public float dampenFactor;   
    public float adjustFactor;

    [Header("Knockback Settings")]
    public float hitTorqueForce;
    public float knockback;

    private Rigidbody rb;
    private float distanceFromGround;

    [Header("Move Settings")]
    public float thrust;
    public float thrustTorque;
    public float height;
    public bool moveForward;
    public bool moveBackward;
    public bool moveLeft;
    public bool moveRight;
    private bool heightReached;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        AvoidObstacles();
        // Hover();
        Fly();
        // CalculateHeight();
    }

    private void AvoidObstacles()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        var colliders = Physics.OverlapSphere(spherePosition, groundedRadius, layersToAvoid, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < colliders.Length; i++)
        {
            distanceFromGround = Vector3.Distance(transform.position, colliders[0].ClosestPointOnBounds(transform.position));
            var dir = (transform.position - colliders[i].ClosestPointOnBounds(transform.position)).normalized;
            rb.AddForce(-Physics.gravity.y * dir * (groundThrust / distanceFromGround), ForceMode.Acceleration);
        }
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

    public void Fly()
    {
        if (moveForward)
        {
            rb.AddForce(transform.forward * thrust, ForceMode.Force);
            // rb.AddTorque(transform.right * thrustTorque, ForceMode.Force);
        }
        if (moveBackward)
        {
            rb.AddForce(-transform.forward * thrust, ForceMode.Force);
            // rb.AddTorque(-(transform.right * thrustTorque), ForceMode.Force);
        }
        if (moveLeft)
        {
            rb.AddForce(-transform.right * thrust, ForceMode.Force);
            // rb.AddTorque(transform.forward * thrustTorque, ForceMode.Force);
        }
        if (moveRight)
        {
            rb.AddForce(transform.right * thrust, ForceMode.Force);
            // rb.AddTorque(-(transform.forward * thrustTorque), ForceMode.Force);
        }
    }

    public void CalculateHeight()
    {
        float calculateHeight = Vector3.Distance(transform.position, new Vector3(transform.position.x, 0, transform.position.z));
        if (Math.Abs(transform.position.y - height) < 0.0001f)
        {
            rb.AddForce(transform.up * -Physics.gravity.y, ForceMode.Acceleration);
        }
        else if (transform.position.y < height)
        {
            rb.AddForce(transform.up * -Physics.gravity.y * (thrust / distanceFromGround), ForceMode.Acceleration);
        }
        // else
        // {
        //     rb.AddForce(transform.up * -(thrust / calculateHeight), ForceMode.Force);
        // }
    }
}
