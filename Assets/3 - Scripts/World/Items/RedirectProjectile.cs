using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectProjectile : MonoBehaviour
{
    public Vector3 camDir;
    public bool shouldRedirect;

    void Awake()
    {
        shouldRedirect = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (shouldRedirect)
        {
            shouldRedirect = false;
            camDir = Camera.main.transform.TransformDirection(Vector3.forward);
            transform.rotation = Quaternion.LookRotation(camDir);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 4, ForceMode.Impulse);
        }
    }
}
