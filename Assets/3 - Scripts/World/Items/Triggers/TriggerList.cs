using System.Collections.Generic;
using UnityEngine;

public class TriggerList : MonoBehaviour
{
    public List<Output> triggerList;
    public bool spawnedFromTrigger = false;
    public Vector3 cameraDirection;
    public Vector3 hitNormal;

    // Start is called before the first frame update
    void Awake()
    {
        triggerList = new List<Output>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (spawnedFromTrigger)
        {
            spawnedFromTrigger = false;
        }
        else
        {
            foreach (var output in triggerList)
            {
                var projectile = output.projectile as IProjectile;
                var instantiatedProjectile = Instantiate(projectile.ProjectilePrefab, transform.position, transform.rotation);
                //Vector3 reflectDir = Vector3.Reflect(cameraDirection, hitNormal);
                //instantiatedProjectile.transform.rotation.SetFromToRotation(cameraDirection, reflectDir);
                instantiatedProjectile.GetComponent<Rigidbody>().AddForce(instantiatedProjectile.transform.forward * 4, ForceMode.Impulse);
            }
            Destroy(gameObject);
        }

    }
}
