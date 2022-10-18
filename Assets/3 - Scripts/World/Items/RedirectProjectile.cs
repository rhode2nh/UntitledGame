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

    void FixedUpdate()
    {
        if (gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(gameObject.GetComponent<Rigidbody>().velocity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // TODO: this is a temporary fix. Since the trigger projectile is destroyed in OnCollisionExit,
        // it's rendered a few frames until the next call to FixedUpdate. This hides that artifact.
        if (shouldRedirect)
        {
            shouldRedirect = false;
            camDir = Camera.main.transform.TransformDirection(Vector3.forward);
            transform.rotation = Quaternion.LookRotation(camDir);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 400 * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }
}
