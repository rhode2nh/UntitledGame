using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRaycast : MonoBehaviour
{
    float maxDistance;
    public bool isHitting;
    public RaycastHit hit;

    private void Start()
    {
        isHitting = false;
        maxDistance = 10.0f;
    }

    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        isHitting = Physics.Raycast(transform.position, fwd, out hit, maxDistance);
        Debug.DrawRay(transform.position, fwd * maxDistance, Color.green);
    }
}
