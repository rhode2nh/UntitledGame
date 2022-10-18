using System.Collections.Generic;
using UnityEngine;

public class TriggerList : MonoBehaviour
{
    public List<Output> triggerList;
    public bool spawnedFromTrigger = false;
    private Vector3 camDir;
    public float xSpread;
    public float ySpread;
    public int numFramesSinceLastCollision = 0;
    public int maxFramesToDestroy = 3;

    // Start is called before the first frame update
    void Awake()
    {
        triggerList = new List<Output>();
    }

    void FixedUpdate()
    {
        if (gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(gameObject.GetComponent<Rigidbody>().velocity);
        }

        if (numFramesSinceLastCollision < maxFramesToDestroy)
        {
            numFramesSinceLastCollision++;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        camDir = Camera.main.transform.TransformDirection(Vector3.forward);
        if (spawnedFromTrigger)
        {
            spawnedFromTrigger = false;
        }
        else
        {
            for (int i = 0; i < triggerList.Count; i++)
            {
                float x = Random.Range(-xSpread * 0.5f, xSpread * 0.5f);
                float y = Random.Range(-ySpread * 0.5f, ySpread * 0.5f);
                var projectile = triggerList[i].projectile as IProjectile;
                var instantiatedProjectile = Instantiate(projectile.ProjectilePrefab);
                var redirect = instantiatedProjectile.GetComponent<RedirectProjectile>();
                if (redirect != null)
                {
                    redirect.shouldRedirect = false;
                }
                instantiatedProjectile.transform.position = transform.position;
                instantiatedProjectile.transform.rotation = transform.rotation;
                var triggers = instantiatedProjectile.GetComponent<TriggerList>();
                if (triggers != null)
                {
                    triggers.spawnedFromTrigger = true;
                    triggers.triggerList = new List<Output>(triggerList);
                    triggers.triggerList.RemoveAt(0);
                    instantiatedProjectile.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 4, ForceMode.Impulse);
                    break;
                }
                instantiatedProjectile.transform.rotation = Quaternion.LookRotation(camDir);
                instantiatedProjectile.transform.rotation = Quaternion.AngleAxis(x, Vector3.up) * instantiatedProjectile.transform.rotation;
                instantiatedProjectile.GetComponent<Rigidbody>().AddForce(instantiatedProjectile.transform.forward * 4, ForceMode.Impulse);
            }

            if (numFramesSinceLastCollision >= maxFramesToDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
